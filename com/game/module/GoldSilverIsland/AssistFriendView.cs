//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：AssistFriendView
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.module.effect;
using com.game.manager;
using com.game.data;
using com.game.Public.Confirm;
using com.game.Public.Message;
using PCustomDataType;
using com.game.consts;

namespace Com.Game.Module.GoldSilverIsland
{
	public class AssistFriendView : BaseView<AssistFriendView> 
	{		
		public override string url { get { return "UI/GoldSilverIsland/AssistFriendView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		private Button btnClose;
		private UIGrid grid;
		private UILabel labTitle;
		private UILabel labFriendGrade;
		private UILabel labFriendName;
		private UILabel labFriendPower;
		private UILabel labFriendAssistCnt;

		private List<Button> itemList = new List<Button>();
		private int friendIndex;

		protected override void Init()
		{	
			btnClose = FindInChild<Button>("center/topright/btn_close");
			grid = FindInChild<UIGrid>("center/container/oncenter");
			itemList.AddRange(grid.gameObject.GetComponentsInChildren<Button>(true));
			labTitle = FindInChild<UILabel>("center/title/zi");
			labFriendGrade = FindInChild<UILabel>("center/header/grade");
			labFriendName = FindInChild<UILabel>("center/header/name");
			labFriendPower = FindInChild<UILabel>("center/header/power");
			labFriendAssistCnt = FindInChild<UILabel>("center/header/assistcount");
			
			btnClose.onClick = CloseOnClick;

			InitLabel();			
			SetToLayerMode();
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("GoldSilverIsland.AssistListTitle");
			labFriendGrade.text = LanguageManager.GetWord("GoldSilverIsland.FriendGrade");
			labFriendName.text = LanguageManager.GetWord("GoldSilverIsland.FriendName");
			labFriendPower.text = LanguageManager.GetWord("GoldSilverIsland.FriendPower");
			labFriendAssistCnt.text = LanguageManager.GetWord("GoldSilverIsland.FriendAssistCnt");
		}
		
		//设置对象层级
		private void SetToLayerMode()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("Mode")); 
		}

		private void CloseOnClick(GameObject go)
		{	
			CloseView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();		
			SendCommandsToServerWhileOpen();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GoldSilverIslandMode>.Instance.GetFriendAssitRemainCountList();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateFriendListHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateFriendAssitRemainHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateFriendListHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateFriendAssitRemainHandle;	
		}

		private void UpdateFriendListHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_FRIEND_LIST == type)
			{
				UpdateFriendList();
			}
		}

		private void UpdateFriendAssitRemainHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_FRIEND_ASSIST_REMAIN == type)
			{
				UpdateFriendAssistRemain();
			}
		}

		private void UpdateFriendList()
		{
			int curCount = itemList.Count;
			int targetCount = Singleton<GoldSilverIslandMode>.Instance.FriendList.Count;

			//增加新Button
			for (int i=0; i<(targetCount-curCount); i++)				
			{
				GameObject newGo = GameObject.Instantiate(itemList[0].gameObject) as GameObject;
				newGo.gameObject.name = itemList[0].gameObject.name;
				newGo.transform.parent = itemList[0].transform.parent.transform;
				newGo.transform.localPosition = Vector3.zero;
				newGo.transform.localRotation = Quaternion.identity;
				newGo.transform.localScale = Vector3.one;
				NGUITools.SetLayer(newGo, LayerMask.NameToLayer("Mode"));

				itemList.Add(newGo.GetComponent<Button>());
			}

			//显示隐藏相应的Buttton
			for (int i=0; i<itemList.Count; i++)
			{
				if (i<targetCount)
				{
					itemList[i].SetActive(true);
					itemList[i].onClick = FriendOnClick;

					PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.FriendList[i];
					UILabel labGrade = itemList[i].gameObject.transform.Find("grade").GetComponent<UILabel>();
					UILabel labName = itemList[i].gameObject.transform.Find("name").GetComponent<UILabel>();
					UILabel labCount = itemList[i].gameObject.transform.Find("assistCount").GetComponent<UILabel>();
					UILabel labPower = itemList[i].gameObject.transform.Find("power").GetComponent<UILabel>();
					labGrade.text = item.lvl.ToString();
					labName.text = item.name;
					labPower.text = item.fightPoint.ToString();
					labCount.text = item.remainTimes + "/" + GameConst.MaxAssistTimes;
				}
				else
				{
					itemList[i].SetActive(false);
					itemList[i].onClick = null;
				}
			}

			grid.repositionNow = true;
		}

		private void UpdateFriendAssistRemain()
		{
			int targetCount = Singleton<GoldSilverIslandMode>.Instance.FriendList.Count;

			for (int i=0; i<targetCount; i++)
			{
				PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.FriendList[i];
				UILabel labCount = itemList[i].gameObject.transform.Find("assistCount").GetComponent<UILabel>();
				labCount.text = item.remainTimes.ToString() + "/" + GameConst.MaxAssistTimes;
			}
		}

		private void FriendOnClick(GameObject go)
		{
			Button currentItem = go.GetComponent<Button>();			

			for (friendIndex=0; friendIndex<itemList.Count; friendIndex++)
			{
				if (itemList[friendIndex].Equals(currentItem))
				{
					break;
				}
			}
			
			if (friendIndex >= itemList.Count)
			{
				return;
			}

			PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.FriendList[friendIndex];
			string[] param = {item.name};
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("GoldSilverIsland.NeedFriendAssist", param), ConfirmCommands.OK_CANCEL, InviteFriend);
		}

		private void InviteFriend()
		{
			PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.FriendList[friendIndex];
			Singleton<GoldSilverIslandMode>.Instance.InviteAssist(item.id);
			Singleton<GoldSilverIslandMode>.Instance.SelectAssistId = item.id;

			string[] param = {item.name};
			MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.WaitFriendReply", param));
			CloseView();
		}
		
	}
}
