/**地图模块--通讯交互类**/

using System.Collections.Generic;
using System.IO;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.loading;
using com.game.module.SystemData;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;
using UnityEngine;

namespace com.game.module.map
{
    public class MapMode : BaseMode<MapMode>
    {
        public const float POS_OFFSET = 2; //切换场景时人物移动目标位置相对人物最小位置边界的距离
        public const ushort INIT_POSX_IN_MAIN_CITY = 5; //玩家通过非传送点方式进入主城出现的位置点
        public const float INIT_POSY_IN_MAIN_CITY = 1.8f; //玩家通过非传送点方式进入主城出现的位置点
        public const ushort MIN_MONSTER_POSY = 18; //怪物出现的Y轴最小值
        public const ushort MAX_MONSTER_POSY = 21; //怪物出现的Y轴最大值 +1
        public const ushort EVENT_CODE_UPDATE_LEFTTIME = 2; //修改剩余时间世界代码
        public const ushort EVENT_CODE_STOP_LEFTTIME = 3; //暂停副本剩余时间
        public static ushort CUR_MAP_PHASE = 1;
        public static ushort NEXT_MAP_PHASE = 1;
        public static uint expire;
        public static int EndTimestamp; //副本结束时间搓，使用这个值得到的副本剩余时间更精确
        public bool IsFirstInToScene = true; //是否第一次进入场景
        public bool NeedGuideMainTask = false; //进入场景是否需要进行任务引导

        public List<PMapMon> waitMonList = new List<PMapMon>();

        static MapMode()
        {
            AutoChangeMap = false;
            CanGoToNextPhase = false;
            WaitRefreshMonster = false;
            StartAutoMove = false;
            DisableInput = false;
        }

        public static bool DisableInput { get; set; }

        public static bool StartAutoMove { get; set; }

        public static bool WaitRefreshMonster { get; set; }

        public static bool CanGoToNextPhase { get; set; }

        public static bool AutoChangeMap { get; set; }

        public static bool InStory { get; set; }
        public static bool InStageEndStory { get; set; }

        public static bool IsTriggered { get; set; }

        /**切换场景请求**/

        public void changeScene(uint mapId, bool isBornPlace, float toX, float toY)
        {
            if (isBornPlace)
            {
                toX = INIT_POSX_IN_MAIN_CITY;
                toY = INIT_POSY_IN_MAIN_CITY;
            }
            MeVo.instance.toX = toX;
            MeVo.instance.toY = toY;
            var msdata = new MemoryStream();
            Module_4.write_4_1(msdata, mapId, (ushort)(toX * Global.CLIENT_TO_SERVICE),
                (ushort)(toY * Global.CLIENT_TO_SERVICE));
            Log.info(this, "changeScene 请求进入场景：" + mapId + " toX: " + toX + " toY: " + toY);
            AppNet.gameNet.send(msdata, 4, 1);
        }

        //进入场景(登陆直接到这一步)(已改动,统一从changeScene开始切场景流程,已改回去，不能这样改，会有问题----modify by lixi)
        public void ApplySceneInfo()
        {
            Log.debug(this, "-EnterScene() 进入场景");
            Log.debug(this, "-EnterScene() 先显示进度条");
            Singleton<StartLoadingView>.Instance.OpenView();
            Log.debug(this, "-ApplySceneInfo() 请求场景信息,发送4-2");
            AppNet.gameNet.send(new MemoryStream(), 4, 2);
        }


        /**
         * 玩家移动
         */

        public void RoleMove(ushort x, ushort y, ushort moveStatus)
        {
            var msdata = new MemoryStream();
            Module_4.write_4_6(msdata, x, y, moveStatus);
            AppNet.gameNet.send(msdata, 4, 6);
        }

        /**
         * 发送怪物死亡消息给服务器
         */

        public void MonsterDeath(uint id)
        {
            Debug.Log("****发送怪物死亡消息给服务器 MapMode.MonsterDeath()");
            var msdata = new MemoryStream();
            Module_4.write_4_12(msdata, id, (uint)AppMap.Instance.me.Controller.ContCutMgr.AttackCounter,
                MeVo.instance.CurHp, MeVo.instance.CurMp);
            AppNet.gameNet.send(msdata, 4, 12);
        }

        /**
         * 角色死亡
         */

        public void RoleDeath(uint id)
        {
            var msdata = new MemoryStream();
            Module_4.write_4_13(msdata, id);
            AppNet.gameNet.send(msdata, 4, 13);
        }

        //在地图中增加玩家
        public void AddPlayerInMap(List<PMapRole> rolesList)
        {
            uint mapId = MeVo.instance.mapId;
            foreach (PMapRole rolesIn in rolesList)
            {
                var playerVo = new PlayerVo();
                playerVo.mapId = mapId;
                playerVo.Id = rolesIn.id;
                playerVo.Name = rolesIn.name;
                playerVo.Level = rolesIn.level;
                playerVo.CurHp = rolesIn.hp;
                playerVo.Hp = rolesIn.hpFull;
                playerVo.job = rolesIn.job;
                playerVo.X = rolesIn.x*GameConst.PositionUnit;
                playerVo.Y = rolesIn.y*GameConst.PositionUnit;
                playerVo.sex = rolesIn.sex;
                playerVo.job = rolesIn.job;
                playerVo.PetId = rolesIn.petId;
                if (playerVo.sex < 1) playerVo.sex = 1;
                PlayerMgr.instance.addPlayer(playerVo);
            }
        }


        // 修改地图剩余时间
        public void UpdateExpire(uint leftTime)
        {
            expire = leftTime;
            EndTimestamp = (int)expire + ServerTime.Instance.Timestamp;
            DataUpdate(EVENT_CODE_UPDATE_LEFTTIME);
        }

        /// <summary>
        /// 暂停副本剩余时间
        /// </summary>
        public void StopLeftTime()
        {
            DataUpdate(EVENT_CODE_STOP_LEFTTIME);
        }

        //在地图中增加怪物并初始化怪物的基础数据
        public void AddMonInMap(List<PMapMon> monList)
        {
            uint mapId = MeVo.instance.mapId;
            foreach (PMapMon monIn in monList)
            {
                bool isNew = false;
                SysMonsterVo monsterVo = null;
                MonsterVo enemyVo = null;

                enemyVo = MonsterMgr.Instance.GetMonster(mapId.ToString(), monIn.id.ToString());
                if (enemyVo == null)
                {
                    isNew = true;
                    enemyVo = new MonsterVo();
                }
                enemyVo.mapId = mapId;
                enemyVo.Id = monIn.id;
                enemyVo.monsterId = monIn.monid;
                //enemyVo.monsterId = 100001;
                enemyVo.bornType = monIn.born;
                //enemyVo.bornType = 2;
                enemyVo.CurHp = monIn.hp;
                enemyVo.Hp = monIn.hpFull;

                //分割线，下面这些数据不是服务器传的,通过读表赋值
                monsterVo = enemyVo.MonsterVO;
                if (monsterVo == null)
                {
                    Log.info(this, "数据库没这个怪物的信息");
                    continue;
                }
                enemyVo.Name = monsterVo.name;
                enemyVo.AttMMax = (uint) monsterVo.att_m_max;
                enemyVo.AttMMin = (uint) monsterVo.att_m_min;
                enemyVo.AttPMin = (uint) monsterVo.att_p_min;
                enemyVo.AttPMax = (uint) monsterVo.att_p_max;
                enemyVo.DefM = (uint) monsterVo.def_m;
                enemyVo.DefP = (uint) monsterVo.def_p;
                enemyVo.Hit = (uint) monsterVo.hit;
                enemyVo.Dodge = monsterVo.dodge;
				enemyVo.Crit = (uint)monsterVo.crit;
                enemyVo.CritRatio = (uint) monsterVo.crit_ratio;
                enemyVo.Flex = (uint) monsterVo.flex;
                enemyVo.HurtRe = (uint) monsterVo.hurt_re;
                enemyVo.HurtResist = (uint) monsterVo.hurt_resist;
                enemyVo.HurtDownResist = (uint) monsterVo.hurt_down_resist;
                enemyVo.MoveRatio = monsterVo.move_ratio*0.001f;
                enemyVo.job = GameConst.JOB_MON; //给job赋值,战斗公式会用到
                enemyVo.Level = monsterVo.lvl;
                enemyVo = MonsterAdapt(enemyVo); //根据场景修改怪物属性
                
                if (isNew)
                {
                    UnityEngine.Debug.Log("****AddMonInMap(), 在地图中增加怪物并初始化怪物的基础数据！id, monId = " + enemyVo.Id + ", " + enemyVo.monsterId);
                    Log.info(this, "这是个新怪物，加到怪物管理中去！");
                    MonsterMgr.Instance.addMonster(mapId.ToString(), enemyVo);
                }
            }
        }

        private MonsterVo MonsterAdapt(MonsterVo enemyVo)
        {
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(MeVo.instance.mapId);

            if (mapVo.adapt)
            {
				enemyVo.Level = MeVo.instance.Level;
#if false
				float zoomFactor = 10000f;
                SysMonsterAdaptRuleVo adapt = BaseDataMgr.instance.GetMonsterAdapt((uint) MeVo.instance.Level);
				enemyVo.Level = MeVo.instance.Level;
				enemyVo.CurHp = (uint) (enemyVo.CurHp*adapt.hp_ratio/zoomFactor);
                enemyVo.Hp = (uint)(enemyVo.Hp * adapt.hp_ratio / zoomFactor);
				enemyVo.AttMMax = (uint) (enemyVo.AttMMax*adapt.att_m_max_ratio/zoomFactor);
				enemyVo.AttMMin = (uint) (enemyVo.AttMMin*adapt.att_m_min_ratio/zoomFactor);
				enemyVo.AttPMin = (uint) (enemyVo.AttPMin*adapt.att_p_min_ratio/zoomFactor);
				enemyVo.AttPMax = (uint) (enemyVo.AttPMax*adapt.att_p_max_ratio/zoomFactor);
				enemyVo.DefM = (uint) (enemyVo.DefM*adapt.def_m_ratio/zoomFactor);
				enemyVo.DefP = (uint) (enemyVo.DefP*adapt.def_p_ratio/zoomFactor);
				enemyVo.Hit = (uint) (enemyVo.Hit*adapt.hit_ratio/zoomFactor);
				enemyVo.Dodge = (int)(enemyVo.Dodge*adapt.dodge_ratio/zoomFactor);
				enemyVo.Crit = (uint) (enemyVo.Crit*adapt.crit_lvl_ratio/zoomFactor);
				enemyVo.CritRatio = (uint) (enemyVo.CritRatio*adapt.crit_hurt_ratio/zoomFactor);
				enemyVo.Flex = (uint) (enemyVo.Flex*adapt.flex_ratio/zoomFactor);
				enemyVo.HurtRe = (uint) (enemyVo.HurtRe*adapt.hurt_re_ratio/zoomFactor);
#endif
            }

            return enemyVo;
        }
    }
}