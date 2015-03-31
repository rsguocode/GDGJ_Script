//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：SignView
//文件描述：激活码界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using com.game.module.SystemData;
using com.game.module.test;
using com.game.utils;
using Proto;

namespace com.game.module.LoginAward
{
	public class SignView : Singleton<SignView>
	{
		private GameObject signViewGameObject;

		private UILabel titleLabel;
		private UILabel currentMonthLabel;

		private UIScrollView scrollView;
		private UIGrid signItemGrid;
		private GameObject signItem;

		private List<ItemContainer> signItemContainerList;
		private int currentCount;
		private int currentMonth;
		private int currentDay;

		public void Init(GameObject obj)
		{
			signViewGameObject = obj;
			initView();
			initTextLanguage();
			setCurrentMonthDay();
			setSignAwardGoods();
			setEverySignItemInfo();
		}

		private void initView()
		{
			titleLabel        = NGUITools.FindInChild<UILabel>(signViewGameObject , "titleLabel");
			currentMonthLabel = NGUITools.FindInChild<UILabel>(signViewGameObject , "currentMonthLabel");

			scrollView   = NGUITools.FindInChild<UIScrollView>(signViewGameObject , "content");
			signItemGrid = NGUITools.FindInChild<UIGrid>(signViewGameObject , "content/grid");
			signItemGrid.onReposition += scrollView.ResetPosition;

			signItemContainerList = new List<ItemContainer>();
			signItem = NGUITools.FindChild(signViewGameObject , "content/grid/item_0");
			ItemContainer itemContainer = signItem.AddMissingComponent<ItemContainer>();
			signItemContainerList.Add(itemContainer);
			signItem.SetActive(false);
		}

		private void initTextLanguage()
		{

		}

		private void setCurrentMonthDay()
		{
			DateTime currentDateTime = TimeUtil.GetTime(ServerTime.Instance.Timestamp.ToString());
			currentMonth = currentDateTime.Month;
			currentDay   = currentDateTime.Day;
		}

		private void setSignAwardGoods()
		{
			for ( int i = 0 ; i < LoginAwardConst.SignDay_31 ; i ++)
			{
				SysSignVo sign = BaseDataMgr.instance.GetSignVo(currentMonth, i + 1);
                if (null != sign)
                {
                    Debug.Log("sign.goods = " + sign.goods);
                }
			}
		}

		private void setEverySignItemInfo()
		{
			GameObject signItemGameObject;
			ItemContainer signItemContainer;
			for(int i = 0 ; i < 30 ; i ++)
			{
				signItemGameObject = NGUITools.AddChild(signItemGrid.gameObject , signItem.gameObject);
				signItemContainer = signItemGameObject.AddMissingComponent<ItemContainer>();
				signItemContainerList.Add(signItemContainer);
			}
			for(int j = 0 ; j < signItemContainerList.Count ; j ++)
			{
				signItemContainer = signItemContainerList[j];
				signItemContainer.SetActive(true);
				SysSignVo sign = BaseDataMgr.instance.GetSignVo(currentMonth, j + 1);
				setEveryItemInfo(signItemContainer , sign);
			}
			signItemGrid.Reposition();
		}

		private void setEveryItemInfo(ItemContainer signItemContainer , SysSignVo signVo)
		{
            if (null == signVo)
            {
                return;
            }
            
			string goodsStr = StringUtils.GetValueString(signVo.goods);
			string[] goods  = StringUtils.GetValueListFromString(goodsStr);
			int id    = int.Parse(goods[0]);
			int count = int.Parse(goods[1]);
			Singleton<ItemManager>.Instance.InitItem(signItemContainer.gameObject, 
			                                         (uint)id, ItemType.BaseGoods);
			setEveryItemTag(signItemContainer.gameObject , signVo , count);
		}

		private void setEveryItemTag(GameObject obj , SysSignVo signVo , int count)
		{
			UILabel countLabel = NGUITools.FindInChild<UILabel>(obj , "count");
			countLabel.text = count + string.Empty;
			Transform vipTag = NGUITools.FindInChild<Transform>(obj , "vipTag");
			UILabel vipLabel = NGUITools.FindInChild<UILabel>(obj , "vipTag/vipNum");
			if(signVo.vip != 0)
			{
				vipTag.gameObject.SetActive(true);
				vipLabel.text = "vip" + signVo.vip;
			}else
			{
				vipTag.gameObject.SetActive(false);
				vipLabel.text = "";
			}
		}

		//更新本月已签次数
		public void updateCurrentSign(GiftSignInfoMsg_29_3 signInf)
		{
			currentCount = signInf.times;
			currentMonthLabel.text = LanguageManager.GetWord("SignView.currentMonth" , signInf.times + string.Empty);
		}
	}
}