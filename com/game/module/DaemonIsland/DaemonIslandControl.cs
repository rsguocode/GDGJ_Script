using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using Proto;
using Com.Game.Module.Waiting;
using com.u3d.bases.debug;
using com.net.interfaces;
using PCustomDataType;
using com.game.cmd;
using com.game.module.map;
using com.game.consts;
using Com.Game.Module.Story;
using com.game.Public.Message;
using com.game.module.loading;
using com.game.manager;
using com.game.vo;
using com.game.Public.LocalVar;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  恶魔岛系统控制中心
 * *******************************************************/


namespace Com.Game.Module.DaemonIsland
{
	public class DaemonIslandControl : BaseControl<DaemonIslandControl> {

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_8_16, Fun_8_16);				//获取恶魔岛副本信息
			AppNet.main.addCMD(CMD.CMD_8_17, Fun_8_17);				//获取恶魔岛奖励领取信息
			AppNet.main.addCMD(CMD.CMD_8_18, Fun_8_18);				//领取奖励
		}
		
		//返回恶魔岛副本信息
		public void Fun_8_16(INetData data)
		{
			Log.info (this, "8-16协议返回恶魔岛副本信息");
			DungeonSuperInfoMsg_8_16 dungeonSuperInfoMsg_8_16 = new DungeonSuperInfoMsg_8_16 ();
			dungeonSuperInfoMsg_8_16.read (data.GetMemoryStream ());
			Singleton<DaemonIslandMode>.Instance.UpdateDaemonIslandInfo (dungeonSuperInfoMsg_8_16);
		}

		//返回恶魔岛奖励领取信息
		public void Fun_8_17(INetData data)
		{
			Log.info (this, "8-17协议返回恶魔岛奖励领取信息");
			DungeonSuperRewardInfoMsg_8_17 dungeonSuperRewardInfoMsg_8_17 = new DungeonSuperRewardInfoMsg_8_17 ();
			dungeonSuperRewardInfoMsg_8_17.read (data.GetMemoryStream ());
			Singleton<DaemonIslandMode>.Instance.InitAwardInfo (dungeonSuperRewardInfoMsg_8_17);
		}

		//返回恶魔岛奖励领取结果
		public void Fun_8_18(INetData data)
		{
			Log.info (this, "8-18协议返回奖励领取结果");
			DungeonGetSuperRewardMsg_8_18 dungeonGetSuperRewardMsg_8_18 = new DungeonGetSuperRewardMsg_8_18 ();
			dungeonGetSuperRewardMsg_8_18.read (data.GetMemoryStream ());

			if (dungeonGetSuperRewardMsg_8_18.code != 0)
			{
				ErrorCodeManager.ShowError(dungeonGetSuperRewardMsg_8_18.code);
			}
			else
			{
				MessageManager.Show("奖励领取成功");
				Singleton<DaemonIslandMode>.Instance.UpdateAwardInfo (dungeonGetSuperRewardMsg_8_18.list[0]);
			}
		}

       

//		//扫荡结果返回
//		public void Fun_8_15(INetData data)
//		{
//			Log.info(this, "8-15协议返回副本扫荡结果");
//			DungeonMopUpMsg_8_15 dungeonMopUpMsg_8_15 = new DungeonMopUpMsg_8_15();
//			dungeonMopUpMsg_8_15.read(data.GetMemoryStream());
//			if (dungeonMopUpMsg_8_15.code == 0)
//			{
//				Singleton<CopyPointMode>.Instance.UpdateFastFightAward(dungeonMopUpMsg_8_15.reward);
//			}
//			else
//			{
//				ErrorCodeManager.ShowError(dungeonMopUpMsg_8_15.code);
//			}
//		}
//		
//		//结束副本
//		public void CopyEnd(bool normalEnded = false, bool isFirstPassed = false)
//		{
//			Log.info(this, "结束副本，回到主城");
////			AppMap.Instance.mapParser.PosX = 0;
//			Singleton<CopyMode>.Instance.openCopyViewFlag = false;  // 切到主城后是否重新打开副本地图ID标识
//			//停止连斩统计
//			AppMap.Instance.me.Controller.ContCutMgr.StopAll();
//
//			Log.info(this, "停止自动行走，移动位置到目标点");
//			MapMode.AutoChangeMap = false;
//			MapMode.DisableInput = false;
////			AppMap.Instance.me.Controller.MoveTo(MapMode.INIT_POSX_IN_MAIN_CITY, MapMode.INIT_POSY_IN_MAIN_CITY);
////			AppMap.Instance.mapParser.PosX = 0;
////			Singleton<RoleControl>.Instance.SendHpMpChange();
//
//			if (normalEnded && isFirstPassed)
//			{
//				Log.info(this, "副本通关，进入剧情模块");
//				//Singleton<StoryControl>.Instance.PlayExitSceneStory (AppMap.Instance.mapParser.MapId, StoryBeforeExitCopySceneEnd);
//			    StoryBeforeExitCopySceneEnd();
//			}
//			else
//			{
//				uint backCityMapId = (uint)BaseDataMgr.instance.GetSysDungeonTree((uint)BaseDataMgr.instance.
//				                                                                  GetSysDungeonTree (AppMap.Instance.mapParser.MapId)
//				                                                                  .parentId ).parentId;
//				Singleton<MapMode>.Instance.changeScene(backCityMapId, true,5,1.8f);
//			}
//		}
//
//		private void StoryBeforeExitCopySceneEnd()
//		{
//			Log.info(this, "副本通关的剧情播放完毕，回到城镇");
////			uint cityMapId = (uint)BaseDataMgr.instance.GetSysDungeonTree((uint)
////				BaseDataMgr.instance.GetSysDungeonTree (MeVo.instance.mapId).parentId).parentId;
//			uint worldId = (uint) LocalVarManager.GetInt (LocalVarManager.COPY_WORLD_ID, 0);
////			worldId = worldId  == 0 ? cityMapId : worldId;
//			Singleton<MapMode>.Instance.changeScene(worldId, true,5,1.8f);
//		}
//
//		//自动切换世界
//		public void AutoChangeWorld(uint targetWorldId)
//		{
//            Singleton<CopyMode>.Instance.autoChangeFlag = true;
//            Singleton<CopyMode>.Instance.autoChangeToNextMapId = targetWorldId;
////			Singleton<WorldMapView>.Instance.OpenView ();
//			this.OpenWorldMapView ();
//		}
//
//		//打开副本失败UI
//		public void OpenCopyFailView(int reason)
//		{
//			//发送强制关闭剧情消息
//			Singleton<StoryMode>.Instance.DataUpdate(Singleton<StoryMode>.Instance.FORCE_STOP_STORY);
//			Singleton<CopyFailView>.Instance.FailReason = reason;
//			Singleton<CopyFailView>.Instance.OpenView();
//		}
//
//		//打开世界地图UI
//		public void OpenWorldMapView()
//		{
//			Singleton<WorldMapView>.Instance.OpenView();
//		}
//
//		//打开副本点选择UI
//		public void OpenCopyPointView()
//		{
////			Singleton<CopyPointView>.Instance.OpenView();
//			Singleton<CopyView>.Instance.OpenView();
//		}


	}
}