//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildSeeView
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
using Proto;
using com.game.utils;

namespace Com.Game.Module.Guild
{
	public class GuildSeeView : BaseView<GuildSeeView>  
	{		
		public override string url { get { return "UI/Guild/GuildSeeView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	
		public override bool IsFullUI { get { return true; }}

		private Button btnClose;

		//左面板
		private UILabel labGuildName;
		private UILabel labGuildGrade;
		private UILabel labGuildRank;
		private UILabel labGuildPersonCnt;
		private UILabel labGuildExp;
		private UILabel labGuildNotice;
		private UIGrid memberGrid;
		private Button btnApply;
		private Button btnCancel;

		//翻译相关
		private UILabel labTitle;
		private UILabel labGuildGradeDes;
		private UILabel labGuildRankDes;
		private UILabel labGuildPersonCntDes;
		private UILabel labGuildExpDes;
		private UILabel labGuildNoticeDes;
		private UILabel labMemberName;
		private UILabel labMemberGrade;
		private UILabel labMemberPos;
		private UILabel labMemberExp;
		private UILabel labMemberDevote;
		private UILabel labMemberLogin;

		private List<Button> memberList = new List<Button>();
		private uint guildId;

		
		protected override void Init()
		{
			btnClose = FindInChild<Button>("common/topright/btn_close");

			labGuildName = FindInChild<UILabel>("center/left/mz/mz");
			labGuildGrade = FindInChild<UILabel>("center/left/info/grade");
			labGuildRank = FindInChild<UILabel>("center/left/info/rank");
			labGuildPersonCnt = FindInChild<UILabel>("center/left/info/personcount");
			labGuildExp = FindInChild<UILabel>("center/left/info/exp");
			labGuildNotice = FindInChild<UILabel>("center/left/info/notice");

			memberGrid = FindInChild<UIGrid>("center/right/member/container/oncenter");
			memberList.Add(memberGrid.transform.Find("member").GetComponent<Button>());

			btnApply = FindInChild<Button>("common/bottom/btn_apply");
			btnCancel = FindInChild<Button>("common/bottom/btn_cancel");

			//翻译相关
			labTitle = FindInChild<UILabel>("common/top/title/label"); 
			labGuildGradeDes = FindInChild<UILabel>("center/left/info/dj"); 
			labGuildRankDes = FindInChild<UILabel>("center/left/info/bp"); 
			labGuildPersonCntDes = FindInChild<UILabel>("center/left/info/mz");
			labGuildExpDes = FindInChild<UILabel>("center/left/info/labexp");
			labGuildNoticeDes = FindInChild<UILabel>("center/left/info/labnotice");
			labMemberName = FindInChild<UILabel>("center/right/member/bt/name");
			labMemberGrade = FindInChild<UILabel>("center/right/member/bt/grade");
			labMemberPos = FindInChild<UILabel>("center/right/member/bt/job");
			labMemberExp = FindInChild<UILabel>("center/right/member/bt/exp");
			labMemberDevote = FindInChild<UILabel>("center/right/member/bt/devote");
			labMemberLogin = FindInChild<UILabel>("center/right/member/bt/logintime");
			
			btnClose.onClick = CloseOnClick;
			btnApply.onClick = ApplyOnClick;
			btnCancel.onClick = CancelOnClick;
			
			InitLabel();
		}
		
		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("Guild.GuildSeeTitle");
			labGuildGradeDes.text = LanguageManager.GetWord("Guild.MyGuildGrade");
			labGuildRankDes.text = LanguageManager.GetWord("Guild.MyGuildRank");
			labGuildPersonCntDes.text = LanguageManager.GetWord("Guild.MyGuildPersonCnt");
			labGuildExpDes.text = LanguageManager.GetWord("Guild.MyGuildExp");
			labGuildNoticeDes.text = LanguageManager.GetWord("Guild.MyGuildNotice");
			labMemberName.text = LanguageManager.GetWord("Guild.MemberName");
			labMemberGrade.text = LanguageManager.GetWord("Guild.MemberGrade");
			labMemberPos.text = LanguageManager.GetWord("Guild.MemberPos");
			labMemberExp.text = LanguageManager.GetWord("Guild.MemberExp");
			labMemberDevote.text = LanguageManager.GetWord("Guild.MemberDevote");
			labMemberLogin.text = LanguageManager.GetWord("Guild.MemberLogin");
			btnApply.label.text = LanguageManager.GetWord("Guild.GuildApply");
			btnCancel.label.text = LanguageManager.GetWord("Guild.Cancel");
		}
		
		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void ApplyOnClick(GameObject go)
		{
			Singleton<GuildMode>.Instance.JoinGuild(guildId);
		}

		private void CancelOnClick(GameObject go)
		{
			CloseView();
		}

		public void ShowWindow(uint guildId)
		{
			this.guildId = guildId;
			OpenView();
		}

		public override void OpenView()
		{			
			SendCommandsToServerWhileOpen();			
			base.OpenView();
		}
		
		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GuildMode>.Instance.GetGuildBaseInfo(guildId);
			Singleton<GuildMode>.Instance.GetGuildMembersInfo(guildId);
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			SetApplyBtnEnabled();
			UpdateViewInfo();
		}

		private void SetApplyBtnEnabled()
		{
			bool hasGuild = (0 != MeVo.instance.guildId);
			NGUITools.SetButtonEnabled(btnApply, !hasGuild);
		}

		private void UpdateViewInfo()
		{
			UpdateGuildInfo();
			UpdateGuildMemberList();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildInfoHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildMemberListHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildIdChangedHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildInfoHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildMemberListHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildIdChangedHandle;
		}

		private void UpdateGuildIdChangedHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_ID_CHANGED == type)			    
			{
				CloseView();
			}
		}

		private void UpdateGuildInfoHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_INFO == type)			    
			{
				UpdateGuildInfo();
			}
		}
		
		private void UpdateGuildMemberListHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_MEMBERS == type)			    
			{
				UpdateGuildMemberList();
			}
		}

		//更新公会基本信息
		private void UpdateGuildInfo()
		{
			GuildBasicInfoMsg_31_6 myGuildInfo = Singleton<GuildMode>.Instance.GetGuildItem(guildId);
			
			if (null == myGuildInfo)
			{
				return;
			}
			
			labGuildName.text = myGuildInfo.guildName.ToString();
			labGuildGrade.text = myGuildInfo.rank.ToString();
			labGuildRank.text = myGuildInfo.level.ToString();
			labGuildPersonCnt.text = myGuildInfo.memberNum.ToString();
			labGuildExp.text = myGuildInfo.curExp.ToString();
			labGuildNotice.text = myGuildInfo.announcement;
		}
		
		//更新公会成员列表
		private void UpdateGuildMemberList()
		{
			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(guildId);
			
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
					
					UILabel labName = memberList[i].gameObject.transform.Find("name").GetComponent<UILabel>();
					UILabel labGrade = memberList[i].gameObject.transform.Find("grade").GetComponent<UILabel>();
					UILabel labJob = memberList[i].gameObject.transform.Find("job").GetComponent<UILabel>();
					UILabel labExp = memberList[i].gameObject.transform.Find("exp").GetComponent<UILabel>();
					UILabel labDevote = memberList[i].gameObject.transform.Find("devote").GetComponent<UILabel>();
					UILabel labLoginTime = memberList[i].gameObject.transform.Find("logintime").GetComponent<UILabel>();
					
					labName.text = item.name;
					labGrade.text = item.lvl.ToString();
					labJob.text = Singleton<GuildMode>.Instance.GetJobName(item.position);
					labExp.text = "0";  //公会经验待定
					labDevote.text = item.contribution.ToString();
					labLoginTime.text = Singleton<GuildMode>.Instance.GetLogoutTimeStr(TimeUtil.GetElapsedTime(item.lastLogoutTime));

					Color color = (item.lastLogoutTime>0) ? Color.gray : Color.yellow;
					labName.color = color;
					labGrade.color = color;
					labJob.color = color;
					labExp.color = color;
					labDevote.color = color;
					labLoginTime.color = color;
				}
				else
				{
					memberList[i].SetActive(false);
				}
			}
			
			memberGrid.repositionNow = true;
		}
		
	}
}
