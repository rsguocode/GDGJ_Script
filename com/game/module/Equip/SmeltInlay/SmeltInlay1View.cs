using System;
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
    public class SmeltInlay1View : BaseView<SmeltInlay1View>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }
        private GameObject tips;//提示
        private List<ItemContainer> itemList = new List<ItemContainer>();

        private List<ItemContainer> smeltList = new List<ItemContainer>();  //宝石

        private GameObject item;
        private UIGrid grid;
        private UIScrollView scrollView;
        private uint uid = 0;//装备唯一Id;

        private UILabel inlayTips;//镶嵌提示
        public ItemContainer GuidetStoneItem;

        //private TweenPosition gridTween;

        protected  override void Init()
        {
            inlayTips = FindInChild<UILabel>("desc");
            tips = FindChild("tips");
            GameObject temp;
            ItemContainer ic;
            for (int i = 1; i < 6; i++)
            {
                temp = FindChild("item" + i);
                ic = temp.AddMissingComponent<ItemContainer>();
                ic.onClick = SmeltBtnClick;
				itemList.Add(ic);
            }
            item = FindChild("content/grid/item1");
            item.name = "0000";
            item.SetActive(false);
            ic = item.AddMissingComponent<ItemContainer>();
            ic.onClick = SelectOnClick;
            smeltList.Add(ic);
            grid = FindInChild<UIGrid>("content/grid");
            grid.sorted = true;
            scrollView = FindInChild<UIScrollView>("content");
            temp = null;
        }
        protected override void HandleAfterOpenView()
        {
            ClearItemData();
            EquipLeftView.Instance.NoEquipHandle = NoEquipHandle;
            Singleton<EquipLeftView>.Instance.ItemClickCallBack = LeftViewClick;
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
            if (code == GoodsMode.Instance.UPDATE_GOODS)   //更新物品栏
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
            for (int i = 0; i < 5; i++)
            {
                ic = itemList[i];
                Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                ic.FindInChild<UISprite>("tips").SetActive(false);
                ic.FindInChild<UISlider>("sld_jy").value = 0;
                ic.isEnabled = false;
            }
            ClearSelectPanel();
            inlayTips.text = string.Empty;
        }
        public void LeftViewClick(uint uid)
        {
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            PEquip equip = goods.equip[0];
            ClearItemData();
            inlayTips.text = VoUtils.GetInlayTips(vo.GetGemType());
            this.uid = uid;
            ItemContainer ic;
            PGemInHole gem;
            for (int i = 0; i < 5; i++)  //默认全部开启
            {
                ic = itemList[i];

                if (i < equip.gemList.Count) //已经开启,而且已经镶嵌
                {
                    gem = equip.gemList[i];
                    ic.Id = gem.gemId;
                    Singleton<ItemManager>.Instance.InitItem(ic, gem.gemId, 0);
                    ic.FindInChild<UISprite>("tips").SetActive(true);
                    if (GoodsMode.Instance.IsTopSmelt(gem.gemId))
                        ic.FindInChild<UISlider>("sld_jy").value = 1f;
                    else
                    {
                        int nextEng = GoodsMode.Instance.GetSmeltNextExp((uint) gem.gemId);
                        int curEng = gem.energy - GoodsMode.Instance.GetSmeltInitExp(gem.gemId);
                        if (nextEng == 0)
                            ic.FindInChild<UISlider>("sld_jy").value = 0f;
                        else
                            ic.FindInChild<UISlider>("sld_jy").value = curEng/(float) nextEng;
                    }
                    ic.isEnabled = true;
                }
                else
                {
                    Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                    ic.FindInChild<UISprite>("tips").SetActive(false);
                    ic.FindInChild<UISlider>("sld_jy").value = 0f;
                    ic.Id = 0;
                    ic.isEnabled = false;
                }
            }
            SelectSmelt();
        }

        //上面一排 镶嵌宝石的Item 
        private void SmeltBtnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Id == 0)
            {
                
            }
            else  //移除
            {
                int pos = 1;
                foreach (ItemContainer container in itemList)
                {
                    if (container.Equals(ic))
                    {
                        break;
                    }
                    else
                        pos ++;
                }
                Singleton<Equip1Control>.Instance.EquipRemove(uid,EquipLeftView.Instance.Repos,pos);
            }
        }
        //将选中宝石界面恢复至默认界面
        private void ClearSelectPanel()
        {
            foreach (ItemContainer container in smeltList)
            {
                container.SetActive(false);
            }
        }

        private int smeltType = 0;//可充能的宝石类型
        //选择宝石
        private void SelectSmelt()
        {
            //gridTween.PlayForward();
            List<PGoods> goodsList = GoodsMode.Instance.GetSmeltGemListById(this.uid);
            //排序按从大到小
            goodsList.Sort(delegate(PGoods left,PGoods right) { return -left.goodsId.CompareTo(right.goodsId); });
            int count = goodsList.Count;
            if (count == 0)
            {
                //MessageManager.Show("没有可充能的宝石!");
                if (grid.gameObject.activeInHierarchy)
                    grid.SetActive(false);
                if (!tips.gameObject.activeInHierarchy)
                    tips.SetActive(true);

            }
            else
            {
                if (!grid.gameObject.activeInHierarchy)
                    grid.SetActive(true);
                if (tips.gameObject.activeInHierarchy)
                    tips.SetActive(false);
                ItemContainer temp;
                PGoods goodsTemp;
                ItemContainer ic;
                if (count % 5 != 0)   //始终多一空行
                {
                    count = count - count % 5 +5 ;
                }
                if (count < 15)
                    count = 15;
                while (smeltList.Count < count)
                {
                    ic = NGUITools.AddChild(grid.gameObject, item).AddMissingComponent<ItemContainer>();
                    ic.gameObject.name = string.Format("{0:D4}", smeltList.Count);
                    smeltList.Add(ic);
                    ic.onClick = SelectOnClick;
                }

                int length = goodsList.Count;
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
                        Singleton<ItemManager>.Instance.InitItem(temp, goodsTemp.goodsId, 0);
                        temp.Id = goodsTemp.id;
                        if (GoodsMode.Instance.IsTopSmelt(goodsTemp.goodsId)) //顶级宝石
                            temp.FindInChild<UISlider>("sld_jy").value = 1f;
                        else
                        {
                            //SysItemVo itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)goodsTemp.goodsId);
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
                        temp.FindInChild<UISlider>("sld_jy").value = 0f;
                        Singleton<ItemManager>.Instance.InitItem(temp, 1, 0);
                        temp.buttonType = Button.ButtonType.None;
                        temp.SetHighLightState(false);
                        temp.Id = 0;
                    }
                }
                grid.Reposition();
                scrollView.ResetPosition();
                GuidetStoneItem = smeltList[0];
            }
        }
        //选择宝石点击按钮
        private void SelectOnClick(GameObject go)
        {
            //判断是否镶嵌满了
            bool isFull = true;
            foreach (ItemContainer container in itemList)
            {
                if (container.Id == 0)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                //MessageManager.Show("已经镶嵌满");
                //return;
            }
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Id != 0)
            {
                Singleton<Equip1Control>.Instance.EquipInlay(uid, GoodsMode.Instance.EQUIP_REPOS, ic.Id);
            }
        }

    }
   
}

