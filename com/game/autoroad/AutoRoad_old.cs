using System;
using System.Collections.Generic;
using com.game.utils;
using com.game.manager;
using com.game.data;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.u3d.bases.task;

using com.game.consts;
using com.u3d.bases.display.character;
using com.game.module.map;
using com.game.vo;


/**自动寻路类**/
namespace com.game.autoroad
{
    public class AutoRoad_old
    {
        private short pathLen = 0;                           //轨迹长度  
        private short curStep = -1;                          //当前行走阶段
        private IList<PathStepVo> pathList;                  //行走轨迹列表
        private IList<string> tempMapList;

        //到达最终目标点类型
        public const short TO_NPC = 1;                       //寻路到NPC跟前
        public const short TO_COPY = 2;                      //寻路到副本传送点
        public const short TO_SCENE = 3;                     //寻路到新场景
        public const short TO_MONSTER = 4;                   //寻到场景怪物

        //到达阶段目标点类型
        private const short TO_STEP_NPC = 1;                 //阶段目标到达NPC跟前
        private const short TO_STEP_COPY = 2;                //阶段目标到达副本传送点跟前
        private const short TO_STEP_SCENE = 3;               //阶段目标到达场景传送点跟前
        private const short TO_STEP_WORLDMAP = 4;            //阶段目标到达世界地图UI终点元素点跟前
        private const short TO_STEP_MONSTER  = 5;            //阶段目标到达怪物跟前
        public static AutoRoad_old intance = new AutoRoad_old();


        public AutoRoad_old() {
            tempMapList = new List<string>();
            pathList = new List<PathStepVo>();
        }

        /**启动或停止自动寻路**/
        public bool isRoading { get; set; }

        /**任务寻路
         * @param taskId 任务ID
         * @param index  指定条件索引(0:当前条件寻路,0>:指定条件索引寻路)
         * **/
        public void createTaskPath(String taskId,int index=0) 
        {
            Task task = TaskExecute.instance.get(taskId);
			if(task==null) return;

			//不可接、未领取、已完成的任务，则自动寻路到NPC前
			if(task.state!=TaskConst.STATE_DOING)
			{
				createPath(TO_NPC,task.curNpc());
                //Log.info(this, "-createTaskPath() 自动寻路到NPC：" + task.curNpc() + " 跟前！");
				return;
			}

			//已接未完成，根据条件追踪。index==0:当前首个可追踪条件，index>0:指定追踪索引
            TaskCondition conditon = index < 1 ? task.curCondition() : task.getCondition(index);
            if (conditon == null) return;
            switch (conditon.sort)
            {
                //杀怪
                case TaskConst.C_3:
                     createPath(TO_MONSTER, conditon.mapId, conditon.targetId);
                     break;
                //采集物品|收集物品
                case TaskConst.C_4:
                case TaskConst.C_5:
                     createPath(TO_SCENE, conditon.mapId);
                     break;
            }
        }

        /**根据传入目标点类型&目标点ID -- 创建寻路轨迹
         * @param targetType 目标点类型[TO_NPC , TO_COPY , TO_SCENE,TO_MONSTER]
         * @param targetId   目标点ID  [npcId  , CopyId  , MapId,monsterId]
         * **/
        public void createPath(short targetType,string targetId,string monsterId=null) {
            if (targetType < TO_NPC || targetType > TO_MONSTER) return;
            if (StringUtils.isEmpty(targetId)) return;

            isRoading = false;
            pathLen = 0;
            curStep = -1;
            bool result = false;
            switch (targetType)
            {
                case TO_NPC    : result = createNpcPath(targetId);  break;
                case TO_COPY   : result = createCopyPath(targetId); break;
                case TO_SCENE  : result = createMapPath(targetId);  break;
               // case TO_MONSTER: result = createMonsterPath(targetId,monsterId); break;
            }

            if (result)
            {//开始自动寻路
                isRoading = true;
                autoWalk();
            } 
        }

        /**构建到达NPC轨迹
         * @param npcId 最终npcID目标点
         * **/
        private bool createNpcPath(string npcId) {
            //NpcVo npcVo = BaseDataMgr.instance.getNpcVo(npcId);      //NPC所在场景
            //string curMapId = AppMap.instance.mapParser.MapId;       //当前场景
            //if (curMapId == null || npcVo == null) return false;     //一种情况mapId=null,即是刚登陆成功时

            //if (curMapId.Equals(npcVo.MapId))
            //{//本场景NPC
            //    add2PathList(1, TO_STEP_NPC, npcId);
            //    return true;
            //}
            //else 
            //{//新场景NPC
            //    return findPath(curMapId, npcVo.MapId, npcVo);
            //}
            return true;
        }

        /**构建到达新场景轨迹
         * @param MapId 目标点场景ID
         * **/
        private bool createMapPath(string mapId)
        {
            string curMapId = AppMap.Instance.mapParser.MapId.ToString();
            if (curMapId == null || curMapId.Equals(mapId))
            {//当前场景
                return false;
            }
            else
            {//新场景
                //return findPath(curMapId, mapId, null);
                return false;
            }
        }

        /**构建到达怪物
         * @param MapId     怪物所在场景
         * @param monsterId 怪物ID
         * **/
        private bool createMonsterPath(string mapId,uint monsterId) {    
            string curMapId = AppMap.Instance.mapParser.MapId.ToString();                         //当前场景
            SysMonsterVo monsterVo = BaseDataMgr.instance.getSysMonsterVo(monsterId);
            if (curMapId == null || mapId == null || monsterVo == null) return false;  //一种情况mapId=null,即是刚登陆成功时
            Log.info(this, "-createMonsterPath() 追踪到场景ID:" + mapId + ",怪物ID:" + monsterId + ",怪物名称:" + monsterVo.name);

            if (curMapId.Equals(mapId))
            {//本场景怪物
                //add2PathList(1, TO_STEP_MONSTER, monsterId);
                return true;
            }
            else
            {//新场景怪物
                //return findPath(curMapId, mapId, null, monsterVo);
                return true;
            }
        }

        /**计算出路径段
         * @param fromMapId 开始场景ID
         * @param toMapId   目标场景ID
         * @param NpcVo     是否寻路到NPC前(null:寻路到场景,!=null:寻路到NPC前)
         * @param MonsterVo 是否寻路到怪物前
         * **/
        //private bool findPath(string fromMapId,string toMapId,NpcVo npcVo=null,SysMonsterVo monsterVo=null) {
            //SysMapVo toMapVo = BaseDataMgr.instance.GetMapVo(toMapId);
            //SysMapVo fromMapVo = BaseDataMgr.instance.GetMapVo(fromMapId);
            //if (toMapVo == null || fromMapVo == null) return false;

            //string startMapId = null; // fromMapVo.type == MapTypeConst.FIELD_BOSS_MAP ? fromMapVo.preId : fromMapId;
            //string endMapId = null; //toMapVo.type == MapTypeConst.FIELD_BOSS_MAP ? toMapVo.preId : toMapId;
            //string fromMainMapId = findMainMapId(startMapId);                 //开始场景的主城场景ID
            //string toMainMapId = findMainMapId(endMapId);                     //目标场景的主城场景ID 
            //if (fromMainMapId == null || toMainMapId == null) return false;
            //Log.info(this, "-findPath() 开始场景:" + fromMapId + "，所在主城：" + fromMainMapId);
            //Log.info(this, "-findPath() 目标场景:" + toMapId + "，所在主城：" + toMainMapId);

            //if (fromMainMapId.Equals(toMainMapId))
            //{//当前环寻路 
            //    Log.info(this, "-findPath() 当前环寻路！");
            //    if (startMapId.Equals(fromMainMapId))
            //    {//当前所在：主城场景，只需下寻
            //        findMapLinks(startMapId, endMapId, false);
            //    }
            //    else
            //    {//当前所在：非主城场景，先上寻再下寻
            //        findMapLinks(startMapId, endMapId, true);
            //        if (tempMapList.Count < 1) findMapLinks(startMapId, endMapId, false);
            //    }

            //    //构造路径列表
            //    int len = 0;
      

            //    for (int i = 0; i < tempMapList.Count; i++)
            //    {
            //        add2PathList(++len, TO_STEP_SCENE, tempMapList[i]);
            //    }
      
            //    if (npcVo != null)     add2PathList(++len, TO_STEP_NPC, npcVo.id);
            //    if (monsterVo != null) add2PathList(++len, TO_STEP_MONSTER, monsterVo.id.ToString());
            //    if (pathLen < 1) return false;
            //}
            //else
            //{//不同环寻路 
            //    Log.info(this, "-findPath() 不同环寻路！");
            //    //当前环
            //    if (startMapId.Equals(fromMainMapId))
            //    {//当前所在：主城场景
            //        tempMapList.Clear();
            //        tempMapList.Add(fromMainMapId);
            //    }
            //    else
            //    {//当前所在：非主城场景
            //        findMapLinks(startMapId, fromMainMapId, true);
            //    } 
            //    if (tempMapList.Count < 1) return false;

            //    int len = 0;
   

            //    for (int i = 0; i < tempMapList.Count; i++)
            //    {
            //        add2PathList(++len, TO_STEP_SCENE, tempMapList[i]);
            //    }
            //    add2PathList(++len, TO_STEP_SCENE, fromMainMapId);
            //    add2PathList(++len, TO_STEP_WORLDMAP, toMainMapId);
             
            //    //新环
            //    if (!toMainMapId.Equals(toMapId))
            //    {
            //        findMapLinks(toMainMapId, endMapId, false);
            //        if (tempMapList.Count < 1) return false;
            //        for (int j = 0; j < tempMapList.Count; j++)
            //        {
            //            add2PathList(++len, TO_STEP_SCENE, tempMapList[j]);
            //        }
            //    }
           
            //    if (npcVo != null)     add2PathList(++len, TO_STEP_NPC, npcVo.id);
            //    if (monsterVo != null) add2PathList(++len, TO_STEP_MONSTER, monsterVo.id.ToString());
            //}
            //println();
            //return true;
        //}

        /**打印路径**/
        private void println() {
            Log.info(this, "=========================================\n");
            PathStepVo stepVo=null;
            for (int i=0;i<pathLen;i++)
            {
                stepVo = pathList[i];
                Log.info(this, "[" + stepVo.stepDesc() + "] targetId:" + stepVo.targetId);
            }
            Log.info(this, "=========================================\n");
        }

        /**构建到达副本轨迹
         * @param copyId 最终副本ID 目标点
         * **/
        private bool createCopyPath(string copyId)
        {
            /*CopyVo copyVo = BaseDataMgr.instance.getCopyVo(copyId);                        //根据副本ID，找出副本组ID
            CopyPointDisplay display = AppMap.instance.getCopyPoint(copyVo.copyGroupId);   //根据副本组ID，找副本传送点对象
            //String MapId = AppMap.instance.mapParser.MapId;
            String copyGroupId = copyVo.copyGroupId.ToString();
            Log.info(this, "-create2CopyPath() copyId:" + copyId + ",copyGroupId:" + copyGroupId);
            if (display != null)
            {//本场景副本
                add2PathList(1, TO_STEP_COPY, copyGroupId);                                //记录副本组ID     
                //Log.info(this, "-create2CopyPath() 追踪到本场景ID:" + MapId + "副本组ID:" + copyGroupId + ",副本ID:" + copyId + " 跟前!");
            }
            else
            {//新场景副本
                TeleportVo vo = BaseDataMgr.instance.getTeleportVo(copyVo.copyGroupId);    //根据副本组ID，找出副本传送点配置信息
                add2PathList(1, TO_STEP_SCENE, null);                                      //到场景跳转点
                add2PathList(2, TO_STEP_WORLDMAP, vo.MapId);                               //在世界地图UI,记录目标场景ID
                add2PathList(3, TO_STEP_COPY, copyGroupId);                                //到新场景NPC跟前
                //Log.info(this, "-create2CopyPath() 追踪到新场景ID:" + MapId + "副本组ID:" + copyGroupId + ",副本ID:" + copyId + " 跟前!");
            }
            return true;*/
            return false;
        }

        /**添加阶段轨迹到列表
         * @param index    在轨迹列表中位置下标从1开始
         * @param stepType 到达阶段目标点类型[TO_STEP_NPC,TO_STEP_COPY,TO_STEP_SCENE,TO_STEP_WORLDMAP]
         * @param targetId 到达阶段目标点ID  [npcId，copyId，MapId]
         * @param isUpWalk 是否向上寻走[true:向上寻,false:向下寻]，该标记决定本玩家在场景出生点坐标 
         * **/
        private void add2PathList(int index, int stepType, string targetId)
        { 
            if(pathList.Count<index) pathList.Add(new PathStepVo());
            PathStepVo pathStepVo=pathList[index-1];
            pathStepVo.stepType = stepType;
            pathStepVo.targetId = targetId;
            pathLen++;
        }

        /**自动行走**/
        public void autoWalk() {
            curStep++;
            //Log.info(this, "-autoWalk() isRoading:" + isRoading + ",curStep:" + curStep + ",pathLen:" + pathLen);
            if (isRoading==false || curStep >= pathLen)
            {
                curStep = -1;
                isRoading = false;
                //if (battleControl != null) battleControl.isAutoRoad = false;
                //Log.info(this, "-autoWalk() 自动寻路已结束,battleControl.isAutoRoad:" + (battleControl != null ? battleControl.isAutoRoad : false) + ",battleControl:" + battleControl);
                return;
            }

            PathStepVo stepVo=pathList[curStep];
            //if (battleControl != null) battleControl.isAutoRoad = true;
			MonsterVo enemyVo;
            switch (stepVo.stepType)
            {
                case TO_STEP_NPC     :
                     moveTo(AppMap.Instance.GetNpc(stepVo.targetId), moveEndBack);
                     break;
                case TO_STEP_SCENE   :
                     moveTo(AppMap.Instance.GetMapPoint(stepVo.targetId));
                     break;
                case TO_STEP_WORLDMAP:
                     //worldmapControl.moveTo(stepVo.targetId);
                     break;
                case TO_STEP_MONSTER:
                     MonsterDisplay monster = getMonsterWithUid(stepVo.targetId); 
                     if (monster != null)
                     {
                         moveTo(monster, moveEndBack);
                     }
                     else 
                     {//从等待队列中找

					enemyVo = MonsterMgr.Instance.GetMonster(stepVo.targetId);
                         if (enemyVo != null) (AppMap.Instance.me.Controller as MeControler).MoveToAndTellServer(enemyVo.X, enemyVo.Y, moveEndBack);
                     }
                     break;
            }
        }

        /**根据怪物UID取得怪物(主要用于任务寻路)**/
        public MonsterDisplay getMonsterWithUid(string uid)
        {
            if (StringUtils.isEmpty(uid)) return null;
            MonsterVo enemyVo=null;
            IList<MonsterDisplay> list = AppMap.Instance.monsterList;
            foreach (MonsterDisplay display in list)
            {
                enemyVo = (MonsterVo)display.GetVo();
                if (enemyVo.monsterId.Equals(uid)) return display;
            }
            return null;
        }

        /**执行移动**/
        private bool moveTo(BaseDisplay display,MoveEndCallback callback=null) {
            if (display == null) return false;
            //if (display.goCloth == null) return false;

            Transform transform = display.GoBase.transform;
            AppMap.Instance.clickVo.SaveClicker(display);
            (AppMap.Instance.me.Controller as MeControler).MoveToAndTellServer(transform.position.x, transform.position.y, callback);
            transform = null;
            return true;
        }

        /**移到怪物附近结束**/
        private void moveEndBack(BaseControler controler) {
            Log.info(this, "-moveEndBack() 移到怪物跟前结束！");
            autoWalk();
        }


        /**寻找mapId所在的主城场景ID
        * @return 返回主城场景ID
        * **/
        private string findMainMapId(string mapId)
        {
            //SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(MapId);
            //if (mapVo == null) return null;
            //if (mapVo.type == MapTypeConst.CITY_MAP) return mapVo.id.ToString();

            //while (true)
            //{
            //    //mapVo = BaseDataMgr.instance.GetMapVo(mapVo.preId);
            //    if (mapVo == null) return null;
            //    if (mapVo.type == MapTypeConst.CITY_MAP) return mapVo.id.ToString();
            //}
            return "";
        }

        /**向上|向下寻找以startMapId为起点,targetMapId为目标点的场景链
         * @param startMapId  起点场景ID
         * @param targetMapId 目标点场景ID
         * @param isUp        寻找方向[true:向上寻找,false:向下寻找]
         * @return 场景链
         * **/
        private void findMapLinks(string startMapId, string targetMapId,bool isUp)
        {
            tempMapList.Clear();
            bool result = false;
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(uint.Parse(startMapId));

            while (true)
            {
                //mapVo = BaseDataMgr.instance.GetMapVo(isUp ? mapVo.preId : mapVo.nextId);
                tempMapList.Add(mapVo.id.ToString());
                if (mapVo.id.Equals(targetMapId))
                {//找到
                    result = true;
                    break;
                }
                if(mapVo.type == MapTypeConst.CITY_MAP) break; //没找到,退出
            }
            if (!result) tempMapList.Clear();
        }

        //private WorldMapControl worldmapControl   { get { return (WorldMapControl)AppFacde.instance.getControl(WorldMapControl.NAME); } }
        //private PlayerBattleControl battleControl { get { return (PlayerBattleControl)(AppMap.instance.me.controler as MeControler).battleControler; } }
    }
}

