using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class EquipInheritView : BaseView<EquipInheritView>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }
        private ItemContainer currentEquip;//当前装备
        private ItemContainer nextEquip;//下一个精炼装备
        private List<UILabel>  currentSuitList = new List<UILabel>();//附加属性

        private UILabel curentName;//当前装备名字
        private UILabel nextName;//下一级装备名字
        private List<UILabel> nextSuitList = new List<UILabel>(); //附加属性

        private UILabel currentAttr;//当前精炼等级
        private UILabel nextAttr;//

        private UILabel costValue;//花费金币
        private UILabel costLabel;
        private UILabel leftLabel;
        private UILabel leftValue;

        private UILabel currentTips;//
        private UILabel nextTips;//

        //private UILabel equipName;//装备名字
        private UILabel tips;//提示

		private Button inheritButton;

        private bool currectSelect = false;
        private bool nextSelect = false;

        private uint currentId;//当前装备唯一Id
        private uint nextId;//继承装备Id
        private int pos;//装备部位
        protected override void Init()
        {
            currentEquip = FindChild("1").AddMissingComponent<ItemContainer>();
            nextEquip = FindChild("2").AddMissingComponent<ItemContainer>();

            for (int i = 1; i < 6; i++)
            {
                currentSuitList.Add(currentEquip.FindInChild<UILabel>("suit" + i));
                nextSuitList.Add(nextEquip.FindInChild<UILabel>("suit" + i));
            }

            currentEquip.onClick = ClearSelectOnClick;
            nextEquip.onClick = ClearSelectOnClick;

            costValue = FindInChild<UILabel>("costvalue");
            costLabel = FindInChild<UILabel>("costlabel");
            leftLabel = FindInChild<UILabel>("leftlabel");
            leftValue = FindInChild<UILabel>("leftvalue");

            tips = FindInChild<UILabel>("tips");
			inheritButton = FindInChild<Button>("btn_qh");
			inheritButton.clickDelegate = InheritClick;


        }

        protected override void HandleAfterOpenView()
        {
            InitItemData();
            Singleton<EquipLeftView>.Instance.ItemClickCallBack = LeftViewClick;
            EquipLeftView.Instance.NoEquipHandle = null;
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
        }

        protected override void HandleBeforeCloseView()
        {
            //EquipLeftView.Instance.Id = 0;
        }
        public override void RegisterUpdateHandler()
        {
            Equip1Mode.Instance.dataUpdated += GoodsUpdateHandle;
            RoleMode.Instance.dataUpdated += UpdateMoneyHandle;
        }
        private void UpdateMoneyHandle(object sender, int code)
        {
            if (code == RoleMode.Instance.UPDATE_FORTUNE)
                UpdateMoney();
        }

        private void UpdateMoney()
        {
			leftValue.text = MeVo.instance.DiamStr;
        }
        private void GoodsUpdateHandle(object sender, int code)
        {
            if ( code == Equip1Mode.Instance.UPDATE_EQUIP_INHERIT)
            {
                InitItemData();
            }

        }
        public override void CancelUpdateHandler()
        {
            Equip1Mode.Instance.dataUpdated -= GoodsUpdateHandle;
            RoleMode.Instance.dataUpdated -= UpdateMoneyHandle;
        }
        

        private void InitItemData()
        {
            ClearNextItem();
            ClearCurrentItem();
            costValue.text = "0";
            UpdateMoney();
            
        }

        private int currentStren = 0;
        private int currentRefine = 0;
        public void LeftViewClick(uint uid)
        {
            
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            if (EquipLeftView.Instance.Repos == GoodsMode.Instance.EQUIP_REPOS)
            {
                currentEquip.Id = uid;
                Singleton<ItemManager>.Instance.InitItem(currentEquip,goods.goodsId,0);
                this.pos = goods.pos;
                currentRefine = goods.equip[0].refine;
                currentStren = goods.equip[0].stren;
                List<PSuitAttr> suitAttr = goods.equip[0].suitList;
                string suitStr;
                UILabel label;
                for (int i = 0; i < 5; i++)
                {
                    label = currentSuitList[i];
                    if (i >= suitAttr.Count)
                    {
                        label.SetActive(false);
                    }
                    else
                    {
                        suitStr = VoUtils.RoleAttrFormat1(suitAttr[i].id, string.Empty + suitAttr[i].attr);
                        label.text = suitStr;
                        label.SetActive(true);
                    }
                }
                currentEquip.FindInChild<UILabel>("stren").text = "强化等级：+" + currentStren;
                currentEquip.FindInChild<UISprite>("tips").SetActive(true);
                currectSelect = true;
                label = currentEquip.FindInChild<UILabel>("name");//.text = vo.name;
                label.text = vo.name;
                ColorUtils.SetEqNameColor(label, vo.color);
                currentEquip.FindInChild<UILabel>("refine").text = "精炼等级：" + currentRefine;
                currentEquip.FindInChild<UILabel>("title").text = string.Empty;
                currentId = uid;
                costValue.text = string.Empty + Singleton<GoodsMode>.Instance.GetInheritCost(goods);
                //if(!nextSelect )
                    EquipLeftView.Instance.ReposChange(GoodsMode.Instance.GOODS_REPOS,this.pos);
                currentEquip.isEnabled = true;
            }
            else if (EquipLeftView.Instance.Repos == GoodsMode.Instance.GOODS_REPOS)
            {
                nextEquip.Id = uid;
                Singleton<ItemManager>.Instance.InitItem(nextEquip, goods.goodsId, 0);
                int tempStren = goods.equip[0].stren;
                int tempRefine = goods.equip[0].refine;
                List<PSuitAttr> suitAttr = goods.equip[0].suitList;
                string suitStr;
                UILabel label;
                for (int i = 0; i < 5; i++)
                {
                    label = nextSuitList[i];
                    if (i >= suitAttr.Count)
                    {
                        label.SetActive(false);
                    }
                    else
                    {
                        suitStr = VoUtils.RoleAttrFormat1(suitAttr[i].id, string.Empty + suitAttr[i].attr);
                        label.text = suitStr;
                        label.SetActive(true);
                    }
                }
                if (tempStren < currentStren)
                    tempStren = currentStren;
                if (tempRefine < currentRefine)
                    tempRefine = currentRefine;
                nextEquip.FindInChild<UILabel>("stren").text = "强化等级：+" + tempStren;
                nextEquip.FindInChild<UISprite>("tips").SetActive(true);
                nextSelect = true;
                label = nextEquip.FindInChild<UILabel>("name");
                label.text = vo.name;
                ColorUtils.SetEqNameColor(label,vo.color);
                nextEquip.FindInChild<UILabel>("refine").text = "精炼等级：" + tempRefine;
                nextEquip.FindInChild<UILabel>("title").text = string.Empty;
                nextId = uid;
                nextEquip.isEnabled = true;
            }
        }

        private void ClearCurrentItem()
        {
            currentEquip.Id = 0;
            Singleton<ItemManager>.Instance.InitItem(currentEquip, ItemManager.Instance.EMPTY_ICON, 0);
            currentEquip.FindInChild<UILabel>("stren").text = string.Empty; ;
            currentEquip.FindInChild<UISprite>("tips").SetActive(false);
            currectSelect = false;
            currentEquip.FindInChild<UILabel>("name").text = string.Empty;
            currentEquip.FindInChild<UILabel>("refine").text = string.Empty;
            currentEquip.FindInChild<UILabel>("title").text = "请选择\n已使用装备";
            this.pos = 0;
            this.currentStren = 0;
            this.currentRefine = 0;
            costValue.text = "0";
            foreach (UILabel label in currentSuitList)
            {
                label.SetActive(false);
            }
            ClearNextItem();
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
            currentEquip.isEnabled = false;

        }

        private void ClearNextItem()
        {
            nextEquip.Id = 0;
            Singleton<ItemManager>.Instance.InitItem(nextEquip, ItemManager.Instance.EMPTY_ICON, 0);
            nextEquip.FindInChild<UILabel>("stren").text = string.Empty; ;
            nextEquip.FindInChild<UISprite>("tips").SetActive(false);
            nextSelect = false;
            nextEquip.FindInChild<UILabel>("name").text = string.Empty; ;
            nextEquip.FindInChild<UILabel>("refine").text = string.Empty; 
            nextEquip.FindInChild<UILabel>("title").text = "请选择\n背包装备";
            nextEquip.isEnabled = false;
            foreach (UILabel label in nextSuitList)
            {
                label.SetActive(false);
            }
            if(this.pos != 0)
                EquipLeftView.Instance.ReposChange(GoodsMode.Instance.GOODS_REPOS,this.pos);
            
        }
        //取消选择
        private void ClearSelectOnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Equals(currentEquip))
            {
                if (ic.Id != 0)
                {
                    ClearCurrentItem();
                }
            }
            else if (ic.Equals(nextEquip))
            {
                if (ic.Id != 0)
                {
                    ClearNextItem();
                }
            }
        }

        private void InheritClick()
        {
            if(currectSelect && nextSelect)
                Singleton<Equip1Control>.Instance.EquipInherit(currentId,nextId);
            else
            {
                MessageManager.Show("请选择装备");
            }
        }
    }

}
