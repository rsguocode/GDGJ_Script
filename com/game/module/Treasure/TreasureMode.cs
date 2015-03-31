using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.IO;
using com.u3d.bases.debug;
using Proto;
using com.game;
using com.game.manager;
using System.Collections.Generic;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/02/25 09:39:52 
 * function:  地宫寻宝系统数据类
 * *******************************************************/
namespace Com.Game.Module.Treasure
{
	public class TreasureMode : BaseMode<TreasureMode> 
	{
		public readonly int UPDATE_TREASURE_INFO = 1;
		public readonly int UPDATE_GETTED_TREASURE = 2;

		private TreasureInfo _treasureInfo = new TreasureInfo();
		private List<uint> _getTreasureList = new List<uint> ();
		private uint _awardId;
		private uint _additionalAwardId;

		public TreasureInfo TreasureInfo{ get {return _treasureInfo;}}
		public List<uint> GetTreasureList{ get {return _getTreasureList;}}
		public uint AwardId{ get {return _awardId;}}
		public uint AdditionalAwardId{ get {return _additionalAwardId;}}
		//-------------------------------------  协议请求 -----------------------------------------
		//客户端请求寻宝信息
		public void ApplyTreasureInfo()
		{
			Log.info(this, "发送27-0给服务器请求寻宝信息");
			MemoryStream msdata = new MemoryStream ();
			Module_27.write_27_0 (msdata);
			AppNet.gameNet.send(msdata, 27, 0);
		}

		//客户端发起寻宝申请
		public void ApplyGetTreasure(byte takedId)
		{
			Log.info(this, "发送27-1给服务器请求获取宝藏：" + takedId);
			MemoryStream msdata = new MemoryStream ();
			Module_27.write_27_1 (msdata, takedId);
			AppNet.gameNet.send(msdata, 27, 1);
		}

		//客户端请求刷新宝藏信息
		public void ApplyFreshTreasureInfo()
		{
			Log.info(this, "发送27-2给服务器请求刷新宝藏信息");
			MemoryStream msdata = new MemoryStream ();
			Module_27.write_27_2 (msdata);
			AppNet.gameNet.send(msdata, 27, 2);
		}

		public void UpdateTreasureInfo(TreasInfoMsg_27_0 treasInfoMsg)
		{
			_treasureInfo.remainFindTimes = treasInfoMsg.remainTimes;
			_treasureInfo.treasure1 = treasInfoMsg.refresh1.ToString ();
			_treasureInfo.treasure2 = treasInfoMsg.refresh2.ToString ();
			_treasureInfo.treasure3 = treasInfoMsg.refresh3.ToString ();
			_treasureInfo.treasure1Name = BaseDataMgr.instance.GetSysTreasure ((uint)treasInfoMsg.refresh1).place;
			_treasureInfo.treasure2Name = BaseDataMgr.instance.GetSysTreasure ((uint)treasInfoMsg.refresh2).place;
			_treasureInfo.treasure3Name = BaseDataMgr.instance.GetSysTreasure ((uint)treasInfoMsg.refresh3).place;
			_treasureInfo.takedTreasure1Id = (uint)treasInfoMsg.take1;
			_treasureInfo.takedTreasure2Id = (uint)treasInfoMsg.take2;
			_treasureInfo.takedTreasure3Id = (uint)treasInfoMsg.take3;
			DataUpdate (UPDATE_TREASURE_INFO);
		}

		public void GetNewTreasure(uint goodId, uint additionalgoodId)
		{
			_getTreasureList.Clear ();
//			for (int i = 0; i < num; ++i)
//			{
				_getTreasureList.Add(goodId);
			if (additionalgoodId != 0)
				_getTreasureList.Add(additionalgoodId);

			_awardId = goodId;
			_additionalAwardId = additionalgoodId;
//			}
			DataUpdate (UPDATE_GETTED_TREASURE);
		}

	}

	public class TreasureInfo
	{
		public byte remainFindTimes;             //免费寻宝剩余次数
		public string treasure1;             //宝藏点1图片名字
		public string treasure2;             //宝藏点2图片名字
		public string treasure3;             //宝藏点3图片名字
		public string treasure1Name;         //宝藏点1名字
		public string treasure2Name;         //宝藏点2名字
		public string treasure3Name;         //宝藏点3名字

		public uint takedTreasure1Id;
		public uint takedTreasure2Id;
		public uint takedTreasure3Id;
//		public string takedTreasure1;        //已获取宝藏点1图片名字
//		public string takedTreasure2;        //已获取宝藏点2图片名字
//		public string takedTreasure3;        //已获取宝藏点3图片名字
//		public string takedTreasure1Name;    //已获取宝藏点1名字     
//		public string takedTreasure2Name;    //已获取宝藏点2名字
//		public string takedTreasure3Name;    //已获取宝藏点3名字

	}

}