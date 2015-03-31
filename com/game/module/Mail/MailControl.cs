//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailControl
//文件描述：邮件通信类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

using com.game.cmd;
using com.game.manager;
using com.game.module.test;
using com.game.Public.Message;
using com.net.interfaces;
using Proto;

namespace com.game.module.Mail
{
	public class MailControl : BaseControl<MailControl>
	{
		private MailMode mailMode;

		protected override void NetListener()
		{
			mailMode = Singleton<MailMode>.Instance;

			AppNet.main.addCMD(CMD.CMD_12_1, Fun_12_1);				//获取收件箱邮件列表信息
			AppNet.main.addCMD(CMD.CMD_12_2, Fun_12_2);				//获取某封邮件详细信息
			AppNet.main.addCMD(CMD.CMD_12_3, Fun_12_3);				//删除邮件
			AppNet.main.addCMD(CMD.CMD_12_4, Fun_12_4);             //提取邮件附件
			AppNet.main.addCMD(CMD.CMD_12_6, Fun_12_6);             //新邮件通知
			AppNet.main.addCMD(CMD.CMD_12_7, Fun_12_7);             //一键删除邮件
			AppNet.main.addCMD(CMD.CMD_12_8, Fun_12_8);             //一键领取附件
		}

		//获取收件箱邮件列表信息
		private void Fun_12_1(INetData data)
		{
			MailGetBoxListMsg_12_1 boxListMsg = new MailGetBoxListMsg_12_1();
			boxListMsg.read(data.GetMemoryStream());
			if(boxListMsg.code != 0)
			{
				ErrorCodeManager.ShowError(boxListMsg.code);
				return;
			}
			if(boxListMsg.mailList.Count == 0)
			{
				mailMode.UpdateMailList();
				return;
			}
			mailMode.SetUpdateMailList(boxListMsg.mailList);
		}

		//获取某封邮件详细信息
		private void Fun_12_2(INetData data)
		{
			MailGetInfoMsg_12_2 infoMsg = new MailGetInfoMsg_12_2();
			infoMsg.read(data.GetMemoryStream());
			if(infoMsg.code != 0)
			{
				ErrorCodeManager.ShowError(infoMsg.code);
				return;
			}
			mailMode.CheckCurrentMailInfo(infoMsg);
		}

		//删除邮件
		private void Fun_12_3(INetData data)
		{
			MailDelMailMsg_12_3 delMailMes = new MailDelMailMsg_12_3();
			delMailMes.read(data.GetMemoryStream());
			if(delMailMes.code != 0)
			{
				ErrorCodeManager.ShowError(delMailMes.code);
				return;
			}
			MessageManager.Show(LanguageManager.GetWord("MailControl.deletedSuccessfullyEmail"));
			mailMode.UpdateDelMailList(delMailMes.mailIds);
		}

		//提取邮件附件
		private void Fun_12_4(INetData data)
		{
			MailTakeAttachMsg_12_4 takeAttachMsg = new MailTakeAttachMsg_12_4();
			takeAttachMsg.read(data.GetMemoryStream());
			if(takeAttachMsg.code != 0)
			{
				ErrorCodeManager.ShowError(takeAttachMsg.code);
				return;
			}
			MessageManager.Show(LanguageManager.GetWord("MailControl.AcceptSuccess"));
			mailMode.AcceptRewardSuccess(takeAttachMsg.mailId);
		}

		//新邮件通知
		private void Fun_12_6(INetData data)
		{
			MailNewMailInformMsg_12_6 mailInfoMsg = new MailNewMailInformMsg_12_6();
			mailInfoMsg.read(data.GetMemoryStream());
			if(mailInfoMsg.code != 0)
			{
				ErrorCodeManager.ShowError(mailInfoMsg.code);
				return;
			}
			mailMode.SetNewUnreadMail(mailInfoMsg.mailCount);
		}

		//一键删除邮件
		private void Fun_12_7(INetData data)
		{
			MailDelAllMsg_12_7 delAllMsg = new MailDelAllMsg_12_7();
			delAllMsg.read(data.GetMemoryStream());
			if(delAllMsg.code != 0)
			{
				ErrorCodeManager.ShowError(delAllMsg.code);
				return;
			}
			mailMode.UpdateOneyKeyDelSuccess();
			MessageManager.Show(LanguageManager.GetWord("MailControl.oneKeyDelSuccess"));
		}

		//一键领取附件
		private void Fun_12_8(INetData data)
		{
			MailGetAllAttachMsg_12_8 allAttachMsg = new MailGetAllAttachMsg_12_8();
			allAttachMsg.read(data.GetMemoryStream());
			if(allAttachMsg.code != 0)
			{
				ErrorCodeManager.ShowError(allAttachMsg.code);
				return;
			}
			mailMode.UpdateOneKeyAcceptSuccess();
			MessageManager.Show(LanguageManager.GetWord("MailControl.oneKeyGetSuccess"));
		}
	}
}
