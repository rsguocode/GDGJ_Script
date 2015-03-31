//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GuildMode
//文件描述：
//创建者：黄江军
//创建日期：2014-03-04
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using com.game.module.test;
using Proto;
using com.game;
using PCustomDataType;
using com.game.vo;
using com.game.utils;
using com.game.manager;

namespace Com.Game.Module.Guild
{
	public enum GuildPosition
	{
		NormalMember = 0,
		President = 1,
		VicePresident = 2
	}

	public class GuildMode : BaseMode<GuildMode> 
	{	
		public readonly byte UPDATE_OTHERGUILD_LIST = 1;
		public readonly byte UPDATE_GUILD_INFO = 2;
		public readonly byte UPDATE_GUILD_MEMBERS = 3;
		public readonly byte UPDATE_LOG_LIST = 4;
		public readonly byte UPDATE_VERIFY_LIST = 5;
		public readonly byte UPDATE_GUILD_ID_CHANGED = 6;
		public readonly byte UPDATE_VEVIRY_FINISHED = 7;
		public readonly byte UPDATE_SET_NOTICEOK = 8;

		private ushort pageCount;
		private ushort pageIndex = 1;
		private List<PGuildBase> otherGuildList = new List<PGuildBase>();

		private Dictionary<uint, GuildBasicInfoMsg_31_6> guildInfoDic = new Dictionary<uint, GuildBasicInfoMsg_31_6>();
		private Dictionary<uint, List<PGuildMember>> guildMembersDic = new Dictionary<uint, List<PGuildMember>>();

		private List<PGuildLog> logList = new List<PGuildLog>();
		private List<PGuildApplyMember> verifyList = new List<PGuildApplyMember>();

		public List<PGuildApplyMember> VerifyList
		{
			get {return verifyList;}
		}

		public List<PGuildLog> LogList
		{
			get {return logList;}
		}

		public ushort PageCount
		{
			get {return pageCount;}
		}

		public ushort PageIndex
		{
			get {return pageIndex;}
			set {pageIndex = value;}
		}

		public List<PGuildBase> OtherGuildList
		{
			get {return otherGuildList;}
		}

		public void NotifyGuildIdChanged()
		{
			DataUpdate(UPDATE_GUILD_ID_CHANGED);
		}

		public void SetVerifyList(List<PGuildApplyMember> list)
		{
			this.verifyList = list;
			DataUpdate(UPDATE_VERIFY_LIST);
		}

		public void VerifyFinish()
		{
			DataUpdate(UPDATE_VEVIRY_FINISHED);
		}

		public void SetNoticeOk()
		{
			DataUpdate(UPDATE_SET_NOTICEOK);
		}

		public void SetLogList(List<PGuildLog> list)
		{
			this.logList = list;
			DataUpdate(UPDATE_LOG_LIST);
		}

		public void SetOtherList(ushort sum, List<PGuildBase> list)
		{
			const ushort numPerPage = 10;
			this.pageCount = (ushort)((sum+numPerPage-1)/numPerPage);
			this.otherGuildList = list;
			DataUpdate(UPDATE_OTHERGUILD_LIST);
		}

		//根据公会Id获得公会信息
		public GuildBasicInfoMsg_31_6 GetGuildItem(uint guildId)
		{
			if (guildInfoDic.ContainsKey(guildId))
			{
				return guildInfoDic[guildId];
			}
			else
			{
				return null;
			}
		}

		//设置公会信息
		public void SetGuildItem(GuildBasicInfoMsg_31_6 otherItem)
		{
			if (null != otherItem)
			{
				guildInfoDic[otherItem.guildId] = otherItem;
				DataUpdate(UPDATE_GUILD_INFO);
			}
		}

		//根据公会Id获得成员列表
		public List<PGuildMember> GetGuildMembersList(uint guildId)
		{
			if (guildMembersDic.ContainsKey(guildId))
			{
				return guildMembersDic[guildId];
			}
			else
			{
				return null;
			}
		}

		//获得公会成员
		public PGuildMember GetMember(List<PGuildMember> List, uint id)
		{
			foreach (PGuildMember item in List)
			{
				if (id == item.id)
				{
					return item;
				}
			}

			return null;
		}

		//设置公会成员列表
		public void SetGuildMembersList(uint guildId, List<PGuildMember> list)
		{
			if (null != list)
			{
				guildMembersDic[guildId] = list;
				DataUpdate(UPDATE_GUILD_MEMBERS);
			}
		}

		//创建公会
		public void CreateGuild(string name)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_1(msdata, name);
			AppNet.gameNet.send(msdata, 31, 1);
		}

		//解散公会
		public void DismissGuild()
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_2(msdata);
			AppNet.gameNet.send(msdata, 31, 2);
		}

		//管理公会
		public void ManageGuild(byte operation, uint roleId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_3(msdata, operation, roleId);
			AppNet.gameNet.send(msdata, 31, 3);
		}

		//退出公会
		public void LeaveGuild()
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_4(msdata);
			AppNet.gameNet.send(msdata, 31, 4);
		}

		//修改公会公告
		public void ModifyGuildNotice(string announcement)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_5(msdata, announcement);
			AppNet.gameNet.send(msdata, 31, 5);
		}

		//公会基本信息
		public void GetGuildBaseInfo(uint guildId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_6(msdata, guildId);
			AppNet.gameNet.send(msdata, 31, 6);
		}

		//公会的人员信息
		public void GetGuildMembersInfo(uint guildId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_7(msdata, guildId);
			AppNet.gameNet.send(msdata, 31, 7);
		}

		//其他公会信息
		public void GetOtherGuildInfo(ushort page, bool mask)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_8(msdata, page, mask);
			AppNet.gameNet.send(msdata, 31, 8);
		}

		//通过公会名字查找
		public void FindGuildByName(string guildName)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_9(msdata, guildName);
			AppNet.gameNet.send(msdata, 31, 9);
		}

		//通过会长名字查询
		public void FindGuildByOwner(string ownerName)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_10(msdata, ownerName);
			AppNet.gameNet.send(msdata, 31, 10);
		}

		//申请加入某个公会
		public void JoinGuild(uint guildId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_11(msdata, guildId);
			AppNet.gameNet.send(msdata, 31, 11);
		}

		//申请审核公会列表
		public void VerifyGuildList()
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_12(msdata);
			AppNet.gameNet.send(msdata, 31, 12);
		}

		//审核申请
		public void Verify(uint roleId, bool result)
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_13(msdata, roleId, result);
			AppNet.gameNet.send(msdata, 31, 13);
		}

		//副会长辞职
		public void Resign()
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_14(msdata);
			AppNet.gameNet.send(msdata, 31, 14);
		}

		//公会日志
		public void GetNoticeList()
		{
			MemoryStream msdata = new MemoryStream();
			Module_31.write_31_15(msdata);
			AppNet.gameNet.send(msdata, 31, 15);
		}

		//获得成员称号
		public string GetJobName(byte jobId)
		{
			switch (jobId)
			{
			case 0:
				return LanguageManager.GetWord("Guild.NormalMember");
			case 1:
				return LanguageManager.GetWord("Guild.President");
			case 2:
				return LanguageManager.GetWord("Guild.VicePresident");
			default:
				return LanguageManager.GetWord("Guild.UnknownPos");
			}
		}

		//格式化录时间
		public string GetLogoutTimeStr(TimeProp timeProp)
		{
			int years = timeProp.Days/365;
			int months = timeProp.Days/12;
			int days = timeProp.Days;
			int hours = timeProp.Hours;
			int minutes = timeProp.Minutes;
			int seconds = timeProp.Seconds;

			if (days+hours+minutes+seconds > 0)
			{
				return LanguageManager.GetWord("Guild.OffLine");
			}
			else
			{
				return LanguageManager.GetWord("Guild.OnLine");
			}
			
			/*
			if (years > 0)
			{
				return years + LanguageManager.GetWord("Guild.YearBefore");
			}
			else if (months > 0)
			{
				return months + LanguageManager.GetWord("Guild.MonthBefore");
			} 
			else if (days > 0)
			{
				return days + LanguageManager.GetWord("Guild.DayBefore");
			} 
			else if (hours > 0)
			{
				return hours + LanguageManager.GetWord("Guild.HourBefore");
			} 
			else if (minutes > 0)
			{
				return minutes + LanguageManager.GetWord("Guild.MinuteBefore");
			} 
			else if (seconds > 0)
			{
				return seconds + LanguageManager.GetWord("Guild.SecondBefore");
			} 
			else 
			{
				return LanguageManager.GetWord("Guild.OnLine");
			} 
			*/
		}

		//主角是否处于某种头衔
		public bool IsPosition(GuildPosition guildPosition)
		{
			List<PGuildMember> dataList = GetGuildMembersList(MeVo.instance.guildId);
			if (null == dataList)
			{
				return false;
			}
			
			PGuildMember me = GetMember(dataList, MeVo.instance.Id);
			if (null == me)
			{
				return false;
			}
			else
			{
				return ((byte)guildPosition == me.position);
			}
		}
		
	}
}
