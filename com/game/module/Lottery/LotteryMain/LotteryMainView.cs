//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LotteryMainView
//文件描述：
//创建者：黄江军
//创建日期：2013-12-25
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.manager;

namespace Com.Game.Module.Lottery
{
	public class LotteryMainView : BaseView<LotteryMainView> 
	{	
		public override string url { get { return "UI/Lottery/LotteryMainView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}
		public override bool IsFullUI { get { return true; }}

		private Button btnClose;
		private UILabel labTitle;

		private Button btnStar1;
		private UILabel labRemain1;

		private Button btnStar2;
		private UILabel labRemain2;

		private Button btnStar3;
		private UILabel labRemain3;

		private Button btnStar4;
		private UILabel labRemain4;

		private Button btnStar5;
		private UILabel labRemain5;

		private Button btnStar6;
		private UILabel labRemain6;

		public LotteryMainView() 
		{	
		}	

		protected override void Init()
		{
			btnClose = FindInChild<Button>("top/btn_close");
			labTitle = FindInChild<UILabel>("common/title/label");

			btnStar1 = FindInChild<Button>("buttons/btn1");
			labRemain1 = FindInChild<UILabel>("buttons/btn1/count");

			btnStar2 = FindInChild<Button>("buttons/btn2");
			labRemain2 = FindInChild<UILabel>("buttons/btn2/count");

			btnStar3 = FindInChild<Button>("buttons/btn3");
			labRemain3 = FindInChild<UILabel>("buttons/btn3/count");

			btnStar4 = FindInChild<Button>("buttons/btn4");
			labRemain4 = FindInChild<UILabel>("buttons/btn4/count");

			btnStar5 = FindInChild<Button>("buttons/btn5");
			labRemain5 = FindInChild<UILabel>("buttons/btn5/count");

			btnStar6 = FindInChild<Button>("buttons/btn6");
			labRemain6 = FindInChild<UILabel>("buttons/btn6/count");

			InitLabel();
			
			btnClose.onClick = CloseOnClick;
			btnStar1.onClick = Star1OnClick;
			btnStar2.onClick = Star2OnClick;
			btnStar3.onClick = Star3OnClick;
			btnStar4.onClick = Star4OnClick;
			btnStar5.onClick = Star5OnClick;
			btnStar6.onClick = Star6OnClick;
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("LotteryView.Star");
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			GetAllStarRemain();
		}

		private void GetAllStarRemain()
		{
			labRemain1.text = Singleton<LotteryMode>.Instance.GetStarRemain(1).ToString();
			labRemain2.text = Singleton<LotteryMode>.Instance.GetStarRemain(2).ToString();
			labRemain3.text = Singleton<LotteryMode>.Instance.GetStarRemain(3).ToString();
			labRemain4.text = Singleton<LotteryMode>.Instance.GetStarRemain(4).ToString();
			labRemain5.text = Singleton<LotteryMode>.Instance.GetStarRemain(5).ToString();
			labRemain6.text = Singleton<LotteryMode>.Instance.GetStarRemain(6).ToString();
		}

		//关闭按钮事件处理
		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}	

		//处理星签1点击事件
		private void Star1OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(1);
		}	

		//处理星签2点击事件
		private void Star2OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(2);
		}	

		//处理星签3点击事件
		private void Star3OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(3);
		}	

		//处理星签4点击事件
		private void Star4OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(4);
		}	

		//处理星签5点击事件
		private void Star5OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(5);
		}	

		//处理星签6点击事件
		private void Star6OnClick(GameObject go)
		{
			Singleton<LotteryView>.Instance.ShowWindow(6);
		}	

		public override void RegisterUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated += UpdateAllStarRemainHandle;			
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated -= UpdateAllStarRemainHandle;
		}

		private void UpdateAllStarRemainHandle(object sender, int type)
		{
			if (Singleton<GoodsMode>.Instance.UPDATE_GOODS == type)
			{
				GetAllStarRemain();
			}
		}
	}
}
