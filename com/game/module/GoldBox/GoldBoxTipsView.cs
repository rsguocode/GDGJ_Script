//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldBoxTipsView
//文件描述：黄金宝箱提示界面类
//创建者：黄江军
//创建日期：2014-02-11
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;

namespace Com.Game.Module.GoldBox
{
	public delegate void CloseCallBack();

	public class GoldBoxTipsView : BaseView<GoldBoxTipsView> 
	{		
		public override string url { get { return "UI/GoldBox/GoldBoxTipsView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	

		public Button btnOk;
		private Button btnCancel;
		private UIToggle togNextHide;
		private UILabel labOpenCnt;
		private UILabel labWaste;
		private UILabel labNextHide;
		private UILabel labDiamond;
		private UILabel labGold;

		private CloseCallBack closeCallBack;
		private bool needHide = false;
		private bool batchBuy = false;
		
		public bool NeedHide
		{
			get
			{
				return needHide;
			}
		} 
		
		protected override void Init()
		{		
			btnOk = FindInChild<Button>("center/btnOk");
			btnCancel = FindInChild<Button>("center/btnCancel");
			togNextHide = FindInChild<UIToggle>("center/chkNextTips");
			labOpenCnt = FindInChild<UILabel>("center/kq");
			labWaste = FindInChild<UILabel>("center/xh");
			labNextHide = FindInChild<UILabel>("center/chkNextTips/Label");
			labDiamond = FindInChild<UILabel>("center/waste/diamond/shuzi");
			labGold = FindInChild<UILabel>("center/waste/gold/shuzi");

			btnOk.onClick = OkOnClick;
			btnCancel.onClick = CancelOnClick;

			togNextHide.value = needHide;
			
			InitLabel();
		}
		
		private void InitLabel()
		{
			labNextHide.text = LanguageManager.GetWord("GoldBoxTipsView.NextHide");
			btnOk.label.text = LanguageManager.GetWord("GoldBoxTipsView.Ok");
			btnCancel.label.text = LanguageManager.GetWord("GoldBoxTipsView.Cancel");
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();

			ushort maxBatchBuy = 10;
			ushort openCount;
			ushort diamWaste;
			uint goldGet;
			
			if (batchBuy)
			{
				ushort remainTimes = Singleton<GoldBoxMode>.Instance.RemainBuyTimes;
				openCount = (remainTimes > maxBatchBuy) ? maxBatchBuy : remainTimes;
				diamWaste = Singleton<GoldBoxMode>.Instance.DiamNeedsForBatch;
			}
			else
			{
				openCount = 1;
				diamWaste = Singleton<GoldBoxMode>.Instance.DiamNeedsForOnce;
			}

			goldGet = Singleton<GoldBoxMode>.Instance.GoldNumNextBuy;
			string[] paramCount = {openCount.ToString()};
			string[] paramWaste = {diamWaste.ToString(), goldGet.ToString()};

			labOpenCnt.text = LanguageManager.GetWord("GoldBoxTipsView.OpenCount", paramCount);
			labWaste.text = LanguageManager.GetWord("GoldBoxTipsView.Waste", paramWaste);

			labDiamond.text = paramWaste[0];
			labGold.text = paramWaste[1];
		}
		
		private void OkOnClick(GameObject go)
		{
			if (batchBuy)
			{
				Singleton<GoldBoxMode>.Instance.BuyGoldBatch();
			}
			else
			{
				Singleton<GoldBoxMode>.Instance.BuyGoldOnce();
			}

			CloseView();
		}
		
		private void CancelOnClick(GameObject go)
		{
			CloseView();
		}

		public void ShowWindow(CloseCallBack callBack, bool batchBuy = false)
		{
			this.closeCallBack = callBack;
			this.batchBuy = batchBuy;
			OpenView();
		}	

		public override void CloseView()
		{
			needHide = togNextHide.value;

			if (null != closeCallBack)
			{
				closeCallBack();
			}

			base.CloseView();
		}

	}
}
