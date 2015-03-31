using UnityEngine;
using System.Collections;
using com.game.module.test;
using Proto;
using com.u3d.bases.debug;
using Com.Game.Module.Waiting;
using com.game;
using System.IO;
using System.Collections.Generic;
using com.game.manager;
using com.game.utils;
using com.game.Public.LocalVar;
using PCustomDataType;
using com.game.data;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  恶魔岛系统数据管理中心
 * *******************************************************/
namespace Com.Game.Module.DaemonIsland
{
	public class DaemonIslandMode: BaseMode<DaemonIslandMode> {
		public readonly int UPDATE_DAEMONISLAND_INFO = 1;
		public readonly int INIT_AWARD_INFO = 2;
		public readonly int UPDATE_AWARD_INFO = 2;
//		public readonly int UPDATE_FAST_FIGHT_AWARDS = 2;
//		private List<PDungeonReward> _fastFightAwards = new List<PDungeonReward> ();
//

//		public List<PDungeonReward> fastFightAwards{ get {return _fastFightAwards;} }
//


		private uint _newOpenMapId;
		private List<PSuperDungeonInfo> _daemonCopyInfo = new List<PSuperDungeonInfo> ();
		private List<PSuperDungeonReward> _daemonAwardInfo = new List<PSuperDungeonReward> ();
		private uint _selectedCopyPoint;
		private int _fightNum;
		
		public uint NewOpenMapId{ get{return _newOpenMapId;}}
		public uint selectedCopyPoint{ get{ return _selectedCopyPoint;} set{ _selectedCopyPoint = value;} }
		public int fightNum{ get{return _fightNum;} set{_fightNum = value;}}
//		public List<PSuperDungeonInfo> DaemonCopyInfo{ get {return _daemonCopyInfo;}}

		public void ApplyDaemonCopyInfo(uint cityId)
		{
			Log.info(this, "发送8-16给服务器请求恶魔岛副本信息");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_16 (msdata, cityId);
			AppNet.gameNet.send(msdata, 8 , 16);
		}

		public void ApplyAwardInfo(uint cityId)
		{
			Log.info(this, "发送8-17给服务器请求恶魔岛奖励领取情况");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_17 (msdata, cityId);
			AppNet.gameNet.send(msdata, 8 , 17);
		}

		public void ApplyGetAward(uint cityId, byte awardId)
		{
			Log.info(this, "发送8-18给服务器请求领取奖励");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_18 (msdata, (ushort)cityId, awardId);
			AppNet.gameNet.send(msdata, 8 , 18);
		}

//		public void ApplyFastFight()
//		{
//			Log.info(this, "发送8-15给服务器请求扫荡");
//			MemoryStream msdata = new MemoryStream ();
//			Module_8.write_8_15 (msdata, _selectedCopyPoint, (byte)_fightNum);
//			AppNet.gameNet.send(msdata, 8 , 15);
//		}

		//---------------------------------------------------------------------------------
		//更新副本数据
		public void UpdateDaemonIslandInfo(DungeonSuperInfoMsg_8_16 daemonIslandinfo)
		{
			_newOpenMapId = daemonIslandinfo.maxid;
			_daemonCopyInfo = daemonIslandinfo.info;

			DataUpdate (UPDATE_DAEMONISLAND_INFO);
		}

		public void InitAwardInfo(DungeonSuperRewardInfoMsg_8_17 awradInfo)
		{
			_daemonAwardInfo = awradInfo.list;
			
			DataUpdate (INIT_AWARD_INFO);
		}

		//更新奖励领取情况
		public void UpdateAwardInfo(PSuperDungeonReward updateAwradInfo)
		{
			for (int i = 0; i < _daemonAwardInfo.Count; ++i)
			{
				if (_daemonAwardInfo[i].cityid == updateAwradInfo.cityid)
				{
					_daemonAwardInfo[i] = updateAwradInfo;
					DataUpdate (UPDATE_AWARD_INFO);
					return;
				}
			}
			_daemonAwardInfo.Add (updateAwradInfo);
			DataUpdate (UPDATE_AWARD_INFO);
		}

		//----------------------------------------------------------------------------------------

		//根据副本点ID获取该副本的星星成绩
		public DaemonCopyVo GetDaemonCopyInfo(uint copyPointId)
		{
			DaemonCopyVo copyVo = new DaemonCopyVo ();
			copyVo.grade = -1;
			copyVo.usedTimes = 0;
			for (int i = 0; i < _daemonCopyInfo.Count; ++i)
			{
				if (_daemonCopyInfo[i].id == copyPointId)
				{
					copyVo.grade = _daemonCopyInfo[i].grade;
					copyVo.usedTimes = _daemonCopyInfo[i].times;
					break;
				}
			}

			return copyVo;
		}

		//获取奖励领取情况
		public DaemonCopyAwardVo GetAdditionalAwardInfo(uint cityId)
		{
			DaemonCopyAwardVo awardInfo = new DaemonCopyAwardVo ();
			for (int i = 0; i < _daemonAwardInfo.Count; ++i)
			{
				if (_daemonAwardInfo[i].cityid == cityId)
				{
					awardInfo.isGettedAward1 = _daemonAwardInfo[i].status1 > 0? true: false;
					awardInfo.isGettedAward2 = _daemonAwardInfo[i].status2 > 0? true: false;
					awardInfo.isGettedAward3 = _daemonAwardInfo[i].status3 > 0? true: false;
					return awardInfo;
				}
			}
			awardInfo.isGettedAward1 = false;
			awardInfo.isGettedAward2 = false;
			awardInfo.isGettedAward3 = false;
			return awardInfo;
		}

		//查询指定恶魔岛副本是否开启
		public bool IsDaemonCopyOpened(uint mapId)
		{
			return (_newOpenMapId == mapId || GetDaemonCopyInfo (mapId).grade >= 0) ? true : false;

		}

	}
}
