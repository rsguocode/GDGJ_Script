//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：Mailconst
//文件描述：邮件枚举类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace com.game.module.Mail
{
	public class MailConst
	{
		public const int ATTACHITEM_3 = 3;//附件物品数量 = 3

		/// <summary>
		/// 附件状态：没有附件；有附件；有附件已领取；
		/// </summary>
		public enum attachStatus
		{
			/// <summary>
			/// 没有附件 = 0
			/// </summary>
			HasNoAttachment  = 0,
			/// <summary>
			/// 有附件 = 1
			/// </summary>
			HasAttachment    = 1,
			/// <summary>
			/// 有附件已领取 = 2
			/// </summary>
			HasGetAttachment = 2,
		};

		/// <summary>
		/// 邮件读取状态：已读；未读；
		/// </summary>
		public enum readStatus
		{
			/// <summary>
			/// 已读 = 0
			/// </summary>
			AleadyRead = 0,
			/// <summary>
			/// 未读 = 1
			/// </summary>
			UnRead     = 1,
		};
	}
}
