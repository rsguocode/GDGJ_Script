using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using com.game.cmd;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Arena;
using com.game.module.battle;
using Com.Game.Module.Boss;
using Com.Game.Module.Copy;
using com.game.module.effect;
using Com.Game.Module.GoGo;
using Com.Game.Module.GoldHit;
using Com.Game.Module.GoldSilverIsland;
using com.game.module.Guide;
using com.game.module.hud;
using com.game.module.loading;
using com.game.module.main;
using Com.Game.Module.Role;
using Com.Game.Module.Story;
using com.game.module.Task;
using com.game.module.test;
using com.game.Public.Message;
using com.game.sound;
using Com.Game.Speech;
using com.game.utils;
using com.game.vo;
using com.net.interfaces;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using com.u3d.bases.display.effect;
using com.u3d.bases.display.vo;
using com.u3d.bases.map;
using PCustomDataType;
using Proto;
using UnityEngine;
using Random = UnityEngine.Random;
using Com.Game.Module.Tips;

namespace com.game.module.map
{
    public class MapControl : BaseControl<MapControl>
    {
        public static int AUTO_CHANGE_MAP_RANGE = 3; //触发自动行走的范围区域（半个手机屏幕宽 / AUTO_CHANGE_MAP_RANGE）

        public static String Name = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        //-----------剧情相关变量----------------//
        public static bool CanPlayHeroHpChangeStory = false;
        public static bool CanPlayMonsterHpChangeStory = false;
        public static bool CanPlayHeroArriveStory = false;
        private MyCamera _myCamera;

        private uint curMapId;
        private bool endStageStoryPlaying;
        private bool enterSceneStoryPlaying;
        private bool fightStageStoryPlaying;
        private bool startStageStoryPlaying;

        public MapControl()
        {
            AppMap.Instance.mapParser.AddMapCallback(MapCallback); //注册场景切换完成回调
        }

        //当前死亡的怪物ID
        public uint MonsterID { get; set; }

        public uint CurTaskCopyMapId { get; set; }

        public MyCamera MyCamera
        {
            get { return _myCamera; }
        }

        /**注册Socket数据返回监听**/

        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_4_1, Fun_4_1); //切换场景
            AppNet.main.addCMD(CMD.CMD_4_2, Fun_4_2); //显示场景信息
            AppNet.main.addCMD(CMD.CMD_4_4, Fun_4_4); //玩家进入视野
            AppNet.main.addCMD(CMD.CMD_4_5, Fun_4_5); //玩家离开视野
            AppNet.main.addCMD(CMD.CMD_4_7, Fun_4_7); //怪物进入场景
            AppNet.main.addCMD(CMD.CMD_4_10, Fun_4_10); //场景角色信息修改
            AppNet.main.addCMD(CMD.CMD_4_11, Fun_4_11); //场景怪物信息修改
            AppNet.main.addCMD(CMD.CMD_4_12, Fun_4_12); //服务器返回怪物死亡信息
            AppNet.main.addCMD(CMD.CMD_4_13, Fun_4_13); //角色死亡
            AppNet.main.addCMD(CMD.CMD_4_15, Fun_4_15); //服务器返回副本通关信息
            AppNet.main.addCMD(CMD.CMD_4_14, Fun_4_14); //服务器返回副本阶段信息
            AppNet.main.addCMD(CMD.CMD_4_17, Fun_4_17); //服务器返回切换地图命令
        }

        /**切换场景请求返回**/

        private void Fun_4_1(INetData data)
		{
            var mapSwitchMsg = new MapSwitchMsg_4_1();
            mapSwitchMsg.read(data.GetMemoryStream());
            if (mapSwitchMsg.code == 0)
            {
                Log.debug(this, "-Fun_4_1 切换场景请求通过");
                Singleton<MapMode>.Instance.ApplySceneInfo();
                if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP) //若为击石成金副本
                {
                    Singleton<GoldHitControl>.Instance.OpenMainView();
                }
            }
            else
            {
                Log.info(this, "进入场景失败，关闭进度条");
//                Singleton<StartLoadingView>.Instance.CloseView();
                ErrorCodeManager.ShowError(mapSwitchMsg.code);
                if (mapSwitchMsg.code == 404)
                {
                    //Log.info(this, "该场景不可进入，切换到主城");
                    //Singleton<MapMode>.Instance.changeScene(MapTypeConst.MajorCity, false, 5, 1.8f);
                }
            }
        }

        /**场景信息请求返回**/

        private void Fun_4_2(INetData data)
        {
            Log.debug(this, "-Fun_4_2 场景信息请求返回成功 ");
            if (Singleton<MapMode>.Instance.IsFirstInToScene)
            {
                AppFacde.instance.InitAfterIntoScene(); //执行第一次进入场景的初始化工作
            }
            var mapSightMsg = new MapSightMsg_4_2();
            mapSightMsg.read(data.GetMemoryStream());
            uint mapId = mapSightMsg.mapId;
            Log.info(this,
                "-Fun_4_2() 本角色：" + MeVo.instance.Id + " 切换场景，从：" + AppMap.Instance.mapParser.MapId + "到：" + mapId);
            Log.info(this, "-Fun_4_2() 更新玩家位置信息");
            MeVo.instance.preMapId = AppMap.Instance.mapParser.MapId;
            if (mapId == MapTypeConst.ARENA_MAP || mapId == MapTypeConst.GoldSilverIsland_MAP)
            {
                MeVo.instance.X = 2.5f;
            }
            else
            {
                MeVo.instance.X = MeVo.instance.toX;
            }
            MeVo.instance.mapId = mapSightMsg.mapId;
            Log.info(this, "-Fun_4_2() 开始切换地图");
            MapMode.CUR_MAP_PHASE = mapSightMsg.phase == 0 ? (ushort) 1 : mapSightMsg.phase;
            //不管成败，结束挂机
            if (AppMap.Instance.me != null)
            {
                var meAiController = AppMap.Instance.me.Controller.AiController as MeAiController;
                if (meAiController != null && meAiController.IsAi)
                {
                    meAiController.SetAi(false);
                    Singleton<BattleMode>.Instance.IsAutoSystem = false;
                }
            }
            //View 处理
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(mapId);
            switch (mapVo.type)
            {
                    //进入城镇场景
                case MapTypeConst.CITY_MAP:
                    ViewTree.battle.SetActive(false);
                    break;
                case MapTypeConst.COPY_MAP:
                    ViewTree.city.SetActive(false);
                    break;
                case MapTypeConst.SPECIAL_MAP:
                    ViewTree.city.SetActive(false);
                    break;
            }

            LoadMap(mapId);
            //根据场景信息生成对应的玩家信息
            Log.info(this,
                "-Fun_4_2()进入场景玩家数量 ： " + mapSightMsg.rolesEnter.Count + " 此时主角的mapId: " + MeVo.instance.mapId);
            Singleton<MapMode>.Instance.AddPlayerInMap(mapSightMsg.rolesEnter);
            Log.info(this, "-Fun_4_2()进入场景怪物数量 ： " + mapSightMsg.monsEnter.Count + " 此时主角的mapId: " + MeVo.instance.mapId);
            if (mapSightMsg.monsEnter.Count > 0)
            {
                AddMonList(mapSightMsg.monsEnter);
            }
        }

        //玩家进入视野
        private void Fun_4_4(INetData data)
        {
            var mapRoleEnterMsg = new MapRoleEnterMsg_4_4();
            mapRoleEnterMsg.read(data.GetMemoryStream());

            //进入视野玩家
            Log.info(this, "进入视野玩家 ： " + mapRoleEnterMsg.roles.Count + " 此时主角的mapId: " + MeVo.instance.mapId);
            Singleton<MapMode>.Instance.AddPlayerInMap(mapRoleEnterMsg.roles);
        }

        //玩家离开视野
        private void Fun_4_5(INetData data)
        {
            var mapRoleLeaveMsg = new MapRoleLeaveMsg_4_5();
            mapRoleLeaveMsg.read(data.GetMemoryStream());
            foreach (uint leaveRoleId in mapRoleLeaveMsg.roles)
            {
                PlayerMgr.instance.removePlayer(leaveRoleId);
            }
        }


        /// <summary>
        ///     怪物进入场景
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_7(INetData data)
        {
            var mapMonEnterMsg = new MapMonEnterMsg_4_7();
            mapMonEnterMsg.read(data.GetMemoryStream());

            AddMonList(mapMonEnterMsg.mons);
        }

        /// <summary>
        ///     添加怪物列表到怪物管理中
        /// </summary>
        /// <param name="monList">Mon list.</param>
        private void AddMonList(List<PMapMon> monList)
        {
            if (MapMode.WaitRefreshMonster)
            {
                Log.info(this, "暂停刷怪，新加入怪物暂且加入缓存，场景切换完毕后再添加");
                foreach (PMapMon mon in monList)
                {
                    Singleton<MapMode>.Instance.waitMonList.Add(mon);
                }
            }
            else
            {
                Singleton<MapMode>.Instance.AddMonInMap(monList);
            }
        }

        //场景角色信息修改
        private void Fun_4_10(INetData data)
        {
            var mapRoleUpdateMsg = new MapRoleUpdateMsg_4_10();
            mapRoleUpdateMsg.read(data.GetMemoryStream());
            Log.info(this, "-Fun_4_10()收到场景中角色信息修改:" + mapRoleUpdateMsg.id);

            PlayerVo changeVo = (mapRoleUpdateMsg.id == MeVo.instance.Id)
                ? MeVo.instance
                : PlayerMgr.instance.getPlayer(mapRoleUpdateMsg.id);
            Log.info(this, "-Fun_4_10()Meid: " + MeVo.instance.Id);
            Log.info(this, "-Fun_4_10()mapRoleUpdateMsg.id: " + mapRoleUpdateMsg.id);
            Log.info(this, "-Fun_4_10()changeVO: " + changeVo);
            if (changeVo == null)
            {
                return;
            }
            foreach (PItem item in mapRoleUpdateMsg.changeList)
            {
                if (item.key == MapTypeConst.ROLE_HP_CHANGE_KEY) changeVo.CurHp = item.value[0];
                else if (item.key == MapTypeConst.ROLE_HP_FULL_CHANGE_KEY) changeVo.Hp = item.value[0];
                else if (item.key == MapTypeConst.ROLE_MP_CHANGE_KEY) changeVo.CurMp = item.value[0];
                else if (item.key == MapTypeConst.ROLE_MP_FULL_CHANGE_KEY) changeVo.Mp = item.value[0];
                else if (item.key == MapTypeConst.ROLE_LV_CHANGE_KEY) changeVo.Level = (byte) item.value[0];
                else if (item.key == MapTypeConst.Role_BUFF_LIST_CHANGE_KEY)
                {
                    //注：与后端约定buff的list<uint>的uint=id*100+lvl;
                    //对buff列表进行判断增删;
                    List<PBuff> tmpPbList = new List<PBuff>();
                    PBuff tmpPb = null;
                    uint tmpBid = 0;
                    uint tmpBlv = 0;
                    foreach (uint tmpValue in item.value)
                    {
                        tmpBlv = tmpValue%100;
                        tmpBid = (tmpValue - tmpBlv)/100;
                        tmpPb = new PBuff();
                        tmpPb.id = tmpBid;
                        tmpPb.lvl = (byte)tmpBlv;
                        tmpPbList.Add(tmpPb);
                    }
                    BuffManager.Instance.AddBuff(changeVo.Id.ToString(), tmpPbList, true);
                }
                else if (item.key == MapTypeConst.RolePetChange) changeVo.PetId = item.value[0];
            }
            if (mapRoleUpdateMsg.id == MeVo.instance.Id)
                MeVo.instance.DataUpdate(MeVo.DataHpMpUpdate);
        }


        //-define(MAP_MON_STATE, 1).      % 怪物状态变化
        //-define(MAP_MON_HP, 2).         % 怪物血量
        //-define(MAP_MON_HP_FULL, 3).    % 怪物最大血量
        //-define(MAP_MON_BUFF, 4).       % 怪物buff列表

        /// <summary>
        ///     更新p_map_mon部分数据
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_11(INetData data)
        {
            var message = new MapMonUpdateMsg_4_11();
            message.read(data.GetMemoryStream());
            if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS) //世界Boss 怪物血量更新
            {
                uint cur = 0, ful = 0;
                foreach (PItem temp in message.changeList)
                {
                    if (temp.key == 2)
                        cur = temp.value[0];
                    else if (temp.key == 3)
                    {
                        ful = temp.value[0];
                    }
                }
                Singleton<BossMode>.Instance.UpdateBossHp(cur, ful);
                Log.info(this, "Boss currHp: " + cur + " Boss fullHp: " + ful);
            }
        }

        /// <summary>
        ///     服务器发送怪物死亡消息
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_12(INetData data)
        {
            var mapMonDeadMsg = new MapMonDeadMsg_4_12();
            mapMonDeadMsg.read(data.GetMemoryStream());
            Log.info(this, "-Fun_4_12()收到服务器返回消息，死亡怪物ID：" + mapMonDeadMsg.id);
            Debug.Log("****-Fun_4_12()收到服务器返回消息，死亡怪物ID：");
            PlayFightStageStory();
        }

        /// <summary>
        ///     角色死亡消息
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_13(INetData data)
        {
            var roleDeadMsg = new MapRoleDeadMsg_4_13();
            roleDeadMsg.read(data.GetMemoryStream());
            PlayerDisplay player = AppMap.Instance.GetPlayer(roleDeadMsg.id.ToString(CultureInfo.InvariantCulture));
            if (player == null)
            {
                Log.info(this, "-Fun_4_13() 获取不到玩家的对象");
                return;
            }
            if (roleDeadMsg.id != MeVo.instance.Id) //其他玩家死亡同步，角色自身走技能同步
                player.Death();
            Log.info(this, "-Fun_4_13() 获取到玩家的对象死亡");
            Log.info(this, "-Fun_4_13()收到服务器返回消息，死亡角色ID：" + roleDeadMsg.id);
        }

        /// <summary>
        ///     服务器发送副本阶段信息
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_14(INetData data)
        {
//			Log.info (this, "-Fun_4_14()");
            var mapPhaseMsg = new MapPhaseMsg_4_14();
            mapPhaseMsg.read(data.GetMemoryStream());
            Log.info(this, "-Fun_4_14() 进入新阶段为：" + mapPhaseMsg.phase);
            UnityEngine.Debug.Log("****服务器发送副本阶段信息 Fun_4_14()");
            MapMode.NEXT_MAP_PHASE = mapPhaseMsg.phase;
            MapMode.IsTriggered = false;
            Log.info(this, "-Fun_4_14()暂停刷怪标识开启");
            MapMode.WaitRefreshMonster = true;
            Singleton<MapMode>.Instance.StopLeftTime();
			AppMap.Instance.me.Controller.ContCutMgr.PauseAttack();

            if (CanPlayCopyStory(AppMap.Instance.mapParser.MapId))
            {
                if (Singleton<StoryControl>.Instance.PlayEndStageStory(curMapId, (uint)(mapPhaseMsg.phase - 1), EndStage))
                {
                    MapMode.InStageEndStory = true;
                    endStageStoryPlaying = true;
                    PreSortStoryPlay();
                }
				else
				{
					EndStage();
				}
            }
            else
            {
                EndStageStoryEnd();
            }
        }

        //阶段结束剧情播放完毕
        private void EndStageStoryEnd()
        {
            Log.info(this, "-EndStageStoryEnd()显示gogogo");
            Singleton<GoGoView>.Instance.OpenView(250, 0);
            Log.info(this, "-EndStageStoryEnd()开启自动切换阶段检测表示");

            MapMode.CanGoToNextPhase = true;
        }

        /// <summary>
        ///     显示波数特效
        /// </summary>
        public void ShowStageEffect()
        {
            //显示波束特效
            string[] effectIdArr = {"", "", EffectId.UI_SecondStage, EffectId.UI_ThirdStage};
            EffectMgr.Instance.CreateUIEffect(effectIdArr[MapMode.CUR_MAP_PHASE], Vector3.zero);
        }

        /// <summary>
        ///     服务器发送副本通关信息
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_4_15(INetData data)
        {
            var copyFinishMsg = new MapFinishMsg_4_15();
            copyFinishMsg.read(data.GetMemoryStream());
            //不管成败，结束挂机
            if (AppMap.Instance.me != null)
            {
                var meAiController = AppMap.Instance.me.Controller.AiController as MeAiController;
                if (meAiController != null && meAiController.IsAi)
                {
                    meAiController.SetAi(false);
                    Singleton<BattleMode>.Instance.IsAutoSystem = false;
                }
            }
            if (copyFinishMsg.code == 0)
            {
                Log.info(this, "-Fun_4_15()副本通关,停止连斩统计");
                if (AppMap.Instance.me != null) AppMap.Instance.me.Controller.ContCutMgr.StopAll(); //停止连斩统计
                //如果是击石成金副本的话，就跳到自己的结算界面
                if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP)
                {
                    Log.debug(this, "打开击石成金结算界面");
                    Singleton<GoldHitMainView>.Instance.OpenSuccessPannel();
                }
            }
            else if (copyFinishMsg.code == 420) //时间到
            {
                if (MeVo.instance.mapId == MapTypeConst.ARENA_MAP
                    || MeVo.instance.mapId == MapTypeConst.GoldSilverIsland_MAP)
                {
                    Singleton<ArenaControl>.Instance.ChallengeFail();
                }
                else if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS)
                {
                    MessageManager.Show("世界boss时间结束");
                }
                else if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP &&
                         Singleton<GoldHitJieSuan>.Instance.fialPannel.gameObject.activeInHierarchy == false) //击石成金副本时间到
                {
                    Log.debug(this, "OpenFailPannel。。。。");
                    Singleton<GoldHitMainView>.Instance.OpenFailPannel(true);
                }
                else if (MeVo.instance.mapId != MapTypeConst.GoldHit_MAP)
                {
                    Singleton<CopyControl>.Instance.OpenCopyFailView((int) CopyFailReason.TIME_OVER);
                }
            }
        }

        //服务器返回切换场景命令
        private void Fun_4_17(INetData data)
        {
            Log.info(this, "-Fun_4_17()服务器发送切换场景命令");
            var mapEnterReqMsg = new MapEnterReqMsg_4_17();
            mapEnterReqMsg.read(data.GetMemoryStream());
            Singleton<MapMode>.Instance.changeScene(mapEnterReqMsg.mapid, false, mapEnterReqMsg.x, mapEnterReqMsg.y);
        }

        // 更新副本剩余时间
        public void ChangeDungeonLeftTime(uint leftTime)
        {
            Singleton<MapMode>.Instance.UpdateExpire(leftTime);
        }

        //切换场景前执行，用于初始化一些数据
        private void ResetDataBeforeChangeScene()
        {
            MapMode.CanGoToNextPhase = false;
            MapMode.AutoChangeMap = false;
            MapMode.StartAutoMove = false;
            MapMode.WaitRefreshMonster = false;
            MapMode.DisableInput = false;
        }

        /**切换场景开始(前端)**/

        private void LoadMap(uint mapId)
        {
            Log.info(this, "-loadMap()关闭PlayerMgr和MonsterMgr");
            ResetDataBeforeChangeScene();
            MapMode.WaitRefreshMonster = true;
            PlayerMgr.instance.stop();
            PlayerMgr.instance.Clear();
            MonsterMgr.Instance.stop();
            PetMgr.Instance.Clear();

            //删除传送点特效
            Log.info(this, "-loadMap()删除传送点特效");
            string copyPointTeleportUrl = UrlUtils.GetMainEffectUrl("20001");
            string worldMapTeleportUrl = copyPointTeleportUrl + "_1";
            EffectMgr.Instance.RemoveEffect(copyPointTeleportUrl);
            EffectMgr.Instance.RemoveEffect(worldMapTeleportUrl);

            Log.info(this, "-loadMap()关闭摄像机跟随");
            if (_myCamera != null) _myCamera.IsRuning = false;

            //清除怪物预加载
            MonsterMgr.Instance.ClearMonsterPreload();

            AppMap.Instance.mapParser.LoadMap(mapId);

            //清除还没显示的怪物
            if (Singleton<MapMode>.Instance.waitMonList.Count > 0)
            {
                Singleton<MapMode>.Instance.waitMonList.Clear();
            }
        }


        /**切换场景[OK]**/

        private void MapCallback(uint mapId)
        {
            Log.info(this, "-mapCallback() 场景切换[OK]");
			//关闭所有tips
			TipsManager.Instance.CloseAllTips();

            MeVo.instance.mapId = mapId;
            //发送强制关闭剧情消息
            Singleton<StoryMode>.Instance.DataUpdate(Singleton<StoryMode>.Instance.FORCE_STOP_STORY);

            CanPlayHeroArriveStory = true;
            curMapId = mapId;
            CurTaskCopyMapId = Singleton<TaskModel>.Instance.TaskCopyMapId;
			//主角ai关闭
			if (null != AppMap.Instance.me)
			{
				AppMap.Instance.me.Controller.AiController.SetAi(false);
			}

            //先关闭场景所有音乐
            SoundMgr.Instance.StopAll();
            //播放场景背景音乐
            string sceneBackMusicName = GetMapBackMusicName(mapId);
            SoundMgr.Instance.PlaySceneAudio(sceneBackMusicName);
            int time = Environment.TickCount;
            CreateMe(Singleton<MapMode>.Instance.IsFirstInToScene);
            Log.info(this, "-mapCallback() 创建本角色耗时:" + (Environment.TickCount - time) + " ms");
        }

        //切换场景完成
        private void ChangeSceneOk()
        {
            uint mapId = MeVo.instance.mapId;
            CreateMapPoint(mapId.ToString(CultureInfo.InvariantCulture));
            PlayerMgr.instance.start();
            MonsterMgr.Instance.start();
            PetMgr.Instance.Start();
            if (!Singleton<MapMode>.Instance.IsFirstInToScene)
            {
                MeVo.instance.PetId = MeVo.instance.PetId;
            }
			SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(mapId);
			switch (mapVo.type)
			{
				//进入城镇场景
				case MapTypeConst.CITY_MAP:
					Singleton<BattleView>.Instance.CloseView();
					Singleton<BossView>.Instance.CloseView();
					Singleton<MainView>.Instance.OpenView();
					Singleton<GoGoView>.Instance.CloseView();
					AppMap.Instance.me.Controller.ContCutMgr.StopAll();
					ViewTree.city.SetActive(true);
					ViewTree.battle.SetActive(false);
					if(Singleton<BossMode>.Instance.IsOpenRank   )   //从世界Boss出来显示奖励信息
					{
						Singleton<BossTips>.Instance.OpenPMView();
						Singleton<BossMode>.Instance.IsOpenRank = false;
					}
					break;
				//进入副本场景
				case MapTypeConst.COPY_MAP:
					ViewTree.city.SetActive(false);
					ViewTree.battle.SetActive(true); 
					Singleton<BattleView>.Instance.OpenView();
					Singleton<MainView>.Instance.CloseView();
					
					//主线副本/恶魔岛副本
					if (mapVo.subtype == MapTypeConst.MAIN_COPY 
				    	|| mapVo.subtype == MapTypeConst.DAEMONISLAND_COPY
				    	|| mapVo.subtype == MapTypeConst.FIRST_BATTLE_COPY)
					{
						CanPlayHeroHpChangeStory = true;
						CanPlayMonsterHpChangeStory = true; 
						//播放进入副本语音
						SpeechMgr.Instance.PlayEnterCopySpeech();
						
						if (CanPlayCopyStory(mapId))
						{						
							if (Singleton<StoryControl>.Instance.PlayEnterSceneStory(mapId, StartBattle))
							{	
								enterSceneStoryPlaying = true;
								PreSortStoryPlay();
							}
							else
							{
								StartBattle();
							}
						}
						else
						{
							StartBattle();
						}  
					}
					else
					{
						RefreshMonster ();
					}
					break;
				case MapTypeConst.SPECIAL_MAP:
					ViewTree.city.SetActive(false);
					ViewTree.battle.SetActive(true); 
					Singleton<BattleView>.Instance.OpenView();
					Singleton<MainView>.Instance.CloseView();   
					RefreshMonster ();
					break;
			}
			//进入地图是英雄榜
			if (mapId == MapTypeConst.ARENA_MAP)
			{
				CreateChallenger();
				//Singleton<ArenaFightView>.Instance.OpenView ();
			}
			//进入地图是金银岛
			else if (mapId == MapTypeConst.GoldSilverIsland_MAP)
			{
				CreateRobberPlayer();
				//Singleton<ArenaFightView>.Instance.OpenView ();
			}
			//进入地图是世界boss
			else if (mapId == MapTypeConst.WORLD_BOSS)
			{
				Singleton<MainView>.Instance.CloseView();
				Singleton<BossView>.Instance.OpenView();
				Singleton<BattleView>.Instance.OpenView();
			}
			Singleton<MapMode>.Instance.IsFirstInToScene = false;
            Singleton<StartLoadingView>.Instance.CloseView();
        }

        //
        public void RefreshMonster()
        {
            MapMode.WaitRefreshMonster = false;
            Log.info(this, "-EnterSceneStoreEnd()开始刷怪");
            if (Singleton<MapMode>.Instance.waitMonList.Count > 0)
            {
                Singleton<MapMode>.Instance.AddMonInMap(Singleton<MapMode>.Instance.waitMonList);
                Singleton<MapMode>.Instance.waitMonList.Clear();
            }
        }

        /**创建本玩家**/

        private void CreateMe(bool isFirstIntoScene)
        {
            MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            if (AppMap.Instance.mapParser.MapVo.type != MapTypeConst.CITY_MAP)
            {
                MeVo.instance.X = mapRange.MinX + 1;
                MeVo.instance.Y = (mapRange.MinY + mapRange.MaxY)/2;
            }
            else
            {
                if (!isFirstIntoScene)
                {
                    MeVo.instance.X = mapRange.MaxX - Random.Range(3, 7); //在传送点旁边
                    MeVo.instance.Y = Random.Range(mapRange.MinY, mapRange.MaxY);
                }
            }
            MeDisplay me = AppMap.Instance.me;
            if (me != null)
            {
                me.ChangeDire(Directions.Right);
                InitMePos();
                _myCamera.InitPos();
                SetHeroIdleType();
                var actionControler = me.Controller as ActionControler;
                if (actionControler != null)
                {
                    actionControler.StopWalk(); //刚进场景的时候停止移动,防止出现误点导致的寻路
                }
                me.SetSortingOrder(false);
                ChangeSceneOk();
                return;
            }
            MeVo.instance.ModelLoadCallBack = LoadMeCallBack;
            MeVo.instance.IsUnbeatable = false;
            AppMap.Instance.CreateMe(MeVo.instance);
        }

        /// <summary>
        ///     进入场景后初始化一次玩家位置，防止因为同步的时间差导致进副本时玩家的初始位置不正确
        /// </summary>
        private void InitMePos()
        {
            MeDisplay me = AppMap.Instance.me;
            MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            if (MeVo.instance.X <= mapRange.MinX)
            {
                MeVo.instance.X = mapRange.MinX + 3f;
            }
            else if (MeVo.instance.X >= mapRange.MaxX)
            {
                MeVo.instance.X = mapRange.MaxX - 3f;
            }
            me.Pos(MeVo.instance.X, MeVo.instance.Y);
            if (AppMap.Instance.mapParser.MapVo.type != MapTypeConst.CITY_MAP)
            {
                if (me.Controller.GoName != null)
                {
                    me.Controller.GoName.SetActive(false); //副本中不显示玩家名字
                    float y = me.BoxCollider2D.center.y + me.BoxCollider2D.size.y/2;
                    me.Controller.GoName.transform.localPosition = new Vector3(0f, y, 0f);
                }
            }
            else
            {
                if (me.Controller.GoName)
                {
                    me.Controller.GoName.SetActive(true);
                    float y = me.BoxCollider2D.center.y + me.BoxCollider2D.size.y/2;
                    me.Controller.GoName.transform.localPosition = new Vector3(0f, y + 0.3f, 0f);
                }
            }
            if (MeVo.instance.mapId != MapTypeConst.GoldHit_MAP) //如果是击石成金副本的话，显示面板
            {
                Singleton<GoldHitView>.Instance.CloseView(); //大副本的界面
                Singleton<GoldHitMainView>.Instance.CloseView();
            }
            //主城避免在传送点出生
            if (MapTypeConst.MajorCity == MeVo.instance.mapId)
            {
                if (AppMap.Instance.InHitPointPos(me.Controller.transform.position))
                {
                    Vector3 newPos = me.Controller.transform.position;
                    newPos.y += (GameConst.HitPointRadius + 0.2f);
                    me.Pos(newPos.x, newPos.y);
                }
                else if (AppMap.Instance.InWorldMapPointPos(me.Controller.transform.position))
                {
                    Vector3 newPos = me.Controller.transform.position;
                    newPos.y += (GameConst.HitPointRadius + 0.2f);
                    me.Pos(newPos.x, newPos.y);
                }
            }
            //脚底灰尘特效位置初始化
            GameObject footSmokeObj = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_FootSmoke);
            if (null != footSmokeObj)
            {
                footSmokeObj.transform.position = me.Controller.transform.position;
            }
            GameObject autoRoad = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_AutoSerachRoad);
            if (null != autoRoad)
            {
                autoRoad.transform.position = AppMap.Instance.me.Controller.transform.position + new Vector3(0, 2.2f, 0);
            }
            if (AppMap.Instance.mapParser.NeedSyn)
            {
                RoleMode.Instance.SendStatuChange(); //主角进入场景时同步一次位置信息
            }
        }

        /// <summary>
        ///     根据不同的地图类型设置玩家不同的待机状态
        /// </summary>
        private void SetHeroIdleType()
        {
            SysMapVo mapVo = AppMap.Instance.mapParser.MapVo;
            switch (mapVo.type)
            {
                case MapTypeConst.CITY_MAP:
                    AppMap.Instance.me.Animator.SetFloat(Status.IDLE_TYPE, Status.NORMAL_IDLE);
                    break;
                case MapTypeConst.COPY_MAP:
                    AppMap.Instance.me.Animator.SetFloat(Status.IDLE_TYPE, Status.BATTLE_IDLE);
                    break;
            }
        }

        //本角色创建成功回调
        private void LoadMeCallBack(BaseDisplay display)
        {
            _myCamera = Camera.main.gameObject.AddMissingComponent<MyCamera>();
            GameObject name = NGUITools.AddChild(display.Controller.gameObject);
            var playerDisplay = display as PlayerDisplay;
            if (playerDisplay != null)
            {
                BoxCollider2D boxCollider2D = playerDisplay.BoxCollider2D;
                float y = (boxCollider2D.center.y + boxCollider2D.size.y/2)*display.GoCloth.transform.localScale.y;
                name.name = "name";
                Transform mTrans = name.transform;
                mTrans.localPosition = new Vector3(0f, y + 0.3f, 0f);
                GameObject goName = HudView.Instance.AddItem(display.GetVo().Name, mTrans, HudView.Type.Player);
                display.Controller.GoName = goName;
            }
            InitMePos();
            AppMap.Instance.me.ChangeDire(Directions.Right);
            SetHeroIdleType();
            _myCamera.Init();
            AppMap.Instance.me.Controller.AiController.SetAi(false);
            ChangeSceneOk();
            MeVo.instance.UpdatePetDisplay();
        }

        /// <summary>
        ///     创建传送点
        /// </summary>
        /// <param name="mapId"></param>
        private void CreateMapPoint(string mapId)
        {
            if (Convert.ToInt32(mapId) > 10000 && Convert.ToInt32(mapId) < 20000)
            {
				SysMapVo map = BaseDataMgr.instance.GetMapVo(uint.Parse(mapId));
				string[] worldTransPos = StringUtils.GetValueListFromString( map.world_trans_pos);
				AppMap.Instance.mapParser.WorldTransPosX = float.Parse( worldTransPos[0]) * GameConst.PositionUnit;
				AppMap.Instance.mapParser.WorldTransPosY = float.Parse( worldTransPos[1]) * GameConst.PositionUnit;
				string[] TransPos = StringUtils.GetValueListFromString( map.trans_pos);
				AppMap.Instance.mapParser.TransPosX = float.Parse( TransPos[0]) * GameConst.PositionUnit;
				AppMap.Instance.mapParser.TransPosY = float.Parse( TransPos[1]) * GameConst.PositionUnit;
                //创建副本传送点
                var vo = new DisplayVo
                {
					X = AppMap.Instance.mapParser.TransPosX,
					Y = AppMap.Instance.mapParser.TransPosY,
                    ClothUrl = "Model/Teleport/Empty.assetbundle",
                    ModelLoadCallBack = LoadMapPointCallBack
                };
                BaseDisplay display = AppMap.Instance.CreateMapPoint(vo);
                var mapPointDisplay = display as MapPointDisplay;
                if (mapPointDisplay != null) mapPointDisplay.jumpMapId = 20001;

                //创建世界传送点
                var voWorld = new DisplayVo
                {
					X = AppMap.Instance.mapParser.WorldTransPosX,
					Y = AppMap.Instance.mapParser.WorldTransPosY,
					ClothUrl = "Model/Teleport/Empty.assetbundle",
                    ModelLoadCallBack = LoadMapPointCallBack
                };
                BaseDisplay displayWorld = AppMap.Instance.CreateWorldMapPoint(voWorld);
                var mapPointDisplayWorld = displayWorld as MapPointDisplay;
                if (mapPointDisplayWorld != null) mapPointDisplayWorld.jumpMapId = 20001;
            }
        }

        //传送点创建成功回调
        private void LoadMapPointCallBack(BaseDisplay display)
        {
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_Teleport, display.Controller.transform.position);
        }

        //自动行走切换到下一阶段图
        public IEnumerator AutoMoveToNextMap()
        {
            Log.info(this, "-AutoMoveToNextMap()停止输入检测");
            MapMode.DisableInput = true;
            MapMode.StartAutoMove = true;
            Log.info(this, "-AutoMoveToNextMap()： 等待玩家当前动画结束后开启自动行走功能");
            StatuControllerBase meStatuController = AppMap.Instance.me.Controller.StatuController;
            while (meStatuController.CurrentStatu != Status.IDLE && meStatuController.CurrentStatu != Status.RUN)
            {
                yield return 0;
            }
            if (MapMode.StartAutoMove)
            {
                Log.info(this, "-AutoMoveToNextMap()玩家已到静止态，更新地图阶段信息，开启自动切图标识，修改地图限制范围,");
                MapMode.AutoChangeMap = true;
                MapMode.CUR_MAP_PHASE = MapMode.NEXT_MAP_PHASE;

                // PosX=上阶段的起点+上阶段的长度，对于阶段重叠的情况，就不合适了
                AppMap.Instance.mapParser.PosX =
                    AppMap.Instance.mapParser.EachPosX[MapMode.CUR_MAP_PHASE - 1];//AccumulatedStagesLength[MapMode.CUR_MAP_PHASE - 2]; 
                Log.info(this, "-AutoMoveToNextMap()玩家自动行走到下个场景");
                Log.info(this,
                    "-AutoMoveToNextMap()，自动行走到下一阶段地图，自动行走前玩家位置： " + AppMap.Instance.me.Controller.transform.position.x);
                while (MapMode.AutoChangeMap)
                {
                    AppMap.Instance.me.Controller.MoveTo(
                        AppMap.Instance.mapParser.PosX + Global.HERO_WEIGHT + MapMode.POS_OFFSET, MeVo.instance.Y);
                    yield return 0;
                }
                MapMode.StartAutoMove = false;
            }
            yield return 0;
        }

        /// <summary>
        ///     创建英雄榜敌方玩家
        /// </summary>
        private void CreateChallenger()
        {
            Log.info(this, "创建英雄榜挑战对手");
            PlayerVo challenger = Singleton<ArenaMode>.Instance.vsPlayerAttr;
            //初始化对手位置和朝向
            challenger.X = MeVo.instance.X + 10;
            challenger.Y = MeVo.instance.Y;

            PlayerMgr.instance.addPlayer(challenger);
        }

        private void CreateRobberPlayer()
        {
            PlayerVo challenger = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer.Vo;
            //初始化对手位置和朝向
            challenger.X = MeVo.instance.X + 10;
            challenger.Y = MeVo.instance.Y;
            PlayerMgr.instance.addPlayer(challenger);
        }

        //--------------------------  剧情相关函数----------------------------//
        private void PreSortStoryPlay()
        {
            MapMode.InStory = true;
            Singleton<CopyMode>.Instance.PauseCopy();
            CloseBattleView();
        }

        private void PostSortStoryStop()
        {
            MapMode.InStory = false;
            Singleton<CopyMode>.Instance.ResumeCopy();
            OpenBattleView();
        }

        private void PlayFightStageStory()
        {
            if (CanPlayCopyStory(AppMap.Instance.mapParser.MapId))
            {
                if (Singleton<StoryControl>.Instance.PlayFightStageStory(curMapId, MapMode.CUR_MAP_PHASE, MonsterID, FightStage))
                {
                    fightStageStoryPlaying = true;
                    AppMap.Instance.StopAllMonstersAi();
                    PreSortStoryPlay();
					AppMap.Instance.me.Controller.ContCutMgr.PauseAttack();
                }
				else
				{
					FightStage();
				}
            }
        }

        private void FightStage()
        {
            if (fightStageStoryPlaying)
            {
                fightStageStoryPlaying = false;
                AppMap.Instance.StartAllMonstersAi();
                PostSortStoryStop();
				AppMap.Instance.me.Controller.ContCutMgr.ResumeAttack();
            }
        }

        private void EndStage()
        {
            if (endStageStoryPlaying)
            {
                MapMode.InStageEndStory = false;
                endStageStoryPlaying = false;
                PostSortStoryStop();
            }
            EndStageStoryEnd();
        }

        public string GetMapBackMusicName(uint mapId)
        {
            string result = StringUtils.GetNoSuffixString(BaseDataMgr.instance.GetMapVo(mapId).bgMusic);

            return result;
        }

        private void StartBattle()
        {
            if (enterSceneStoryPlaying)
            {
                enterSceneStoryPlaying = false;
                PostSortStoryStop();
            }
            PlayStartStageStory();
        }

        public bool CanPlayCopyStory(uint mapId)
        {
            const string firstBattleMapId = "10000";
            if (Singleton<TaskModel>.Instance.TaskCopyMapId == mapId
                || firstBattleMapId == mapId.ToString(CultureInfo.InvariantCulture))
            {
                return true;
            }
            return false;
        }

        public void PlayStartStageStory()
        {
            if (CanPlayCopyStory(curMapId))
            {
                if (Singleton<StoryControl>.Instance.PlayStartStageStory(curMapId, MapMode.CUR_MAP_PHASE, StartStage))
                {
                    startStageStoryPlaying = true;
                    PreSortStoryPlay();
                }
				else
				{
					StartStage();
				}
            }
            else
            {
                StartStage();
            }
        }

        private void StartStage()
        {
            if (startStageStoryPlaying)
            {
                startStageStoryPlaying = false;
                PostSortStoryStop();
            }

            Singleton<CopyMode>.Instance.ResumeCopy();
            RefreshMonster();

            if (MeVo.instance.mapId == MapTypeConst.FirstCopy)
            {
                Singleton<SkillControl>.Instance.SetFirstSkillPos();
            }

			AppMap.Instance.me.Controller.ContCutMgr.ResumeAttack();
        }

        private void OpenBattleView()
        {
            Singleton<BattleView>.Instance.OpenView();

        }

        private void CloseBattleView()
        {
            Singleton<BattleView>.Instance.CloseView();
        }

        //---------------------------------------------------------------------------------------------//
        //自动切换阶段
        public void CheckChangePhase()
        {
            if (MapMode.CanGoToNextPhase && !MapMode.StartAutoMove)
            {
                if (AppMap.Instance.me.Controller.transform.position.x >
                    AppMap.Instance.mapParser.CurrentMapRange.MaxX - 3)
                {
                    CoroutineManager.StartCoroutine(AutoMoveToNextMap());
                }
            }
        }
    }
}