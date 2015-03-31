using Assets.Scripts.com.game.consts;
using com.game.manager;
using UnityEngine;
using com.game.consts;
using com.game.module.NpcDialog;
using com.game.module.Task;
using com.game.module.test;
using com.game.Public.Message;
using com.game.vo;
using com.u3d.bases.map;
using com.u3d.bases.display;
using com.u3d.bases.debug;
using com.u3d.bases.consts;
using com.game.autoroad;
using com.u3d.bases.display.controler;
using Com.Game.Module.Copy;
using com.game.module.effect;
using com.game.sound;
using com.u3d.bases.controller;
using com.u3d.bases.display.character;
using System.Collections.Generic;
using com.game.data;
using com.game.module.map;


namespace com.game
{
    public class AppMap : GameMap
    {
        public int CurCopyGroupId = 0;           //当前城镇场景的副本组ID

        public static AppMap Instance = new AppMap();

        public delegate void MapPointHitCallback();

        private MapPointHitCallback mapPointHitCallBack;

        public AppMap()
        {

        }

        public bool HitMapPoint(MapPointHitCallback callBack = null)
        {
            var teleportEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_Teleport);

            if (teleportEffect == null)
            {
                return false;
            }

            Vector3 posTeleport = teleportEffect.transform.position;
            posTeleport.y = AppMap.Instance.mapParser.TransPosY - GameConst.HitPointEffectOffH;
            this.me.Controller.MoveTo(posTeleport.x, posTeleport.y);
            mapPointHitCallBack = callBack;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_RoleToTeleport, posTeleport, true, openCopyPoint);
            //播放传送点音效
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_Teleport);

            return true;
        }

        public bool HitWorldMapPoint(MapPointHitCallback callBack = null)
        {
            var teleportEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_Teleport, "_1");

            if (teleportEffect == null)
            {
                return false;
            }

            Vector3 posTeleport = teleportEffect.transform.position;
            posTeleport.y = AppMap.Instance.mapParser.WorldTransPosY - GameConst.HitPointEffectOffH;
            this.me.Controller.MoveTo(posTeleport.x, posTeleport.y);
            mapPointHitCallBack = callBack;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_RoleToTeleport, posTeleport, true, openWorldMapPoint);
            //播放传送点音效
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_Teleport);

            return true;
        }

        /**本角色与游戏对象(场景传送点,副本传送点,NPC，怪物,BOSS)碰撞时回调**/
        override public void hitCallback(BaseDisplay display)
        {
            switch (display.Type)
            {
                case DisplayType.MAP_POINT:
                    HitMapPoint();
                    break;
                case DisplayType.NPC:
                    var npcId = display.GetVo().Id;
                    if (npcId == NpcIdConst.NpcWantedTask)
                    {
                        /*if (me.GetVo().Level < 36)
                        {
                            var message = LanguageManager.GetWord("AppMap.WantedTaskLimit");
                            MessageManager.Show(message);
                        }
                        else
                        {
                            if (TaskModel.Instance.CurrentSubTaskVo != null)
                            {
                                var message = LanguageManager.GetWord("AppMap.WantedTaskRemind");
                                MessageManager.Show(message);
                            }
                            else
                            {
                                Singleton<WantedTaskView>.Instance.OpenView();
                            }
                        }*/
                        //暂时屏蔽掉悬赏任务入口
                    }
                    else
                    {
                        Singleton<NpcDialogModel>.Instance.CurrentNpcId = display.GetVo().Id;
                        Singleton<NpcDialogView>.Instance.OpenView();
                    }
                    break;
            }
        }

        private void openCopyPoint()
        {
            //			Singleton<CopyView>.Instance.OpenCopyPointMapView ();   //点击非主城的副本传送点。打开副本地图
            Singleton<CopyControl>.Instance.OpenCopyPointView();   //点击非主城的副本传送点。打开副本地图

            if (null != mapPointHitCallBack)
            {
                mapPointHitCallBack();
            }
        }

        private void openWorldMapPoint()
        {
            //			Singleton<CopyView>.Instance.OpenCopyPointMapView ();   //点击非主城的副本传送点。打开副本地图
            Singleton<CopyControl>.Instance.OpenWorldMapView();

            if (null != mapPointHitCallBack)
            {
                mapPointHitCallBack();
            }
        }

        /**可否处理鼠标点击
         * @return [true:可处理,false:禁止处理]
         * **/
        override public bool monseClickEnable()
        {
            if (UICamera.hoveredObject)
            {
                return true;    //鼠标在UI层移动
            }
            return false;
        }

        /**停止自动寻路**/
        public override void stopAutoRoad()
        {
            if (AutoRoad.intance.isRoading)
            {
                AutoRoad.intance.isRoading = false;
                var meControler = me.Controller as MeControler;
                if (meControler != null) meControler.StopWalk();
                AutoRoad.intance.autoWalk();

                //停止自动寻路
                Singleton<MeStatuController>.Instance.AutoRoundEffect.SetActive(false);
            }
        }

        public MeControler MeControler()
        {
            var meControler = me.Controller as MeControler;
            return meControler;
        }

        /**发送本玩家移动坐标给服务器**/
        override public void tellServer(float x, float y)
        {
            //Log.info(this, "-tellServer() 玩家移动 x:" + x + ",y:" + y);
            //(mapControl.mode as MapMode).moveXY(x, y);    //注释掉点击时的时时同步玩家位置
        }

        //是否在副本传送点位置
        public bool InHitPointPos(Vector3 pos)
        {
            if ((Mathf.Abs(pos.x - AppMap.Instance.mapParser.TransPosX) <= GameConst.HitPointRadius)
                && (Mathf.Abs(pos.y - AppMap.Instance.mapParser.TransPosY + GameConst.HitPointEffectOffH) <= GameConst.HitPointRadius))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //是否在世界传送点位置
        public bool InWorldMapPointPos(Vector3 pos)
        {
            if ((Mathf.Abs(pos.x - AppMap.Instance.mapParser.WorldTransPosX) <= GameConst.HitPointRadius)
                && (Mathf.Abs(pos.y - AppMap.Instance.mapParser.WorldTransPosY + GameConst.HitPointEffectOffH) <= GameConst.HitPointRadius))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据地图Id和玩家id获取在指定范围内的怪;
        /// 注意：检测范围可参考PlayConstant.NORMAL_GRAB_X_OFFSET;
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="xOffsex">正方向(正数)</param>
        /// <param name="x2Offset">正方向(正数)</param>
        /// <param name="yOffset">正方向(正数)</param>
        /// <param name="y2Offset">负方向(正数)</param>
        /// <param name="zOffset">负方向(正数)</param>
        /// <param name="z2Offset">负方向(正数)</param>
        /// <returns></returns>
        public IList<MonsterDisplay> getMonsterNearestBy(string playerId, float xOffsex = 0, float x2Offset = 0, float yOffset = 0, float y2Offset = 0, float zOffset = 0, float z2Offset = 0)
        {
            IList<MonsterDisplay> listMonsters = new List<MonsterDisplay>();
            //获取基准点;
            PlayerDisplay playerDisplay = GetPlayer(playerId);
            if (playerDisplay != null)
            {
                Vector3 tmpPos;
                Vector3 playerPos = playerDisplay.Controller.transform.position;
                foreach (MonsterDisplay tmpMonster in _monsterList)
                {
                    if (tmpMonster != null && tmpMonster.Controller != null)
                    {
                        tmpPos = tmpMonster.Controller.transform.position;
                        if ((playerPos.x + xOffsex >= tmpPos.x) && (tmpPos.x >= playerPos.x - x2Offset) &&
                            (playerPos.y + yOffset >= tmpPos.y) && (tmpPos.y >= playerPos.y - y2Offset) &&
                            (playerPos.z + zOffset >= tmpPos.z) && (tmpPos.z >= playerPos.z - z2Offset))
                        {
                            listMonsters.Add(tmpMonster);
                        }
                    }
                }
            }
            return listMonsters;
        }
        /// <summary>
        ///  判断角色是否在触发的区域内
        /// </summary>
        /// <param name="x"> 玩家X坐标</param>
        /// <param name="y">玩家y坐标</param>
        /// <param name="z">玩家z坐标</param>
        /// <returns></returns>
        public bool isTrigger(float x, float y, float z)
        {
            SysDungeonMon MonData;
            uint phase = MapMode.CUR_MAP_PHASE;
            uint mapid = AppMap.Instance.mapParser.MapId;
            MonData = BaseDataMgr.instance.getSysDungeonVo(mapid);
            if (MonData == null)
            {
                return false;
            }
            string pos = MonData.pos;
            if (pos == null || pos == "0")
            {
                return false;
            }
            string[] result;
            result = pos.Split(',');
            float tempX = float.Parse(result[0]);
            float tempY = float.Parse(result[1]);
            float tempZ = float.Parse(result[2]);

            float l = float.Parse(result[3]);
            float w = float.Parse(result[4]);
            float h = float.Parse(result[5]);

            if ((x >= tempX && x <= (tempX + l)) && (y >= tempY && y <= tempY + h) && (z >= tempZ && z <= tempZ + w))
            {
                return true;
            }

            return false;
        }

    }
}
