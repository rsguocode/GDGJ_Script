//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailVo
//文件描述：收件箱邮件列表Vo
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PCustomDataType;

namespace com.game.module.Mail
{
	public class MailBoxListVo
	{
		private uint totalCount;//邮件总数
		private List<PMailBasicInfo> mailList = new List<PMailBasicInfo>();//邮件列表

		/// <summary>
		/// 邮件总数
		/// </summary>
		public uint TotalCount
		{
			get
			{
				return totalCount;
			}
			set
			{
				totalCount = value;
			}
		}

		/// <summary>
		/// 邮件列表
		/// </summary>
		public List<PMailBasicInfo> MailList
		{
			get
			{
				return mailList;
			}
			set
			{
				mailList = value;
			}
		}
	}
}
