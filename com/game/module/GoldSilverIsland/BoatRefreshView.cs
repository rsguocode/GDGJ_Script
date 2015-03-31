//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：BoatRefreshView
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
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
using com.game.utils;
using Com.Game.Module.Role;
using Com.Game.Module.Manager;
using com.game.vo;

namespace Com.Game.Module.GoldSilverIsland
{
	public class BoatRefreshView : BaseView<BoatRefreshView>   
	{		
		public override string url { get { return "UI/GoldSilverIsland/BoatRefreshView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		private UIAtlas atlas;
		private float timeOut = 8f;

		private const int maxShip = 5;
		private const byte normalRandom = 0;
		private const byte appointHighest = 1;
		private bool loopRefresh = false;
		private bool inLoopRefreshOp = false;  

		private Button btnClose;
		private UILabel labShipTime;
		private UILabel labShipRewardGold;
		private UILabel labShipRewardRep;
		private UIToggle togShipRefreshToHigh;
		private Button btnShipAppoint;
     
		private Button btnInvite;
		private UILabel labAssistLvl;
		private UILabel labAssistName;
		private UILabel labAssistForce;
		private UILabel labAssistRemainCnt;
		private UILabel labTitle;
		private UILabel labShip1;
		private UILabel labShip2;
		private UILabel labShip3;
		private UILabel labShip4;
		private UILabel labShip5;
		private UILabel labShipTimeDes;
		private UILabel labShipRewardDes;
		private UILabel labLoopRefresh;
		private UILabel labAssistNameDes;
		private UILabel labAssistForceDes;
		private UILabel labAssistRemainCntDes;
		private UISprite sprAssistHead;
		private UILabel labAssistNoMan;

		private UISprite[] sprKuangs = new UISprite[maxShip];
		private UISprite[] sprIcons = new UISprite[maxShip];
		private UISprite[] sprBgs = new UISprite[maxShip];
		private UIToggle[] chkShipSelect = new UIToggle[maxShip];
		private bool startRefresh = false;
		private bool forceStopLoop = false; 
		private bool inRefreshEndStage = false;  //刷新收尾阶段
		private bool freshBtnClicked = false;
		private int boatIndex = 0;
		private float lastTime;

        //指引要用的
        public Button btnShipRefresh;  //刷新
        public Button btnStartSail;  //出发
	    public EventDelegate.Callback RefreshFinishCallback;

		protected override void Init()
		{			
			btnClose = FindInChild<Button>("center/topright/btn_close");
			labShipTime = FindInChild<UILabel>("center/xx/czxx/fz");
			labShipRewardGold = FindInChild<UILabel>("center/xx/czxx/jlyb");
			labShipRewardRep = FindInChild<UILabel>("center/xx/czxx/jllq");
			btnShipRefresh = FindInChild<Button>("center/bottom/btn_sx");
			togShipRefreshToHigh = FindInChild<UIToggle>("center/xx/czxx/sx/chkRefresh");
			btnShipAppoint = FindInChild<Button>("center/lb/btn_zd");
			btnStartSail = FindInChild<Button>("center/bottom/btn_ch");
			btnInvite = FindInChild<Button>("center/bottom/btn_yq");
			labAssistLvl = FindInChild<UILabel>("center/bottom/level");
			labAssistName = FindInChild<UILabel>("center/bottom/name");
			labAssistForce = FindInChild<UILabel>("center/bottom/power");
			labAssistRemainCnt = FindInChild<UILabel>("center/bottom/remainCnt");
			labTitle = FindInChild<UILabel>("center/title/hjbx");
			labShip1 = FindInChild<UILabel>("center/lb/chuan/chuan1/name");
			labShip2 = FindInChild<UILabel>("center/lb/chuan/chuan2/name");
			labShip3 = FindInChild<UILabel>("center/lb/chuan/chuan3/name");
			labShip4 = FindInChild<UILabel>("center/lb/chuan/chuan4/name");
			labShip5 = FindInChild<UILabel>("center/lb/chuan/chuan5/name");
			labShipTimeDes = FindInChild<UILabel>("center/xx/czxx/sj");
			labShipRewardDes = FindInChild<UILabel>("center/xx/czxx/jl");
			labLoopRefresh = FindInChild<UILabel>("center/xx/czxx/sx/chkRefresh/Label");
			labAssistNameDes = FindInChild<UILabel>("center/xx/xzxx/xzz");
			labAssistForceDes = FindInChild<UILabel>("center/bottom/labPower");
			labAssistRemainCntDes = FindInChild<UILabel>("center/bottom/labRemainCnt");
			sprAssistHead = FindInChild<UISprite>("center/bottom/head/icon");
			sprAssistHead.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
			labAssistNoMan = FindInChild<UILabel>("center/bottom/head/noman");

			togShipRefreshToHigh.value = false;

			for (int i=1; i<=maxShip; i++)
			{
				sprKuangs[i-1] = FindInChild<UISprite>("center/lb/chuan/chuan" + i.ToString() + "/kuang");
				sprKuangs[i-1].gameObject.SetActive(false);
				sprIcons[i-1] = FindInChild<UISprite>("center/lb/chuan/chuan" + i.ToString() + "/icon");
				sprBgs[i-1] = FindInChild<UISprite>("center/lb/chuan/chuan" + i.ToString() + "/background");
				chkShipSelect[i-1] = FindInChild<UIToggle>("center/lb/chuan/chuan" + i.ToString() + "/chkSelect");
				FindInChild<UISprite>("center/lb/chuan/chuan" + i.ToString() + "/icon").atlas = atlas;;
			}
			
			btnClose.onClick = CloseOnClick;
			btnShipRefresh.onClick = ShipRefreshOnClick;
			togShipRefreshToHigh.onChange.Add(new EventDelegate(ShipRefreshToHighOnClick));
			btnShipAppoint.onClick = ShipAppointOnClick;
			btnStartSail.onClick = StartSailOnClick;
			btnInvite.onClick = InviteOnClick;
			
			InitLabel();
			InitShipInfo();

			SetToLayerMode();
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("GoldSilverIsland.Title");
			labShip1.text = LanguageManager.GetWord("GoldSilverIsland.Ship1");
			labShip2.text = LanguageManager.GetWord("GoldSilverIsland.Ship2");
			labShip3.text = LanguageManager.GetWord("GoldSilverIsland.Ship3");
			labShip4.text = LanguageManager.GetWord("GoldSilverIsland.Ship4");
			labShip5.text = LanguageManager.GetWord("GoldSilverIsland.Ship5");
			labShipTimeDes.text = LanguageManager.GetWord("GoldSilverIsland.ShipTime");
			labShipRewardDes.text = LanguageManager.GetWord("GoldSilverIsland.ShipReward");
			labLoopRefresh.text = LanguageManager.GetWord("GoldSilverIsland.ShipLoopRefresh");
			labAssistNameDes.text = LanguageManager.GetWord("GoldSilverIsland.AssistName");
			labAssistForceDes.text = LanguageManager.GetWord("GoldSilverIsland.Power");
			labAssistRemainCntDes.text = LanguageManager.GetWord("GoldSilverIsland.AssistRemainCnt");
			btnShipAppoint.label.text = LanguageManager.GetWord("GoldSilverIsland.Appoint");
			btnShipRefresh.label.text = LanguageManager.GetWord("GoldSilverIsland.Refresh");
			btnInvite.label.text = LanguageManager.GetWord("GoldSilverIsland.Invite");
			btnStartSail.label.text = LanguageManager.GetWord("GoldSilverIsland.StartSail");
			labAssistNoMan.text = LanguageManager.GetWord("GoldSilverIsland.NoAssist");
		}

		private void InitShipInfo()
		{
			for (int i=1; i<=maxShip; i++)
			{
				UILabel labShipName = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/name");
				UILabel labShipTimeDesc = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/labsj");
				UILabel labShipRewardDesc = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/labreward");
				UILabel labShipTime = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/sj");
				UILabel labShipRewardDiam = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/rewardjb");
				UILabel labShipRewardRep = FindInChild<UILabel>("center/lb/chuan/chuan" + i.ToString() + "/rewardsw");

				labShipName.text = LanguageManager.GetWord("GoldSilverIsland.Ship" + i.ToString());
				labShipTimeDes.text = LanguageManager.GetWord("GoldSilverIsland.ShipTime");
				labShipRewardDes.text = LanguageManager.GetWord("GoldSilverIsland.ShipReward");

				SysForest shipInfo = BaseDataMgr.instance.GetSysForestVo((uint)i);
				labShipTime.text = (shipInfo.time/60).ToString() + LanguageManager.GetWord("GoldSilverIsland.Minute");
				labShipRewardDiam.text = shipInfo.gold.ToString();
				labShipRewardRep.text = shipInfo.repu.ToString();
			}
		}

		//设置对象层级
		private void SetToLayerMode()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("Mode")); 
		}

		private bool waiting
		{
			get {return (inLoopRefreshOp || startRefresh || inRefreshEndStage);}
		}

		private void CloseOnClick(GameObject go)
		{
			if (waiting)
			{
				return;
			}

			Singleton<IslandMainView>.Instance.ShowSearchRefreshButtons(true);
			CloseView();
		}

		private void ShipRefreshOnClick(GameObject go)
		{
			if (waiting)
			{
				if (inLoopRefreshOp)
				{
					//正处于强制停止循环刷新中
					if (forceStopLoop)
					{
						return;
					}

					forceStopLoop = true;
				}
				else
				{
					return;
				}
			}

			if (maxShip == Singleton<GoldSilverIslandMode>.Instance.Grade)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.ShipHighest"));
			}
			else
			{
				int diamCnt = (Singleton<GoldSilverIslandMode>.Instance.RefreshTimes+1) * 2;
				string[] param = {diamCnt.ToString()};

				loopRefresh = togShipRefreshToHigh.value;

				if (loopRefresh)
				{
					btnShipRefresh.label.text = LanguageManager.GetWord("GoldSilverIsland.StopRefresh");
					RefreshCallBack();
				}
				else
				{
					if (MeVo.instance.diamond < diamCnt && MeVo.instance.bindingDiamond < diamCnt)
					{
						MessageManager.Show(LanguageManager.GetWord("CommonTips.DiamondNotEnough"));
					}
					else
					{
						ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("GoldSilverIsland.ShipRefreshDiamNeed", param), ConfirmCommands.OK_CANCEL, RefreshCallBack);
					}
				}
			}
		}

		private void RefreshCallBack()
		{
			freshBtnClicked = true;
			inLoopRefreshOp = loopRefresh;

			if (inLoopRefreshOp)
			{
				ShowLoopRefreshTips();
			}

			StartRefreshEffect();
			vp_Timer.In(3f, ContinueRefreshBoat, 1, 1f);
		}

		private void StartRefreshEffect()
		{
			startRefresh = true;
			boatIndex = Singleton<GoldSilverIslandMode>.Instance.Grade-1;
		}

		private void StopRefreshEffect()
		{
			startRefresh = false;
		}

		private void ShipAppointOnClick(GameObject go)
		{
			if (waiting)
			{
				return;
			}

			if (maxShip == Singleton<GoldSilverIslandMode>.Instance.Grade)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.ShipHighest"));
			}
			else
			{
				SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeAppointHighestShip);
				string shipName = LanguageManager.GetWord("GoldSilverIsland.Ship5");
				string diam = StringUtils.GetValueListFromString(priceVo.diam)[0];
				string[] param = {shipName, diam};
				ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("GoldSilverIsland.AppointWaste", param), ConfirmCommands.OK_CANCEL, AppointHighestShip);	
			}
		}

		private void AppointHighestShip()
		{
			Singleton<GoldSilverIslandMode>.Instance.RandomGrade(appointHighest);
		}

		private void StartSailOnClick(GameObject go)
		{
			if (waiting)
			{
				return;
			}

			Singleton<GoldSilverIslandMode>.Instance.StartAdventure();
			Singleton<GoldSilverIslandMode>.Instance.GetPlayersInfo();

			Singleton<IslandMainView>.Instance.ShowSearchButton(false);
			Singleton<IslandMainView>.Instance.ShowRefreshButton(true);
			Singleton<IslandMainView>.Instance.ShowAutoSearchGoldEffect();

			CloseView();
		}

		private void InviteOnClick(GameObject go)
		{
			Singleton<AssistFriendView>.Instance.OpenView();
		}		

		private void ShipRefreshToHighOnClick()
		{
		}

		public override void OpenView()
		{			
			SendCommandsToServerWhileOpen();
			
			base.OpenView();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GoldSilverIslandMode>.Instance.GetFriendAssitRemainCountList();
			Singleton<GoldSilverIslandMode>.Instance.GetRefreshCount();
			Singleton<GoldSilverIslandMode>.Instance.GetCurAssistInfo();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();		
			inLoopRefreshOp = false;
			freshBtnClicked = false;
			UpdateViewInfo();	
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateGradeRefreshHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateGradeRefreshErrorHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateFriendReplyHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateAssistInfoHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateGradeRefreshHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateGradeRefreshErrorHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateFriendReplyHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateAssistInfoHandle;
		}

		private void UpdateAssistInfoHandle(object sender, int type)
		{
			if ((Singleton<GoldSilverIslandMode>.Instance.UPDATE_ASSIST_INFO == type)
			    || (Singleton<GoldSilverIslandMode>.Instance.UPDATE_FRIEND_ASSIST_REMAIN == type))
			{
				UpdateAssistInfo();
			}
		}

		private void UpdateGradeRefreshHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_GRADE_REFRESH == type)
			{
				UpdateGradeRefresh();
			}
		}

		private void UpdateGradeRefreshErrorHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_GRADE_ERROR == type)
			{
				UpdateGradeRefreshError();
			}
		}

		private void UpdateFriendReplyHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_FRIEND_REPLY_FINISHED == type)
			{
				UpdateFriendReply();
			}
		}

		private void UpdateAssistInfo()
		{
			uint assistId = Singleton<GoldSilverIslandMode>.Instance.AssistId;

			if (0 != assistId)
			{
				PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.GetAssist(assistId);
				SetAssistInfo(item);
			}
			else
			{
				SetAssistInfo(null);
			}
		}

		private void SetAssistInfo(PWoodsFriendInfo item)
		{
			if (null != item)
			{
				labAssistLvl.text = "Lv." + item.lvl.ToString();
				labAssistName.text = item.name;
				labAssistForce.text = item.fightPoint.ToString();
				labAssistRemainCnt.text = item.remainTimes.ToString() + "/5";
				sprAssistHead.spriteName = Singleton<RoleMode>.Instance.GetPlayerHeadSpriteName(item.job); //协助者职业
				labAssistForceDes.SetActive(true);
				labAssistRemainCntDes.SetActive(true);
				labAssistNoMan.SetActive(false);
			}
			else
			{
				labAssistLvl.text = "";
				labAssistName.text = "";
				labAssistForce.text = "";
				labAssistRemainCnt.text = "";
				sprAssistHead.spriteName = "";
				labAssistForceDes.SetActive(false);
				labAssistRemainCntDes.SetActive(false);
				labAssistNoMan.SetActive(true);
			}
		}
		
		private void UpdateFriendReply()
		{
			uint assistId = Singleton<GoldSilverIslandMode>.Instance.SelectAssistId;
			byte friendReply = Singleton<GoldSilverIslandMode>.Instance.FriendReply;

			if ((byte)AssistReply.Accept == friendReply && 0 != assistId)
			{
				PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.GetAssist(assistId);
				SetAssistInfo(item);
			}
			else
			{
				SetAssistInfo(null);
			}
		}

		private void RestoreBoatShow()
		{
			foreach (UISprite item in sprKuangs)
			{
				item.gameObject.SetActive(false);
			}

			foreach (UIToggle item in chkShipSelect)
			{
				item.gameObject.SetActive(false);
			}
			
			foreach (UISprite item in sprIcons)
			{
				item.color = ColorConst.GRAY;
			}
			
			foreach (UISprite item in sprBgs)
			{
				item.color = ColorConst.GRAY;
			}

			byte grade = Singleton<GoldSilverIslandMode>.Instance.Grade;
			if (grade >= 1 && grade <= maxShip)
			{
				sprKuangs[grade-1].gameObject.SetActive(true);
				chkShipSelect[grade-1].gameObject.SetActive(true);
				chkShipSelect[grade-1].value = true;
				sprIcons[grade-1].color = Color.white;
				sprBgs[grade-1].color = Color.white;
				UpdateShipInfo(grade);
			}
		}

		private void StopRefresh()
		{
			inLoopRefreshOp = false;
			inRefreshEndStage = true;
			StopRefreshEffect();
		}

		private void ResetRefreshBtn()
		{
			forceStopLoop = false;
			btnShipRefresh.label.text = LanguageManager.GetWord("GoldSilverIsland.Refresh");
		}

		private void UpdateGradeRefresh()
		{
			byte grade = Singleton<GoldSilverIslandMode>.Instance.Grade;

			if (inLoopRefreshOp)
			{
				if (grade != maxShip)
				{
					ShowLoopRefreshTips();

					if (forceStopLoop)
					{
						StopRefresh();
					}
					else
					{
						vp_Timer.In(1f, ContinueRefreshBoat, 1, 1f);
					}
				}
				else
				{
					StopRefresh();
				}
			}
			else
			{
				StopRefresh();
			}
		}

		private void ContinueRefreshBoat()
		{
			Singleton<GoldSilverIslandMode>.Instance.RandomGrade(normalRandom);
			//启动超时定时器，避免服务器无返回
			vp_Timer.CancelAll("TimeOutCallback");
			vp_Timer.In(timeOut, TimeOutCallback, 1, 0);
		}

		private void TimeOutCallback()
		{
			if (startRefresh)
			{
				StopRefresh();
			}
		}

		private void ShowLoopRefreshTips()
		{
			int diamCnt = (Singleton<GoldSilverIslandMode>.Instance.RefreshTimes+1) * 2;
			string[] param = {diamCnt.ToString()};
			
			MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.RefreshDiamWaste", param));
		}

		private void UpdateGradeRefreshError()
		{
			StopRefresh();
		}

		private void UpdateShipInfo(byte grade)
		{
			SysForest shipInfo = BaseDataMgr.instance.GetSysForestVo((uint)grade);
			labShipTime.text = (shipInfo.time/60).ToString() + LanguageManager.GetWord("GoldSilverIsland.Minute");
			labShipRewardGold.text = shipInfo.gold.ToString();
			labShipRewardRep.text = shipInfo.repu.ToString();
		}

		private void UpdateViewInfo()
		{
			RestoreBoatShow();
			UpdateFriendReply();
		}

		public override void Update()
		{
			//是否船只刷新数据准备就绪，但是动画还没刷新完毕
			if (!inRefreshEndStage)
			{
				if (!startRefresh || forceStopLoop)
				{
					return;
				}
			}
			else
			{
				if (boatIndex == Singleton<GoldSilverIslandMode>.Instance.Grade-1)
				{
					inRefreshEndStage = false;
					SortRefreshEndStageFinish();
					return;
				}
			}

			float curTime = Time.time;
			if (curTime-lastTime > 0.2f)
			{
				lastTime = curTime;
				boatIndex++;
				boatIndex %= maxShip;
				SetBoatActive(boatIndex);
			}
		}

		private void SortRefreshEndStageFinish()
		{
			RestoreBoatShow();
			ResetRefreshBtn();
			ShowRefreshFinishedTips();
		    if (RefreshFinishCallback != null)
		    {
		        RefreshFinishCallback();
		        RefreshFinishCallback = null;
		    }
		}

		private void ShowRefreshFinishedTips()
		{
			if (!freshBtnClicked)
			{
				return;
			}

			if (loopRefresh)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.Congratuation"));
			}
			else
			{
				string section = "GoldSilverIsland.Ship" + Singleton<GoldSilverIslandMode>.Instance.Grade;
				string shipName = LanguageManager.GetWord(section);
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.ShipRefreshSucc", shipName));
			}
		}

		private void SetBoatActive(int index)
		{
			for (int i=0; i<maxShip; i++)
			{
				sprKuangs[i].gameObject.SetActive(false);
				chkShipSelect[i].gameObject.SetActive(false);
				sprIcons[i].color = ColorConst.GRAY;
				sprBgs[i].color = ColorConst.GRAY;
			}

			sprKuangs[index].gameObject.SetActive(true);
			sprIcons[index].color = Color.white;
			sprBgs[index].color = Color.white;
		}

		public void ShowWindow(UIAtlas atlas)
		{
			this.atlas = atlas;
			OpenView();
		}
		
	}
}
