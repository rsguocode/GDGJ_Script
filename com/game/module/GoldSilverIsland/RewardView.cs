//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：RewardView
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
	public delegate void CloseCallback();

	public class RewardView : BaseView<RewardView>   
	{		
		public override string url { get { return "UI/GoldSilverIsland/RewardView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		private Button btnOk;
		private UILabel labRewardDiam;
		private UILabel labRewardRep;

		private CloseCallback closedCallback;
		private int diam;
		private int rep;

		protected override void Init()
		{
			btnOk = FindInChild<Button>("center/btn_sy");
			labRewardDiam = FindInChild<UILabel>("center/nr/szyb");
			labRewardRep = FindInChild<UILabel>("center/nr/szsw");

			btnOk.onClick = OkOnClick;

			InitLabel();
			SetToLayerMode();
		}

		private void InitLabel()
		{
			btnOk.label.text = LanguageManager.GetWord("ConfirmView.Ok");
		}
		
		//设置对象层级
		private void SetToLayerMode()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("Mode")); 
		}

		private void OkOnClick(GameObject go)
		{
			CloseView();
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			ShowReward();	
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();	

			if (null != closedCallback)
			{
				closedCallback();
			}
		}

		private void ShowReward()
		{
			labRewardDiam.text = "+" + diam.ToString();
			labRewardRep.text = "+" + rep.ToString();
		}

		public void ShowWindow(int diam, int rep, CloseCallback callBack = null)
		{
			this.diam = diam;
			this.rep = rep;
			this.closedCallback = callBack;

			OpenView();
		}
		
	}
}
