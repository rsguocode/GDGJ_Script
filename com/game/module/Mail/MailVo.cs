//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailVo
//文件描述：邮件数据模型类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Proto;

namespace com.game.module.Mail
{
	public class MailVo
	{
		private uint currentDelMailId;                      //当前删除的邮件Id
		private uint totalUnReadMailCount;                  //未读邮件总数量
		private uint currentGetAwardMailId;                 //当前领取奖励邮件Id
		private List<uint> selectMail = new List<uint>();   //勾选的邮件列表
		private MailGetInfoMsg_12_2 currentMailItemInfo;    //当前选中的邮件
		private bool isOpenAttachView;                      //是否打开附件界面
		private List<uint> mailIdList = new List<uint>();   //当前邮件列表ID

		/// <summary>
		/// 当前删除的邮件Id
		/// </summary>
		public uint CurrentDelMailId
		{
			get
			{
				return currentDelMailId;
			}
			set
			{
				currentDelMailId = value;
			}
		}

		/// <summary>
		/// 未读邮件总数量
		/// </summary>
		public uint TotalUnReadMailNum
		{
			get
			{
				return totalUnReadMailCount;
			}
			set
			{
				totalUnReadMailCount = value;
			}
		}

		/// <summary>
		/// 勾选的邮件列表
		/// </summary>
		public List<uint> SelectMail
		{
			get
			{
				return selectMail;
			}
			set
			{
				selectMail = value;
			}
		}

		/// <summary>
		/// 勾选的邮件列表
		/// </summary>
		public MailGetInfoMsg_12_2 CurrentMailItemInfo
		{
			get
			{
				return currentMailItemInfo;
			}
			set
			{
				currentMailItemInfo = value;
			}
		}

		/// <summary>
		/// 是否打开附件界面
		/// </summary>
		public bool IsOpenAttachView
		{
			get
			{
				return isOpenAttachView;
			}
			set
			{
				isOpenAttachView = value;
			}
		}

		/// <summary>
		/// 当前领取奖励邮件Id
		/// </summary>
		public uint CurrentGetAwardMailId
		{
			get
			{
				return currentGetAwardMailId;
			}
			set
			{
				currentGetAwardMailId = value;
			}
		}

		/// <summary>
		/// 当前邮件列表ID
		/// </summary>
		public List<uint> MailIdList
		{
			get
			{
				return mailIdList;
			}
			set
			{
				mailIdList = value;
			}
		}
	}
}
