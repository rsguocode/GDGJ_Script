//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildControl
//文件描述：
//创建者：黄江军
//创建日期：2014-03-04
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.cmd;
using com.net.interfaces;
using com.game;
using Proto;
using com.u3d.bases.debug;
using com.game.Public.Message;
using com.game.vo;

namespace Com.Game.Module.Guild
{
	public class GuildControl : BaseControl<GuildControl>  
	{	
		//注册Socket数据返回监听
		override protected void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_31_1, Fun_31_1);        //创建公会
			AppNet.main.addCMD(CMD.CMD_31_2, Fun_31_2);        //解散公会
			AppNet.main.addCMD(CMD.CMD_31_3, Fun_31_3);        //管理公会
			AppNet.main.addCMD(CMD.CMD_31_4, Fun_31_4);        //退出公会
			AppNet.main.addCMD(CMD.CMD_31_5, Fun_31_5);        //修改公会公告
			AppNet.main.addCMD(CMD.CMD_31_6, Fun_31_6);        //公会基本信息
			AppNet.main.addCMD(CMD.CMD_31_7, Fun_31_7);        //公会的人员信息
			AppNet.main.addCMD(CMD.CMD_31_8, Fun_31_8);        //其他公会信息
			AppNet.main.addCMD(CMD.CMD_31_9, Fun_31_9);        //通过公会名字查找
			AppNet.main.addCMD(CMD.CMD_31_10, Fun_31_10);      //通过会长名字查询
			AppNet.main.addCMD(CMD.CMD_31_11, Fun_31_11);      //申请加入某个公会
			AppNet.main.addCMD(CMD.CMD_31_12, Fun_31_12);      //申请加入公会列表
			AppNet.main.addCMD(CMD.CMD_31_13, Fun_31_13);      //审核申请
			AppNet.main.addCMD(CMD.CMD_31_14, Fun_31_14);      //副会长辞职
			AppNet.main.addCMD(CMD.CMD_31_15, Fun_31_15);      //公会日志
			AppNet.main.addCMD(CMD.CMD_31_16, Fun_31_16);      //公会信息发生变化
		}

		//创建公会
		private void Fun_31_1(INetData data)
		{
			GuildCreateMsg_31_1 guildCreateMsg = new GuildCreateMsg_31_1();
			guildCreateMsg.read(data.GetMemoryStream());

			if (0 == guildCreateMsg.code)
			{
				MeVo.instance.guildId = guildCreateMsg.guildId;
				Singleton<GuildMode>.Instance.NotifyGuildIdChanged();
				MessageManager.Show("公会创建成功");
			}
			else
			{
				ErrorCodeManager.ShowError(guildCreateMsg.code);
			}
		}

		//解散公会
		private void Fun_31_2(INetData data)
		{
			GuildDisbandMsg_31_2 guildDisbandMsg = new GuildDisbandMsg_31_2();
			guildDisbandMsg.read(data.GetMemoryStream());

			if (0 == guildDisbandMsg.code)
			{
				MeVo.instance.guildId = 0;
				Singleton<GuildMode>.Instance.NotifyGuildIdChanged();
				MessageManager.Show("解散成功");
			}
			else
			{
				ErrorCodeManager.ShowError(guildDisbandMsg.code);
			}
		}

		//管理公会
		private void Fun_31_3(INetData data)
		{
			GuildDealMsg_31_3 guildDealMsg = new GuildDealMsg_31_3();
			guildDealMsg.read(data.GetMemoryStream());

			if (0 == guildDealMsg.code)
			{
				MessageManager.Show("操作成功");
				Singleton<GuildMode>.Instance.GetGuildMembersInfo(MeVo.instance.guildId);
			}
			else
			{
				ErrorCodeManager.ShowError(guildDealMsg.code);
			}
		}

		//退出公会
		private void Fun_31_4(INetData data)
		{
			GuildQuitMsg_31_4 guildQuitMsg = new GuildQuitMsg_31_4();
			guildQuitMsg.read(data.GetMemoryStream());

			if (0 == guildQuitMsg.code)
			{
				MeVo.instance.guildId = 0;
				Singleton<GuildMode>.Instance.NotifyGuildIdChanged();
				MessageManager.Show("成功退出公会");
			}
			else
			{
				ErrorCodeManager.ShowError(guildQuitMsg.code);
			}
		}

		//修改公会公告
		private void Fun_31_5(INetData data)
		{
			GuildModifyAnnouncementMsg_31_5 guildModifyAnnouncementMsg = new GuildModifyAnnouncementMsg_31_5();
			guildModifyAnnouncementMsg.read(data.GetMemoryStream());

			if (0 == guildModifyAnnouncementMsg.code)
			{
				Singleton<GuildMode>.Instance.SetNoticeOk();
				MessageManager.Show("公告设置成功");
			}
			else
			{
				ErrorCodeManager.ShowError(guildModifyAnnouncementMsg.code);
			}
		}

		//公会基本信息
		private void Fun_31_6(INetData data)
		{
			GuildBasicInfoMsg_31_6 guildBasicInfoMsg = new GuildBasicInfoMsg_31_6();
			guildBasicInfoMsg.read(data.GetMemoryStream());

			if (0 == guildBasicInfoMsg.code)
			{
				Singleton<GuildMode>.Instance.SetGuildItem(guildBasicInfoMsg);
			}
			else
			{
				ErrorCodeManager.ShowError(guildBasicInfoMsg.code);
			}
		}

		//公会的人员信息
		private void Fun_31_7(INetData data)
		{
			GuildMemberListMsg_31_7 guildMemberListMsg = new GuildMemberListMsg_31_7();
			guildMemberListMsg.read(data.GetMemoryStream());

			if (0 == guildMemberListMsg.code)
			{
				Singleton<GuildMode>.Instance.SetGuildMembersList(guildMemberListMsg.guildId, guildMemberListMsg.list);
			}
			else
			{
				ErrorCodeManager.ShowError(guildMemberListMsg.code);
			}
		}

		//其他公会信息
		private void Fun_31_8(INetData data)
		{
			GuildOtherListMsg_31_8 guildOtherListMsg = new GuildOtherListMsg_31_8();
			guildOtherListMsg.read(data.GetMemoryStream());

			if (0 == guildOtherListMsg.code)
			{
				Singleton<GuildMode>.Instance.SetOtherList(guildOtherListMsg.sum, guildOtherListMsg.list);
			}
			else
			{
				ErrorCodeManager.ShowError(guildOtherListMsg.code);
			}
		}

		//通过公会名字查找
		private void Fun_31_9(INetData data)
		{
			GuildSearchByNameMsg_31_9 guildSearchByNameMsg = new GuildSearchByNameMsg_31_9();
			guildSearchByNameMsg.read(data.GetMemoryStream());
		}

		//通过会长名字查询
		private void Fun_31_10(INetData data)
		{
			GuildSearchByOwenerMsg_31_10 guildSearchByOwenerMsg = new GuildSearchByOwenerMsg_31_10();
			guildSearchByOwenerMsg.read(data.GetMemoryStream());
		}

		//申请加入某个公会
		private void Fun_31_11(INetData data)
		{
			GuildApproveMsg_31_11 guildApproveMsg = new GuildApproveMsg_31_11();
			guildApproveMsg.read(data.GetMemoryStream());

			if (0 == guildApproveMsg.code)
			{
				MessageManager.Show("成功提交申请");
			}
			else			
			{
				ErrorCodeManager.ShowError(guildApproveMsg.code);
			}
		}

		//申请审核公会列表
		private void Fun_31_12(INetData data)
		{
			GuildApproveListMsg_31_12 guildApproveListMsg = new GuildApproveListMsg_31_12();
			guildApproveListMsg.read(data.GetMemoryStream());

			if (0 == guildApproveListMsg.code)
			{
				Singleton<GuildMode>.Instance.SetVerifyList(guildApproveListMsg.list);
			}
			else
			{
				ErrorCodeManager.ShowError(guildApproveListMsg.code);
			}
		}

		//审核申请
		private void Fun_31_13(INetData data)
		{
			GuildHandleApproveMsg_31_13 guildHandleApproveMsg = new GuildHandleApproveMsg_31_13();
			guildHandleApproveMsg.read(data.GetMemoryStream());

			if (0 == guildHandleApproveMsg.code)
			{
				Singleton<GuildMode>.Instance.VerifyFinish();
				MessageManager.Show("审核完成");
			}
			else
			{
				ErrorCodeManager.ShowError(guildHandleApproveMsg.code);
			}
		}

		//副会长辞职
		private void Fun_31_14(INetData data)
		{
			GuildResignMsg_31_14 guildResignMsg = new GuildResignMsg_31_14();
			guildResignMsg.read(data.GetMemoryStream());
		}

		//公会日志
		private void Fun_31_15(INetData data)
		{
			GuildLogMsg_31_15 guildLogMsg = new GuildLogMsg_31_15();
			guildLogMsg.read(data.GetMemoryStream());

			if (0 == guildLogMsg.code)
			{
				Singleton<GuildMode>.Instance.SetLogList(guildLogMsg.list);
			}
			else
			{
				ErrorCodeManager.ShowError(guildLogMsg.code);
			}
		}

		//公会信息发生变化
		private void Fun_31_16(INetData data)
		{
			GuildChangeInfoMsg_31_16 guildChangeInfoMsg = new GuildChangeInfoMsg_31_16();
			guildChangeInfoMsg.read(data.GetMemoryStream());
			
			MeVo.instance.guildId = guildChangeInfoMsg.guildId;
			MeVo.instance.guildName = guildChangeInfoMsg.guildName;
			Singleton<GuildMode>.Instance.NotifyGuildIdChanged();
		}
	}
}
