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
using Com.Game.Module.Copy;
using com.game.module.map;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  副本系统数据管理中心
 * *******************************************************/
namespace Com.Game.Module.Copy
{
	public class CopyMode : BaseMode<CopyMode> {

		public readonly int LOADING_VIEW_OPEND = 1;

		public bool openCopyViewFlag = false;
		public bool autoChangeFlag = false;
		public uint autoChangeToNextMapId;
		public bool openCopyById = false;

		public readonly int UPDATE_WORLDMAP = 1;

		//--------------------------------------------------------//
		private WorldMapInfo _worldMapInitInfo = new WorldMapInfo ();  // 初始化世界地图信息
		private List<uint> _openedWorldIdList = new List<uint>();  //后端返回的世界地图信息

		public WorldMapInfo worldMapInitInfo{ get{return _worldMapInitInfo;}}
		public List<uint> openedWorldIdList{ get { return _openedWorldIdList; }}
		//-------------------------------------------------------------//


		public void ApplyWorldMapInfo()
		{
			Log.info(this, "发送8-1给服务器请求副本信息");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_1 (msdata);
			AppNet.gameNet.send(msdata, 8 , 1);
		}


		//副本暂停请求
		public void PauseCopy()
		{
			Log.info(this, "发送8-6给服务器暂停副本");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_6 (msdata);
			AppNet.gameNet.send(msdata, 8, 6);
		}

		//副本继续请求
		public void ResumeCopy()
		{
			Log.info(this, "发送8-7给服务器继续副本");
			MemoryStream msdata = new MemoryStream ();
			Module_8.write_8_7 (msdata);
			AppNet.gameNet.send(msdata, 8, 7);
		}


        //请求离开副本
        public void ApplyQuitCopy()
        {
            Log.info(this, "发送8-4给服务器离开副本");
            MemoryStream msdata = new MemoryStream();
            Module_8.write_8_4(msdata);
            AppNet.gameNet.send(msdata, 8, 4);
        }

        /// <summary>
        /// 区域触发请求服务器刷新怪物
        /// </summary>
        public void TriggerMonList()
        {
            Log.info(this, "发送8-20给服务器刷新区域触发的怪物");
            MemoryStream msdata = new MemoryStream();
            uint phase = MapMode.CUR_MAP_PHASE;
            Module_8.write_8_20(msdata, phase);
            AppNet.gameNet.send(msdata, 8, 20);
        }

		//更新世界地图信息
		public void UpdateWorldMapInfo(DungeonWorldInfoMsg_8_1 msg)
		{
			_openedWorldIdList = msg.ids;
			DataUpdate (UPDATE_WORLDMAP);
		}

		//从配置表读取世界地图信息。只在首次打开副本地图时执行
		public void InitWorldMapInfo()
		{
			//从配置表读取全部世界点的初始信息
			string[] worldIds = StringUtils.GetValueListFromString (BaseDataMgr.instance.GetSysDungeonTree (0).list);
			for (int i = 0; i < worldIds.Length; ++i)
			{
				CopyVo worldInfo = new CopyVo ();
				worldInfo.mapId = uint.Parse(worldIds[i]);
				worldInfo.name = BaseDataMgr.instance.GetSysDungeonTree(uint.Parse(worldIds[i])).name;
				worldInfo.x = this.AdjustPosX(BaseDataMgr.instance.GetSysDungeonTree(uint.Parse(worldIds[i])).x);  
				worldInfo.y = this.AdjustPosY(BaseDataMgr.instance.GetSysDungeonTree(uint.Parse(worldIds[i])).y);
				worldInfo.icon = BaseDataMgr.instance.GetSysDungeonTree(uint.Parse(worldIds[i])).icon;
				worldInfo.remark = BaseDataMgr.instance.GetSysDungeonTree(uint.Parse(worldIds[i])).remark;
				_worldMapInitInfo.WorldPointInfoList.Add(worldInfo);
			}
		}

		//XY配置值需要根据屏幕情况进行转换
		private int AdjustPosX(int x)
		{
//			x -= 512;
			return x;
		}
		private int AdjustPosY(int y)
		{
//			y -= 384;
			return y;
		}
	
	}
}
