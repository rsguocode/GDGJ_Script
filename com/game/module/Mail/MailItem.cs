//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailItem
//文件描述：邮件列表
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.manager;
using com.game.module.test;
using com.game.utils;
using PCustomDataType;

namespace com.game.module.Mail
{
	public class MailItem : Singleton<MailItem>
	{
		private GameObject mailItemGameObejct;

		private UISprite giftIcon;
		private UISprite readTag;
		private UISprite ReadBackground;
		private UISprite UnReadBackground;
		private UISprite SelectedBackground;

		private UILabel timeLabel;
		private UILabel titleLabel;
		private UILabel unReadTag;

		private UIToggle checkBox;

		private bool isSelect;
		private PMailBasicInfo mailBaseInfo;

		public void Init(GameObject obj)
		{
			mailItemGameObejct = obj;
			
			initView();
		}

		private void initView()
		{
			checkBox = NGUITools.FindInChild<UIToggle>(mailItemGameObejct , "checkBox");
			checkBox.value = false;
			checkBox.SetActive(true);
			EventDelegate.Add(checkBox.onChange , checkBoxOnChange);

			giftIcon           = NGUITools.FindInChild<UISprite>(mailItemGameObejct , "giftIcon");
			readTag            = NGUITools.FindInChild<UISprite>(mailItemGameObejct , "readTag");
			ReadBackground     = NGUITools.FindInChild<UISprite>(mailItemGameObejct , "ReadBackground");
			UnReadBackground   = NGUITools.FindInChild<UISprite>(mailItemGameObejct , "UnReadBackground");
			SelectedBackground = NGUITools.FindInChild<UISprite>(mailItemGameObejct , "SelectedBackground");

			timeLabel  = NGUITools.FindInChild<UILabel>(mailItemGameObejct , "timeLabel");
			titleLabel = NGUITools.FindInChild<UILabel>(mailItemGameObejct , "titleLabel");
			unReadTag  = NGUITools.FindInChild<UILabel>(mailItemGameObejct , "unReadTag");
		}

		private void checkBoxOnChange()
		{
			//下面这两句是为了保证勾选了后UI能立即响应，这个是NGUI的一个Bug
			checkBox.activeSprite.gameObject.SetActive(false);
			checkBox.activeSprite.gameObject.SetActive(true);
			List<uint> selectMail = Singleton<MailMode>.Instance.MailInfoVo.SelectMail;
			if(checkBox.value == true)
			{
				isSelect = true;
				if(mailBaseInfo == null)
				{
					return;
				}
				if(!selectMail.Contains(mailBaseInfo.id))
				{
					Singleton<MailMode>.Instance.MailInfoVo.SelectMail.Add(mailBaseInfo.id);
				}
			}else
			{
				isSelect = false;
				if(mailBaseInfo == null)
				{
					return;
				}
				if(selectMail.Contains(mailBaseInfo.id))
				{
					Singleton<MailMode>.Instance.MailInfoVo.SelectMail.Remove(mailBaseInfo.id);
				}
			}
		}

		public void SetItemInfo(PMailBasicInfo info)
		{
			mailBaseInfo = info;
			setMailTitleAndTime();
			setMailState();
			setMailAttach();
		}

		public void SetItemSelect(uint id)
		{
			SelectedBackground.SetActive(false);
			if(id == mailBaseInfo.id)
			{
				SelectedBackground.SetActive(true);
			}
		}

		//设置邮件标题和时间
		private void setMailTitleAndTime()
		{
			timeLabel.text = TimeUtil.GetTimeYyyymmddHhmmss(mailBaseInfo.sendTime);
			titleLabel.text = mailBaseInfo.title;
		}

		//设置邮件状态
		private void setMailState()
		{
			if(mailBaseInfo.status == (int)MailConst.readStatus.AleadyRead)
			{
				unReadTag.text = "";
				timeLabel.color = titleLabel.color = Color.gray;
				UnReadBackground.gameObject.SetActive(false);
				readTag.gameObject.SetActive(true);
				ReadBackground.gameObject.SetActive(true);
			}else
			{
				timeLabel.color = titleLabel.color = Color.yellow;
				unReadTag.text = LanguageManager.GetWord("MailItem.UnRead");
				UnReadBackground.gameObject.SetActive(true);
				readTag.gameObject.SetActive(false);
				ReadBackground.gameObject.SetActive(false);
			}
		}

		//设置邮件是否有附件
		private void setMailAttach()
		{
			if(mailBaseInfo.type == (int)MailConst.attachStatus.HasAttachment)
			{
				giftIcon.gameObject.SetActive(true);
			}else
			{
				giftIcon.gameObject.SetActive(false);
			}
		}

		//设置checkBox的选中状态
		public void SetCheckBoxState(bool state)
		{
			checkBox.value = state;
		}

		//销毁
		public void Dispose()
		{
			timeLabel.text = "";
			titleLabel.text = "";
			unReadTag.text = "";
			mailBaseInfo = null;

			checkBox.value = false;
			checkBox.SetActive(false);
			giftIcon.SetActive(false);
			readTag.SetActive(false);
			ReadBackground.SetActive(false);
			UnReadBackground.SetActive(false);
			SelectedBackground.SetActive(false);
		}
	}
}