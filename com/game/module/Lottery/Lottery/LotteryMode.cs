//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LotteryMode
//文件描述：
//创建者：黄江军
//创建日期：2013-12-26
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using com.game.module.test;
using com.u3d.bases.debug;
using Proto;
using com.game;
using com.game.manager;
using com.game.data;
using com.game.utils;
using Com.Game.Module.Role;
using PCustomDataType; 

namespace Com.Game.Module.Lottery
{
	public class RewardItem
	{
		public int RewardId;  //奖品id
		public int Count;  //奖品数量
		public int Chance;  //抽奖概率（万分比）
	}

	public class LotteryDataItem
	{
		public int LotteryRemain;  //当前剩余抽奖次数
		public string StarName;  //星签名称
		public int LotteryLimit;    //每日最大抽奖次数
		public int StarRemain;   //星签剩余数量
		public int StarId;  //星签id
		public int StarIcon; //星签图标
		public int StarNeeds;    //每次抽奖所需星签数量
		public IList<RewardItem> RewardList;  //奖品列表
		public IList<int> HighRewardList;  //高级奖品id列表
		public IList<uint> CurRewardList;  //当前奖品列表
	}

	public class LotteryMode : BaseMode<LotteryMode> 
	{	
		public readonly int UPDATE_REMAIN = 1;
		public readonly int UPDATE_REWARD = 2;

		private Dictionary<uint, LotteryDataItem> lotteryData;

		public Dictionary<uint, LotteryDataItem> LotteryData
		{
			get
			{
				return lotteryData;
			}
		}

		public LotteryMode() 
		{
			lotteryData = new Dictionary<uint, LotteryDataItem>();

			for (uint id=1; id<=6; id++)
			{
				lotteryData[id] = new LotteryDataItem();
				lotteryData[id].RewardList = new List<RewardItem>();
				lotteryData[id].HighRewardList = new List<int>();
				lotteryData[id].CurRewardList = new List<uint>();
			}

			InitLotteryData();
		}	

		private void InitLotteryData()
		{
			for (uint id=1; id<=6; id++)
			{
				SysExchangeVo vo = BaseDataMgr.instance.GetSysExchangeVo(id);
				if (null != vo)
				{
					lotteryData[id].LotteryLimit = vo.limit;
					lotteryData[id].LotteryRemain = vo.limit;
					GetLotteryExtraInfo(id, vo);
				}
			}
		}

		private void GetLotteryExtraInfo(uint id, SysExchangeVo vo)
		{
			//获得星签id，每次抽奖所需数量
			string[] needsArr = GetNeedsArr(vo.needs);
			lotteryData[id].StarId = Convert.ToInt32(needsArr[0]);
			lotteryData[id].StarNeeds = Convert.ToInt32(needsArr[1]);

			//获得星签名称、图标
			SysItemVo starVo = BaseDataMgr.instance.getGoodsVo((uint)lotteryData[id].StarId);
			if (null != starVo)
			{
				lotteryData[id].StarName = starVo.name;
				lotteryData[id].StarIcon = starVo.icon;
			}

			//获得高级奖品列表
			string[] highRewardArr = StringUtils.SplitVoString(vo.higheritemid);
			foreach (string item in highRewardArr)
			{
				lotteryData[id].HighRewardList.Add(Convert.ToInt32(item));
			}

			//获得奖品列表
			string[] rewardArr = StringUtils.SplitVoString(vo.rewarditemid, "},");
			foreach (string item in rewardArr)
			{
				lotteryData[id].RewardList.Add(GetRewardItem(item));
			}
		}

		private RewardItem GetRewardItem(string str)
		{
			string[] resultArr;
			str = str.Replace(" ", "");//去除字符串中的空格
			str = str.TrimStart('{');
			str = str.TrimEnd('}');			

			resultArr = str.Split(',');

			RewardItem item = new RewardItem();
			item.RewardId = Convert.ToInt32(resultArr[0]);
			item.Count = Convert.ToInt32(resultArr[1]);
			item.Chance = Convert.ToInt32(resultArr[2]);

			return item;
		}

		private string[] GetNeedsArr(string str)
		{
			string[] resultArr;
			char[] startChar = {'[', '['};
			char[] endChar = {']', ']'};
			str = str.Replace(" ", "");//去除字符串中的空格
			str = str.TrimStart(startChar);
			str = str.TrimEnd(endChar);			

			resultArr = str.Split(',');

			return resultArr;
		}

		public int GetStarRemain(uint id)
		{
			//获得星签剩余数量
			uint goodsId = (uint)lotteryData[id].StarId;
			PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsByGoodsId(goodsId);

			if (null != goods)
			{
				lotteryData[id].StarRemain = goods.count;
			}
			else
			{
				lotteryData[id].StarRemain = 0;
			}

			return lotteryData[id].StarRemain;
		}

		public int GetLotteryRemain(uint id)
		{
			return lotteryData[id].LotteryRemain;
		}

		public void SetLotteryRemain(uint id, int value)
		{
			lotteryData[id].LotteryRemain = value;
			DataUpdate(UPDATE_REMAIN);
		}

		public void SetCurRewardList(uint id, List<uint> value)
		{
			lotteryData[id].CurRewardList.Clear();
			foreach (uint item in value)
			{
				lotteryData[id].CurRewardList.Add(item);
			}

			DataUpdate(UPDATE_REWARD);
		}

		//查询抽奖信息
		//id: 范围0~6， 为0表示查询所有
		public void QueryLotteryInfo(uint id)
		{
			MemoryStream msdata = new MemoryStream();
			Module_16.write_16_1(msdata, id);
			AppNet.gameNet.send(msdata, 16, 1);
			Log.info(this, "查询抽奖信息，id为" + id);
		}

		//开始抽奖
		public void StartLottery(uint id, ushort times)
		{
			MemoryStream msdata = new MemoryStream();
			Module_16.write_16_2(msdata, id, times);
			AppNet.gameNet.send(msdata, 16, 2);
			Log.info(this, "开始抽奖，id：" + id + " 次数：" + times);
		}
		
	}
}
