//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：LoginAwardView
//文件描述：登陆奖励界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using com.game.module.test;
using Com.Game.Module.Tips;
using com.game.utils;

namespace com.game.module.LoginAward
{
	public class LoginAwardView : Singleton<LoginAwardView>
	{
		private GameObject loginAwardViewGameObject;

		private List<Transform> loginAwardItemList;//登陆奖励列表
		private List<Transform> loginAwardGoodsList;

		private Transform goodsListContainer;
		private UIScrollView goodsListScrollView;
		private UICenterOnChild centerOnChild;

		private const uint LINE_POSITION = 40;//每行Y坐标差值
		private const uint LINE_ITEM_H   = 145;//每行高

		public void Init(GameObject obj)
		{
			loginAwardViewGameObject = obj;
			initView();
			initTextLanguage();
			setLoginAwardGoods();
		}

		private void initView()
		{
			goodsListContainer  = NGUITools.FindInChild<Transform>(loginAwardViewGameObject , "goodsListContainer");
			goodsListScrollView = NGUITools.FindInChild<UIScrollView>(loginAwardViewGameObject , "goodsListContainer");
			centerOnChild       = NGUITools.FindInChild<UICenterOnChild>(loginAwardViewGameObject , "goodsListContainer");

			loginAwardItemList  = new List<Transform>();
			loginAwardGoodsList = new List<Transform>();
			for ( int i = 0 ; i < LoginAwardConst.LoginAward_7 ; i ++)
			{
				Transform gridItem = NGUITools.FindInChild<Transform>(loginAwardViewGameObject, "goodsListContainer/grid/item_" + i);
				loginAwardItemList.Add(gridItem);
			}
			foreach ( Transform item in loginAwardItemList)
			{
				Transform itemList = NGUITools.FindInChild<Transform>(item.gameObject , "itemList");
				loginAwardGoodsList.Add(itemList);
			}
		}

		private void initTextLanguage()
		{
			int index = 1;
			foreach ( Transform item in loginAwardItemList)
			{
				UILabel titleLabel = NGUITools.FindInChild<UILabel>(item.gameObject, "titleLabel");
				titleLabel.text = LanguageManager.GetWord("LoginAwardView.loginDay" , string.Empty + index);
				index ++;
			}
		}

		//设置登陆奖励物品
		private void setLoginAwardGoods()
		{
			for ( int i = 0 ; i < LoginAwardConst.LoginAward_7 ; i ++)
			{
				SysGiftVo gift = BaseDataMgr.instance.GetGiftPack(1000001, i + 1);
				Transform goodsGrid = loginAwardGoodsList[i];
				setItemGoods(goodsGrid , gift);
			}
		}

		private void setItemGoods(Transform goodsGrid , SysGiftVo gift)
		{
			string[] goodsListByStr = StringUtils.GetValueCost(gift.goods);
			for (int i = 0; i < LoginAwardConst.LoginAwardItem_5 ; i ++ )
			{
				Transform itemGoods = NGUITools.FindInChild<Transform>(goodsGrid.gameObject , "item_" + i);
				UILabel countLabel  = NGUITools.FindInChild<UILabel>(itemGoods.gameObject , "count");
				ItemContainer mailitemContainer = itemGoods.gameObject.AddMissingComponent<ItemContainer>();
				countLabel.text = "";
				mailitemContainer.Id = 0;
				if(i < goodsListByStr.Length)
				{
					int id    = int.Parse(goodsListByStr[i].Split(',')[0]);
					int count = int.Parse(goodsListByStr[i].Split(',')[1]);
					mailitemContainer.Id = (uint)id;
					itemGoods.GetComponent<Button>().onClick = itemOnClick;
					Singleton<ItemManager>.Instance.InitItem(itemGoods.gameObject, 
					                                         (uint)id, ItemType.BaseGoods);
					countLabel.text = "x" + string.Empty + count;
				}
			}
		}

		//设置当天可领取状态
		public void SetCurrentDayState(int dayCount , int status)
		{
			int index = 1;
			foreach ( Transform item in loginAwardItemList)
			{
				Button receiveBtn = NGUITools.FindInChild<Button>(item.gameObject , "btn_receive");
				UISprite receiveTag = NGUITools.FindInChild<UISprite>(item.gameObject , "receiveTag");
				UILabel receiveLabel = receiveBtn.FindInChild<UILabel>("lk").GetComponent<UILabel>();
				if(index < dayCount)
				{
					setReceiveTag(receiveTag , receiveLabel);
				}else if(index > dayCount)
				{
					receiveLabel.text = LanguageManager.GetWord("LoginAward.NoGet");//未领取
				}else
				{
					if(status == (int)LoginAwardConst.GetStatus.HaveReveive)//已领取
					{
						setReceiveTag(receiveTag , receiveLabel);
					}else
					{
						receiveLabel.text = LanguageManager.GetWord("LoginAward.Get");//可领取
						receiveBtn.onClick = receiveOnClick;
					}
				}
				index ++;
			}
			//setCurerntCanGetItem(dayCount);
		}

		//设置已领取状态
		private void setReceiveTag(UISprite receiveTag , UILabel receiveLabel)
		{
			receiveTag.SetActive(true);
			receiveLabel.text = LanguageManager.GetWord("LoginAward.Got");//已领取
		}

		//打开界面，设置当前可领取
		private void setCurerntCanGetItem(int day)
		{
			int current = day - 1;
			string itemName = "goodsListContainer/grid/item_" + current;
			centerOnChild.CenterOn(centerOnChild.transform.Find(itemName));
		}

		//点击领取
		private void receiveOnClick(GameObject go)
		{
			if(Singleton<LoginAwardMode>.Instance.dayInfo.status == (int)LoginAwardConst.GetStatus.HaveReveive)
			{
				return;
			}
			Singleton<LoginAwardMode>.Instance.ApplyAward();
		}

		//点击显示tips
		private void itemOnClick(GameObject go)
		{
			ItemContainer currentClickMailItem = go.GetComponent<ItemContainer>();
			if(currentClickMailItem.Id != 0)
			{
				Singleton<TipsManager>.Instance.OpenTipsByGoodsId(currentClickMailItem.Id, null, null, "", "", 0);
			}
		}
	}
}
