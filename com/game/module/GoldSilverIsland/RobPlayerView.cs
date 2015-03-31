//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：RobPlayerView
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
using com.game.module.map;
using com.game.vo;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.consts;

namespace Com.Game.Module.GoldSilverIsland
{
	public class RobPlayerView : BaseView<RobPlayerView>   
	{		
		public override string url { get { return "UI/GoldSilverIsland/RobRewardView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		private Button btnClose;
		private Button btnRob;
		private UILabel labTitle;
		private UISprite sprHead;
		private UILabel labLevel;
		private UILabel labName;
		private UILabel labRobRemain;
		private UILabel labPowerDes;
		private UILabel labPower;
		private UISprite sprAssistHead;
		private UILabel labAssistNoMan;
		private UILabel labAssistLvl;
		private UILabel labAssistName;
		private UILabel labAssistPowerDes;
		private UILabel labAssistPower;
		private UILabel labRewardGold;
		private UILabel labRewardRep;
		private UILabel labRobbedCntDes;
		private UILabel labRobRewardDes;

		private uint playerId;

		protected override void Init()
		{
			btnClose = FindInChild<Button>("center/topright/btn_close");
			btnRob = FindInChild<Button>("center/btn_sy");

			labTitle = FindInChild<UILabel>("center/title/title");

			sprHead = FindInChild<UISprite>("center/role/touxiang/tou");
			labLevel = FindInChild<UILabel>("center/role/shijian");
			labName = FindInChild<UILabel>("center/role/tg");
			labRobbedCntDes = FindInChild<UILabel>("center/role/labbj");
			labRobRemain = FindInChild<UILabel>("center/role/shuzi");
			labPowerDes = FindInChild<UILabel>("center/role/labpower");
			labPower = FindInChild<UILabel>("center/role/power");

			sprAssistHead = FindInChild<UISprite>("center/assist/touxiang/tou");
			labAssistNoMan = FindInChild<UILabel>("center/assist/touxiang/noman");
			labAssistLvl = FindInChild<UILabel>("center/assist/shijian");
			labAssistName = FindInChild<UILabel>("center/assist/tg");
			labAssistPowerDes = FindInChild<UILabel>("center/assist/labpower");
			labAssistPower = FindInChild<UILabel>("center/assist/power");

			labRewardGold = FindInChild<UILabel>("center/reward/szyb");
			labRewardRep = FindInChild<UILabel>("center/reward/szlq");
			labRobRewardDes = FindInChild<UILabel>("center/reward/labjc");

			btnClose.onClick = CloseOnClick;
			btnRob.onClick = RobOnClick;

			InitLabel();
			SetToLayerMode();
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("GoldSilverIsland.RobbedPlayerInfo");
			labRobbedCntDes.text = LanguageManager.GetWord("GoldSilverIsland.RobbedCnt");
			labRobRewardDes.text = LanguageManager.GetWord("GoldSilverIsland.RobReward");
			btnRob.label.text = LanguageManager.GetWord("GoldSilverIsland.StartRob");
			labPowerDes.text = LanguageManager.GetWord("GoldSilverIsland.Power");
			labAssistPowerDes.text = LanguageManager.GetWord("GoldSilverIsland.Power");
			labAssistNoMan.text = LanguageManager.GetWord("GoldSilverIsland.NoAssist");
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

		private void RobOnClick(GameObject go)
		{
			Singleton<GoldSilverIslandMode>.Instance.StartRob(playerId);
			CloseView();
		}

		public void ShowWindow(uint playerId)
		{
			this.playerId = playerId;

			OpenView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();		
			UpdateViewInfo();	
			SendCommandsToServerWhileOpen();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GoldSilverIslandMode>.Instance.RobRewardPreview(playerId);
		}

		private void UpdateViewInfo()
		{
			UpdatePlayerInfo();
		}

		private void UpdateRobPlayerRewardPreviewHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ROBBED_PLAYER_PREREWARD == type)
			{
				UpdateRobPlayerRewardPreview();
			}
		}

		private void UpdateRobPlayerRewardPreview()
		{
			labRewardGold.text = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayerPreGold.ToString();
			labRewardRep.text = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayerPreRep.ToString();
		}

		private void UpdatePlayerInfo()
		{
			PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.GetPlayer(playerId);

			if (null == player)
			{
				return;
			}

			labName.text = player.name;
			labRobRemain.text = player.robTimes.ToString() + "/" + GameConst.MaxRobTimes;
			labLevel.text = "Lv." + player.lvl.ToString();
			labPower.text = player.fightPoint.ToString();  //玩家战斗力
			sprHead.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
			sprHead.spriteName = Singleton<RoleMode>.Instance.GetPlayerHeadSpriteName(player.job);	

			if (string.Empty != player.assistName)
			{
				labAssistName.text = player.assistName;
				labAssistLvl.text = "Lv." + player.assistLvl.ToString();
				labAssistPower.text = player.assistFightPoint.ToString(); //协助者战斗力
				labAssistPowerDes.SetActive(true);
				labAssistNoMan.SetActive(false);
			}
			else
			{
				labAssistName.text = "";
				labAssistLvl.text = "";
				labAssistPower.text = ""; //协助者战斗力
				labAssistPowerDes.SetActive(false);
				labAssistNoMan.SetActive(true);
			}

			sprAssistHead.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
			sprAssistHead.spriteName = Singleton<RoleMode>.Instance.GetPlayerHeadSpriteName(player.assistJob); //协助者职业	
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdatePlayerRobTimesHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateRobPlayerRewardPreviewHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdatePlayerRobTimesHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateRobPlayerRewardPreviewHandle;	
		}

		private void UpdatePlayerRobTimesHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_PLAYER_ROB_TIMES == type)
			{
				UpdatePlayerRobTimes();
			}
		}

		private void UpdatePlayerRobTimes()
		{
			PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.GetPlayer(playerId);
			labRobRemain.text = player.robTimes.ToString() + "/" + GameConst.MaxRobTimes;
		}
		
	}
}
