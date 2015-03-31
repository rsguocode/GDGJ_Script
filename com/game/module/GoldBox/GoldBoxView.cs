//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldBoxView
//文件描述：黄金宝箱界面类
//创建者：黄江军
//创建日期：2014-02-11
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using com.game.module.Guide;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.module.effect;
using com.game.sound;
using Com.Game.Speech;

namespace Com.Game.Module.GoldBox
{
	public class GoldBoxView : BaseView<GoldBoxView> 
	{		
		public override string url { get { return "UI/GoldBox/GoldBoxView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		public Button btnClose;
		public Button btnOpen;
		private Button btnBatchOpen;
		private UILabel labTitle;
		private UILabel labSmallCrit;
		private UILabel labLargeCrit;
		private UILabel labWaste;
		private UILabel labRemainDisp;
		private UILabel labRemainCnt;
		private UITexture texGoldBox;
		private UILabel labDiamond;
		private UILabel labGold;

		protected override void Init()
		{			
			btnClose = FindInChild<Button>("center/topright/btn_close");
			btnOpen = FindInChild<Button>("center/btnOpen");
			btnBatchOpen = FindInChild<Button>("center/btnBatchOpen");
			labTitle = FindInChild<UILabel>("center/nr/hjbx");
			labSmallCrit = FindInChild<UILabel>("center/nr/xiaobaoji");
			labLargeCrit = FindInChild<UILabel>("center/nr/dabaoji");
			labWaste = FindInChild<UILabel>("center/nr/xh");
			labRemainDisp = FindInChild<UILabel>("center/nr/shenyu");
			labRemainCnt = FindInChild<UILabel>("center/nr/shuzi");
			texGoldBox = FindInChild<UITexture>("center/nr/tupian");
			labDiamond = FindInChild<UILabel>("center/waste/diamond/shuzi");
			labGold = FindInChild<UILabel>("center/waste/gold/shuzi");
			
			btnClose.onClick = CloseOnClick;
			btnOpen.onClick = OpenOnClick;
			btnBatchOpen.onClick = BatchOpenOnClick;
			
			InitLabel();
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("GoldBoxView.Title");
			labSmallCrit.text = LanguageManager.GetWord("GoldBoxView.SmallCrit");
			labLargeCrit.text = LanguageManager.GetWord("GoldBoxView.LargeCrit");
			labRemainDisp.text = LanguageManager.GetWord("GoldBoxView.Remain");
			btnOpen.label.text = LanguageManager.GetWord("GoldBoxView.OpenBox");
			btnBatchOpen.label.text = LanguageManager.GetWord("GoldBoxView.BatchOpenBox");
		}

		public override void OpenView()
		{			
			SendCommandsToServerWhileOpen();			
			base.OpenView();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GoldBoxMode>.Instance.GetGoldBuyInfo();
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();

			UpdateGoldInfo();
		}

		private void UpdateGoldInfo()
		{
			string[] param = {Singleton<GoldBoxMode>.Instance.DiamNeedsForOnce.ToString(),
							  Singleton<GoldBoxMode>.Instance.GoldNumNextBuy.ToString()};
			labWaste.text = LanguageManager.GetWord("GoldBoxView.Waste", param);
			labRemainCnt.text = Singleton<GoldBoxMode>.Instance.RemainBuyTimes.ToString();

			NGUITools.SetButtonEnabled(btnOpen, Singleton<GoldBoxMode>.Instance.RemainBuyTimes > 0);
			NGUITools.SetButtonEnabled(btnBatchOpen, Singleton<GoldBoxMode>.Instance.DiamNeedsForBatch > 0);

			labDiamond.text = param[0];
			labGold.text = param[1];
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void OpenOnClick(GameObject go)
		{
			if (!Singleton<GoldBoxTipsView>.Instance.NeedHide)
			{
				Singleton<GoldBoxTipsView>.Instance.ShowWindow(OpenView);
				CloseView();
			}
			else
			{
				Singleton<GoldBoxMode>.Instance.BuyGoldOnce();
			}
		}

		private void BatchOpenOnClick(GameObject go)
		{
			if (!Singleton<GoldBoxTipsView>.Instance.NeedHide)
			{
				Singleton<GoldBoxTipsView>.Instance.ShowWindow(OpenView, true);
				CloseView();
			}
			else
			{
				Singleton<GoldBoxMode>.Instance.BuyGoldBatch();
			}
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldBoxMode>.Instance.dataUpdated += updateGoldInfoHandle;	
			Singleton<GoldBoxMode>.Instance.dataUpdated += updateGoldGetTypeHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldBoxMode>.Instance.dataUpdated -= updateGoldInfoHandle;
			Singleton<GoldBoxMode>.Instance.dataUpdated -= updateGoldGetTypeHandle;
		}
		
		//金币信息获得回调
		private void updateGoldInfoHandle(object sender, int type)
		{
			if (Singleton<GoldBoxMode>.Instance.UPDATE_GOLD_INFO == type)
			{
				UpdateGoldInfo();
			}
		}

		//金币获得方式更新回调
		private void updateGoldGetTypeHandle(object sender, int type)
		{
			if (Singleton<GoldBoxMode>.Instance.UPDATE_GOLD_GET_TYPE == type)
			{
				UpdateGoldGetType();
			}
		}

		private void UpdateGoldGetType()
		{
			Vector3 effectPos = texGoldBox.transform.position;

			//小暴击特效
			if (GoldGetType.SmallCrit == (GoldGetType)Singleton<GoldBoxMode>.Instance.GoldGetType)
			{
				SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicGetBigReward);
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_SmallCrit, effectPos);
			}
			//大暴击特效
			else if (GoldGetType.LargeCrit == (GoldGetType)Singleton<GoldBoxMode>.Instance.GoldGetType)
			{
				SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicGetBigReward);
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_LargeCrit, effectPos);
			}

			//显示金币爆出特效
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_GoldBurst, effectPos);
			//播放音效
			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_OpenGoldBox);

			//获得下次金币购买信息
			Singleton<GoldBoxMode>.Instance.GetGoldBuyInfo();
		}
		
	}
}
