using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class SmeltMerge1View : BaseView<SmeltMerge1View>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }
        private GameObject tips;//提示
        private UILabel sprite2;//= NGUITools.FindInChild<UISprite>(tips, "sprite2");
        private UISprite sprite1;
        private List<ItemContainer> itemList = new List<ItemContainer>();  //镶嵌位置

        private List<ItemContainer> smeltList = new List<ItemContainer>();  //宝石

        private int smeltType;//宝石类型

        private GameObject item;
        private UIGrid grid;
        private UIScrollView scrollView;
        //private TweenPosition gridTween;
        private UISlider mergeSlider;//充灵进度条
        

        public uint uid;
        private List<uint> idList = new List<uint>();

        private int energyFull ;//满经验
        private int energy;//当前经验
        private uint currenId;//充灵达到宝石的id
        private ItemContainer currentItem;//当前选中的宝石

        private UILabel mergeTips;  //充灵宝石提示

        private UISprite bottomSprite;//下面装饰块,显示“请选择已镶嵌宝石” 是要隐藏
        public ItemContainer GuideStoneItemContainer;
        public ItemContainer GuideMergeStoneItemContainer;
        public Button MergeButton;//充灵按钮
        private int _guideIndex;


        protected override void Init()
        {
            mergeTips = FindInChild<UILabel>("desc");
            bottomSprite = FindInChild<UISprite>("di1");
            GameObject temp;
            ItemContainer ic;
            for (int i = 1; i < 6; i++)
            {
                temp = FindChild("item" + i);
                ic = temp.AddMissingComponent<ItemContainer>();
				ic.onClick = SmeltBtnClick;
                ic.buttonType = Button.ButtonType.Toggle;
				itemList.Add(ic);
            }
            temp = null;

            item = FindChild("content/grid/item1");
            item.name = "0000";
            //gridTween = FindInChild<TweenPosition>("content/grid/");
            ic = item.AddMissingComponent<ItemContainer>();
            smeltList.Add(ic);
            item.SetActive(false);
            ic.onClick = SelectOnClick;
            grid = FindInChild<UIGrid>("content/grid");
            grid.sorted = true;
            scrollView = FindInChild<UIScrollView>("content");
            mergeSlider = FindInChild<UISlider>("sld_exp");
            MergeButton = FindInChild<Button>("btn_qh");
            tips = FindChild("tips");
            sprite2 = NGUITools.FindInChild<UILabel>(tips, "label");
            sprite1 = NGUITools.FindInChild<UISprite>(tips, "sprite1");
            MergeButton.clickDelegate = MergeClick;
            GuideStoneItemContainer = itemList[0];
            //ClearItemData();
        }
        protected override void HandleAfterOpenView()
        {
            ClearItemData();
            Singleton<EquipLeftView>.Instance.ItemClickCallBack =
                this.LeftViewClick;
            EquipLeftView.Instance.NoEquipHandle = NoEquipHandle;
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
        }

        private void NoEquipHandle()
        {
            this.CloseView();
            Equip1View.Instance.RightTips.SetActive(true);
            Equip1View.Instance.TipsSprite.text = "请先穿戴装备";
        
        }
        public override void RegisterUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated += UpdateEquipHandle;
        }

        private void UpdateEquipHandle(object sender, int code)
        {
			if(code == GoodsMode.Instance.UPDATE_GOODS)
			{
				SelectSmelt();
			}
        }

        public override void CancelUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated -= UpdateEquipHandle;
        }
        private void ClearItemData()
        {
            this.uid = 0;
            ItemContainer ic;
            isInlaySmelt = false;
            mergeTips.text = string.Empty;
            idList.Clear();
            currenId = 0;
            HoleId = 0;
            currentItem = null;
            for (int i = 0; i < 5; i++)
            {
                ic = itemList[i];
                Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);

                ic.FindInChild<UISprite>("tips").SetActive(false);
                ic.SetHighLightState(false);
                ic.isEnabled = false;
                ic.FindInChild<UISlider>("sld_jy").value = 0;
            }
            mergeSlider.value = 0f;
            mergeSlider.FindInChild<UILabel>("label").text = "0/0";
            ClearSelectPanel();
        }

        //将选中宝石界面恢复至默认界面
        private void ClearSelectPanel()
        {
            foreach (ItemContainer container in smeltList)
            {
                container.SetActive(false);
            }
        }

        private bool isInlaySmelt = false;//装备是否已镶嵌宝石
        public void LeftViewClick(uint uid)
        {
            
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            PEquip equip = goods.equip[0];

            PGemInHole gem;
            SysItemVo itemVo;

            ClearItemData();
            mergeTips.text = VoUtils.GetMergeTips(vo.GetGemType());
            this.uid = uid;
            ItemContainer ic;
            for (int i = 0; i < 5; i++)
            {
                ic = itemList[i];
                Singleton<ItemManager>.Instance.InitItem(ic,0,0);
                if (i < equip.gemList.Count) //已经开启,而且已经镶嵌
                {
                    isInlaySmelt = true;
                    gem= equip.gemList[i];
                    Singleton<ItemManager>.Instance.InitItem(ic, gem.gemId, 0);
                    ic.isEnabled = true;
                    ic.buttonType = Button.ButtonType.HoldHighlight;
                    ic.Id = gem.gemId;
                    ic.FindInChild<UISprite>("tips").SetActive(false);
                    if (GoodsMode.Instance.IsTopSmelt(gem.gemId)) //顶级宝石
                        ic.FindInChild<UISlider>("sld_jy").value = 1f;
                    else
                    {
                        itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint) gem.gemId);
                        int nextEng = GoodsMode.Instance.GetSmeltNextExp((uint) itemVo.id);
                        int curEng = gem.energy - GoodsMode.Instance.GetSmeltInitExp(gem.gemId);
                        if (nextEng == 0)
                            ic.FindInChild<UISlider>("sld_jy").value = 0f;
                        else
                            ic.FindInChild<UISlider>("sld_jy").value = curEng/(float) nextEng;
                    }
                }
                else
                {
                    Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                    ic.FindInChild<UISprite>("tips").SetActive(false);
                    ic.FindInChild<UISlider>("sld_jy").value = 0f;
                    ic.isEnabled = false;
                    ic.Id = 0;
                }
            }
            if (!isInlaySmelt)  //没有镶嵌宝石 显示提示信息
            {
                bottomSprite.SetActive(false);
                if (grid.gameObject.activeInHierarchy)
                    grid.SetActive(false);
                if (!tips.gameObject.activeInHierarchy)
                    tips.SetActive(true);
                MergeButton.SetActive(false);
                sprite2.text = "装备还未镶嵌宝石";
                sprite2.gradientTop = new Color(112/225f,112/255f,112/255f);
                sprite2.gradientBottom = Color.white;
                sprite2.color = Color.white;
                sprite2.transform.localPosition = Vector3.zero;
                sprite1.SetActive(false);

            }
            else   //有镶嵌宝石
            {
                bottomSprite.SetActive(false);
                if (grid.gameObject.activeInHierarchy)
                    grid.SetActive(false);
                if (!tips.gameObject.activeInHierarchy)
                    tips.SetActive(true);
                MergeButton.SetActive(false);
                sprite2.text = "请选择已镶嵌宝石";
                sprite2.gradientTop = new Color(45/255f,109/255f,144/255f);
                sprite2.gradientBottom = Color.white;
                sprite2.color = new Color(167/255f,238/255f,254/255f);
                sprite2.transform.localPosition = Vector3.zero;
                sprite1.SetActive(false);
            }
            
        }

        public int HoleId; //当前宝石的空位置
        
        //装备镶嵌宝石点击事件
        private void SmeltBtnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            for (int i = 0; i < 5; i++)
            {
                if (itemList[i].Equals(ic))
                {
                    HoleId = i + 1;
                    break;
                }
            }
            if (ic.Id != 0) //已经镶嵌，可以充能
            {
                //SelectSmelt();
                if (currentItem && ic != currentItem)
                    currentItem.SetHighLightState(false);
                if (ic != currentItem)
                {
                    idList.Clear();
                    SelectSmelt();
                }
                currentItem = ic;
                //currenId = ic.Id;

                PGoods goods = GoodsMode.Instance.GetPGoodsById(uid);
                
                int index = HoleId - 1;
                PGemInHole gem = goods.equip[0].gemList[index];
                currenId = gem.gemId;
                
                this.energyFull = GoodsMode.Instance.GetSmeltNextExp(gem.gemId);
                if (GoodsMode.Instance.IsTopSmelt(gem.gemId))
                {
                    this.energy = this.energyFull ;
                }
                else
                    this.energy = gem.energy - GoodsMode.Instance.GetSmeltInitExp(gem.gemId);
                UpdateMergeSlider();
            }
            _guideIndex = 0;
            GuideMergeStoneItemContainer = smeltList[0];
        }
        //调整充灵进度条
        private void UpdateMergeSlider()
        {
            int energyMerge = energy;
            uint tempId = currenId;
            energyFull = GoodsMode.Instance.GetSmeltNextExp(tempId);
            uint lvl = tempId % 10;
            if (energyMerge >= GoodsMode.Instance.GetTopExp())  //判断经验值是否已经达到顶级
            {
                energyMerge = energyFull = GoodsMode.Instance.GetTopExp();
                lvl = 10;
            }
            else     //没有达到顶级，计算到达充灵的等级
            {
                while (energyMerge > energyFull)
                {
                    energyMerge -= energyFull;
                    tempId++;
                    lvl = tempId % 10;
                    energyFull = GoodsMode.Instance.GetSmeltNextExp(tempId);
                }
            }
            if (energyFull != 0)
            {
                mergeSlider.value = energyMerge / (float)energyFull;
                mergeSlider.FindInChild<UILabel>("label").text = "Lv: "+lvl +"     "+ energyMerge + "/" + energyFull;
            }
        }
        //初始化选择宝石列表
        private void SelectSmelt()
        {
            List<PGoods> goodsList = GoodsMode.Instance.GetSmeltGemListById(this.uid);
            int count = goodsList.Count;
            if (count == 0)   //没有宝石 显示对应的提示信息
            {
                if (grid.gameObject.activeInHierarchy)
                    grid.SetActive(false);
                if (!tips.gameObject.activeInHierarchy)
                    tips.SetActive(true);
                sprite2.text = "暂无可充灵宝石";
                sprite2.gradientTop = new Color(112/255f, 112/255f, 112/255f);
                sprite2.gradientBottom = Color.white;
                sprite2.color = Color.white;
                sprite2.transform.localPosition = new Vector3( 0 ,-69f,0);
                sprite1.SetActive(true);
                MergeButton.SetActive(false);
                bottomSprite.SetActive(false);
            }
            else
            {
                
                if (!grid.gameObject.activeInHierarchy)
                    grid.SetActive(true);
                if (tips.gameObject.activeInHierarchy)
                    tips.SetActive(false);
                MergeButton.SetActive(true);
                bottomSprite.SetActive(true); 
                ItemContainer temp;
                if (count % 5 != 0)   //始终多一空行
                {
                    count = count - count % 5 + 5;
                }
                if (count < 10)
                    count = 10;
                ItemContainer ic;
                while (smeltList.Count < count)
                {
                    ic = NGUITools.AddChild(grid.gameObject, item).AddMissingComponent<ItemContainer>();
                    ic.gameObject.name = string.Format("{0:D4}", smeltList.Count);
                    smeltList.Add(ic);
                    ic.onClick = SelectOnClick;
                }

                int length = goodsList.Count;
                PGoods goodsTemp;
                for (int i = 0; i < smeltList.Count; i++)
                {
                    temp = smeltList[i];

                    if (i > count - 1)
                    {
                        temp.SetActive(false);
                        temp.Id = 0;
                    }
                    else if (i < length)
                    {
                        goodsTemp = goodsList[i];
                        temp.SetActive(true);
                        temp.buttonType = Button.ButtonType.Toggle;
                        temp.SetHighLightState(false);
                        temp.FindInChild<UISprite>("tips").SetActive(false);
                        Singleton<ItemManager>.Instance.InitItem(temp, goodsTemp.goodsId, 0);
                        temp.Id = goodsTemp.id;
                        if (GoodsMode.Instance.IsTopSmelt(goodsTemp.goodsId)) //顶级宝石
                            temp.FindInChild<UISlider>("sld_jy").value = 1f;
                        else
                        {
                            int nextEng = GoodsMode.Instance.GetSmeltNextExp(goodsTemp.goodsId);
                            int curEng = GoodsMode.Instance.GetSmeltMerge(goodsTemp.id);
                            if (nextEng == 0)
                                temp.FindInChild<UISlider>("sld_jy").value = 0f;
                            else
                                temp.FindInChild<UISlider>("sld_jy").value = curEng/(float) nextEng;
                        }

                    }
                    else
                    {
                        temp.SetActive(true);
                        temp.FindInChild<UISprite>("tips").SetActive(false);
                        temp.FindInChild<UISlider>("sld_jy").value = 0f;
                        Singleton<ItemManager>.Instance.InitItem(temp, 1, 0);
                        temp.buttonType = Button.ButtonType.None;
                        temp.SetHighLightState(false);
                        temp.Id = 0;
                    }
                }
                grid.Reposition();

            }
        }
        
        //点击充灵宝石响应
        private void SelectOnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (currenId == 0)
            {
                MessageManager.Show("请选择充灵宝石");
            }
            //已经充到顶级
            else if (GoodsMode.Instance.IsTopSmelt(currenId))
            {
                MessageManager.Show("已经充到顶级");
            }
            else if (ic.Id != 0)
            {
                if (idList.Contains(ic.Id))
                {
                    idList.Remove(ic.Id);
                    energy -= GoodsMode.Instance.GetSmeltExp(ic.Id);
                    ic.FindInChild<UISprite>("tips").SetActive(false);
                }
                else
                {
                    idList.Add(ic.Id);
                    energy += GoodsMode.Instance.GetSmeltExp(ic.Id);
                    ic.FindInChild<UISprite>("tips").SetActive(true);
                }
                UpdateMergeSlider();
                
            }
            if (_guideIndex < 2)
            {
                _guideIndex++;
                GuideMergeStoneItemContainer = smeltList[_guideIndex];
            }
        }
        private void MergeClick()
        {
            if(idList.Count != 0)
                Singleton<Equip1Control>.Instance.EquipMerge(this.uid, GoodsMode.Instance.EQUIP_REPOS, HoleId,idList);
            else
            {
                MessageManager.Show("请选择充灵宝石");
            }
        }
    }

}

