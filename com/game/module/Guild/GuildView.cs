//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildView
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
using com.game.manager;
using com.game.consts;
using Com.Game.Module.Chat;

namespace Com.Game.Module.Guild
{
	public class GuildView : BaseView<GuildView>  
	{		
		public override string url { get { return "UI/Guild/GuildView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	
		public override bool IsFullUI { get { return true; }}

		private GameObject createGuildObj;
		private GameObject myGuildObj;
		private GameObject otherGuildObj;

		public Button btnClose;
		private UIToggle togOne;
		private UIToggle togTwo;
		private UILabel labOneTitle;
		private UILabel labTwoTitle;

		//创建公会面板
		private UIInput inpGuildName;
		private Button btnCreateGuild;

		//我的公会面板
		//左面板
		private UILabel labGuildName;
		private UILabel labGuildGrade;
		private UILabel labGuildRank;
		private UILabel labGuildPersonCnt;
		private UILabel labGuildExp;
		private UILabel labGuildNotice;
		private Button btnGuildManage;
		//切换按钮
		private UIToggle togMember;
		private UIToggle togLog;
		private UIToggle togActivities;
		private UIToggle togVerify;
		//功能面板对象
		private GameObject memberObj;
		private GameObject logObj;
		private GameObject verifyObj;
		//成员面板
		private UIGrid memberGrid;
		//日志面板
		private UIGrid logGrid;
		//审核面板
		private UIGrid verifyGrid;

		//其他公会面板
		private UIGrid otherGuildGrid;
		private Button btnPrevPage;
		private Button btnNextPage;

		//翻译相关
		private UILabel labTogOne;
		private UILabel labTogTwo;
		private UILabel labOtherGuildRank;
		private UILabel labOtherGuildName;
		private UILabel labOtherGuildLeader;
		private UILabel labOtherGuildGrade;
		private UILabel labOtherGuildPersonCnt;
		private UILabel labOtherGuildCurPage;
		private UILabel labPrevPage;
		private UILabel labNextPage;
		private UILabel labCreateGuildName;
		private UILabel labWaste; 
		private UILabel labConfirmCreate;
		private UILabel labMyGuildGrade;
		private UILabel labMyGuildRank;
		private UILabel labMyGuildPersonCnt;
		private UILabel labMyGuildExp;
		private UILabel labMyGuildNotice;
		private UILabel labMyGuildOpt;
		private UILabel labTogMember;
		private UILabel labTogLog;
		private UILabel labTogActivity;
		private UILabel labTogVerify;
		private UILabel labMemberName;
		private UILabel labMemberGrade;
		private UILabel labMemberPos;
		private UILabel labMemberExp;
		private UILabel labMemberDevote;
		private UILabel labMemberLogin;
		private UILabel labLogMemberName;
		private UILabel labLogDevote;
		private UILabel labLogTime;
		private UILabel labVerifyList;
		private UILabel labVerifyGrade;
		private UILabel labVerifyTime;
		private UILabel labVerifyOpt;

		//数据结构
		private List<Button> memberList = new List<Button>();
		private List<Button> logList = new List<Button>();
		private List<Button> verifyList = new List<Button>();
		private List<Button> otherGuildList = new List<Button>();

		private bool togOneActive = true;
		private int memberIndex;

		protected override void Init()
		{
			//通用对象
			createGuildObj = gameObject.transform.Find("createguild").gameObject;
			myGuildObj = gameObject.transform.Find("myguild").gameObject;
			otherGuildObj = gameObject.transform.Find("otherguild").gameObject;

			btnClose = FindInChild<Button>("common/topright/btn_close");
			togOne = FindInChild<UIToggle>("common/top/ckb_liebiao1");
			togTwo = FindInChild<UIToggle>("common/top/ckb_liebiao2");
			labOneTitle = FindInChild<UILabel>("common/top/ckb_liebiao1/label");
			labTwoTitle = FindInChild<UILabel>("common/top/ckb_liebiao2/label");

			//创建公会面板
			inpGuildName = FindInChild<UIInput>("createguild/center/info/inp_input");
			btnCreateGuild = FindInChild<Button>("createguild/center/btn_create");

			//我的公会面板
			//左面板
			labGuildName = FindInChild<UILabel>("myguild/left/mz/mz");
			labGuildGrade = FindInChild<UILabel>("myguild/left/info/grade");
			labGuildRank = FindInChild<UILabel>("myguild/left/info/rank");
			labGuildPersonCnt = FindInChild<UILabel>("myguild/left/info/personcount");
			labGuildExp = FindInChild<UILabel>("myguild/left/info/exp");
			labGuildNotice = FindInChild<UILabel>("myguild/left/info/notice");
			btnGuildManage = FindInChild<Button>("myguild/left/btn_zlbb");
			//切换按钮
			togMember = FindInChild<UIToggle>("myguild/right/toggles/zl");
			togLog = FindInChild<UIToggle>("myguild/right/toggles/rz");
			togActivities = FindInChild<UIToggle>("myguild/right/toggles/hd");
			togVerify = FindInChild<UIToggle>("myguild/right/toggles/sh");
			//功能面板对象
			memberObj = gameObject.transform.Find("myguild/right/member").gameObject;
			logObj = gameObject.transform.Find("myguild/right/log").gameObject;
			verifyObj = gameObject.transform.Find("myguild/right/verify").gameObject;
			//成员面板
			memberGrid = FindInChild<UIGrid>("myguild/right/member/container/oncenter");
			memberList.Add(memberGrid.transform.Find("member").GetComponent<Button>());
			//日志面板
			logGrid = FindInChild<UIGrid>("myguild/right/log/container/oncenter");
			logList.Add(logGrid.transform.Find("member").GetComponent<Button>());
			//审核面板
			verifyGrid = FindInChild<UIGrid>("myguild/right/verify/container/oncenter");
			verifyList.Add(verifyGrid.transform.Find("member").GetComponent<Button>());

			//其他公会面板
			otherGuildGrid = FindInChild<UIGrid>("otherguild/guildsinfo/container/oncenter");
			otherGuildList.Add(otherGuildGrid.transform.Find("member").GetComponent<Button>());
			btnPrevPage = FindInChild<Button>("otherguild/navbuttons/btn_prev");
			btnNextPage = FindInChild<Button>("otherguild/navbuttons/btn_next");

			//翻译相关
			labTogOne = FindInChild<UILabel>("common/top/ckb_liebiao1/label");
			labTogTwo = FindInChild<UILabel>("common/top/ckb_liebiao2/label");
			labOtherGuildRank = FindInChild<UILabel>("otherguild/guildsinfo/bt/rank");
			labOtherGuildName = FindInChild<UILabel>("otherguild/guildsinfo/bt/guildname");
			labOtherGuildLeader = FindInChild<UILabel>("otherguild/guildsinfo/bt/leader");
			labOtherGuildGrade = FindInChild<UILabel>("otherguild/guildsinfo/bt/grade");
			labOtherGuildPersonCnt = FindInChild<UILabel>("otherguild/guildsinfo/bt/personcount");
			labOtherGuildCurPage = FindInChild<UILabel>("otherguild/guildsinfo/bt/pageindex");
			labPrevPage = FindInChild<UILabel>("otherguild/navbuttons/btn_prev/label");
			labNextPage = FindInChild<UILabel>("otherguild/navbuttons/btn_next/label");
			labCreateGuildName = FindInChild<UILabel>("createguild/center/info/name");
			labWaste = FindInChild<UILabel>("createguild/center/info/waste"); 
			labConfirmCreate = FindInChild<UILabel>("createguild/center/btn_create/label");
			labMyGuildGrade = FindInChild<UILabel>("myguild/left/info/dj"); 
			labMyGuildRank = FindInChild<UILabel>("myguild/left/info/bp"); 
			labMyGuildPersonCnt = FindInChild<UILabel>("myguild/left/info/mz");
			labMyGuildExp = FindInChild<UILabel>("myguild/left/info/labexp");
			labMyGuildNotice = FindInChild<UILabel>("myguild/left/info/labnotice");
			labMyGuildOpt = FindInChild<UILabel>("myguild/left/btn_zlbb/label");
			labTogMember = FindInChild<UILabel>("myguild/right/toggles/zl/zhanli");
			labTogLog = FindInChild<UILabel>("myguild/right/toggles/rz/zhanli");
			labTogActivity = FindInChild<UILabel>("myguild/right/toggles/hd/zhanli");
			labTogVerify = FindInChild<UILabel>("myguild/right/toggles/sh/zhanli");
			labMemberName = FindInChild<UILabel>("myguild/right/member/bt/name");
			labMemberGrade = FindInChild<UILabel>("myguild/right/member/bt/grade");
			labMemberPos = FindInChild<UILabel>("myguild/right/member/bt/job");
			labMemberExp = FindInChild<UILabel>("myguild/right/member/bt/exp");
			labMemberDevote = FindInChild<UILabel>("myguild/right/member/bt/devote");
			labMemberLogin = FindInChild<UILabel>("myguild/right/member/bt/logintime");
			labLogMemberName = FindInChild<UILabel>("myguild/right/log/bt/name");
			labLogDevote = FindInChild<UILabel>("myguild/right/log/bt/devote");
			labLogTime = FindInChild<UILabel>("myguild/right/log/bt/time");
			labVerifyList = FindInChild<UILabel>("myguild/right/verify/bt/applylist");
			labVerifyGrade = FindInChild<UILabel>("myguild/right/verify/bt/grade");
			labVerifyTime = FindInChild<UILabel>("myguild/right/verify/bt/applytime");
			labVerifyOpt = FindInChild<UILabel>("myguild/right/verify/bt/operate");

			//事件绑定
			btnClose.onClick = CloseOnClick;
			btnCreateGuild.onClick = CreateGuildClick;
			btnGuildManage.onClick = GuildManageOnClick;
			btnPrevPage.onClick = PrevPageOnClick;
			btnNextPage.onClick = NextPageOnClick;

			togOne.onChange.Add(new EventDelegate(OneOnClick));
			togTwo.onChange.Add(new EventDelegate(TwoOnClick));

			togMember.onChange.Add(new EventDelegate(MemberOnClick));
			togLog.onChange.Add(new EventDelegate(LogOnClick));
			togActivities.onChange.Add(new EventDelegate(ActivitiesOnClick));
			togVerify.onChange.Add(new EventDelegate(VerifyOnClick));

			InitLabel();
		}

		private void InitLabel()
		{
			SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeGuild);
			string gold = StringUtils.GetValueListFromString(priceVo.gold)[0];
			string[] param = {gold};
			labWaste.text = LanguageManager.GetWord("Guild.CreateWaste", param);

			labOtherGuildRank.text = LanguageManager.GetWord("Guild.Rank");
			labOtherGuildName.text = LanguageManager.GetWord("Guild.GuildName");
			labOtherGuildLeader.text = LanguageManager.GetWord("Guild.Leader");
			labOtherGuildGrade.text = LanguageManager.GetWord("Guild.Grade");
			labOtherGuildPersonCnt.text = LanguageManager.GetWord("Guild.PersonCnt");
			labPrevPage.text = LanguageManager.GetWord("Guild.PrevPage");
			labNextPage.text = LanguageManager.GetWord("Guild.NextPage");
			labCreateGuildName.text = LanguageManager.GetWord("Guild.InputGuildName");
			labConfirmCreate.text = LanguageManager.GetWord("Guild.ConfirmCreate");
			labMyGuildGrade.text = LanguageManager.GetWord("Guild.MyGuildGrade");
			labMyGuildRank.text = LanguageManager.GetWord("Guild.MyGuildRank");
			labMyGuildPersonCnt.text = LanguageManager.GetWord("Guild.MyGuildPersonCnt");
			labMyGuildExp.text = LanguageManager.GetWord("Guild.MyGuildExp");
			labMyGuildNotice.text = LanguageManager.GetWord("Guild.MyGuildNotice");
			labMyGuildOpt.text = LanguageManager.GetWord("Guild.GuildOpt");
			labTogMember.text = LanguageManager.GetWord("Guild.TogMember");
			labTogLog.text = LanguageManager.GetWord("Guild.TogLog");
			labTogActivity.text = LanguageManager.GetWord("Guild.TogActivity");
			labTogVerify.text = LanguageManager.GetWord("Guild.TogVevify");
			labMemberName.text = LanguageManager.GetWord("Guild.MemberName");
			labMemberGrade.text = LanguageManager.GetWord("Guild.MemberGrade");
			labMemberPos.text = LanguageManager.GetWord("Guild.MemberPos");
			labMemberExp.text = LanguageManager.GetWord("Guild.MemberExp");
			labMemberDevote.text = LanguageManager.GetWord("Guild.MemberDevote");
			labMemberLogin.text = LanguageManager.GetWord("Guild.MemberLogin");
			labLogMemberName.text = LanguageManager.GetWord("Guild.MemberName");
			labLogDevote.text = LanguageManager.GetWord("Guild.LogDevote");
			labLogTime.text = LanguageManager.GetWord("Guild.LogTime");
			labVerifyList.text = LanguageManager.GetWord("Guild.VerifyList");
			labVerifyGrade.text = LanguageManager.GetWord("Guild.MemberGrade");
			labVerifyTime.text = LanguageManager.GetWord("Guild.VerifyTime");
			labVerifyOpt.text = LanguageManager.GetWord("Guild.VerifyOP");
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void MemberOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togMember))
				{
					memberObj.SetActive(true);
					logObj.SetActive(false);
					verifyObj.SetActive(false);

					Singleton<GuildMode>.Instance.GetGuildMembersInfo(MeVo.instance.guildId);
				}
			}	
		}

		private void LogOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togLog))
				{
					memberObj.SetActive(false);
					logObj.SetActive(true);
					verifyObj.SetActive(false);
				}
			}	
		}

		private void ActivitiesOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togActivities))
				{
					MessageManager.Show(LanguageManager.GetWord("Guild.NotSupport"));
				}
			}	
		}

		private void VerifyOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togVerify))
				{
					memberObj.SetActive(false);
					logObj.SetActive(false);
					verifyObj.SetActive(true);
					Singleton<GuildMode>.Instance.VerifyGuildList();
				}
			}	
		}

		private void OneOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togOne))
				{
					ShowTogOnePanel();
				}
			}	
		}

		private void ShowTogOnePanel()
		{
			createGuildObj.SetActive(false);
			myGuildObj.SetActive(false);
			otherGuildObj.SetActive(false);
			
			if (0 == MeVo.instance.guildId)
			{
				//搜索公会
				otherGuildObj.SetActive(true);
			}
			else
			{
				//我的公会
				myGuildObj.SetActive(true);
			}

			togOneActive = true;
		}
		
		private void TwoOnClick()
		{
			if (true == UIToggle.current.value)
			{
				if (UIToggle.current.Equals(togTwo))
				{
					ShowTogTwoPanel();
				}
			}
		}

		private void ShowTogTwoPanel()
		{
			createGuildObj.SetActive(false);
			myGuildObj.SetActive(false);
			otherGuildObj.SetActive(false);
			
			if (0 == MeVo.instance.guildId)
			{
				//创建公会
				createGuildObj.SetActive(true);
			}
			else
			{
				//其他公会
				otherGuildObj.SetActive(true);
			}

			togOneActive = false;
		}
		
		private void CreateGuildClick(GameObject go)
		{
			string name = inpGuildName.label.text.Trim();
			int length = name.Length;

			if (length < 2 || length > 7)			
			{
				MessageManager.Show(LanguageManager.GetWord("Guild.NameLengthInvalid"));
				return;
			}

            if (Singleton<ChatView>.Instance.ContainsFilter(name))
            {
                MessageManager.Show("命名中包含敏感词汇！");
                inpGuildName.label.text = "";
            }
            else
            {
                Singleton<GuildMode>.Instance.CreateGuild(name);
            }
		}

		private void GuildManageOnClick(GameObject go)
		{
			bool bPresident = Singleton<GuildMode>.Instance.IsPosition(GuildPosition.President);
			Singleton<GuildManageView>.Instance.ShowWindow(bPresident);
		}

		private void PrevPageOnClick(GameObject go)
		{
			if (Singleton<GuildMode>.Instance.PageIndex <= 1)
			{
				MessageManager.Show(LanguageManager.GetWord("Guild.InFirstPage"));
				return;
			}

			Singleton<GuildMode>.Instance.PageIndex--;
			Singleton<GuildMode>.Instance.GetOtherGuildInfo(Singleton<GuildMode>.Instance.PageIndex, false);
		}

		private void NextPageOnClick(GameObject go)
		{
			if (Singleton<GuildMode>.Instance.PageIndex >= Singleton<GuildMode>.Instance.PageCount)
			{
				MessageManager.Show(LanguageManager.GetWord("Guild.InLastPage"));
				return;
			}
			
			Singleton<GuildMode>.Instance.PageIndex++;
			Singleton<GuildMode>.Instance.GetOtherGuildInfo(Singleton<GuildMode>.Instance.PageIndex, false);
		}

		public override void OpenView()
		{			
			SendCommandsToServerWhileOpen();			
			base.OpenView();
		}

		private void SendCommandsToServerWhileOpen()
		{
			if (0 != MeVo.instance.guildId)
			{
				Singleton<GuildMode>.Instance.GetGuildBaseInfo(MeVo.instance.guildId);
				Singleton<GuildMode>.Instance.GetGuildMembersInfo(MeVo.instance.guildId);
				Singleton<GuildMode>.Instance.GetNoticeList();
			}

			Singleton<GuildMode>.Instance.GetOtherGuildInfo(1, false);
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			ShowEntry();
			UpdateViewInfo();

			togOne.value = true;
		}

		//根据玩家是否已加入公会显示不同入口
		private void ShowEntry()
		{
			string myGuildTile = LanguageManager.GetWord("Guild.MyGuild");
			string otherGuildTile = LanguageManager.GetWord("Guild.OtherGuild");
			string serachGuildTile = LanguageManager.GetWord("Guild.SearchGuild");
			string createGuildTile = LanguageManager.GetWord("Guild.CreateGuild");

			createGuildObj.SetActive(false);
			myGuildObj.SetActive(false);
			otherGuildObj.SetActive(false);

			//还没加入公会
			if (0 == MeVo.instance.guildId)
			{
				labOneTitle.text = serachGuildTile;
				labTwoTitle.text = createGuildTile;
			}
			//已加入公会
			else
			{
				labOneTitle.text = myGuildTile;
				labTwoTitle.text = otherGuildTile;
			}

			if (togOneActive)
			{
				ShowTogOnePanel();
			}
			else
			{
				ShowTogTwoPanel();
			}
		}

		private void UpdateViewInfo()
		{
			if (0 != MeVo.instance.guildId)
			{
				UpdateMyGuildInfo();
				UpdateMyGuildMemberList();
				UpdateMyGuildLogList();
				UpdateMyGuildVerifyList();
			}

			UpdateOtherGuildList();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated += UpdateMyGuildInfoHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateMyGuildMemberListHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateMyGuildLogListHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateMyGuildVerifyListHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateMyGuildVerifyFinishHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateOtherGuildListHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildIdChangedHandle;
			Singleton<GuildMode>.Instance.dataUpdated += UpdateGuildNoticeSetHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateMyGuildInfoHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateMyGuildMemberListHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateMyGuildLogListHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateMyGuildVerifyListHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateMyGuildVerifyFinishHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateOtherGuildListHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildIdChangedHandle;
			Singleton<GuildMode>.Instance.dataUpdated -= UpdateGuildNoticeSetHandle;
		}

		private void UpdateMyGuildVerifyFinishHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_VEVIRY_FINISHED == type)			    
			{
				Singleton<GuildMode>.Instance.VerifyGuildList();
			}
		}

		private void UpdateGuildIdChangedHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_ID_CHANGED == type)			    
			{
				CloseView();
			}
		}

		private void UpdateGuildNoticeSetHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_SET_NOTICEOK == type)			    
			{
				Singleton<GuildMode>.Instance.GetGuildBaseInfo(MeVo.instance.guildId);
			}
		}

		private void UpdateMyGuildInfoHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_INFO == type)			    
			{
				UpdateMyGuildInfo();
			}
		}

		private void UpdateMyGuildMemberListHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_GUILD_MEMBERS == type)			    
			{
				UpdateMyGuildMemberList();
			}
		}

		private void UpdateMyGuildLogListHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_LOG_LIST == type)			    
			{
				UpdateMyGuildLogList();
			}
		}

		private void UpdateMyGuildVerifyListHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_VERIFY_LIST == type)			    
			{
				UpdateMyGuildVerifyList();
			}
		}

		private void UpdateOtherGuildListHandle(object sender, int type)
		{
			if (Singleton<GuildMode>.Instance.UPDATE_OTHERGUILD_LIST == type)			    
			{
				UpdateOtherGuildList();
			}
		}

		//更新我的公会基本信息
		private void UpdateMyGuildInfo()
		{
			GuildBasicInfoMsg_31_6 myGuildInfo = Singleton<GuildMode>.Instance.GetGuildItem(MeVo.instance.guildId);

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

		//更新我的公会成员列表
		private void UpdateMyGuildMemberList()
		{
			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(MeVo.instance.guildId);

			if (null == dataList)
			{
				return;
			}

			bool bPresident = Singleton<GuildMode>.Instance.IsPosition(GuildPosition.President);
			bool bVicePresident = Singleton<GuildMode>.Instance.IsPosition(GuildPosition.VicePresident);

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

					if (bPresident)
					{
						memberList[i].onClick = MemberRuleOnClick;
					}
				}
				else
				{
					memberList[i].onClick = null;
					memberList[i].SetActive(false);
				}
			}
			
			memberGrid.repositionNow = true;

			//审核面板、公会管理是否打开
			bool bShow = (bPresident || bVicePresident); 
			togVerify.gameObject.SetActive(bShow);
		}

		private void MemberRuleOnClick(GameObject go)
		{
			Button currentItem = go.GetComponent<Button>();			
			
			for (memberIndex=0; memberIndex<memberList.Count; memberIndex++)
			{
				if (memberList[memberIndex].Equals(currentItem))
				{
					break;
				}
			}
			
			if (memberIndex >= memberList.Count)
			{
				return;
			}

			List<PGuildMember> dataList = Singleton<GuildMode>.Instance.GetGuildMembersList(MeVo.instance.guildId);
			PGuildMember item = dataList[memberIndex];

			if ((byte)GuildPosition.President != item.position)
			{
				Singleton<GuildRuleView>.Instance.ShowWindow(item);
			}
		}

		//更新日志列表
		private void UpdateMyGuildLogList()
		{
			List<PGuildLog> dataList = Singleton<GuildMode>.Instance.LogList;
			
			if (null == dataList)
			{
				return;
			}
			
			int curCount = logList.Count;
			int targetCount = dataList.Count;
			
			//增加新Button
			for (int i=0; i<(targetCount-curCount); i++)				
			{
				GameObject newGo = GameObject.Instantiate(logList[0].gameObject) as GameObject;
				newGo.gameObject.name = logList[0].gameObject.name;
				newGo.transform.parent = logList[0].transform.parent.transform;
				newGo.transform.localPosition = Vector3.zero;
				newGo.transform.localRotation = Quaternion.identity;
				newGo.transform.localScale = Vector3.one;
				NGUITools.SetLayer(newGo, LayerMask.NameToLayer("UI"));
				
				logList.Add(newGo.GetComponent<Button>());
			}
			
			//显示隐藏相应的Buttton
			for (int i=0; i<logList.Count; i++)
			{
				if (i<targetCount)
				{
					logList[i].SetActive(true);					
					PGuildLog item = dataList[i];
					
					UILabel labName = logList[i].gameObject.transform.Find("name").GetComponent<UILabel>();
					UILabel labDevote = logList[i].gameObject.transform.Find("devote").GetComponent<UILabel>();
					UILabel labTime = logList[i].gameObject.transform.Find("time").GetComponent<UILabel>();
					
					labName.text = item.name;
					string[] param = {item.repu.ToString(), item.exp.ToString()};
					labDevote.text = LanguageManager.GetWord("Guild.DevoteInfo", param);
					labTime.text = Singleton<GuildMode>.Instance.GetLogoutTimeStr(TimeUtil.GetElapsedTime(item.time));
				}
				else
				{
					logList[i].SetActive(false);
				}
			}
			
			logGrid.repositionNow = true;
		}

		//更新审核列表
		private void UpdateMyGuildVerifyList()
		{
			List<PGuildApplyMember> dataList = Singleton<GuildMode>.Instance.VerifyList;
			
			if (null == dataList)
			{
				return;
			}
			
			int curCount = verifyList.Count;
			int targetCount = dataList.Count;
			
			//增加新Button
			for (int i=0; i<(targetCount-curCount); i++)				
			{
				GameObject newGo = GameObject.Instantiate(verifyList[0].gameObject) as GameObject;
				newGo.gameObject.name = verifyList[0].gameObject.name;
				newGo.transform.parent = verifyList[0].transform.parent.transform;
				newGo.transform.localPosition = Vector3.zero;
				newGo.transform.localRotation = Quaternion.identity;
				newGo.transform.localScale = Vector3.one;
				NGUITools.SetLayer(newGo, LayerMask.NameToLayer("UI"));
				
				verifyList.Add(newGo.GetComponent<Button>());
			}
			
			//显示隐藏相应的Buttton
			for (int i=0; i<verifyList.Count; i++)
			{
				if (i<targetCount)
				{
					verifyList[i].SetActive(true);					
					PGuildApplyMember item = dataList[i];
					
					UILabel labName = verifyList[i].gameObject.transform.Find("name").GetComponent<UILabel>();
					UILabel labGrade = verifyList[i].gameObject.transform.Find("grade").GetComponent<UILabel>();
					UILabel labApplyTime = verifyList[i].gameObject.transform.Find("applytime").GetComponent<UILabel>();
					Button btnAccept = verifyList[i].gameObject.transform.Find("btn_accept").GetComponent<Button>();
					Button btndeny = verifyList[i].gameObject.transform.Find("btn_deny").GetComponent<Button>();
					btnAccept.onClick = AcceptOnClick;
					btndeny.onClick = DenyOnClick;
					btnAccept.label.text = LanguageManager.GetWord("Guild.Accept");
					btndeny.label.text = LanguageManager.GetWord("Guild.Deny");
					
					labName.text = item.name;
					labGrade.text = item.lvl.ToString();
					labApplyTime.text = TimeUtil.GetTimeYyyymmdd(item.time);
				}
				else
				{
					verifyList[i].SetActive(false);
				}
			}
			
			verifyGrid.repositionNow = true;
		}

		//同意审核按钮
		private void AcceptOnClick(GameObject go)
		{
			Verify(go, true);
		}

		//拒绝审核按钮
		private void DenyOnClick(GameObject go)
		{
			Verify(go, false);
		}

		//审核
		private void Verify(GameObject go, bool result)
		{
			Button currentItem = go.transform.parent.GetComponent<Button>();			
			int index;

			for (index=0; index<verifyList.Count; index++)
			{
				if (verifyList[index].Equals(currentItem))
				{
					break;
				}
			}
			
			if (index >= verifyList.Count)
			{
				return;
			}
			
			PGuildApplyMember item = Singleton<GuildMode>.Instance.VerifyList[index];
			Singleton<GuildMode>.Instance.Verify(item.id, result);
		}

		//更新审核列表
		private void UpdateOtherGuildList()
		{
			List<PGuildBase> dataList = Singleton<GuildMode>.Instance.OtherGuildList;
			
			if (null == dataList)
			{
				return;
			}
			
			int curCount = otherGuildList.Count;
			int targetCount = dataList.Count;
			
			//增加新Button
			for (int i=0; i<(targetCount-curCount); i++)				
			{
				GameObject newGo = GameObject.Instantiate(otherGuildList[0].gameObject) as GameObject;
				newGo.gameObject.name = otherGuildList[0].gameObject.name;
				newGo.transform.parent = otherGuildList[0].transform.parent.transform;
				newGo.transform.localPosition = Vector3.zero;
				newGo.transform.localRotation = Quaternion.identity;
				newGo.transform.localScale = Vector3.one;
				NGUITools.SetLayer(newGo, LayerMask.NameToLayer("UI"));
				
				otherGuildList.Add(newGo.GetComponent<Button>());
			}
			
			//显示隐藏相应的Buttton
			for (int i=0; i<otherGuildList.Count; i++)
			{
				if (i<targetCount)
				{
					otherGuildList[i].SetActive(true);					
					PGuildBase item = dataList[i];
					
					UILabel labRank = otherGuildList[i].gameObject.transform.Find("rank").GetComponent<UILabel>();
					UILabel labName = otherGuildList[i].gameObject.transform.Find("guildname").GetComponent<UILabel>();
					UILabel labLeader = otherGuildList[i].gameObject.transform.Find("leader").GetComponent<UILabel>();
					UILabel labGrade = otherGuildList[i].gameObject.transform.Find("grade").GetComponent<UILabel>();
					UILabel labPersonCnt = otherGuildList[i].gameObject.transform.Find("personcount").GetComponent<UILabel>();
					Button btnShowGuild = otherGuildList[i].gameObject.transform.Find("btn_show").GetComponent<Button>();
					btnShowGuild.onClick = ShowGuildOnClick;
					btnShowGuild.label.text = LanguageManager.GetWord("Guild.ShowGuild");
					
					labRank.text = item.lvl.ToString();
					labName.text = item.name;
					labLeader.text = item.ownerName;
					labGrade.text = item.rank.ToString();
					labPersonCnt.text = item.memberNum.ToString();
				}
				else
				{
					otherGuildList[i].SetActive(false);
				}
			}

			//当前页数
			int pageIndex = Singleton<GuildMode>.Instance.PageIndex;

			if (pageIndex > Singleton<GuildMode>.Instance.PageCount)
			{
				pageIndex = 0;
			}

			string[] param = {pageIndex.ToString(), Singleton<GuildMode>.Instance.PageCount.ToString()};
			labOtherGuildCurPage.text = LanguageManager.GetWord("Guild.CurPage", param);
			
			otherGuildGrid.repositionNow = true;
		}

		//显示其他公会
		private void ShowGuildOnClick(GameObject go)
		{
			Button currentItem = go.transform.parent.GetComponent<Button>();			
			int index;
			
			for (index=0; index<otherGuildList.Count; index++)
			{
				if (otherGuildList[index].Equals(currentItem))
				{
					break;
				}
			}
			
			if (index >= otherGuildList.Count)
			{
				return;
			}			

			uint guildId = Singleton<GuildMode>.Instance.OtherGuildList[index].id;
			Singleton<GuildSeeView>.Instance.ShowWindow(guildId);
		}


	}

}
