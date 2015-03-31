//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LuckDrawMode
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using com.game.module.test;
using Proto;
using com.game;
using PCustomDataType;
using com.game.consts;
using com.game.data;
using com.game.utils;
using com.game.manager;

namespace Com.Game.Module.LuckDraw
{
	public enum LockDrawTypeEnum
	{
		Free = 0,
		Once = 1,
		Ten = 2
	}

	public class LuckDrawMode : BaseMode<LuckDrawMode> 
	{	
		public readonly int UPDATE_LUCKDRAW_INFO = 1;
		public readonly int UPDATE_UPDATE_LUCKDRAW_TYPE = 2;
		public readonly int UPDATE_UPDATE_BUY_ERROR = 3;
		public readonly byte UPDATE_TIPS = 4;

		private bool firstLuckDraw = true;
		private uint cdTime;
		private LockDrawTypeEnum luckDrawType;

		private List<PGift> rewardList = new List<PGift>();

		private bool canShowTips = false;

		public override bool ShowTips
		{
			get {return canShowTips && (firstLuckDraw || 0==cdTime);}
		}

		public void StartTips()
		{
			canShowTips = true;
			GetLuckDrawBaseInfo();
		}
		
		public void StopTips()
		{
			canShowTips = false;
			DataUpdate(UPDATE_TIPS);
		}

		private void NotifyTips()
		{
			if (canShowTips)
			{
				DataUpdate(UPDATE_TIPS);
			}
		}

		private void FirstLuckDrawCountDown()
		{
			if (cdTime>0)
			{
				vp_Timer.In(cdTime, CountDownCallback, 1, 0);
			}
		}
		
		private void CountDownCallback()
		{
			if (canShowTips)
			{
				GetLuckDrawBaseInfo();
			}
		}

		public List<PGift> RewardList
		{
			get {return rewardList;}
		}

		public bool FirstLuckDraw
		{
			get {return firstLuckDraw;}
		}

		public uint CDTime
		{
			get {return cdTime;}
		}

		public LockDrawTypeEnum LuckDrawType
		{
			get {return luckDrawType;}
		}

		public void SetLuckDrawTypeAndReward(byte type, List<PGift> reward)
		{
			this.luckDrawType = (LockDrawTypeEnum)type;
			this.rewardList = reward;
			DataUpdate(UPDATE_UPDATE_LUCKDRAW_TYPE);
		}

		public void BuyError()
		{
			DataUpdate(UPDATE_UPDATE_BUY_ERROR);
		}

		//萌宠献礼抽一次所需钻石
		public int DiamondNeedsForOnce
		{
			get
			{
				SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.LuckDrawOnece);
				string diamNeed = StringUtils.GetValueListFromString(priceVo.diam)[0];
				return int.Parse(diamNeed);
			}
		}

		//萌宠献礼抽十次所需钻石
		public int DiamondNeedsForTenth
		{
			get
			{
				SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.LuckDrawTenth);
				string diamNeed = StringUtils.GetValueListFromString(priceVo.diam)[0];
				return int.Parse(diamNeed);
			}
		}

		public void SetBaseInfo(bool first, uint freeCd)
		{
			this.firstLuckDraw = first;
			this.cdTime = freeCd;
			DataUpdate(UPDATE_LUCKDRAW_INFO);
			NotifyTips();
			FirstLuckDrawCountDown();
		}

		//抽奖基础信息
		public void GetLuckDrawBaseInfo()
		{
			MemoryStream msdata = new MemoryStream();
			Module_33.write_33_1(msdata);
			AppNet.gameNet.send (msdata, 33, 1); 
		}

		//免费抽一次
		public void LuckDrawFree()
		{
			StartLuckDraw(0);
		}

		//付费抽1次
		public void LuckDrawOnce()
		{
			StartLuckDraw(1);
		}

		//付费抽10次
		public void LuckDrawTen()
		{
			StartLuckDraw(2);
		}

		//抽奖
		private void StartLuckDraw(byte type)
		{
			MemoryStream msdata = new MemoryStream();
			Module_33.write_33_2(msdata, type);
			AppNet.gameNet.send (msdata, 33, 2); 
		}
		
	}
}
