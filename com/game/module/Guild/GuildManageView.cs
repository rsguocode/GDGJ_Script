//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildManageView
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
using com.game.vo;

namespace Com.Game.Module.Guild
{
	public class GuildManageView : BaseView<GuildManageView> 
	{		
		public override string url { get { return "UI/Guild/GuildManageView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	

		private GameObject transferObj;
		private GameObject dismissObj;
		private GameObject settingObj;
		private GameObject presidentObj;
		private GameObject memberObj;

		private Button btnClose;
		//切换按钮
		private UIToggle togTransfer;
		private UIToggle togDismiss;
		private UIToggle togSetting;

		//转让面板
		private UIGrid memberGrid;
		private Button btnTransfer;	

		//解散面板
		private Button btnDismiss;	

		//设置面板
		private UILabel labNotice;
		private Button btnSetNotice;

		//退出面板
		private Button btnLeave;

		//翻译相关
		private UILabel labTitle;
		private UILabel labTransfer;
		private UILabel labDismiss;
		private UILabel labSetting;
		private UILabel labDismissNotice;
		private UILabel labSettingTitle;
		private UILabel labSettingDefault;
		private UILabel labLeaveNotice;

		private List<Button> memberList = new List<Button>();
		private int selIndex = -1;
		private bool bPresident;

		protected override void Init()
		{
			transferObj = gameObject.transform.Find("center/president/zr").gameObject;
			dismissObj = gameObject.transform.Find("center/president/js").gameObject;
			settingObj = gameObject.transform.Find("center/president/sz").gameObject;
			presidentObj = gameObject.transform.Find("center/president").gameObject;
			memberObj = gameObject.transform.Find("center/member").gameObject;

			btnClose = FindInChild<Button>("center/topright/btn_close");
			togTransfer = FindInChild<UIToggle>("center/president/toggles/zr");
			togDismiss = FindInChild<UIToggle>("center/president/toggles/js");
			togSetting = FindInChild<UIToggle>("center/president/toggles/gl");

			//转让面板
			memberGrid = FindInChild<UIGrid>("center/president/zr/container/oncenter");
			memberList.Add(memberGrid.transform.Find("member").GetComponent<Button>());
			btnTransfer = FindInChild<Button>("center/president/zr/btn_zlbb");

			//解散面板
			btnDismiss = FindInChild<Button>("center/president/js/btn_zlbb");	

			//设置面板
			labNotice = FindInChild<UILabel>("center/president/sz/inp_input/label");
			btnSetNotice = FindInChild<Button>("center/president/sz/btn_zlbb");

			//退出面板
			btnLeave = FindInChild<Button>("center/member/tc/btn_zlbb");

			//翻译相关
			labTitle = FindInChild<UILabel>("center/common/zz/mz");
			labTransfer = FindInChild<UILabel>("center/president/toggles/zr/zhanli");
			labDismiss = FindInChild<UILabel>("center/president/toggles/js/zhanli");
			labSetting = FindInChild<UILabel>("center/president/toggles/gl/zhanli");
			labDismissNotice = FindInChild<UILabel>("center/president/js/gg");
			labSettingTitle = FindInChild<UILabel>("center/president/sz/gg");
			labSettingDefault = FindInChild<UILabel>("center/president/sz/inp_input/label");
			labLeaveNotice = FindInChild<UILabel>("center/member/tc/gg");

			//事件绑定
			btnClose.onClick = CloseOnClick;
			btnTransfer.onClick = TransferOnClick;
			btnDismiss.onClick = DismissOnClick;
			btnSetNotice.onClick = SetNoticeOnClick;
			btnLeave.onClick = LeaveOnClick;
			togTransfer.onChange.Add(new EventDelegate(TogTransferOnClick));
			togDismiss.onChange.Add(new EventDelegate(TogDismissOnClick));
			togSetting.onChange.Add(new EventDelegate(TogSettingOnClick));

			InitLabel();
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("Guild.ManageTitle");
			labTransfer.text = LanguageManager.GetWord("Guild.GuildTransfer");
			labDismiss.text = LanguageManager.GetWord("Guild.GuildDismiss");
			labSetting.text = LanguageManager.GetWord("Guild.GuildSetting");
			btnTransfer.label.text = LanguageManager.GetWord("Guild.GuildTransfer");
			btnDismiss.label.text = LanguageManager.GetWord("Guild.GuildDismiss");
			btnSetNotice.label.text = LanguageManager.GetWord("Guild.GuildSetting");
			labDismissNotice.text = LanguageManager.GetWord("Guild.GuildDissmissWarn");
			labSettingTitle.text = LanguageManager.GetWord("Guild.GuildSettingTitle");
			labSettingDefault.text = LanguageManager.GetWord("Guild.GuildSettingDefault");
			labLeaveNotice.text = LanguageManager.GetWord("Guild.GuildLeaveNotice");
			btnLeave.label.text = LanguageManager.GetWord("Guild.GuildLeave");

		}

		private void TogTransferOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togTransfer))
				{
					transferObj.SetActive(true);
					dismissObj.SetActive(false);
					settingObj.SetActive(false);
				}
			}	
		}

		private void TogDismissOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togDismiss))
				{
					transferObj.SetActive(false);
					dismissObj.SetActive(true);
					settingObj.SetActive(false);
				}
			}	
		}

		private void TogSettingOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togSetting))
				{
					transferObj.SetActive(false);
					dismissObj.SetActive(false);
					settingObj.SetActive(true);
				}
			}	
		}

		private void TransferOnClick(GameObject go)
		{
			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(MeVo.instance.guildId);
			selIndex = -1;
			
			if (null == dataList)
			{
				return;
			}			

			int targetCount = dataList.Count;

			for (int i=0; i<targetCount; i++)
			{
				UIToggle togTransSel = memberList[i].gameObject.transform.Find("chkbj").GetComponent<UIToggle>();

				if (true == togTransSel.value)
				{
					selIndex = i;
					break;
				}
			}

			if (selIndex < 0)
			{
				MessageManager.Show(LanguageManager.GetWord("Guild.SelTransferMember"));
				return;
			}

			PGuildMember item = dataList[selIndex];

			if ((byte)GuildPosition.President == item.position)
			{
				MessageManager.Show(LanguageManager.GetWord("Guild.TransferSelfNotNeed"));
				return;
			}

			string[] param = {item.name};
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.TransferQuestion", param), ConfirmCommands.OK_CANCEL, TrasnferLeaderPos);
		}

		private void TrasnferLeaderPos()
		{
			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(MeVo.instance.guildId);
			PGuildMember item = dataList[selIndex];
			Singleton<GuildMode>.Instance.ManageGuild(0, item.id);
		}

		private void DismissOnClick(GameObject go)
		{
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.TransferNotice"), ConfirmCommands.OK_CANCEL, DismissGuild);
		}

		private void DismissGuild()
		{
			Singleton<GuildMode>.Instance.DismissGuild();
		}

		private void LeaveGuild()
		{
			Singleton<GuildMode>.Instance.LeaveGuild();
		}

		private void SetNoticeOnClick(GameObject go)
		{
			string notice = labNotice.text;
			Singleton<GuildMode>.Instance.ModifyGuildNotice(notice);
		}

		private void LeaveOnClick(GameObject go)
		{
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.LeaveQuestion"), ConfirmCommands.OK_CANCEL, LeaveGuild);
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			AdjustUIShow();
			UpdateViewInfo();
		}

		private void AdjustUIShow()
		{
			if (bPresident)
			{
				presidentObj.SetActive(true);
				memberObj.SetActive(false);
			}
			else
			{
				presidentObj.SetActive(false);
				memberObj.SetActive(true);
			}
		}

		private void UpdateViewInfo()
		{
			if (bPresident)
			{
				UpdateMemberList();
			}
		}

		//更新成员列表
		private void UpdateMemberList()
		{
			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(MeVo.instance.guildId);
			
			if (null == dataList)
			{
				return;
			}
			
			int curCount = memberList.Count;
			int targetCount = dataList.Count;
			
			//增加新Button
			for (int i=0; i<(targetCount-curCount); i++)				
			{
				GameObject newGo = GameObject.Instantiate(memberList[0].gameObject) as GameObject;
				newGo.gameObject.name = memberList[0].gameObject.name;
				newGo.transform.parent = memberList[0].transform.parent.transform;
				newGo.transform.localPosition = Vector3.zero;
				newGo.transform.localRotation = Quaternion.identity;
				newGo.transform.localScale = Vector3.one;
				NGUITools.SetLayer(newGo, LayerMask.NameToLayer("UI"));
				
				memberList.Add(newGo.GetComponent<Button>());
			}
			
			//显示隐藏相应的Buttton
			for (int i=0; i<memberList.Count; i++)
			{
				if (i<targetCount)
				{
					memberList[i].SetActive(true);					
					PGuildMember item = dataList[i];
					
					UILabel labName = memberList[i].gameObject.transform.Find("mz").GetComponent<UILabel>();					
					labName.text = item.name;
				}
				else
				{
					memberList[i].SetActive(false);
				}
			}
			
			memberGrid.repositionNow = true;
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildIdChangedHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildIdChangedHandle;
		}
		
		private void UpdateGuildIdChangedHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_ID_CHANGED == type)			    
			{
				CloseView();
			}
		}

		public void ShowWindow(bool bPresident)
		{
			this.bPresident = bPresident;
			OpenView();
		}
		
	}
}
