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
	public class MailGetInfoVo
	{
		private uint mailId;                       //邮件id
		private string title;                      //邮件标题
		private uint sendTime;                     //邮件标题
		private string content;                    //邮件内容
		private List<PMailAttach> mailAttachList = new List<PMailAttach>();  //邮件附件列表
		private uint gold;                         //金币
		private uint diam;                         //钻石
		private uint diamBind;                     //礼金
		private List<PMailOther> mailOtherDataList = new List<PMailOther>();//邮件其他数据列表

		/// <summary>
		/// 邮件id
		/// </summary>
		public uint MailId
		{
			get
			{
				return mailId;
			}
			set
			{
				mailId = value;
			}
		}

		/// <summary>
		/// 邮件标题
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		/// <summary>
		/// 发送时间
		/// </summary>
		public uint SendTime
		{
			get
			{
				return sendTime;
			}
			set
			{
				sendTime = value;
			}
		}

		/// <summary>
		/// 邮件内容
		/// </summary>
		public string Content
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
			}
		}

		/// <summary>
		/// 邮件附件列表
		/// </summary>
		public List<PMailAttach> MailAttachList
		{
			get
			{
				return mailAttachList;
			}
			set
			{
				mailAttachList = value;
			}
		}

		/// <summary>
		/// 金币
		/// </summary>
		public uint Gold
		{
			get
			{
				return gold;
			}
			set
			{
				gold = value;
			}
		}

		/// <summary>
		/// 钻石
		/// </summary>
		public uint Diam
		{
			get
			{
				return diam;
			}
			set
			{
				diam = value;
			}
		}

		/// <summary>
		/// 绑定钻石
		/// </summary>
		public uint DiamBind
		{
			get
			{
				return diamBind;
			}
			set
			{
				diamBind = value;
			}
		}

		/// <summary>
		/// 邮件其他数据列表
		/// </summary>
		public List<PMailOther> MailOtherDataList
		{
			get
			{
				return mailOtherDataList;
			}
			set
			{
				mailOtherDataList = value;
			}
		}
	}
}