//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LotteryRewardView
//文件描述：
//创建者：黄江军
//创建日期：2013-12-25
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using Com.Game.Module.Tips;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.game.module.test;
using com.u3d.bases.debug;
using Com.Game.Module.Manager;
using com.game.data;
using com.game.manager;

namespace Com.Game.Module.Lottery
{
	public enum RewardType
	{
		RewardPreview = 0,
		GetReward = 1
	}

	public class LotteryRewardView : BaseView<LotteryRewardView>  
	{
		public override string url { get { return "UI/Lottery/LotteryRewardView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	
		public override bool IsFullUI { get { return true; }}

		private uint lotteryId;	
		private RewardType rewardType;

		private UILabel labTitle;
		private UILabel labCurTitle;

		private Button btnClose;
		private UICenterOnChild centerOnChild;
		private UIGrid gridCenterOnChild;

		private UIGrid[] gridList;
		private PageIndex pageIndex;
		private int maxPage;

		private List<UIWidgetContainer> itemList;

		public LotteryRewardView() 
		{	
			itemList = new List<UIWidgetContainer>();  
		}	

		protected override void Init()
		{
			labTitle = FindInChild<UILabel>("common/title/label");
			labCurTitle = FindInChild<UILabel>("center/title");
			btnClose = FindInChild<Button>("top/btn_close");
			centerOnChild = FindInChild<UICenterOnChild>("center/container/oncenter");
			gridCenterOnChild = FindInChild<UIGrid>("center/container/oncenter");
			gridList = transform.GetComponentsInChildren<UIGrid>();

			try
			{
				centerOnChild.enabled = false;

				foreach (UIGrid grid in gridList) 
				{
					grid.repositionNow = true;
				}

				InitRewardItemList();
				InitLabel();

				maxPage = gridCenterOnChild.maxPerLine;
				pageIndex = FindChild("center/fanye").AddMissingComponent<PageIndex>();
				pageIndex.RegisterOnCenter(centerOnChild);
				pageIndex.InitPage(1, maxPage);

				btnClose.onClick += CloseOnClick;
			}
			finally
			{
				centerOnChild.enabled = true;
			}
		}

		private void InitLabel()
		{
			labTitle.text = LanguageManager.GetWord("LotteryView.Star");
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			UpdateRewardsInfo();
		}

		public void ShowWindow(uint id, RewardType type)
		{
			lotteryId = id;
			rewardType = type;
			OpenView();
		}
		
		//关闭按钮事件处理
		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		//初始化奖品列表
		private void InitRewardItemList()
		{
			for (int i=1; i<=5; i++)
			{
				UIGrid grid = FindInChild<UIGrid>("center/container/oncenter/" + i + "/grid");
				itemList.AddRange(grid.gameObject.GetComponentsInChildren<UIWidgetContainer>(true));
			}

			for (int i = 0; i < itemList.Count; i++)
			{
				if (itemList[i].name.Equals("grid"))
				{
					itemList.RemoveAt(i);
					i--;
				}
			}

			foreach (UIWidgetContainer temp in itemList)
			{
				Singleton<ItemManager>.Instance.InitItem(temp, 1, ItemType.BaseGoods);
			}			
		}

		//更新奖品信息
		private void UpdateRewardsInfo()
		{
			//先统一处理为空格子
			for (int i = 0, length = itemList.Count; i < length; i++) 
			{
				UIWidgetContainer item = itemList[i];
				Singleton<ItemManager>.Instance.InitItem(item, 1, 0);
				item.onClick = null;
			}

			//显示奖品图标
			if (RewardType.RewardPreview == rewardType)
			{
				labCurTitle.text = LanguageManager.GetWord("LotteryView.RewardPreview");
				for (int i=0; i<Singleton<LotteryMode>.Instance.LotteryData[lotteryId].RewardList.Count; i++)
				{
					RewardItem item = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].RewardList[i];
					Singleton<ItemManager>.Instance.InitItem(itemList[i], (uint)item.RewardId, 0);
					itemList[i].onClick = ShowRewardTips;
				}
			}
			else
			{
				labCurTitle.text = LanguageManager.GetWord("LotteryView.GetReward");
				for (int i=0; i<Singleton<LotteryMode>.Instance.LotteryData[lotteryId].CurRewardList.Count; i++)
				{
					uint goodsId = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].CurRewardList[i];
					Singleton<ItemManager>.Instance.InitItem(itemList[i], goodsId, 0);
					itemList[i].onClick = ShowRewardTips;
				}
			}
		}

		//显示奖品说明
		private void ShowRewardTips(GameObject go)
		{
			UIWidgetContainer currentItem = go.GetComponent<UIWidgetContainer>();

			int itemIndex;
			for (itemIndex=0; itemIndex<itemList.Count; itemIndex++)
			{
				if (itemList[itemIndex].Equals(currentItem))
				{
					break;
				}
			}

			if (itemIndex >= itemList.Count)
			{
				return;
			}

			RewardItem item = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].RewardList[itemIndex];
			SysItemVo goodsVo = BaseDataMgr.instance.getGoodsVo((uint)item.RewardId);
            Singleton<TipsManager>.Instance.OpenTipsByGoodsId((uint)item.RewardId, null, null, string.Empty, string.Empty);
		}
		
	}
}
