//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailView
//文件描述：邮件界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.manager;
using com.game.module.test;

using PCustomDataType;
using Proto;

namespace com.game.module.Mail
{
	public class MailView : BaseView<MailView>
	{
		public override string url { get { return "UI/Mail/MailView.assetbundle"; } }
		public override ViewLayer layerType { get  { return ViewLayer.MiddleLayer ;} }
		public override bool IsFullUI { get { return true ; } } 

		private UILabel titleLabel;
		private Button btn_close;
		private Button btn_oneKeyExtract;
		private Button btn_oneKeyDel;
		private Button btn_del;

		private UIToggle delCheckBox;
		private GameObject noMailListImage;
		private TweenPlay mailViewTP;
		private UIScrollView scrollView;
		private UIGrid mailItemGrid;
		private GameObject mailItem;

		private bool isInstantiationChildView;
		private List<ItemContainer> mailItemContainerList;
		private List<MailItem> mailItemList;

		protected override void Init()
		{
			initView();
			initClick();
			initTextLanguage();
		}

		private void initView()
		{
			titleLabel = FindInChild<UILabel>("left/titleLabel");

			btn_close = FindInChild<Button>("left/btn_close");
			btn_oneKeyExtract = FindInChild<Button>("left/BtnOneKeyExtract");
			btn_oneKeyDel     = FindInChild<Button>("left/BtnOneKeyDel");
			btn_del           = FindInChild<Button>("left/BtnDelete");

			noMailListImage   = NGUITools.FindChild(gameObject , "left/noMailList");
			delCheckBox       = NGUITools.FindInChild<UIToggle>(gameObject, "left/Checkbox");
			delCheckBox.value = false;
			delCheckBox.SetActive(false);
			delCheckBox.onChange.Add(new EventDelegate(delCheckBoxOnClick));

			mailViewTP        = FindInChild<TweenPlay>("left");
			scrollView = FindInChild<UIScrollView>("left/content");
			mailItemGrid = FindInChild<UIGrid>("left/content/grid");
			mailItemGrid.onReposition += scrollView.ResetPosition;

			mailItemContainerList = new List<ItemContainer>();
			mailItem = NGUITools.FindChild(gameObject , "left/content/grid/item_0");
			ItemContainer itemContainer = mailItem.AddMissingComponent<ItemContainer>();
			mailItemContainerList.Add(itemContainer);
			mailItem.SetActive(false);
		}

		private void initClick()
		{
			btn_close.onClick = closeOnClick;
			btn_oneKeyExtract.onClick = oneKeyExtractOnClick;
			btn_oneKeyDel.onClick     = oneKeyDelOnClick;
			btn_del.onClick = delBtnOnClick;
		}

		private void initTextLanguage()
		{
			titleLabel.text = LanguageManager.GetWord("MainTopLeftView.Mail");
			btn_oneKeyExtract.FindInChild<UILabel>("label").GetComponent<UILabel>().text = LanguageManager.GetWord("MailInboxView.onekeyextract");
			btn_oneKeyDel.FindInChild<UILabel>("label").GetComponent<UILabel>().text = LanguageManager.GetWord("MailInboxView.onekeydel");
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			Singleton<MailMode>.Instance.RequestMailBasicInfo();
			instantiationChildView();
			mailViewTP.PlayReverse();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
		}

		public override void Destroy()
		{
			base.Destroy();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<MailMode>.Instance.dataUpdated += UpdateMailInfoHandler;
			Singleton<MailMode>.Instance.dataUpdated += UpdateCurrentMailHandler;
			Singleton<MailMode>.Instance.dataUpdated += GetAwardSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated += DelMailSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated += OnekeyAttachSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated += OneKeyDelSuccessHandler;
		}

		public override void CancelUpdateHandler()
		{
			Singleton<MailMode>.Instance.dataUpdated -= UpdateMailInfoHandler;
			Singleton<MailMode>.Instance.dataUpdated -= UpdateCurrentMailHandler;
			Singleton<MailMode>.Instance.dataUpdated -= GetAwardSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated -= DelMailSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated -= OnekeyAttachSuccessHandler;
			Singleton<MailMode>.Instance.dataUpdated -= OneKeyDelSuccessHandler;
		}

		//实例化子界面（附件）
		private void instantiationChildView()
		{
			if(!isInstantiationChildView)
			{
				Singleton<MailAttachmentView>.Instance.Init(NGUITools.FindChild(gameObject , "right"));
				isInstantiationChildView = true;
			}
		}

		//一键领取
		private void oneKeyExtractOnClick(GameObject go)
		{
			Singleton<MailMode>.Instance.OneyKeyGetAllAttach();
		}

		//一键删除
		private void oneKeyDelOnClick(GameObject go)
		{
			Singleton<MailMode>.Instance.OneKeyDelAll();
		}

		//删除邮件
		private void delBtnOnClick(GameObject go)
		{
			Singleton<MailMode>.Instance.RequestDeleteMail();
		}

		//更新邮件信息
		private void UpdateMailInfoHandler(object sender , int code)
		{
			if(code == MailMode.UPDATE_MAILLIST)
			{
				setEveryMailItemInfo();
				setNoMailList();
			}
		}

		//更新当前邮件信息
		private void UpdateCurrentMailHandler(object sender , int code)
		{
			MailGetInfoMsg_12_2 currentMailItemInfo = Singleton<MailMode>.Instance.MailInfoVo.CurrentMailItemInfo;
			if(code == MailMode.UPDATE_CURRENTMAIL)
			{
				if(!Singleton<MailMode>.Instance.MailInfoVo.IsOpenAttachView)
				{
					setOnekeyBtnState(false);
					mailViewTP.PlayForward();
					Singleton<MailAttachmentView>.Instance.openView();
				}
				for(int i = 0 ; i < mailItemContainerList.Count ; i ++)
				{
					if(mailItemContainerList[i].Id == currentMailItemInfo.mailId)
					{
						MailItem mailItem = mailItemList[i];
						PMailBasicInfo mailBasicInfo = MailUtil.findMailDataByID(currentMailItemInfo.mailId);
						mailItem.SetItemInfo(mailBasicInfo);
						Singleton<MailAttachmentView>.Instance.setItemInfo(currentMailItemInfo);
						break;
					}
				}
			}
		}

		//成功领取附件奖励
		private void GetAwardSuccessHandler(object sender , int code)
		{
			if(code == MailMode.GETAWARD_SUCCESS)
			{
				MailGetInfoMsg_12_2 currentMailItemInfo = Singleton<MailMode>.Instance.MailInfoVo.CurrentMailItemInfo;
				Singleton<MailAttachmentView>.Instance.setItemInfo(currentMailItemInfo);
				for(int i = 0 ; i < mailItemContainerList.Count ; i ++)
				{
					if(mailItemContainerList[i].Id == currentMailItemInfo.mailId)
					{
						MailItem mailItem = mailItemList[i];
						PMailBasicInfo mailBasicInfo = MailUtil.findMailDataByID(currentMailItemInfo.mailId);
						mailItem.SetItemInfo(mailBasicInfo);
						break;
					}
				}
			}
		}

		//成功删除邮件
		private void DelMailSuccessHandler(object sender , int code)
		{
			if(code == MailMode.DELETE_MAILSUCCESS)
			{
				List<PMailBasicInfo> mailList = Singleton<MailMode>.Instance.MailListInfoVo.MailList;
				int mailListCount = mailList.Count;
				ItemContainer mailItemContainer;
				for(int i = 0 ; i < mailItemContainerList.Count ; i ++)
				{
					mailItemContainer = mailItemContainerList[i];
					if(i >= mailListCount)
					{
						mailItemContainer.SetActive(false);
					}else
					{
						mailItemContainer.SetActive(true);
						mailItemList[i].SetItemInfo(mailList[i]);
					}
				}
				if(mailListCount == 0)
				{
					setOnekeyBtnState(true);
					setNoMailList();
					Singleton<MailAttachmentView>.Instance.closeView();
				}

			}
			mailItemGrid.Reposition();
		}

		//设置一键领取附件成功
		private void OnekeyAttachSuccessHandler(object sender , int code)
		{
			if(code == MailMode.UPDATE_ONEKEYATTACHE)
			{
				List<PMailBasicInfo> mailList = Singleton<MailMode>.Instance.MailListInfoVo.MailList;
				for(int i = 0 ; i < mailList.Count ; i ++)
				{
					MailItem mailItem = mailItemList[i];
					mailItem.SetItemInfo(mailList[i]);
				}
			}
		}

		//设置一键删除邮件成功
		private void OneKeyDelSuccessHandler(object sender , int code)
		{
			if(code == MailMode.UPDATE_ONEKEYDEL)
			{
				List<PMailBasicInfo> mailList = Singleton<MailMode>.Instance.MailListInfoVo.MailList;
				int mailListCount = mailList.Count;
				ItemContainer mailItemContainer;
				for(int i = 0 ; i < mailItemContainerList.Count ; i ++)
				{
					mailItemContainer = mailItemContainerList[i];
					if(i >= mailListCount)
					{
						mailItemContainer.SetActive(false);
					}
				}
				mailItemList = new List<MailItem>();
				setNoMailList();
			}
		}

		//无邮件提示
		private void setNoMailList()
		{
			if(Singleton<MailMode>.Instance.MailListInfoVo.MailList.Count == 0)
			{
				noMailListImage.SetActive(true);
			}else
			{
				noMailListImage.SetActive(false);
			}
		}

		//设置每一封邮件信息
		private void setEveryMailItemInfo()
		{
			noMailListImage.SetActive(false);
			GameObject mailItemGameObject;
			ItemContainer mailItemContainer;
			List<PMailBasicInfo> mailList = Singleton<MailMode>.Instance.MailListInfoVo.MailList;
			int mailListCount = mailList.Count;
			while(mailItemContainerList.Count < mailListCount)
			{
				mailItemGameObject = NGUITools.AddChild(mailItemGrid.gameObject , mailItem.gameObject);
				mailItemContainer = mailItemGameObject.AddMissingComponent<ItemContainer>();
				mailItemContainerList.Add(mailItemContainer);
			}
			mailItemList = new List<MailItem>();
			for(int i = 0 ; i < mailItemContainerList.Count ; i ++)
			{
				mailItemContainer = mailItemContainerList[i];
				if(i >= mailListCount)
				{
					mailItemContainer.SetActive(false);
				}else
				{
					mailItemContainer.SetActive(true);
					mailItemContainer.Id = mailList[i].id;
					mailItemContainer.onClick = mailItemOnClick;
					setEveryMailInfo(mailItemContainer , mailList[i]);
				}
			}
			mailItemGrid.Reposition();
		}

		//设置每封邮件信息
		private void setEveryMailInfo(ItemContainer mailItemContainer , PMailBasicInfo info)
		{
			MailItem mailItem = new MailItem();
			mailItem.Init(mailItemContainer.gameObject);
			mailItem.SetItemInfo(info);
			mailItemList.Add(mailItem);
		}

		//点击某一封邮件
		private void mailItemOnClick(GameObject go)
		{
			ItemContainer currentMailItem = go.GetComponent<ItemContainer>();
			for(int i = 0 ; i < mailItemList.Count ; i ++)
			{
				mailItemList[i].SetItemSelect(currentMailItem.Id);
			}
			Singleton<MailMode>.Instance.MailInfoVo.CurrentDelMailId = currentMailItem.Id;
			Singleton<MailMode>.Instance.RequestMailDetail(currentMailItem.Id);
		}

		//清空邮件列表选中状态
		private void disposeMailItem()
		{
			if(mailItemList == null || mailItemList.Count == 0)
			{
				return;
			}
			for(int i = 0 ; i < mailItemList.Count ; i ++)
			{
				mailItemList[i].Dispose();
			}
		}

		//隐藏一键删除/一键领取
		private void setOnekeyBtnState(bool state)
		{
			btn_oneKeyDel.SetActive(state);
			btn_oneKeyExtract.SetActive(state);
			btn_del.SetActive(!state);
			delCheckBox.SetActive(!state);
		}

		//选中全部删除
		private void delCheckBoxOnClick()
		{
			if(delCheckBox.value == true)
			{
				for(int i = 0 ; i < mailItemList.Count ; i ++)
				{
					mailItemList[i].SetCheckBoxState(true);
				}
			}else
			{
				for(int i = 0 ; i < mailItemList.Count ; i ++)
				{
					mailItemList[i].SetCheckBoxState(false);
				}
			}
		}

		//界面右-左播放
		public void mailViewPlayReverse()
		{
			mailViewTP.PlayReverse();
		}

		private void closeOnClick(GameObject go)
		{
			isInstantiationChildView = false;
			delCheckBox.value = false;
			Singleton<MailAttachmentView>.Instance.closeView();
			disposeMailItem();
			setOnekeyBtnState(true);
			Singleton<MailMode>.Instance.MailInfoVo.CurrentDelMailId = 0;
			this.CloseView();
		}
	}
}