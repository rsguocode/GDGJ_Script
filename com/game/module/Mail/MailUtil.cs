//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailUtil
//文件描述：邮件工具类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

using com.game.module.test;
using PCustomDataType;

namespace com.game.module.Mail
{
	public class MailUtil
	{
		/// <summary>
		/// 邮件列表时间排序
		/// </summary>
		public static int SortMailList(PMailBasicInfo x , PMailBasicInfo y)
		{
			return (int)(y.sendTime - x.sendTime);
		}

		//根据ID查询出当前邮件信息
		public static PMailBasicInfo findMailDataByID(uint id)
		{
			MailBoxListVo mailListInfoVo = Singleton<MailMode>.Instance.MailListInfoVo;
			for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
			{
				PMailBasicInfo mailBasicInfo = mailListInfoVo.MailList[i];
				if(mailBasicInfo.id == id)
				{
					return mailBasicInfo;
				}
			}
			return null;
		}
	}
}