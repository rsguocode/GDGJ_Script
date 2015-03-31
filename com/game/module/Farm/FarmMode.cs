using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using System.IO;
using Proto;
using com.game;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/04 09:39:52 
 * function:  家园种植系统数据类
 * *******************************************************/
using System.Collections.Generic;
using PCustomDataType;
using com.game.vo;
using com.game.Public.Message;
using com.game.manager;
using com.game.data;

namespace Com.Game.Module.Farm
{
	public class FarmMode : BaseMode<FarmMode> {
		public readonly int UPDATE_FARM_INFO = 1;
		public readonly int UPDATE_FARM_LOG = 2;
		public readonly int UPDATE_FRIENDS_FARM_BASE_INFO = 3;
		public readonly int UPDATE_EXPAND_NEW_LAND = 4;
		public readonly int UPDATE_FARM_ATTR = 5;
		public readonly int UPDATE_MY_SEEDS = 6;
		public readonly int UPDATE_SEED_GOODS_NUM = 7;


		private FarmInfo _farmInfo = new FarmInfo();
		private List<FarmLog> _farmLog = new List<FarmLog> ();
		private List<FriendFarmBaseInfo> _friendsFarmBaseInfo = new List<FriendFarmBaseInfo> ();
//		private Dictionary<byte, LandVo> landDic = new Dictionary<byte, LandVo> (); 
		private List<MySeedVo> _mySeedsList = new List<MySeedVo>();
		private Dictionary<uint, uint> _seedGoodsDic = new Dictionary<uint, uint> ();
//		private List<SysSeedMallVo> _seedMallList = new List<SysSeedMallVo> ();            
		private byte _newExpandLand;
		private byte _selectedLandPos;

		public FarmInfo farmInfo { get{return _farmInfo;}}
		public List<FarmLog> farmLog { get{return _farmLog;}}
		public List<FriendFarmBaseInfo> friendsFarmBaseInfo { get{return _friendsFarmBaseInfo;}}
		public List<MySeedVo> mySeedsList{ get{return _mySeedsList;}}
		public Dictionary<uint, uint> seedGoodsDic{ get{return _seedGoodsDic;}}
//		public List<SysSeedMallVo> seedMallList { get{return _seedMallList;}}
		public byte newExpandLand 
		{ 
			get{return _newExpandLand;}
			set
			{
				_newExpandLand = value;
				LandVo newLand = new LandVo();
				newLand.pos = _newExpandLand;
				newLand.color = 1;
				newLand.canSteal = false;
				newLand.status = 0;
				newLand.num = 0;
				newLand.seedId = 0;
				newLand.remainTime = 0;

				_farmInfo.landInfo.Add(_newExpandLand, newLand);

				DataUpdate(UPDATE_EXPAND_NEW_LAND);
			}
		}
		public byte selectedLandPos 
		{ 
			get{return _selectedLandPos;}
			set
			{
				_selectedLandPos = value;
			}
		}
		//-------------------------------------  协议请求 -----------------------------------------//
		//客户端请求农场信息
		public void ApplyFarmInfo()
		{
			Log.info(this, "发送30-1给服务器请求农场信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_1 (msdata);
			AppNet.gameNet.send(msdata, 30, 1);
		}

		//客户端请求农场日志信息
		public void ApplyFarmLog()
		{
			Log.info(this, "发送30-2给服务器请求农场日志信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_2 (msdata);
			AppNet.gameNet.send(msdata, 30, 2);
		}

		//客户端请求所有好友农场的简略信息
		public void ApplyFriendsFarmBaseInfo()
		{
			Log.info(this, "发送30-3给服务器请求所有好友农场的简略信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_3 (msdata);
			AppNet.gameNet.send(msdata, 30, 3);
		}

		//客户端好友农场的信息
		public void ApplyFriendFarmInfo(uint friendId)
		{
			Log.info(this, "发送30-4给服务器请求好友农场信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_4 (msdata, friendId);
			AppNet.gameNet.send(msdata, 30, 4);
		}

		//客户端请求种子背包信息
		public void ApplySeedInfo()
		{
			Log.info(this, "发送30-5给服务器请求种子背包信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_5 (msdata);
			AppNet.gameNet.send(msdata, 30, 5);
		}

		//客户端请求加速成长（pos：田编号  goodTypeId：加速成长需要消耗的物品）
		public void ApplyFastGrowUp(byte pos, uint goodTypeId)
		{
			Log.info(this, "发送30-8给服务器请求加速成长");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_8 (msdata, pos, goodTypeId);
			AppNet.gameNet.send(msdata, 30, 8);
		}

		//客户端请求扩充土地
		public void ApplyExpandLand()
		{
			Log.info(this, "发送30-9给服务器请求扩充土地");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_9 (msdata);
			AppNet.gameNet.send(msdata, 30, 9);
		}

		//客户端请求升级土地
		public void ApplyUpdateLand()
		{
			Log.info(this, "发送30-10给服务器请求升级土地");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_10 (msdata);
			AppNet.gameNet.send(msdata, 30, 10);
		}

		//客户端请求种子商店信息（type：1 金币专区 type：2 钻石专区）
		public void ApplySeedStoreInfo(byte type)
		{
			Log.info(this, "发送30-11给服务器请求种子商店信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_11 (msdata, type);
			AppNet.gameNet.send(msdata, 30, 11);
		}

		//客户端请求购买种子
		//（type：1 金币专区 type：2 钻石专区  goodsId：物品ID  num：购买数量）
		public void ApplyBuySeed(byte type, uint goodsId, uint num)
		{
			Log.info(this, "发送30-12给服务器请求购买种子");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_12 (msdata, type, goodsId, num);
			AppNet.gameNet.send(msdata, 30, 12);
		}

		//客户端请求农场操作
		//（id:待操作农场所属角色ID   pos：田编号  type：操作类型 0收货/偷取；1除虫；2除草；3浇水）
		public void ApplyFarmOpe(uint id, byte pos, byte opeType)
		{
			Log.info(this, "发送30-13给服务器请求农场操作");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_13 (msdata, id, pos, opeType);
			AppNet.gameNet.send(msdata, 30, 13);
		}

		//客户端请求一键收获
		public void ApplyGainAll()
		{
			Log.info(this, "发送30-14给服务器请求种子商店信息");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_14 (msdata);
			AppNet.gameNet.send(msdata, 30, 14);
		}

		//客户端请求播种
		public void ApplyCrop(uint seedId, byte pos)
		{
			Log.info(this, "发送30-15给服务器请求播种");
			MemoryStream msdata = new MemoryStream ();
			Module_30.write_30_15 (msdata, seedId, pos);
			AppNet.gameNet.send(msdata, 30, 15);
		}

		//-----------------------------------  数据更新 ------------------------------------------//
		//更新农场信息
		public void UpdateFarmInfo(uint id, uint lvl, uint exp, uint expful, List<PLand> landList)
		{
			_farmInfo.id = id;
			_farmInfo.level = lvl;
			_farmInfo.exp = exp;
			_farmInfo.expful = expful;

			_farmInfo.landInfo.Clear ();
			LandVo land;
			for (int i = 0; i < landList.Count; ++i)
			{
				land = new LandVo();
				land.pos = landList[i].pos;
				land.color = landList[i].color;
				land.seedId = landList[i].seedId;
				land.remainTime = landList[i].remainTime;
				land.num = landList[i].num;
				land.status = landList[i].status;
				
				_farmInfo.landInfo.Add(land.pos, land);
			}
			DataUpdate (UPDATE_FARM_INFO);
		}

		//更新农场日志
		public void UpdateFarmLog(List<PFarmLog> log)
		{
			FarmLog logVo;
			_farmLog.Clear ();
			for (int i = 0; i < log.Count; ++i)
			{
				logVo = new FarmLog();
				logVo.time = log[i].time;
				logVo.id = log[i].id;
				logVo.name = log[i].name;
				logVo.type = log[i].type;
				logVo.goodsId = log[i].goodsid;
				logVo.num = log[i].num;

				_farmLog.Add(logVo);
			}
			DataUpdate (UPDATE_FARM_LOG);
		}

		//更新所有好友的农场基本信息
		public void UpdateFriendsFarmInfo(List<PFarmSimple> info)
		{
			FriendFarmBaseInfo baseInfo;
			_friendsFarmBaseInfo.Clear ();
			for (int i = 0; i < info.Count; ++i)
			{
				baseInfo = new FriendFarmBaseInfo();
				baseInfo.id = info[i].id;
				baseInfo.name = info[i].name;
				baseInfo.level = info[i].lvl;
				baseInfo.exp = info[i].exp;
				baseInfo.expful = info[i].fullExp;
				baseInfo.status = info[i].status;

				_friendsFarmBaseInfo.Add(baseInfo);
			}

			_friendsFarmBaseInfo.Sort (new ICFriendFarmBaseInfo ());  //排序
			DataUpdate (UPDATE_FRIENDS_FARM_BASE_INFO);
		}

		//更新农场属性
		public void UpdateFarmAttr(uint lvl, uint exp, uint expFull)
		{
			uint addExp;
			if (expFull == _farmInfo.expful)
			{
				addExp = exp - _farmInfo.exp;
			}
			else
			{
				addExp = _farmInfo.expful - _farmInfo.exp + exp;
			}
			MessageManager.Show("果园经验 + " + addExp);

			_farmInfo.level = lvl;
			_farmInfo.exp = exp;
			_farmInfo.expful = expFull;
			DataUpdate (UPDATE_FARM_ATTR);
		}

		//更新我的种子背包
		public void UpdateMySeeds(List<PSeed> seeds)
		{
			_mySeedsList.Clear ();
			for (int i = 0; i < seeds.Count; ++i)
			{
//				if (_mySeedsDic.ContainsKey(seeds[i].id))
//					_mySeedsDic[seeds[i].id] = seeds[i].num;
//				else
				MySeedVo seed = new MySeedVo();
				seed.id = seeds[i].id;
				seed.num = seeds[i].num;
				_mySeedsList.Add(seed);
			}
			DataUpdate (UPDATE_MY_SEEDS);
		}

		public void UpdateSeedsStore(List<PSeed> seeds)
		{
			_seedGoodsDic.Clear ();
			for (int i = 0; i < seeds.Count; ++i)
			{
//				if (_seedGoodsDic.ContainsKey(seeds[i].id))
//					_seedGoodsDic[seeds[i].id] = seeds[i].num;
//				else
				_seedGoodsDic.Add(seeds[i].id, seeds[i].num);
			}
			DataUpdate (UPDATE_SEED_GOODS_NUM);
		}

		public void UpdateSeedGoodNum(uint seedId, uint remainNum)
		{
			_seedGoodsDic [seedId] = remainNum;
			DataUpdate (UPDATE_SEED_GOODS_NUM);
		}


	}
}