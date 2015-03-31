//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LotteryView
//文件描述：
//创建者：黄江军
//创建日期：2013-12-25
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using Com.Game.Module.Tips;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using Com.Game.Module.Manager;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Role;
using com.game.consts;

namespace Com.Game.Module.Lottery
{
	public class LotteryView : BaseView<LotteryView>  
	{	
		public override string url { get { return "UI/Lottery/LotteryView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}
		public override bool IsFullUI { get { return true; }}

		private uint lotteryId;		
		private int starNeeds;

		private UILabel labHighGoods;
		private UILabel labTitle;

		private Button btnClose;
		private Button btnRewardPreview;
		private Button btnLotteryOne;
		private Button btnLotteryTen;
		private Button btnLotteryHundred;

		private UISprite sprStarBk;
		private UILabel labStarRemain;

		private UISprite sprHighGoods1;
		private UILabel labHighGoodsInfo1;
		private UIWidgetContainer widgetGoods1;

		private UISprite sprHighGoods2;
		private UILabel labHighGoodsInfo2;
		private UIWidgetContainer widgetGoods2;

		private UISprite sprHighGoods3;
		private UILabel labHighGoodsInfo3;
		private UIWidgetContainer widgetGoods3;

		public LotteryView() 
		{	
		}		

		
		protected override void Init()
		{
			labTitle = FindInChild<UILabel>("common/title/label");
			labHighGoods = FindInChild<UILabel>("bottom/topleft/taitou/xsqg");

			btnClose = FindInChild<Button>("top/btn_close");
			btnRewardPreview = FindInChild<Button>("bottom/topright/btn_reward");
			btnLotteryOne = FindInChild<Button>("bottom/bottomright/lottery_one");
			btnLotteryTen = FindInChild<Button>("bottom/bottomright/lottery_ten");
			btnLotteryHundred = FindInChild<Button>("bottom/bottomright/lottery_hundred");

			sprStarBk = FindInChild<UISprite>("bottom/topright/star/background");
			labStarRemain = FindInChild<UILabel>("bottom/topright/star/count");

			sprHighGoods1 = FindInChild<UISprite>("bottom/topleft/wp/wp1/item/tubiao");
			labHighGoodsInfo1 = FindInChild<UILabel>("bottom/topleft/wp/wp1/shuzi");
			widgetGoods1 = FindInChild<UIWidgetContainer>("bottom/topleft/wp/wp1/item");

			sprHighGoods2 = FindInChild<UISprite>("bottom/topleft/wp/wp2/item/tubiao");
			labHighGoodsInfo2 = FindInChild<UILabel>("bottom/topleft/wp/wp2/shuzi");
			widgetGoods2 = FindInChild<UIWidgetContainer>("bottom/topleft/wp/wp2/item");

			sprHighGoods3 = FindInChild<UISprite>("bottom/topleft/wp/wp3/item/tubiao");
			labHighGoodsInfo3 = FindInChild<UILabel>("bottom/topleft/wp/wp3/shuzi");
			widgetGoods3 = FindInChild<UIWidgetContainer>("bottom/topleft/wp/wp3/item");

			InitLabel();
			
			btnClose.onClick = CloseOnClick;
			btnRewardPreview.onClick = RewardPreviewOnClick;
			btnLotteryOne.onClick = LotteryOnClick;
			btnLotteryTen.onClick = LotteryTenClick;
			btnLotteryHundred.onClick = LotteryHundredClick;
		}

		private void InitLabel() 
		{
			labTitle.text = LanguageManager.GetWord("LotteryView.Star");
			labHighGoods.text = LanguageManager.GetWord("LotteryView.HighRewardShow");
			btnRewardPreview.label.text = LanguageManager.GetWord("LotteryView.RewardPreview");
			btnLotteryOne.label.text = LanguageManager.GetWord("LotteryView.LotteryOne");
			btnLotteryTen.label.text = LanguageManager.GetWord("LotteryView.LotteryTen");
			btnLotteryHundred.label.text = LanguageManager.GetWord("LotteryView.LotteryHundred");
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();			

			string GOODS_ATLAS = "OthersIcon";
			string[] starSpriteName = {"", "bai_btn_bai", "lv_btn_lv", "lan_btn_lan", "zi_btn_zi", "huang_btn_huang", "hong_btn_red",};

			sprStarBk.spriteName = starSpriteName[lotteryId];
			SortStarRemain();

			//获得高级物品信息
			int highGoodsCount = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].HighRewardList.Count;
			UISprite[] sprHighGoodsArr = {sprHighGoods1, sprHighGoods2, sprHighGoods3};
			UILabel[] labHighGoodsArr = {labHighGoodsInfo1, labHighGoodsInfo2, labHighGoodsInfo3};
			UIWidgetContainer[] wigGoodsArr = {widgetGoods1, widgetGoods2, widgetGoods3};
			for (int i=0; i<highGoodsCount; i++)			
			{
				int goodsId = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].HighRewardList[i];
				SysItemVo goodsVo = BaseDataMgr.instance.getGoodsVo((uint)goodsId);

				if (null != goodsVo)
				{
					sprHighGoodsArr[i].atlas = Singleton<AtlasManager>.Instance.GetAtlas(GOODS_ATLAS);
					sprHighGoodsArr[i].spriteName = goodsVo.icon.ToString();
					labHighGoodsArr[i].text = goodsVo.name;
					wigGoodsArr[i].onClick = ShowHighGoodTips;
				}
				else
				{
					sprHighGoodsArr[i].atlas = null;
					labHighGoodsArr[i].text = "";
					wigGoodsArr[i].onClick = null;
				}
			}
		}

		private void ShowHighGoodTips(GameObject go)
		{
			UIWidgetContainer currentItem = go.GetComponent<UIWidgetContainer>();
			UIWidgetContainer[] wigGoodsArr = {widgetGoods1, widgetGoods2, widgetGoods3};
			
			int itemIndex;
			for (itemIndex=0; itemIndex<wigGoodsArr.Length; itemIndex++)
			{
				if (wigGoodsArr[itemIndex].Equals(currentItem))
				{
					break;
				}
			}
			
			if (itemIndex >= wigGoodsArr.Length)
			{
				return;
			}
			
			int goodsId = Singleton<LotteryMode>.Instance.LotteryData[lotteryId].HighRewardList[itemIndex];
			SysItemVo goodsVo = BaseDataMgr.instance.getGoodsVo((uint)goodsId);
            Singleton<TipsManager>.Instance.OpenTipsByGoodsId((uint)goodsId, null, null, string.Empty, string.Empty);
		}

		//根据星签数量处理相关业务
		private void SortStarRemain()
		{
			labStarRemain.text = Singleton<LotteryMode>.Instance.GetStarRemain(lotteryId).ToString();
			SortCanLottery();
		}

		//处理是否可以抽奖
		private void SortCanLottery()
		{
			int curStarRemain = Singleton<LotteryMode>.Instance.GetStarRemain(lotteryId);
			int curLotteryRemain = Singleton<LotteryMode>.Instance.GetLotteryRemain(lotteryId);

			bool canLotteryOne = (curLotteryRemain >= 1) && (curStarRemain >= starNeeds*1);
			bool canLotteryTen = (curLotteryRemain >= 10) && (curStarRemain >= starNeeds*10);
			bool canLotteryHundred = (curLotteryRemain >= 100) && (curStarRemain >= starNeeds*100);

			bool[] lottetySwitch = {canLotteryOne, canLotteryTen, canLotteryHundred};
			Button[] buttons = {btnLotteryOne, btnLotteryTen, btnLotteryHundred};

			for (int i=0; i<lottetySwitch.Length; i++)
			{
				NGUITools.SetButtonEnabled(buttons[i], lottetySwitch[i]);
			}

		}

		//显示获得奖励界面
		private void ShowGetRewardView()
		{
			Singleton<LotteryRewardView>.Instance.ShowWindow(lotteryId, RewardType.GetReward);
		}

		public void ShowWindow(uint id)
		{
			lotteryId = id;
			starNeeds = Singleton<LotteryMode>.Instance.LotteryData[id].StarNeeds;
			OpenView();
		}
		
		//关闭按钮事件处理
		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}	

		//处理奖励预览点击事件
		private void RewardPreviewOnClick(GameObject go)
		{
			Singleton<LotteryRewardView>.Instance.ShowWindow(lotteryId, RewardType.RewardPreview);
		}	

		//处理抽奖1次按钮点击事件
		private void LotteryOnClick(GameObject go)
		{
			Singleton<LotteryMode>.Instance.StartLottery(lotteryId, 1);
		}	

		//处理抽奖10次按钮点击事件
		private void LotteryTenClick(GameObject go)
		{
			Singleton<LotteryMode>.Instance.StartLottery(lotteryId, 10);
		}	

		//处理抽奖100次按钮点击事件
		private void LotteryHundredClick(GameObject go)
		{
			Singleton<LotteryMode>.Instance.StartLottery(lotteryId, 100);
		}	

		public override void RegisterUpdateHandler()
		{
			Singleton<LotteryMode>.Instance.dataUpdated += UpdateLotteryRemainHandle;	
			Singleton<LotteryMode>.Instance.dataUpdated += UpdateRewardHandle;	
			Singleton<GoodsMode>.Instance.dataUpdated += UpdateStarRemainHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<LotteryMode>.Instance.dataUpdated -= UpdateLotteryRemainHandle;
			Singleton<LotteryMode>.Instance.dataUpdated -= UpdateRewardHandle;	
			Singleton<GoodsMode>.Instance.dataUpdated -= UpdateStarRemainHandle;
		}

		//抽奖剩余次数变更回调
		private void UpdateLotteryRemainHandle(object sender, int type)
		{
			if (Singleton<LotteryMode>.Instance.UPDATE_REMAIN == type)
			{
				SortCanLottery();
			}
		}

		//星签数量变更回调
		private void UpdateStarRemainHandle(object sender, int type)
		{
			if (Singleton<GoodsMode>.Instance.UPDATE_GOODS == type)
			{
				SortStarRemain();
			}
		}

		//抽中奖品回调
		private void UpdateRewardHandle(object sender, int type)
		{
			if (Singleton<LotteryMode>.Instance.UPDATE_REWARD == type)
			{
				ShowGetRewardView();
			}
		}
	}
}
