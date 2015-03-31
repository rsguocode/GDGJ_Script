//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoreShopInputView
//文件描述：商城购买输入框
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.consts;
using com.game.manager;
using com.game.module.test;
using com.game.Public.Message;

namespace com.game.module.Store
{
	public class StoreShopInputView : Singleton<StoreShopInputView>
	{
		private GameObject inputViewObject;

		private Button btn_close;
		private Button btn_comfirm;
		private Button btn_del;

		private List<Transform> numberBtnList;

		private UILabel inputLabel;

		private bool isOpen;
		private string numInputStr;

		private int _limitBuyMax;//限购最大数

		public void Init(GameObject obj)
		{
			inputViewObject = obj;

			initView();
			initClick();
			initLanguage();
		}

		private void initView()
		{
			numInputStr = "";

			btn_close   = NGUITools.FindInChild<Button>(inputViewObject, "btn_close");
			btn_comfirm = NGUITools.FindInChild<Button>(inputViewObject, "btn_comfirm");
			btn_del     = NGUITools.FindInChild<Button>(inputViewObject, "btn_del");

			inputLabel  = NGUITools.FindInChild<UILabel>(inputViewObject, "inputLabel");
		}

		private void initClick()
		{
			btn_close.onClick   = closeOnClick;
			btn_comfirm.onClick = closeOnClick;
			btn_del.onClick     = delOnClick;

			numberBtnList = new List<Transform>();
			for ( int i = 0 ; i < StoreShopConst.INPUT_NUMBER ; i ++)
			{
				ItemContainer btnContainer = NGUITools.FindInChild<Button>(inputViewObject , "btn/btn_" + i).gameObject.AddMissingComponent<ItemContainer>();
				btnContainer.Id = (uint)i;
				NGUITools.FindInChild<Button>(inputViewObject , "btn/btn_" + i).onClick = numberBtnOnClick;
			}
		}

		private void initLanguage()
		{
			NGUITools.FindInChild<UILabel>(inputViewObject , "btn_comfirm/label").text = 
				LanguageManager.GetWord("StoreShopView.ensure");
		}

		private void closeOnClick(GameObject go)
		{
			Singleton<StoreShopAlertView>.Instance.alertPlayReverse();
			closeView();
		}
	
		private void delOnClick(GameObject go)
		{
			if(numInputStr.Length != 0)
			{
				string inputValue = numInputStr.Remove(numInputStr.Length - 1 , 1);
				if(inputValue == "")
				{
					inputValue = "1";
					inputLabel.text = "0";
					numInputStr = "";
				}else
				{
					inputLabel.text = numInputStr = inputValue;
				}
				Singleton<StoreShopAlertView>.Instance.setCountNumberLabel(inputValue);
			}
		}

		private void numberBtnOnClick(GameObject go)
		{
			ItemContainer numberBtn = go.GetComponent<ItemContainer>();
			if(numInputStr == "" && numberBtn.Id == 0)
			{
				return;
			}
			numInputStr += numberBtn.Id;
			int buyCount = int.Parse(numInputStr);
			if(_limitBuyMax == 0)//无限购买，最大99
			{
				if(buyCount >= StoreShopConst.BUY_MAX)
				{
					buyCount = StoreShopConst.BUY_MAX;
					numInputStr = string.Empty + StoreShopConst.BUY_MAX;
				}
			}else
			{
				if(buyCount > _limitBuyMax)
				{
					buyCount = _limitBuyMax;
					numInputStr = string.Empty + _limitBuyMax;
					MessageManager.Show(LanguageManager.GetWord("StoreShopAlertView.upperLimit"));
				}
			}
			inputLabel.text = string.Empty + buyCount;
			Singleton<StoreShopAlertView>.Instance.setCountNumberLabel(inputLabel.text);
		}

		public void closeView()
		{
			isOpen = false;
			inputViewObject.SetActive(false);
			inputLabel.text = "0";
			numInputStr = "";
		}

		public void openView()
		{
			inputViewObject.SetActive(true);
			inputLabel.text = "0";
			numInputStr = "";
		}

		public int LimitBuyMax
		{
			get
			{
				return _limitBuyMax;
			}
			set
			{
				_limitBuyMax = value;
			}
		}
	}
}