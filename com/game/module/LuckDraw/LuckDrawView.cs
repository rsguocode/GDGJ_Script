//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LuckDrawView
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
using com.game.sound;
using com.game.consts;
using com.game.utils;

namespace Com.Game.Module.LuckDraw
{
	public class LuckDrawView : BaseView<LuckDrawView> 
	{	
		public override string url { get { return "UI/LuckDraw/LuckDrawView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		public override bool IsFullUI { get { return false; }}

		private Button btnClose;
		public Button BtnOnce;
		private Button btnTenth;
		private UILabel labOnecePrice;
		private UILabel labTenthPrice;
		private UILabel labCDTime;
		private UISprite sprFirst;
		private UISprite sprTenth;
		private UILabel labBuyOne;
		private UILabel labBuyTen;

		private float prevTime;
		private int cdTime = -1;

		public bool canBuyFree
		{
			get 
			{
				return (true == Singleton<LuckDrawMode>.Instance.FirstLuckDraw || 0 == cdTime);
			}
		}

		protected override void Init()
		{
			btnClose = FindInChild<Button>("center/btn_guanbi");
			BtnOnce = FindInChild<Button>("center/anniu/btn_anniu1");
			btnTenth = FindInChild<Button>("center/anniu/btn_anniu2");
			labOnecePrice = FindInChild<UILabel>("center/anniu/btn_anniu1/shuzi");
			labTenthPrice = FindInChild<UILabel>("center/anniu/btn_anniu2/shuzi");
			labCDTime = FindInChild<UILabel>("center/shijian");
			sprFirst = FindInChild<UISprite>("center/shouchou");
			sprTenth = FindInChild<UISprite>("center/shilianchou");
			labBuyOne = FindInChild<UILabel>("center/anniu/btn_anniu1/zi");
			labBuyTen = FindInChild<UILabel>("center/anniu/btn_anniu2/zi");

			btnClose.onClick = CloseOnClick;
			BtnOnce.onClick = OneceOnClick;
			btnTenth.onClick = TenthOnClick;

			InitLabel();
		}

		private void InitLabel()
		{
			labBuyOne.text = LanguageManager.GetWord("LuckDraw.BuyOne");
			labBuyTen.text = LanguageManager.GetWord("LuckDraw.BuyTen");
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void OneceOnClick(GameObject go)
		{
			if (canBuyFree)
			{
				Singleton<LuckDrawMode>.Instance.LuckDrawFree();
			}
			else
			{
				Singleton<LuckDrawMode>.Instance.LuckDrawOnce();
			}
		}

		private void TenthOnClick(GameObject go)
		{
			Singleton<LuckDrawMode>.Instance.LuckDrawTen();
		}

		public override void OpenView()
		{			
			SendCommandsToServerWhileOpen();			
			base.OpenView();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<LuckDrawMode>.Instance.GetLuckDrawBaseInfo();
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();			
			UpdateLuckDrawInfo();
			Singleton<LuckDrawMode>.Instance.StopTips();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();			
			Singleton<LuckDrawMode>.Instance.StartTips();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<LuckDrawMode>.Instance.dataUpdated += UpdateLuckDrawInfoHandle;	
			Singleton<LuckDrawMode>.Instance.dataUpdated += UpdateLuckDrawTypeHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<LuckDrawMode>.Instance.dataUpdated -= UpdateLuckDrawInfoHandle;
			Singleton<LuckDrawMode>.Instance.dataUpdated -= UpdateLuckDrawTypeHandle;
		}

		//抽奖信息获得回调
		private void UpdateLuckDrawInfoHandle(object sender, int type)
		{
			if (Singleton<LuckDrawMode>.Instance.UPDATE_LUCKDRAW_INFO == type)
			{
				UpdateLuckDrawInfo();
			}
		}

		//抽奖后回调
		private void UpdateLuckDrawTypeHandle(object sender, int type)
		{
			if (Singleton<LuckDrawMode>.Instance.UPDATE_UPDATE_LUCKDRAW_TYPE == type)
			{
				UpdateLuckDrawType();
			}
		}

		private void UpdateLuckDrawInfo()
		{
			cdTime = (int)Singleton<LuckDrawMode>.Instance.CDTime;
			labOnecePrice.text = Singleton<LuckDrawMode>.Instance.DiamondNeedsForOnce.ToString();
			labTenthPrice.text = Singleton<LuckDrawMode>.Instance.DiamondNeedsForTenth.ToString();

			if (canBuyFree)
			{
				sprFirst.gameObject.SetActive(true);
				sprTenth.gameObject.SetActive(false);
				labCDTime.text = LanguageManager.GetWord("LuckDraw.BuyFree");
				labOnecePrice.text = LanguageManager.GetWord("LuckDraw.ForFree");
			}
			else
			{
				sprFirst.gameObject.SetActive(false);
				sprTenth.gameObject.SetActive(true);
				labCDTime.text = TimeUtil.GetTimeHhmmss(cdTime) + " " + LanguageManager.GetWord("LuckDraw.AfterForFree");
			}
		}

		private void UpdateLuckDrawType()
		{
			Singleton<LuckDrawMode>.Instance.GetLuckDrawBaseInfo();
			Singleton<LuckDrawTipsView>.Instance.OpenView();
		}

		private void AutoUpdateCDTime()
		{
			if (canBuyFree)
			{
				return;
			}

			if (cdTime > 0)
			{
				cdTime--;
			}

			labCDTime.text = TimeUtil.GetTimeHhmmss(cdTime) + " " + LanguageManager.GetWord("LuckDraw.AfterForFree");
		}

		public override void Update()
		{
			if ((null == gameObject) || !gameObject.activeSelf)
			{
				return;
			}
			
			float curTime = Time.time;
			
			//每秒自动更新
			if (curTime - prevTime >= 1f)
			{
				prevTime = curTime;
				
				AutoUpdateCDTime();
			}	
		}
		
	}
}
