using com.game.consts;
using com.game.module.EquipIdentification;
using UnityEngine;
using com.game.module.test;
using System.Collections.Generic;
using PCustomDataType;
using com.game.data;
using com.game.manager;
using com.u3d.bases.debug;
using Com.Game.Module.Tips;
using com.game.vo;
using Com.Game.Module.Manager;
using Com.Game.Module.Equip;


namespace Com.Game.Module.Role
{
	public delegate void EquipViewHandle(GameObject equipView);
    public delegate void ResetPositionDelegate();
    public class GoodsView : BaseView<GoodsView>
    {

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

        public UIToggle ckb_equip;//装备
        private UIToggle ckb_smelt;//宝石
        private UIToggle ckb_pet;//宠物
        private UIToggle ckb_prop;//道具

        private Button btn_tujian;//图鉴按钮

        private UIGrid grid;//背包grid
        private UIScrollView scrollView;//

		private UILabel zhanliLabel;
		private UILabel zhanLi;

        private List<ItemContainer> goodsList = new List<ItemContainer>();  //右边背包
        private GameObject item;
        public PGoods currentGoods;
        private List<ItemContainer> equipList = new List<ItemContainer>();//左边面板装备

		public byte repos{get;set;}
        private GoodsMode.GoodsType showType;// 背包显示类型
        public ResetPositionDelegate ResetPositionDelegate;

		protected override void  Init()
        {
            ckb_equip = FindInChild<UIToggle>("ckb_equip");
		    ckb_equip.FindInChild<UILabel>("label").text = LanguageManager.GetWord("GoodsView.EquipTab");
            ckb_smelt = FindInChild<UIToggle>("ckb_smelt");
            ckb_smelt.FindInChild<UILabel>("label").text = LanguageManager.GetWord("GoodsView.SmeltTab");
            ckb_pet = FindInChild<UIToggle>("ckb_pet");
            ckb_pet.FindInChild<UILabel>("label").text = LanguageManager.GetWord("GoodsView.PetTab");
            ckb_prop = FindInChild<UIToggle>("ckb_prop");
            ckb_prop.FindInChild<UILabel>("label").text = LanguageManager.GetWord("GoodsView.PropTab");

		    btn_tujian = FindInChild<Button>("btn_tujian");

            EventDelegate.Add(ckb_equip.onChange, GoodsTypeHandle);
            EventDelegate.Add(ckb_smelt.onChange, GoodsTypeHandle);
            EventDelegate.Add(ckb_pet.onChange, GoodsTypeHandle);
            EventDelegate.Add(ckb_prop.onChange, GoodsTypeHandle);
		    btn_tujian.onPress = TuJianOnPress;
		    btn_tujian.clickDelegate = TuJianClick;

            grid = FindInChild<UIGrid>("content/grid");
		    scrollView = FindInChild<UIScrollView>("content");
            grid.onReposition += scrollView.ResetPosition;
            item = FindChild("content/grid/item1");
		    item.name = "0000";
            ItemContainer ic = item.AddMissingComponent<ItemContainer>();

            ic.FindInChild<UILabel>("num").text = string.Empty;
            ic.onClick = ShowRightTips;
            ic.onDoubleClick = RightDoubleClick;
            goodsList.Add(ic);
            item.SetActive(false);
			zhanLi = FindInChild<UILabel>("zl");
            
            GetLeftItemList();
		     

        }
		
		private void InitDefalutEquipItem()
		{
		    int index = 1;
            foreach (ItemContainer temp in equipList)
			{
				ItemManager.Instance.InitItem(temp,ItemManager.Instance.EMPTY_ICON,ItemType.Equip);
                temp.FindInChild<UILabel>("pos").text = LanguageManager.GetWord("Equip.Pos" + (index));
			    temp.FindInChild<UILabel>("stren").text = string.Empty;
			    temp.FindInChild<UISprite>("background").alpha = 0.3f;
			    temp.FindInChild<UISprite>("icon").alpha = 0.3f;
			    temp.isEnabled = false;
                temp.Id = 0;
			    index ++;
			}
		}
		private void GetLeftItemList()
		{
			ItemContainer temp ;
			for (int i = 1; i < 11; i++)
			{
				temp = FindChild("left/item"+i).AddMissingComponent<ItemContainer>();
                temp.onClick = ShowLeftTips;
			    temp.onDoubleClick = LeftDoubleClick;
                equipList.Add(temp);
			}
			InitDefalutEquipItem();
			
		}
		public override void RegisterUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated += UpdateGoodsHandler;
		    RoleMode.Instance.dataUpdated += UpdateFightPoint;
		}
		public override void CancelUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated -= UpdateGoodsHandler;
            RoleMode.Instance.dataUpdated -= UpdateFightPoint;
		}

        protected override void HandleAfterOpenView()
        {
            UpdateGoodsInfo();
            UpdateEquipInfo();
            ckb_prop.value = true;
            GoodsControl.Instance.RequestWrapInfo(1);
            GoodsControl.Instance.RequestWrapInfo(2);
            zhanLi.text = LanguageManager.GetWord("Goods.FightPoint") + ": " + MeVo.instance.fightPoint;
            if (GoodsMode.Instance.ShowTips && showType != GoodsMode.GoodsType.Equip)
                ckb_equip.FindInChild<UISprite>("tips").SetActive(true);
            else
            {
                ckb_equip.FindInChild<UISprite>("tips").SetActive(false);
            }
        }

        private void UpdateFightPoint(object sender,int code)
        {
            if (code == RoleMode.Instance.UPDATE_ROLE_ATTR)
                zhanLi.text = LanguageManager.GetWord("Goods.FightPoint") + ": " + MeVo.instance.fightPoint;
        }
		private void UpdateGoodsHandler(object sender,int code)
		{
			if(code == Singleton<GoodsMode>.Instance.UPDATE_PET_GOODS && showType == GoodsMode.GoodsType.Pet)
				UpdateGoodsInfo();
            else if (code == Singleton<GoodsMode>.Instance.UPDATE_PROP_GOODS && showType == GoodsMode.GoodsType.Other)
                UpdateGoodsInfo();
            else if (code == Singleton<GoodsMode>.Instance.UPDATE_SMELT_GOODS && showType == GoodsMode.GoodsType.Smelt)
                UpdateGoodsInfo();
            else if (code == Singleton<GoodsMode>.Instance.UPDATE_ROLE_EQUIP && showType == GoodsMode.GoodsType.Equip)
                UpdateGoodsInfo();
			else if (code == Singleton<GoodsMode>.Instance.UPDATE_EQUIP)
			{
			    UpdateEquipInfo();
                if (showType  == GoodsMode.GoodsType.Equip)  //更新装备 可装备提示
                    UpdateGoodsInfo();
			}
            else if (code == GoodsMode.Instance.UPDATE_TIPS)
            {
                if (GoodsMode.Instance.ShowTips && showType != GoodsMode.GoodsType.Equip)
                    ckb_equip.FindInChild<UISprite>("tips").SetActive(true);
                else
                {
                    ckb_equip.FindInChild<UISprite>("tips").SetActive(false);
                }
            }
		}
        
        //更新物品
        private void UpdateGoodsInfo()
        {
            List<PGoods> goodsArray = Singleton<GoodsMode>.Instance.GetPGoodsByType(showType);
            if(showType == GoodsMode.GoodsType.Equip)
                Log.info(this,"装备数量： " + goodsArray.Count);
            if (GoodsMode.Instance.ShowTips && showType != GoodsMode.GoodsType.Equip)
                ckb_equip.FindInChild<UISprite>("tips").SetActive(true);
            else
            {
                ckb_equip.FindInChild<UISprite>("tips").SetActive(false);
            }
            int count = goodsArray.Count;
            if (count % 4 != 0)   //始终多一空行
            {
                count = count - count % 4 + 8;
            }
            if (count < 24)
                count = 24;
            GameObject temp;
            ItemContainer ic;
            PGoods goods;
            while (goodsList.Count < count)
            {
                temp = NGUITools.AddChild(grid.gameObject, item);
                temp.name = string.Format("{0:D4}", goodsList.Count);
                ic = temp.AddMissingComponent<ItemContainer>();
                ic.onClick = ShowRightTips;
                ic.onDoubleClick = RightDoubleClick;
                goodsList.Add(ic);
            }
            
            for (int i = 0, length = goodsList.Count;i<length; i++)
            {
                ic = goodsList[i];
                if (i >= count)
                {
                    ic.SetActive(false);
                }
                else if (i >= goodsArray.Count)
                {
                    ic.SetActive(true);
                    ItemManager.Instance.InitItem(ic,ItemManager.Instance.EMPTY_ICON, 0);
                    ic.buttonType = Button.ButtonType.None;
                    ic.FindInChild<UILabel>("num").text = string.Empty;
                    ic.FindInChild<UILabel>("stren").text = string.Empty;
                    ic.FindInChild<UISprite>("uptips").SetActive(false);
                    ic.Id = 0;
                }
                else 
                {
                    goods = goodsArray[i];
                    ic.SetActive(true);
                    if (goods.count > 1)   //只有数量大于 0 时才显示
                        ic.FindInChild<UILabel>("num").text = string.Empty + goods.count;
                    else
                        ic.FindInChild<UILabel>("num").text = string.Empty;

                    if (goods.equip.Count > 0) //装备
                    {
                        ic.FindInChild<UILabel>("stren").text = "+" + goods.equip[0].stren;
                        ic.FindInChild<UISprite>("uptips").SetActive(GoodsMode.Instance.IsShowEquipTips(goods.id));
                    }
                    else
                    {
                        ic.FindInChild<UILabel>("stren").text = string.Empty;
                        ic.FindInChild<UISprite>("uptips").SetActive(false);
                    }
                    ItemManager.Instance.InitItem(ic, goods.goodsId, 0);
                    ic.buttonType = Button.ButtonType.Toggle;
                    ic.Id = goods.id;
                }
            }
            grid.Reposition();
        }

        public ItemContainer GetWeaponItem()
        {
            int job = MeVo.instance.job;
            ItemContainer result=null;
            foreach (var good in goodsList)
            {
                if (!good.IsEquip)
                {
                    continue;
                }
                var equipVo = good.EquipVo;
                if (equipVo.type == 0 && (equipVo.job == job||equipVo.job==0))
                {
                    result = good;
                    break;
                }
            }
            return result;
        }

        public void ResetGird()
        {
            if (gameObject.activeInHierarchy)
            {
                grid.Reposition();
                if (ResetPositionDelegate != null)
                {
                    ResetPositionDelegate();
                }
            }
        }

        private void UpdateEquipInfo()
        {
			List<PGoods> equipInfoList = Singleton<GoodsMode>.Instance.equipList;
            InitDefalutEquipItem();
            SysEquipVo vo;
            ItemContainer ic;
            foreach (PGoods goods in equipInfoList)
            {
                vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                ic = equipList[vo.pos - 1];
                ic.Id = goods.id;
                ic.FindInChild<UILabel>("pos").text = string.Empty;
                Singleton<ItemManager>.Instance.InitItem(ic, goods.goodsId, ItemType.Equip);
                ic.FindInChild<UILabel>("stren").text = "+" + goods.equip[0].stren;
                ic.FindInChild<UISprite>("background").alpha = 1f;
                ic.FindInChild<UISprite>("icon").alpha = 1f;
                ic.isEnabled = true;
                ic.buttonType = Button.ButtonType.Toggle;

            }
        }

        private void GoodsTypeHandle()
        {
            UIToggle current = UIToggle.current;
            if (current.value)
            {
                current.FindInChild<UILabel>("label").color = Color.white;
                if (current.Equals(ckb_equip))
                {
                    showType = GoodsMode.GoodsType.Equip;
                }
                else if (current.Equals(ckb_prop))
                {
                    showType = GoodsMode.GoodsType.Other;
                }
                else if (current.Equals(ckb_pet))
                {
                    showType = GoodsMode.GoodsType.Pet;
                }
                else if (current.Equals(ckb_smelt))
                {
                    showType = GoodsMode.GoodsType.Smelt;
                }
                UpdateGoodsInfo();
                
            }
            else
            {
                current.FindInChild<UILabel>("label").color = ColorConst.FONT_GRAY;
            }
        }

        private void TuJianOnPress(GameObject go, bool state)
        {
            if (state)
                btn_tujian.label.color = Color.white;
            else
            {
                btn_tujian.label.color = ColorConst.FONT_GRAY;
            }
        }

        //图鉴按钮点击响应
        private void TuJianClick()
        {
            EquipIdentificationView.Instance.OpenView();
        }
        //单击左边装备，显示 Tips 界面
		private void ShowLeftTips(GameObject go)
		{
            ItemContainer ic = go.GetComponent<ItemContainer>();
		    if (ic.Id != 0)
		    {
                currentGoods = GoodsMode.Instance.GetPGoodsById(ic.Id);
                repos = 2;
                vp_Timer.In(0.26f, delegate()
                {
                    Singleton<TipsManager>.Instance.OpenTipsById(currentGoods.id, ForgeEquipment, UnWearEquipment, LanguageManager.GetWord("Goods.Forge"),
                                                           LanguageManager.GetWord("Goods.TakeOff"), TipsType.DEFAULT_TYPE);
					
                }, 1, leftClickHandle);
                
		    }
			

        }

        
        //双击装备栏
        private void LeftDoubleClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Id != 0)
            {
                if (leftClickHandle != null)
                {
                    leftClickHandle.Cancel();//取消单击事件
                }
                //EquipTips.Instance.CloseView();
                //TipsManager.Instance.CloseTipsById(currentGoods.id);
                UnWearEquipment();
            }
        }
        //双击物品栏
        private void RightDoubleClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Id != 0)
            {
                if (GoodsMode.Instance.GetPGoodsById(ic.Id).GetGoodsType() == GoodsMode.GoodsType.Equip)
                {
                    if (rightClickHandle != null)
                    {
                        rightClickHandle.Cancel();//取消单击事件
                    }
                    WearEquipment();
                }
                
            }
        }

        private vp_Timer.Handle rightClickHandle = new vp_Timer.Handle(), leftClickHandle = new vp_Timer.Handle();
        //单击右边背包物品，显示 Tips 界面
		private void ShowRightTips(GameObject go)
        {
            ItemContainer ic  = go.GetComponent<ItemContainer>();
		    if (ic.Id != 0)
		    {
		        UIWidgetContainer.ClickDelegate leftDelegate = null, rightDelegate = null;
		        string leftText = null, rightText = null;
                currentGoods = GoodsMode.Instance.GetPGoodsById(ic.Id);
                if (currentGoods.goodsId > GoodsMode.Instance.GoodsId)
                {
                    SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(currentGoods.goodsId);
                    if (vo.can_use == true) //可使用
                    {
                        leftText = LanguageManager.GetWord("Goods.QuickUse");
                        rightText = LanguageManager.GetWord("Goods.Use");
                        leftDelegate = QuickUseOnClick;
                        rightDelegate = UseGoodsOnClick;
                    }
                    else
                    {
                    }
                }
                else    //装备
                {
                    repos = 1;
                    SysEquipVo equip = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentGoods.goodsId);
                    if (equip.type == GoodsConst.ROLE_EQUIP)
                    {
                        leftText = LanguageManager.GetWord("Goods.Forge");
                        rightText = LanguageManager.GetWord("Goods.PutOn");
                        leftDelegate = ForgeEquipment1;
                        rightDelegate = WearEquipment;
                    }
                    else if (equip.type == GoodsConst.PET_EQUIP)
                    {
                    }
                }
                vp_Timer.In(0.26f, delegate()
                {
                    Singleton<TipsManager>.Instance.OpenTipsById(currentGoods.id,
                        leftDelegate, rightDelegate, leftText, rightText,
                        TipsType.DEFAULT_TYPE);
                },1,rightClickHandle);
		    }
            
        }
        //一键使用
        private void QuickUseOnClick()
        {
            GoodsControl.Instance.UseGoodsMany(currentGoods.id,currentGoods.count);
        }
		//使用物品
        private void UseGoodsOnClick()
        {
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(currentGoods.goodsId);
            GoodsControl.Instance.UseGoods(currentGoods.id, vo.name);
        }

        //丢弃物品
        private void DiscardGoodsOnClick()
        {
            GoodsControl.Instance.DestroyGoods(currentGoods.id, 1);
        }
		//锻造装备
		private void ForgeEquipment( )
		{
		    EquipLeftView.Instance.Repos = GoodsMode.Instance.EQUIP_REPOS;
		    EquipLeftView.Instance.IsOpenByBag = true;
		    EquipLeftView.Instance.Id = currentGoods.id;
            Equip1View.Instance.OpenStrenView();
		}

        //物品栏切换到锻造装备
        private void ForgeEquipment1()
        {
            EquipLeftView.Instance.Repos = GoodsMode.Instance.GOODS_REPOS;
            EquipLeftView.Instance.IsOpenByBag = true;
            EquipLeftView.Instance.Id = currentGoods.id;

            Equip1View.Instance.OpenDestroyView();
            Singleton<EquipTips>.Instance.CloseView();
        }
        //装备装备
        private void WearEquipment()
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentGoods.goodsId);
            if (vo == null)
                Log.info(this, "xinx null................物品Id ："+ currentGoods.goodsId);
            GoodsControl.Instance.WearEquipment(currentGoods.id, (byte)vo.pos);
			Singleton<EquipTips>.Instance.CloseView();
        }

        //卸下装备 
        private void UnWearEquipment()
        {
            GoodsControl.Instance.UnWearEquipment(currentGoods.id, (byte)0);
			Singleton<EquipTips>.Instance.CloseView();
        }
        
    }
}

