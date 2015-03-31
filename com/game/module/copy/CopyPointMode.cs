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
 * function:  副本系统数据管理中心
 * *******************************************************/
using com.game.module.Task;


namespace Com.Game.Module.Copy
{
	public class CopyPointMode: BaseMode<CopyPointMode> {
		public readonly int UPDATE_COPY_INFO = 1;
		public readonly int UPDATE_FAST_FIGHT_AWARDS = 2;
		public readonly int UPDATE_ACTIVED_SUBWORLDID = 3;

		private List<PDungeonMapInfo> _copyStarsInfo = new List<PDungeonMapInfo> ();
		private List<uint> _activedSubworldIdList = new List<uint> ();
		private uint _selectedCopyPoint;
		private int _fightNum;
		private List<PDungeonReward> _fastFightAwards = new List<PDungeonReward> ();

		public int fightNum{ get{return _fightNum;} set{_fightNum = value;}}
		public List<PDungeonMapInfo> copyStarsInfo{ get {return _copyStarsInfo;}}
		public List<PDungeonReward> fastFightAwards{ get {return _fastFightAwards;} }
		public uint selectedCopyPoint
		{
			get
			{
				return _selectedCopyPoint;
			}
			set
			{
				_selectedCopyPoint = value;
			}
		}
		//-------------------------------------- 新的副本协议  -------------------------------//	
		public void ApplyCopyInfo(uint subWordlId)
		{
			Log.info(this, "发送8-3给服务器请求副本点星级奖励信息");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_3 (msdata, subWordlId);
			AppNet.gameNet.send(msdata, 8 , 3);
		}

		public void ApplyFastFight(uint copyPointId, byte fightNum)
		{
			Log.info(this, "发送8-15给服务器请求扫荡");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_15 (msdata, copyPointId, fightNum);
			AppNet.gameNet.send(msdata, 8 , 15);
		}

		public void ActiveGirl(uint subworldId)
		{
			Log.info(this, "发送8-19给服务器激活女神：" + subworldId);
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_19 (msdata, subworldId);
			AppNet.gameNet.send(msdata, 8 , 19);
		}

		//---------------------------------------------------------------------------------
		public void UpdateCopyInfo(DungeonGroupInfoMsg_8_3 copyInfo )
		{
			_copyStarsInfo = copyInfo.group;
			_activedSubworldIdList = copyInfo.cities;
			DataUpdate (UPDATE_COPY_INFO);
		}

		//根据副本点ID获取该副本的星星成绩
		public int GetStarNum(uint copyPointId)
		{
			int grade = -1;
			for (int i = 0; i < _copyStarsInfo.Count; ++i)
			{
				if (_copyStarsInfo[i].mapid == copyPointId)
				{
					grade = _copyStarsInfo[i].grade;
					break;
				}
			}
			return grade;
		}

		//更新副本扫荡奖励
		public void UpdateFastFightAward(List<PDungeonReward> awards)
		{
			_fastFightAwards.Clear ();
			_fastFightAwards = awards;
			DataUpdate (UPDATE_FAST_FIGHT_AWARDS);
		}

		//查询指定副本点是否开启
		public bool IsMainCopyOpend(uint mapId)
		{
			return (mapId == Singleton<TaskModel>.Instance.TaskCopyMapId ||
			        GetStarNum (mapId) >= 0);
		}

		//判断女神是否已激活
		public bool IsGirlActived(uint subworldId)
		{
			for (int i = 0; i < _activedSubworldIdList.Count; ++i)
			{
				if (_activedSubworldIdList[i] == subworldId)
				{
					return true;
				}
			}
			return false;
		}

		//更新已激活女神列表
		public void UpdateActivedSubworldId(uint subworldId)
		{
			if (! _activedSubworldIdList.Contains(subworldId))
				_activedSubworldIdList.Add (subworldId);
			DataUpdate (UPDATE_ACTIVED_SUBWORLDID);
		}

	}
}
