//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：UpdateAnnounceView
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;

namespace Com.Game.Module.UpdateAnnounce
{
	public class UpdateAnnounceView : BaseView<UpdateAnnounceView> 
	{		
		public override string url { get { return "UI/UpdateAnnounce/UpdateAnnounceView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.HighLayer; }}	

		private Button btnClose;
		private Button btnOk;
		private UILabel labTime;
		private UILabel labAnnounce;

		private string date;
		private string announce;

		protected override void Init()
		{
			btnClose = FindInChild<Button>("btn_close");
			btnOk = FindInChild<Button>("btn_ok");
			labTime = FindInChild<UILabel>("center/updatetime/time");
			labAnnounce = FindInChild<UILabel>("center/content/announce");

			btnClose.onClick = CloseOnClick;
			btnOk.onClick = OkOnClick;
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			
			labTime.text = date;
			labAnnounce.text = announce;
		}

		public void ShowWindow(string date, string announce)
		{
			this.date = date;
			this.announce = announce;

			OpenView();
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void OkOnClick(GameObject go)
		{
			CloseView();
		}
		
	}
}
