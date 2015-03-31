//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailMode
//文件描述：邮件数据类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using com.game.manager;
using com.game.module.test;
using com.game.Public.Confirm;
using com.game.Public.Message;
using PCustomDataType;
using Proto;

namespace com.game.module.Mail
{
	public class MailMode : BaseMode<MailMode>
	{
		public const int UPDATE_MAILLIST = 1;          //更新邮件列表
		public const int UPDATE_CURRENTMAIL = 2;       //更新邮件详细信息
		public const int GETAWARD_SUCCESS = 3;         //领取附件成功
		public const int DELETE_MAILSUCCESS = 4;       //删除邮件成功
		public const int UPDATE_ONEKEYATTACHE = 5;     //更新一键领取附件
		public const int UPDATE_ONEKEYDEL = 6;         //更新一键删除邮件

		private MailVo mailInfoVo = new MailVo();
		private MailBoxListVo mailListInfoVo = new MailBoxListVo();

		/// <summary>
		/// 更新邮件列表
		/// </summary>
		public void SetUpdateMailList(List<PMailBasicInfo> mailBasicInfo)
		{
			if(mailListInfoVo.MailList.Count == 0)
			{
				mailListInfoVo.MailList = mailBasicInfo;
			}else
			{
				for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
				{
					PMailBasicInfo info = mailListInfoVo.MailList[i];
					mailInfoVo.MailIdList.Add(info.id);
				}
				foreach(PMailBasicInfo info in mailBasicInfo)
				{
					mailListData(info);
				}
			}
			mailListInfoVo.MailList.Sort(MailUtil.SortMailList);
			UpdateMailList();
		}

		//邮件列表数据
		private void mailListData(PMailBasicInfo info)
		{
			for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
			{
				PMailBasicInfo mailBasicInfo = mailListInfoVo.MailList[i];
				if(!mailInfoVo.MailIdList.Contains(info.id))
				{
					mailListInfoVo.MailList.Add(info);
					break;
				}
			}
		}

		//更新邮件列表
		public void UpdateMailList()
		{
			DataUpdate(UPDATE_MAILLIST);
		}

		//查看当前邮件
		public void CheckCurrentMailInfo(MailGetInfoMsg_12_2 infoMsg)
		{
			mailInfoVo.CurrentMailItemInfo = infoMsg;
			mailInfoVo.TotalUnReadMailNum = 0;
			for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
			{
				if(mailListInfoVo.MailList[i].id == infoMsg.mailId)
				{
					mailListInfoVo.MailList[i].status = (int)MailConst.readStatus.AleadyRead;
				}
				if(mailListInfoVo.MailList[i].status == (int)MailConst.readStatus.UnRead)//未读
				{
					mailInfoVo.TotalUnReadMailNum += 1;
				}
			}
			DataUpdate(UPDATE_CURRENTMAIL);
		}

		//领取奖励成功，更新邮件状态和邮件详情
		public void AcceptRewardSuccess(uint mailId)
		{
			for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
			{
				if(mailListInfoVo.MailList[i].id == mailId)
				{
					mailListInfoVo.MailList[i].type = (int)MailConst.attachStatus.HasNoAttachment;
					break;
				}
			}
			MailInfoVo.CurrentMailItemInfo.diamBind = 0;
			MailInfoVo.CurrentMailItemInfo.diamBind = 0;
			MailInfoVo.CurrentMailItemInfo.gold = 0;
			MailInfoVo.CurrentMailItemInfo.mailAttachList = new List<PMailAttach>();
			DataUpdate(GETAWARD_SUCCESS);
		}

		//成功删除邮件，更新邮件列表
		public void UpdateDelMailList(List<uint> delMailList)
		{
			foreach (uint mailId in delMailList)
			{
				for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
				{
					if(mailListInfoVo.MailList[i].id == mailId)
					{
						mailListInfoVo.MailList.Remove(mailListInfoVo.MailList[i]);
						break;
					}
				}
			}
			DataUpdate(DELETE_MAILSUCCESS);
		}

		//一键删除邮件成功
		public void UpdateOneyKeyDelSuccess()
		{
			mailListInfoVo.MailList = new List<PMailBasicInfo>();
			mailInfoVo.TotalUnReadMailNum = 0;
			DataUpdate(UPDATE_ONEKEYDEL);
		}

		//一键领取附件成功，更新邮件状态和邮件详情
		public void UpdateOneKeyAcceptSuccess()
		{
			mailInfoVo.TotalUnReadMailNum = 0;
			foreach(PMailBasicInfo info in mailListInfoVo.MailList)
			{
				if(info.type == (int)MailConst.attachStatus.HasAttachment)
				{
					info.type = (int)MailConst.attachStatus.HasGetAttachment;
					info.status = (int)MailConst.readStatus.AleadyRead;
				}
			}
			mailInfoVo.TotalUnReadMailNum = 0;
			DataUpdate(UPDATE_ONEKEYATTACHE);
		}

		//未读新邮件通知
		public void SetNewUnreadMail(uint mailCount)
		{
			mailInfoVo.TotalUnReadMailNum = mailCount;
			UpdateMailList();
		}

		public MailVo MailInfoVo
		{
			get
			{
				return mailInfoVo;
			}
		}

		public MailBoxListVo MailListInfoVo
		{
			get
			{
				return mailListInfoVo;
			}
		}


		//以下为协议相关处理数据-----------------------------------------------------------------//
		/// <summary>
		/// 请求邮件列表
		/// </summary>
		public void RequestMailBasicInfo()
		{
			MemoryStream msdata = new MemoryStream();
			Module_12.write_12_1(msdata);
			AppNet.gameNet.send(msdata, 12, 1);
		}

		/// <summary>
		/// 请求邮件详情
		/// </summary>
		public void RequestMailDetail(uint mailId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_12.write_12_2(msdata, mailId);
			AppNet.gameNet.send(msdata, 12, 2);
		}

		/// <summary>
		/// 请求删除邮件
		/// </summary>
		public void RequestDeleteMail()
		{
			if(mailInfoVo.SelectMail.Count == 0)
			{
				MessageManager.Show(LanguageManager.GetWord("MailMode.DeleteAlert"));
				return;
			}
			foreach(uint id in mailInfoVo.SelectMail)
			{
				for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
				{
					if(mailListInfoVo.MailList[i].type == (int)MailConst.attachStatus.HasAttachment)
					{
						ConfirmMgr.Instance.ShowOkCancelAlert("你还有没领取的附件，真的要删除他们吗？", ConfirmCommands.OK_CANCEL, RequestDeletaAll);
						return;
					}
				}
			}
			RequestDeletaAll();
		}
		//附件全部领过，可以请求删除
		private void RequestDeletaAll()
		{
			MemoryStream msdata = new MemoryStream();
			Module_12.write_12_3(msdata, mailInfoVo.SelectMail);
			AppNet.gameNet.send(msdata, 12, 3);
			mailInfoVo.SelectMail.Clear(); //清空选择
		}

		/// <summary>
		/// 请求领取奖励
		/// </summary>
		public void AcceptReward(uint mailID)
		{
			var msdata = new MemoryStream();
			Module_12.write_12_4(msdata, mailID);
			AppNet.gameNet.send(msdata, 12, 4);
		}
		
		/// <summary>
		/// 请求一键提取
		/// </summary>
		public void OneyKeyGetAllAttach()
		{
			var msdata = new MemoryStream();
			Module_12.write_12_8(msdata);
			AppNet.gameNet.send(msdata, 12, 8);
		}

		/// <summary>
		/// 请求一键删除
		/// </summary>
		public void OneKeyDelAll()
		{
			if(mailListInfoVo.MailList.Count == 0)
			{
				MessageManager.Show("当前没有邮件");
				return;
			}
			for(int i = 0 ; i < mailListInfoVo.MailList.Count ; i ++)
			{
				if(mailListInfoVo.MailList[i].type == (int)MailConst.attachStatus.HasAttachment)
				{
					ConfirmMgr.Instance.ShowOkCancelAlert("你还有没领取的附件，真的要删除他们吗？", ConfirmCommands.OK_CANCEL, oneToDelAllMail);
					return;
				}
			}
			oneToDelAllMail();
		}
		private void oneToDelAllMail()
		{
			var msdata = new MemoryStream();
			Module_12.write_12_7(msdata);
			AppNet.gameNet.send(msdata , 12 , 7);
		}
	}
}
