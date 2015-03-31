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
using Com.Game.Speech;


/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  副本系统控制中心
 * *******************************************************/
using com.game.Public.LocalVar;
using com.game.data;
using Com.Game.Module.DaemonIsland;


namespace Com.Game.Module.Copy
{
	public class CopyControl : BaseControl<CopyControl> {

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_8_1, Fun_8_1);				//获取已打世界点信息
			AppNet.main.addCMD(CMD.CMD_8_3, Fun_8_3);				//获取已打副本星级信息
            //AppNet.main.addCMD(CMD.CMD_8_4, Fun_8_4);            //请求离开副本
//			AppNet.main.addCMD(CMD.CMD_8_9, Fun_8_9);				//领取星级奖励
			AppNet.main.addCMD(CMD.CMD_8_8, Fun_8_8);				//获取副本结束时间
            AppNet.main.addCMD(CMD.CMD_8_7, Fun_8_7);
            AppNet.main.addCMD(CMD.CMD_8_6, Fun_8_6);
			AppNet.main.addCMD(CMD.CMD_8_15, Fun_8_15);             //扫荡返回
			AppNet.main.addCMD(CMD.CMD_8_19, Fun_8_19);             //激活女神返回

			Singleton<CopyPointMode>.Instance.ApplyCopyInfo (0);
			Singleton<DaemonIslandMode>.Instance.ApplyDaemonCopyInfo (0);
		}

		//返回世界地图信息
		public void Fun_8_1(INetData data)
		{
			//Singleton<WaitingView>.Instance.CloseView ();
			Log.info (this, "8-1协议返回副本信息");
			DungeonWorldInfoMsg_8_1 dungeonWorldInfoMsg = new DungeonWorldInfoMsg_8_1 ();
			dungeonWorldInfoMsg.read (data.GetMemoryStream ());
			Singleton<CopyMode>.Instance.UpdateWorldMapInfo (dungeonWorldInfoMsg);
		}

		//返回已打副本信息
		public void Fun_8_3(INetData data)
		{
			Log.info (this, "8-3协议返回星级奖励信息");
			DungeonGroupInfoMsg_8_3 dungeonGroupInfoMsg = new DungeonGroupInfoMsg_8_3 ();
			dungeonGroupInfoMsg.read (data.GetMemoryStream ());
			Singleton<CopyPointMode>.Instance.UpdateCopyInfo (dungeonGroupInfoMsg);
		}


        ///// <summary>
        ///// 退出副本的协议返回
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public void Fun_8_4(INetData data)
        //{
        //    Log.debug(this, "服务器返回退出副本的协议");
        //    DungeonLeaveMsg_8_4 dungeonLeaveMsg_8_4 = new DungeonLeaveMsg_8_4();
        //    dungeonLeaveMsg_8_4.read(data.GetMemoryStream());
        //    if(dungeonLeaveMsg_8_4.code != 0 )
        //    {
        //        ErrorCodeManager.ShowError(dungeonLeaveMsg_8_4.code);
        //    }
        //}


        //副本继续协议返回
        public void Fun_8_7(INetData data)
        {
            DungeonResumeMsg_8_7 dungeonResumeMsg = new DungeonResumeMsg_8_7();
            dungeonResumeMsg.read(data.GetMemoryStream());
            if (dungeonResumeMsg.code != 0)
            {
                ErrorCodeManager.ShowError(dungeonResumeMsg.code);
            }
            Singleton<MapControl>.Instance.ChangeDungeonLeftTime(dungeonResumeMsg.remainTime);
            Log.info(this, "8-7协议返回副本剩余时间信息:" + dungeonResumeMsg.remainTime);
        }

        //副本暂停协议返回
        public void Fun_8_6(INetData data)
        {
            Log.info(this, "8-6协议返回副本暂停信息");
            DungeonPauseMsg_8_6 dungeonPauseMsg = new DungeonPauseMsg_8_6();
            dungeonPauseMsg.read(data.GetMemoryStream());
            if (dungeonPauseMsg.code != 0)
            {
                ErrorCodeManager.ShowError(dungeonPauseMsg.code);
            }
            //Singleton<MapControl>.Instance.ChangeDungeonLeftTime(dungeonInfoMsg.expire);
        }

		//副本结束时间推送
		public void Fun_8_8(INetData data)
		{
			DungeonInfoMsg_8_8 dungeonInfoMsg = new DungeonInfoMsg_8_8();
			dungeonInfoMsg.read(data.GetMemoryStream());
			Singleton<MapControl>.Instance.ChangeDungeonLeftTime(dungeonInfoMsg.expire);
            Log.info(this,"8-8返回副本剩余时间：" + dungeonInfoMsg.expire);
		}

		//扫荡结果返回
		public void Fun_8_15(INetData data)
		{
			Log.info(this, "8-15协议返回副本扫荡结果");
			DungeonMopUpMsg_8_15 dungeonMopUpMsg_8_15 = new DungeonMopUpMsg_8_15();
			dungeonMopUpMsg_8_15.read(data.GetMemoryStream());
			if (dungeonMopUpMsg_8_15.code == 0)
			{
				Singleton<CopyPointMode>.Instance.UpdateFastFightAward(dungeonMopUpMsg_8_15.reward);
			}
			else
			{
				ErrorCodeManager.ShowError(dungeonMopUpMsg_8_15.code);
			}
		}

		//女神激活返回
		public void Fun_8_19(INetData data)
		{
			Log.info(this, "8-19协议申请激活女神返回");
			DungeonActivateCityMsg_8_19 dungeonActivateCityMsg_8_19 = new DungeonActivateCityMsg_8_19();
			dungeonActivateCityMsg_8_19.read(data.GetMemoryStream());
			if (dungeonActivateCityMsg_8_19.code == 0)
			{
				MessageManager.Show("激活成功");
				Singleton<CopyPointMode>.Instance.UpdateActivedSubworldId(dungeonActivateCityMsg_8_19.cityid);
			}
			else
			{
				ErrorCodeManager.ShowError(dungeonActivateCityMsg_8_19.code);
			}
		}
		
		//结束副本
		public void CopyEnd(bool normalEnded = false, bool isFirstPassed = false)
		{
			Log.info(this, "结束副本，回到主城");
//			AppMap.Instance.mapParser.PosX = 0;
			Singleton<CopyMode>.Instance.openCopyViewFlag = false;  // 切到主城后是否重新打开副本地图ID标识
			//停止连斩统计
			AppMap.Instance.me.Controller.ContCutMgr.StopAll();

			Log.info(this, "停止自动行走，移动位置到目标点");
			MapMode.AutoChangeMap = false;
			MapMode.DisableInput = false;
//			AppMap.Instance.me.Controller.MoveTo(MapMode.INIT_POSX_IN_MAIN_CITY, MapMode.INIT_POSY_IN_MAIN_CITY);
//			AppMap.Instance.mapParser.PosX = 0;
//			Singleton<RoleControl>.Instance.SendHpMpChange();

			if (normalEnded && isFirstPassed)
			{
				Log.info(this, "副本通关，进入剧情模块");
				//Singleton<StoryControl>.Instance.PlayExitSceneStory (AppMap.Instance.mapParser.MapId, StoryBeforeExitCopySceneEnd);
			    StoryBeforeExitCopySceneEnd();
			}
			else
			{
//				uint backCityMapId = (uint)BaseDataMgr.instance.GetSysDungeonTree((uint)BaseDataMgr.instance.
//				                                                                  GetSysDungeonTree (AppMap.Instance.mapParser.MapId)
//				                                                                  .parentId ).parentId;
//				Singleton<MapMode>.Instance.changeScene(backCityMapId, true,5,1.8f);
				StoryBeforeExitCopySceneEnd();
			}
		}

		private void StoryBeforeExitCopySceneEnd()
		{
			Log.info(this, "副本通关的剧情播放完毕，回到城镇");

//            uint worldId = (uint) LocalVarManager.GetInt (LocalVarManager.COPY_WORLD_ID, 0);
//            Singleton<MapMode>.Instance.changeScene(worldId, true,5,1.8f);
			Singleton<CopyMode>.Instance.ApplyQuitCopy ();  //-----后端控制切出副本。modify by lixi
		}

		//自动切换世界
		public void AutoChangeWorld(uint targetWorldId)
		{
            Singleton<CopyMode>.Instance.autoChangeFlag = true;
            Singleton<CopyMode>.Instance.autoChangeToNextMapId = targetWorldId;
//			Singleton<WorldMapView>.Instance.OpenView ();
			this.OpenWorldMapView ();
		}

		//打开副本失败UI
		public void OpenCopyFailView(int reason)
		{
			//发送强制关闭剧情消息
			Singleton<StoryMode>.Instance.DataUpdate(Singleton<StoryMode>.Instance.FORCE_STOP_STORY);
			Singleton<CopyFailView>.Instance.FailReason = reason;
			SpeechMgr.Instance.PlayFailedSpeech();
			Singleton<CopyFailView>.Instance.OpenView();
		}

		//打开世界地图UI
		public void OpenWorldMapView()
		{
			Singleton<WorldMapView>.Instance.OpenView();
		}
		

		//打开主线副本界面，不指定具体副本点
		public void OpenCopyPointView()
		{
			Singleton<CopyMode>.Instance.openCopyById = false;
			//Singleton<CopyPointView>.Instance.OpenView();
            CopyView.Instance.OpenCopyPointView();
		}
		//打开恶魔岛界面，不指定具体副本点
		public void OpenDaemonIslandView()
		{
			Singleton<CopyMode>.Instance.openCopyById = false;
			//Singleton<DaemonIslandView>.Instance.OpenView();
            CopyView.Instance.OpenDameonIslandView();
		}

//		根据副本ID，打开对应的副本界面
		public void OpenCopyById(uint copyId)
		{
			SysMapVo map = BaseDataMgr.instance.GetMapVo (copyId);

			switch (map.subtype)
			{
				case 1:
					OpenMainCopyById(copyId);
					break;
				case 7:
					OpenDaemonCopyById(copyId);
					break;
				default:
					break;
			}
		}

		//打开指定主线副本
		private void OpenMainCopyById(uint copyId)
		{
			Singleton<CopyMode>.Instance.openCopyById = true;
			Singleton<CopyPointMode>.Instance.selectedCopyPoint = copyId;
			//Singleton<CopyPointView>.Instance.OpenView();
            CopyView.Instance.OpenCopyPointView();
//			Singleton<CopyDetailView>.Instance.OpenView ();
		}

		//打开指定精英副本
		private void OpenDaemonCopyById(uint copyId)
		{
			Singleton<CopyMode>.Instance.openCopyById = true;
			Singleton<DaemonIslandMode>.Instance.selectedCopyPoint = copyId;
			//Singleton<DaemonIslandView>.Instance.OpenView();
            CopyView.Instance.OpenDameonIslandView();
//			Singleton<DaemonCopyDetailView>.Instance.OpenView ();
		}

		//根据副本点返回该副本是否已开启
		public bool IsCopyOpened(uint copyId)
		{
			bool isOpened;
			SysMapVo map = BaseDataMgr.instance.GetMapVo (copyId);
			
			switch (map.subtype)
			{
				case 1:
					isOpened = Singleton<CopyPointMode>.Instance.IsMainCopyOpend(copyId);
					break;
				case 7:
					isOpened = Singleton<DaemonIslandMode>.Instance.IsDaemonCopyOpened(copyId);
					break;
				default:
					isOpened = false;
					break;
			}
			return isOpened;
		}

	}
}