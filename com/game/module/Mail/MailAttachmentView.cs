//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：MailAttachmentView
//文件描述：邮件附件界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.manager;
using Com.Game.Module.Manager;
using com.game.module.test;
using Com.Game.Module.Tips;
using PCustomDataType;
using Proto;

namespace com.game.module.Mail
{
	public class MailAttachmentView : Singleton<MailAttachmentView>
	{
		private GameObject attachmentViewGameObject;

		private Button btn_close;
		private Button btn_accept;

		private UILabel diamondLabel;
		private UILabel bindDiamondLabel;
		private UILabel goldLabel;
		private UILabel tips;
		private UILabel contentLabel;
		private UILabel titleLabel;

		private List<Transform> itemGoodsList;
		private MailGetInfoMsg_12_2 getInfo;

		public void Init(GameObject obj)
		{
			attachmentViewGameObject = obj;
			
			initView();
			initClick();
			initLanguage();
		}

		private void initView()
		{
			btn_close        = NGUITools.FindInChild<Button>(attachmentViewGameObject, "btn_close");
			btn_accept       = NGUITools.FindInChild<Button>(attachmentViewGameObject, "btn_accept");

			diamondLabel     = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "Money/Diamond/label");
			bindDiamondLabel = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "Money/BindDiamond/label");
			goldLabel        = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "Money/Gold/label");
			tips         = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "tishi");
			contentLabel = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "contentLabel");
			titleLabel   = NGUITools.FindInChild<UILabel>(attachmentViewGameObject , "titleLabel");
		}

		private void initClick()
		{
			btn_close.onClick  = btnCloseOnClick;
			btn_accept.onClick = btnAcceptOnClick;

			itemGoodsList = new List<Transform>();
			for (int i = 0 ; i < MailConst.ATTACHITEM_3 ; i ++)
			{
				itemGoodsList.Add(NGUITools.FindInChild<Transform>(attachmentViewGameObject , "Rewards/item_" + i));
			}
		}

		private void initLanguage()
		{
			tips.text = LanguageManager.GetWord("MailAttachmentView.tips");
		}

		private void btnCloseOnClick(GameObject go)
		{
			closeView();
		}

		//领取
		private void btnAcceptOnClick(GameObject go)
		{
			Singleton<MailMode>.Instance.MailInfoVo.CurrentGetAwardMailId = getInfo.mailId;
			Singleton<MailMode>.Instance.AcceptReward(getInfo.mailId);
		}

		public void setItemInfo(MailGetInfoMsg_12_2 info)
		{
			getInfo = info;
			titleLabel.text = getInfo.title;
			contentLabel.text = getInfo.content;
			setMoneyValue(getInfo.diam , info.diamBind , info.gold);
			setReward(getInfo.mailAttachList);
			setAcceptBtn(getInfo.mailAttachList.Count);
		}

		//设置货币值
		private void setMoneyValue(uint diam , uint diamBind , uint gold)
		{
			diamondLabel.text = diam + string.Empty;
			bindDiamondLabel.text = diamBind + string.Empty;
			goldLabel.text = gold + string.Empty;
		}

		//设置附件奖励物品显示
		private void setReward(List<PMailAttach> mailAttachList)
		{
			for(int i = 0 ; i < MailConst.ATTACHITEM_3 ; i ++)
			{
				Transform awardItem = itemGoodsList[i];
				ItemContainer awardItemContainer = awardItem.gameObject.AddMissingComponent<ItemContainer>();
				awardItem.FindChild("background").GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
				awardItem.FindChild("background").GetComponent<UISprite>().spriteName = "tbk";
				awardItem.FindChild("icon").GetComponent<UISprite>().spriteName = "";
				awardItem.FindChild("icon").GetComponent<UISprite>().atlas = null;
				awardItem.FindChild("count").GetComponent<UILabel>().text = "";
				awardItemContainer.Id = 0;
				if(i < mailAttachList.Count)
				{
					int count   = mailAttachList[i].count;
					uint goodId = mailAttachList[i].id;
					if(count != 0)
					{
						awardItem.FindChild("count").GetComponent<UILabel>().text = "X" + count;
					}
					Singleton<ItemManager>.Instance.InitItem(awardItem.gameObject, 
					                                         (uint)goodId, ItemType.BaseGoods);
					awardItemContainer.Id = goodId;
					awardItem.GetComponent<Button>().onClick = awardItemOnClick;
				}
			}
		}

		//点击到了具体的奖励物品
		private void awardItemOnClick(GameObject go)
		{
			ItemContainer currentClickMailItem = go.GetComponent<ItemContainer>();
			if(currentClickMailItem.Id != 0)
			{
				Singleton<TipsManager>.Instance.OpenTipsByGoodsId(currentClickMailItem.Id, null, null, "", "", 0);
			}
		}

		//设置领取按钮状态
		private void setAcceptBtn(int count)
		{
			if(count > 0)
			{
				btn_accept.SetActive(true);
			}else
			{
				btn_accept.SetActive(false);
			}
		}

		public void openView()
		{
			Singleton<MailMode>.Instance.MailInfoVo.IsOpenAttachView = true;
			attachmentViewGameObject.SetActive(true);
		}

		public void closeView()
		{
			Singleton<MailMode>.Instance.MailInfoVo.CurrentGetAwardMailId = 0;
			diamondLabel.text = bindDiamondLabel.text = goldLabel.text = "0";
			tips.text = "";
			attachmentViewGameObject.SetActive(false);
			Singleton<MailMode>.Instance.MailInfoVo.IsOpenAttachView = false;
			Singleton<MailView>.Instance.mailViewPlayReverse();
		}
	}
}