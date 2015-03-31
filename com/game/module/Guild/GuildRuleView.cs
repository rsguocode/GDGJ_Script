//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildRuleView
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

namespace Com.Game.Module.Guild
{
	public class GuildRuleView : BaseView<GuildRuleView> 
	{		
		public override string url { get { return "UI/Guild/GuildRuleView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	

		private Button btnClose;
		private Button btnAssign;
		private Button btnDismiss;
		private Button btnKickOut;

		private PGuildMember member;

		protected override void Init()
		{
			btnClose = FindInChild<Button>("center/btn_close");
			btnAssign = FindInChild<Button>("center/button1");
			btnDismiss = FindInChild<Button>("center/button2");
			btnKickOut = FindInChild<Button>("center/button3");

			btnClose.onClick = CloseOnClick;
			btnAssign.onClick = AssignOnClick;
			btnDismiss.onClick = DismissOnClick;
			btnKickOut.onClick = KickOutOnClick;

			InitLabel();
		}

		private void InitLabel()
		{
			btnAssign.label.text = LanguageManager.GetWord("Guild.AssignPos");
			btnDismiss.label.text = LanguageManager.GetWord("Guild.DismissPos");
			btnKickOut.label.text = LanguageManager.GetWord("Guild.KickOut");
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void AssignOnClick(GameObject go)
		{
			string[] param = {member.name};
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.AssignPosQuestion", param), ConfirmCommands.OK_CANCEL, AssignVicePresident);
		}

		private void DismissOnClick(GameObject go)
		{
			string[] param = {member.name};
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.DismissPosQuestion", param), ConfirmCommands.OK_CANCEL, Dismiss);
		}

		private void KickOutOnClick(GameObject go)
		{
			string[] param = {member.name};
			ConfirmMgr.Instance.ShowOkCancelAlert(LanguageManager.GetWord("Guild.KickOutQuestion", param), ConfirmCommands.OK_CANCEL, KickOut);
		}

		private void AssignVicePresident()
		{
			Singleton<GuildMode>.Instance.ManageGuild(1, member.id);
		}

		private void Dismiss()
		{
			Singleton<GuildMode>.Instance.ManageGuild(2, member.id);
		}

		private void KickOut()
		{
			Singleton<GuildMode>.Instance.ManageGuild(3, member.id);
		}

		public void ShowWindow(PGuildMember member)
		{
			this.member = member;
			OpenView();
		}
		
	}
}
