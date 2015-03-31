//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoreShopAlertView
//文件描述：商城购买弹出框
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;

using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using com.game.module.test;

namespace com.game.module.Store
{
	public class StoreShopAlertView : Singleton<StoreShopAlertView>
	{
		private GameObject alertViewGameObject;

		private Button btn_close;
		private Button btn_comfirm;

		private Transform icon;
		private Transform unitPriceIcon;
		private Transform currencryPriceIcon;
		private UIWidgetContainer countValueInput;

		private UILabel nameLabel;
		private UILabel levelLabel;
		private UILabel levelValueLabel;
		private UILabel descriptionLabel;
		private UILabel priceLabel;
		private UILabel priceValueLabel;
		private UILabel countLabel;
		private UILabel countValueLabel;
		private UILabel goldLabel;
		private UILabel goldValueLabel;

		private TweenPlay alterViewTP;

		private SysVipMallVo currentGoodsVo;
		private SysItemVo currentItemVo;

		private bool isOpen;

		private SelectedGoods currentSelectGoods;
		private uint clickType;

		public void Init(GameObject obj)
		{
			alertViewGameObject = obj;

			initView();
			initClick();
			initLanguage();
		}

		private void initView()
		{
			btn_close   = NGUITools.FindInChild<Button>(alertViewGameObject, "view/btn_close");
			btn_comfirm = NGUITools.FindInChild<Button>(alertViewGameObject, "view/btn_comfirm");

			icon = NGUITools.FindInChild<Transform>(alertViewGameObject, "view/topleft");
			unitPriceIcon      = NGUITools.FindInChild<Transform>(alertViewGameObject, "view/unitPrice");
			currencryPriceIcon = NGUITools.FindInChild<Transform>(alertViewGameObject, "view/costPrice");
			countValueInput    = NGUITools.FindInChild<UIWidgetContainer>(alertViewGameObject, "view/input");

			nameLabel        = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/nameLabel");
			levelLabel       = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/levelLabel");
			levelValueLabel  = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/levelValueLabel");
			descriptionLabel = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/descriptionLabel");
			priceLabel       = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/priceLabel");
			priceValueLabel  = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/priceValueLabel");
			countLabel       = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/countLabel");
			countValueLabel  = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/input/countValueLabel");
			goldLabel        = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/goldLabel");
			goldValueLabel   = NGUITools.FindInChild<UILabel>(alertViewGameObject, "view/goldValueLabel");

			alterViewTP      = NGUITools.FindInChild<TweenPlay>(alertViewGameObject, "view");
		}

		private void initClick()
		{
			btn_close.onClick   = closeOnClick;
			btn_comfirm.onClick = comfirmOnClick;
			countValueInput.onPress = countValueInptOnPress;
		}

		private void initLanguage()
		{
			levelLabel.text = LanguageManager.GetWord("Tips.UseLevel");
			priceLabel.text = LanguageManager.GetWord("StoreShopView.uintprice");
			countLabel.text = LanguageManager.GetWord("StoreShopView.count");
			goldLabel.text  = LanguageManager.GetWord("Money.Gold") + ":";

			NGUITools.FindInChild<UILabel>(alertViewGameObject , "view/btn_comfirm/label").text = 
				LanguageManager.GetWord("StoreShopView.comfirmBuy");
		}

		private void closeOnClick(GameObject go)
		{
			closeView();
			Singleton<StoreShopInputView>.Instance.closeView();
		}

		//确定购买
		private void comfirmOnClick(GameObject go)
		{
			if(clickType == (uint)StoreShopConst.GoodType.Limit)//限购商品
			{
				Singleton<StoreMode>.Instance.BuyLimitGoods(currentSelectGoods);
			}else//普通商品
			{
				Singleton<StoreMode>.Instance.BuyNormalGoods(currentSelectGoods);
			}
			closeView();
			Singleton<StoreShopInputView>.Instance.closeView();
		}

		private void closeView()
		{
			isOpen = false;
			goldValueLabel.text = "1";
			countValueLabel.text = "1";
			alertViewGameObject.SetActive(false);
		}

		//打开输入界面
		private void countValueInptOnPress(GameObject go , bool state)
		{
			if(state)
			{
				alterViewTP.PlayForward();
				Singleton<StoreShopInputView>.Instance.openView();
			}
		}

		//设置tips
		private void setCurrentGoodsInfo()
		{
			UIAtlas Atlas = null;
			string Name = "";
			Singleton<ItemManager>.Instance.InitItem(icon.gameObject,
			                                         (uint)currentGoodsVo.id, ItemType.BaseGoods);
			nameLabel.text = currentItemVo.name;
			levelValueLabel.text  = string.Empty + currentItemVo.lvl;
			descriptionLabel.text = currentItemVo.desc;
			priceValueLabel.text  = string.Empty + currentGoodsVo.curr_price;
			goldValueLabel.text   = string.Empty + int.Parse(countValueLabel.text) * int.Parse(priceValueLabel.text);
		}

		//设置货币类型
		private void setCurretnCostIcon()
		{
			UIAtlas commonAtlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
			//单价图标和文本
			UISprite costIconSprite = unitPriceIcon.GetComponent<UISprite>();
			unitPriceIcon.GetComponent<UISprite>().atlas = commonAtlas;
			//货币类型图标和文本
			UISprite currencrySpite = currencryPriceIcon.GetComponent<UISprite>();
			currencryPriceIcon.GetComponent<UISprite>().atlas = commonAtlas;

			if(clickType == (uint)StoreShopConst.GoodType.Diamond || 
			   clickType == (uint)StoreShopConst.GoodType.Hot ||
			   clickType == (uint)StoreShopConst.GoodType.Limit)
			{
				costIconSprite.spriteName = currencrySpite.spriteName = "zs";
				goldLabel.text = LanguageManager.GetWord("StoreShopView.diamond");
			}else if(clickType == (int)StoreShopConst.GoodType.BindingDiamond)
			{
				costIconSprite.spriteName = currencrySpite.spriteName = "bdzs";
				goldLabel.text = LanguageManager.GetWord("StoreShopAlertView.bindingDiamond");
			}else
			{
				costIconSprite.spriteName = currencrySpite.spriteName =  "jb";
				goldLabel.text = LanguageManager.GetWord("Money.Gold");
			}
		}

		//设置当前选中物品
		public void setCurrentGoods(uint type , SysVipMallVo vipmallVo , SysItemVo itemVo)
		{
			clickType      = type; 
			currentGoodsVo = vipmallVo;
			currentItemVo  = itemVo;

			currentSelectGoods         = new SelectedGoods();
			currentSelectGoods.id      = (uint)vipmallVo.id;
			currentSelectGoods.type    = (ushort)vipmallVo.type;
			currentSelectGoods.subType = (ushort)vipmallVo.small_type;
			currentSelectGoods.num     = 1;

			isOpen = true;
			alertViewGameObject.SetActive(true);
			alertPlayReverse();
			setCurrentGoodsInfo();
			setCurretnCostIcon();

			Singleton<StoreShopInputView>.Instance.LimitBuyMax = vipmallVo.buy_max;
		}

		//同步更新tips输入数量
		public void setCountNumberLabel(string countStr)
		{
			if(countStr == "")
			{
				countValueLabel.text = "0";
			}else
			{
				countValueLabel.text = countStr;
			}
			int totalAmout = int.Parse(countValueLabel.text) * int.Parse(priceValueLabel.text);
			goldValueLabel.text = string.Empty + totalAmout;
			currentSelectGoods.num = Convert.ToUInt32(countValueLabel.text);
		}

		public void closeAlertAndInputView()
		{
			alertViewGameObject.SetActive(false);
			Singleton<StoreShopInputView>.Instance.closeView();
		}

		public void alertPlayReverse()
		{
			alterViewTP.PlayReverse();
		}
	}
}