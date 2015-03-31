//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldBoxMode
//文件描述：
//创建者：黄江军
//创建日期：2014-02-13
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.IO;
using com.game.module.test;
using Proto;
using com.game;

namespace Com.Game.Module.GoldBox
{
	public enum GoldGetType
	{
		Normal = 1,
		SmallCrit = 2,
		LargeCrit = 5
	}

	public class GoldBoxMode : BaseMode<GoldBoxMode>  
	{	
		public readonly int UPDATE_GOLD_INFO = 1;
		public readonly int UPDATE_GOLD_GET_TYPE = 2;

		private ushort buyTimesUsed = 0;  //当天已购买的次数
		private ushort remainBuyTimes = 0;   //当天还可购买的次数
		private ushort diamNeedsForOnce = 0;  //下次购买需要花费的钻石数
		private uint goldNumNextBuy = 0;  //下次购买可获得的金币
		private ushort diamNeedsForBatch = 0;  //下次批量购买需要花费的钻石数

		private byte goldGetType = 0;

		public ushort BuyTimesUsed 
		{
			get
			{
				return buyTimesUsed;
			}
		}

		public ushort RemainBuyTimes 
		{
			get
			{
				return remainBuyTimes;
			}
		}

		public ushort DiamNeedsForOnce 
		{
			get
			{
				return diamNeedsForOnce;
			}
		}

		public uint GoldNumNextBuy 
		{
			get
			{
				return goldNumNextBuy;
			}
		}

		public ushort DiamNeedsForBatch 
		{
			get
			{
				return diamNeedsForBatch;
			}
		}

		public byte GoldGetType 
		{
			get
			{
				return goldGetType;
			}

			set
			{
				goldGetType = value;
				DataUpdate(UPDATE_GOLD_GET_TYPE);
			}
		}

		public GoldBoxMode() 
		{	
		}	

		//获得金币信息
		public void GetGoldBuyInfo()
		{
			MemoryStream msdata = new MemoryStream();
			Module_3.write_3_42(msdata);
			AppNet.gameNet.send (msdata, 3, 42); 
		}

		//购买金币请求
		private void BuyGold(byte value)
		{
			MemoryStream msdata = new MemoryStream();
			Module_3.write_3_43(msdata, value);
			AppNet.gameNet.send (msdata, 3, 43); 
		}

		//单次购买金币
		public void BuyGoldOnce()
		{
			BuyGold(0);
		}

		//批量购买金币
		public void BuyGoldBatch()
		{
			BuyGold(1);
		}

		//设置购买金币信息
		public void SetGoldBuyInfo(ushort buyTimesUsed, ushort remainBuyTimes, ushort diamNeedsForOnce, 
		                             uint goldNumNextBuy, ushort diamNeedsForBatch)
		{
			this.buyTimesUsed = buyTimesUsed;
			this.remainBuyTimes = remainBuyTimes;
			this.diamNeedsForOnce = diamNeedsForOnce;
			this.goldNumNextBuy = goldNumNextBuy;
			this.diamNeedsForBatch = diamNeedsForBatch;

			DataUpdate(UPDATE_GOLD_INFO);
		}
	}
}
