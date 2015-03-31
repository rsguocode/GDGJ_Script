//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoreShopView
//文件描述：商城界面
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.utils;
using com.game.vo;

namespace com.game.module.Store
{
	public class StoreShopView : BaseView<StoreShopView>
	{
		public override string url { get { return "UI/StoreShop/StoreView.assetbundle" ; } } 
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer ; } }
		public override bool IsFullUI { get { return true ; } } 

		private Button btn_close;
		private List<Transform> limitItemList;

		private UIToggle btn_bestSell;
		private UIToggle btn_diamond;
		private UIToggle btn_bindingDiamond;
		private UIToggle btn_gold;

		private Transform leftItemContainer;

		//右侧物品
		private List<Transform> commonGoodsList;
		private UIScrollView commonGoodsListScrollView;
		private UIGrid commonGoodsGrid;

		private UILabel bDiamondValueLabel;
		private UILabel diamondValueLabel;
		private UILabel goldValueLabel;
		private UILabel limitTimeLabel;
		private UILabel timeLabel;
		private UILabel timeValueLabel;

		private bool isTimeCount;//是否倒计时
		private float preRemainTime;//剩余时间
		private float curRemainTime;//当前时间

		private LimitGoods[] limitInfoArr;//限购信息
		private SysVipMallVo[] limitedMallVoArr;//限购商品信息
		private List<SysVipMallVo> mallList;//存储商品数据（热卖、钻石、绑钻）
		private uint clickNavigationType;
		private bool isInstantiationAlertView;//是否实例化弹出框

		protected override void Init ()
		{
			initView();
			initClick();
			initTextLanguage();
		}

		private void initView()
		{
			btn_close = FindInChild<Button>("mainView/btn_close");

			leftItemContainer  = FindInChild<Transform>("mainView/left/itemContainer");

			commonGoodsListScrollView = FindInChild<UIScrollView>("mainView/right/goodsListContainer");
			commonGoodsGrid = FindInChild<UIGrid>("mainView/right/goodsListContainer/grid");

			btn_bestSell        = FindInChild<UIToggle>("mainView/naigationBar/ckb_bestSell");
			btn_diamond         = FindInChild<UIToggle>("mainView/naigationBar/ckb_diamond");
			btn_bindingDiamond  = FindInChild<UIToggle>("mainView/naigationBar/ckb_bindingDiamond");
			btn_gold            = FindInChild<UIToggle>("mainView/naigationBar/ckb_gold");

			bDiamondValueLabel   = FindInChild<UILabel>("mainView/leftTop/bDiamondValueLabel");
			diamondValueLabel    = FindInChild<UILabel>("mainView/leftTop/diamondValueLabel");
			goldValueLabel       = FindInChild<UILabel>("mainView/leftTop/goldValueLabel");
			limitTimeLabel       = FindInChild<UILabel>("mainView/left/limitTimeLabel");
			timeLabel            = FindInChild<UILabel>("mainView/left/timeLabel");
			timeValueLabel       = FindInChild<UILabel>("mainView/left/timeValueLabel");
		}

		private void initClick()
		{
			limitItemList = new List<Transform>();
			for ( int i = 0 ; i < StoreShopConst.LIMITTIME_3 ; i ++)
			{
				limitItemList.Add(FindInChild<Transform>("mainView/left/itemContainer/item_" + i));
				FindInChild<Button>("mainView/left/itemContainer/item_" + i).onClick = limitTimeItemOnClick;
			}

			commonGoodsList = new List<Transform>();
			for (int i = 0 ; i < StoreShopConst.ITEMNUMBER_9 ; i ++)
			{
				commonGoodsList.Add(FindInChild<Transform>("mainView/right/goodsListContainer/grid/item_" + i));
				Button itemBtn = FindInChild<Button>("mainView/right/goodsListContainer/grid/item_" + i);
				itemBtn.onClick = commonItemOnClick;
				itemBtn.FindInChild<UILabel>("limitLabel").GetComponent<UILabel>().text = LanguageManager.GetWord("StoreView.xiangou") + "：";
			}

			btn_close.onClick          = closeOnClick;
			btn_bestSell.onStateChange = bestSellOnClick;
			btn_diamond.onStateChange  = diamondOnClick;
			btn_bindingDiamond.onStateChange  = bindingDiamondOnClick;
			btn_gold.onStateChange     = goldOnClick;
		}

		private void initTextLanguage()
		{
			FindInChild<UILabel>("mainView/naigationBar/ckb_bestSell/label").text = LanguageManager.GetWord("StoreShopView.hotSell");
			FindInChild<UILabel>("mainView/naigationBar/ckb_bindingDiamond/label").text = LanguageManager.GetWord("StoreShopView.bindingDiamond");
			FindInChild<UILabel>("mainView/naigationBar/ckb_diamond/label").text = LanguageManager.GetWord("StoreShopView.diamond");
			FindInChild<UILabel>("mainView/naigationBar/ckb_gold/label").text = LanguageManager.GetWord("Money.Gold");

			limitTimeLabel.text = LanguageManager.GetWord("StoreShopView.limitTimeBuy");
			timeLabel.text      = LanguageManager.GetWord("VIPView.LeftTime") + ":";
		}

		private void closeOnClick(GameObject go)
		{
			isInstantiationAlertView = false;
			CloseView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			instanticationAlertView();
			initGetGoldGoodsInfo();

			btn_bestSell.value = true;
			isTimeCount = false;
			clickNavigationType = (uint)StoreShopConst.GoodType.Hot;
			Singleton<StoreControl>.Instance.UpdateLimitedGoods();

			updateTimeCount();
			updateMoneyInfo();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			isTimeCount = false;
		}

		public override void Update()
		{
			base.Update();
			updateTimeCount();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<StoreMode>.Instance.dataUpdated += updateLimitedInfoHander;
			Singleton<StoreMode>.Instance.dataUpdated += updateCommonGoodsHandler;
			RoleMode.Instance.dataUpdated += updateRoleMoneyInfoHandler;
		}

		public override void CancelUpdateHandler()
		{
			Singleton<StoreMode>.Instance.dataUpdated -= updateLimitedInfoHander;
			Singleton<StoreMode>.Instance.dataUpdated -= updateCommonGoodsHandler;
			RoleMode.Instance.dataUpdated -= updateRoleMoneyInfoHandler;
		}

		//实例化弹出框（tips、输入框）
		private void instanticationAlertView()
		{
			if(!isInstantiationAlertView)
			{
				Singleton<StoreShopAlertView>.Instance.Init(NGUITools.FindChild(gameObject,"alertView"));
				Singleton<StoreShopInputView>.Instance.Init(NGUITools.FindChild(gameObject,"inputView"));
			}
			isInstantiationAlertView = true;
		}

		private void initGetGoldGoodsInfo()
		{
			clickNavigationType = (uint)StoreShopConst.GoodType.Gold;
			Singleton<StoreMode>.Instance.UpdateGoodsInfoListByType((int)StoreShopConst.GoodType.Gold);
		}

		//导航栏
		//热卖
		private void bestSellOnClick(bool state)
		{
			if(state)
			{
				clickNavigationType = (uint)StoreShopConst.GoodType.Hot;
				Singleton<StoreMode>.Instance.UpdateGoodsInfoListByType((int)StoreShopConst.GoodType.Hot);
			}
		}

		//钻石
		private void diamondOnClick(bool state)
		{
			if(state)
			{
				clickNavigationType = (uint)StoreShopConst.GoodType.Diamond;
				Singleton<StoreMode>.Instance.UpdateGoodsInfoListByType((int)StoreShopConst.GoodType.Diamond);
			}
		}

		//绑定钻石
		private void bindingDiamondOnClick(bool state)
		{
			if(state)
			{
				clickNavigationType = (uint)StoreShopConst.GoodType.BindingDiamond;
				Singleton<StoreMode>.Instance.UpdateGoodsInfoListByType((int)StoreShopConst.GoodType.BindingDiamond);
			}
		}

		//金币
		private void goldOnClick(bool state)
		{
			if(state)
			{
				Singleton<StoreMode>.Instance.GoldGoodsInfo();
				clickNavigationType = (uint)StoreShopConst.GoodType.Gold;
				Singleton<StoreMode>.Instance.UpdateGoodsInfoListByType((int)StoreShopConst.GoodType.Gold);
			}
		}

		//更新倒计时
		private void updateTimeCount()
		{
			if(isTimeCount)
			{
				curRemainTime -= Time.deltaTime;
				if(curRemainTime < 0)
				{
					curRemainTime = 0;
				}
				if(preRemainTime - curRemainTime > 1.0f)
				{
					preRemainTime = curRemainTime;
					showRemainTime();
				}
			}
		}

		//显示倒计时
		private void showRemainTime()
		{
			timeValueLabel.text = string.Empty + TimeUtil.GetTimeHhmmss((int)curRemainTime);
		}

		//更新钻石、绑定钻石、钻石
		private void updateMoneyInfo()
		{
			diamondValueLabel.text  = MeVo.instance.DiamondStr;
			bDiamondValueLabel.text = MeVo.instance.BindingDiamondStr;
			goldValueLabel.text     = MeVo.instance.DiamStr;
		}

		//更新货币
		private void updateRoleMoneyInfoHandler(object sender , int code)
		{
			if(Singleton<RoleMode>.Instance.UPDATE_FORTUNE == code)
			{
				updateMoneyInfo();
			}
		}

		//更新限购：倒计时、列表
		private void updateLimitedInfoHander(object sender , int code)
		{
			if(code == Singleton<StoreMode>.Instance.UPDATE_LIMITED)
			{
				curRemainTime = Singleton<StoreMode>.Instance.limitedRemainTime;
				preRemainTime = curRemainTime;
				updateTimeCount();
				showLimitedGoods();
				isTimeCount = true;
			}
		}

		//显示限购物品
		public void showLimitedGoods()
		{
			Transform updateLimitedItem;
			SysVipMallVo limitedMallVo;
			limitedMallVoArr = Singleton<StoreMode>.Instance.limitedGoodsArr;

			LimitGoods limitInfo;
			limitInfoArr = Singleton<StoreMode>.Instance.limitInfoArr;
			for (int i = 0 ; i < StoreShopConst.LIMITTIME_3 ; i ++)
			{
				ItemContainer limietItemContainer = FindChild("mainView/left/itemContainer/item_" + i).AddMissingComponent<ItemContainer>();
				updateLimitedItem = leftItemContainer.FindChild("item_" + i.ToString());
				limitedMallVo     = limitedMallVoArr[i];
				limitInfo = limitInfoArr[i];
				limietItemContainer.Id = (uint)limitedMallVo.id;
				updateLimitGoods(updateLimitedItem , limitedMallVo , limitInfo);
			}
		}

		//更新每个限购物品
		private void updateLimitGoods(Transform item , SysVipMallVo mallVo , LimitGoods limitInfo)
		{
			SysItemVo itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)mallVo.id);
			Singleton<ItemManager>.Instance.InitItem(item.gameObject, 
			                                         (uint)itemVo.id, ItemType.BaseGoods);
			item.FindChild("nameLabel").GetComponent<UILabel>().text = itemVo.name;
			item.FindChild("goldLabel").GetComponent<UILabel>().text = string.Empty + mallVo.curr_price;
			item.FindChild("limitValueLabel").GetComponent<UILabel>().text = string.Empty + limitInfo.count;
		}

		//更新物品列表
		private void updateCommonGoodsHandler(object sender , int code)
		{
			if(code == Singleton<StoreMode>.Instance.UPDATE_GOODS)
			{
				mallList = Singleton<StoreMode>.Instance.goodsList;
			}else if(code == Singleton<StoreMode>.Instance.UPDATE_GOLDGOODS)
			{
				mallList = Singleton<StoreMode>.Instance.goldGoodsList;
			}
			showRightGoods();
		}

		//显示右侧物品
		private void showRightGoods()
		{
			for(int i = 0 ; i < StoreShopConst.ITEMNUMBER_9 ; i ++)
			{
				Transform commonGoodsItem = commonGoodsList[i];
				commonGoodsItem.gameObject.SetActive(true);
				ItemContainer goodsItemContainer = FindChild("mainView/right/goodsListContainer/grid/item_" + i).AddMissingComponent<ItemContainer>();
				if(i < mallList.Count)
				{
					updateCommonGoods(commonGoodsItem , mallList[i]);
					goodsItemContainer.Id = (uint)mallList[i].id;
				}else
				{
					commonGoodsItem.gameObject.SetActive(false);
				}
			}
		}

		//更新每个普通物品
		private void updateCommonGoods(Transform item , SysVipMallVo mallVo)
		{
			SysItemVo itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)mallVo.id);
			Singleton<ItemManager>.Instance.InitItem(item.gameObject, 
			                                         (uint)itemVo.id, ItemType.BaseGoods);
			item.FindChild("nameLabel").GetComponent<UILabel>().text = itemVo.name;
			item.FindChild("goldValueLabel").GetComponent<UILabel>().text  = string.Empty + mallVo.curr_price;
			setGoodsLimitNumber(mallVo.buy_max , item);
			setGoodsCostIcon(item);
		}

		//设置商品货币图标
		private void setGoodsCostIcon(Transform item)
		{
			UIAtlas commonAtlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
			UISprite itemSprite = item.FindChild("jb").GetComponent<UISprite>();
			item.FindChild("jb").GetComponent<UISprite>().atlas = commonAtlas;
			if(clickNavigationType == (uint)StoreShopConst.GoodType.Diamond || 
			   clickNavigationType == (uint)StoreShopConst.GoodType.Hot)
			{
				itemSprite.spriteName = "zs";
			}else if(clickNavigationType == (int)StoreShopConst.GoodType.BindingDiamond)
			{
				itemSprite.spriteName = "bdzs";
			}else
			{
				itemSprite.spriteName = "jb";
			}
		}

		//设置商品是否显示限购数量
		private void setGoodsLimitNumber(int buyMax , Transform item)
		{
			if(clickNavigationType == (int)StoreShopConst.GoodType.Gold)
			{
				item.FindChild("limitLabel").GetComponent<UILabel>().SetActive(true);
				item.FindChild("limitValueLabel").GetComponent<UILabel>().SetActive(true);
				item.FindChild("limitValueLabel").GetComponent<UILabel>().text = string.Empty + buyMax;
				Transform icon = item.FindChild("jb").GetComponent<Transform>();
				icon.localPosition = new Vector3(-70f , icon.localPosition.y , 0);
				Transform goldLabel = item.FindChild("goldValueLabel").GetComponent<Transform>();
				goldLabel.localPosition = new Vector3(-50f , goldLabel.localPosition.y , 0);
			}else
			{
				item.FindChild("limitLabel").GetComponent<UILabel>().SetActive(false);
				item.FindChild("limitValueLabel").GetComponent<UILabel>().SetActive(false);
				Transform icon = item.FindChild("jb").GetComponent<Transform>();
				icon.localPosition = new Vector3(-10f , icon.localPosition.y , 0);
				Transform diamLabel = item.FindChild("goldValueLabel").GetComponent<Transform>();
				diamLabel.localPosition = new Vector3(10f , diamLabel.localPosition.y , 0);
			}
		}

		//点击限购
		private void limitTimeItemOnClick(GameObject go)
		{
			ItemContainer currentClickGoodsItem = go.GetComponent<ItemContainer>();
			uint clickType = (uint)StoreShopConst.GoodType.Limit;
			SysVipMallVo currentClickGoodsVo = BaseDataMgr.instance.GetSysVipMallVo(currentClickGoodsItem.Id ,clickType);
			clickOpenTips(clickType , currentClickGoodsItem.Id , currentClickGoodsVo);
		}
		
		//点击商品
		private void commonItemOnClick(GameObject go)
		{
			ItemContainer currentClickGoodsItem = go.GetComponent<ItemContainer>();
			uint clickType = clickNavigationType;
			SysVipMallVo currentClickGoodsVo = BaseDataMgr.instance.GetSysVipMallVo(currentClickGoodsItem.Id , clickType);
			clickOpenTips(clickType , currentClickGoodsItem.Id , currentClickGoodsVo);
		}

		private void clickOpenTips(uint clickType , uint id , SysVipMallVo currentClickGoodsVo)
		{
			SysItemVo currentClickItemVo = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
			Singleton<StoreShopAlertView>.Instance.setCurrentGoods(clickType , currentClickGoodsVo , currentClickItemVo);
		}
	}
}