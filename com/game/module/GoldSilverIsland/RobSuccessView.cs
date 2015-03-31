//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：RobSuccessView
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
using Com.Game.Module.Arena;

namespace Com.Game.Module.GoldSilverIsland
{
	public class RobSuccessView : BaseView<RobSuccessView>   
	{		
		public override string url { get { return "UI/GoldSilverIsland/RobSuccessView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		private Button btnLeave;
		private UILabel labRewardGold;
		private UILabel labRewardRep;
		private UILabel labRewardGoldDes;
		private UILabel labRewardRepDes;

		protected override void Init()
		{
			btnLeave = FindInChild<Button>("center/btn_sy");
			labRewardGold = FindInChild<UILabel>("center/nr/szyb");
			labRewardRep = FindInChild<UILabel>("center/nr/szsw");
			labRewardGoldDes = FindInChild<UILabel>("center/nr/labyb");
			labRewardRepDes = FindInChild<UILabel>("center/nr/labsw");

			btnLeave.onClick = LeaveOnClick;

			InitLabel();
			SetToLayerMode();
		}

		private void InitLabel()
		{
			labRewardGoldDes.text = LanguageManager.GetWord("GoldSilverIsland.GetGold");
			labRewardRepDes.text = LanguageManager.GetWord("GoldSilverIsland.GetRep");
			btnLeave.label.text = LanguageManager.GetWord("GoldSilverIsland.Leave");
		}
		
		//设置对象层级
		private void SetToLayerMode()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("Mode")); 
		}

		private void LeaveOnClick(GameObject go)
		{
//			Singleton<ArenaControl>.Instance.ChallengeEnd();  //----modify by lixi
			Singleton<GoldSilverIslandControl>.Instance.ChallengeEnd();
			CloseView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			UpdateViewInfo();	
		}

		private void UpdateViewInfo()
		{
			UpdateRobReward();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateRobRewardHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateRobRewardHandle;
		}

		private void UpdateRobRewardHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ROB_REWARD == type)
			{
				UpdateRobReward();
			}
		}

		private void UpdateRobReward()
		{
			labRewardGold.text = "+" + Singleton<GoldSilverIslandMode>.Instance.RobRewardGold.ToString();
			labRewardRep.text = "+" + Singleton<GoldSilverIslandMode>.Instance.RobRewardRep.ToString();
		}
		
	}
}
