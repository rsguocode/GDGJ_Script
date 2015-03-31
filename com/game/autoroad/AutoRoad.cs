﻿using System;
using System.Collections.Generic;
using com.game.utils;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.u3d.bases.task;
using com.game.module.effect;
using com.u3d.bases.controller;
using com.game.module.test;


/**自动寻路类**/
namespace com.game.autoroad
{
    public class AutoRoad
    {
        private short pathLen = 0;                           //轨迹长度  
        private short curStep = -1;                          //当前行走阶段
        private IList<PathStepVo> pathList;                  //行走轨迹列表

        //到达最终目标点类型
        public const short TO_NPC = 1;                       //寻路到NPC跟前
        public const short TO_COPY = 2;                      //寻路到副本传送点
        public const short TO_SCENE = 3;                     //寻路到新场景

        //到达阶段目标点类型
        private const short TO_STEP_NPC = 1;                 //阶段目标到达NPC跟前
        private const short TO_STEP_COPY = 2;                //阶段目标到达副本传送点跟前
        private const short TO_STEP_SCENE = 3;               //阶段目标到达场景传送点跟前
        private const short TO_STEP_WORLDMAP = 4;            //阶段目标到达世界地图UI终点元素点跟前
        public static AutoRoad intance = new AutoRoad();


        public AutoRoad()
        {
            pathList = new List<PathStepVo>();
        }

        /**启动或停止自动寻路**/
        public bool isRoading { get; set; }

        /**取消自动寻路**/
        public void cancelAutoWalk() {
            isRoading = false;
            (AppMap.Instance.me.Controller as MeControler).StopWalk();
        }

        /**任务寻路
         * @param taskId 任务ID
         * @param index  指定条件索引(0:当前条件寻路,0>:指定条件索引寻路)
         * **/
        public void createTaskPath(String taskId,int index=0) 
        {  //com.u3d.bases.task
            com.u3d.bases.task.Task task = TaskExecute.instance.get(taskId);
			if(task==null) return;

			//不可接、未领取、已完成的任务，则自动寻路到NPC前
			if(task.state!=TaskConst.STATE_DOING)
			{
				createPath(TO_NPC,task.curNpc());
                Log.info(this, "-createTaskPath() 自动寻路到NPC：" + task.curNpc() + " 跟前！");
				return;
			}

			//已接未完成，根据条件追踪。index==0:当前首个可追踪条件，index>0:指定追踪索引
            TaskCondition conditon = index < 1 ? task.curCondition() : task.getCondition(index);
            if (conditon == null) return;
            switch (conditon.sort)
            {
                //采集(直接跳转到指定场景)
                case TaskConst.C_4:
                     //AppFacde.instance.getControl(MapControl.
                     break;
                //到副本
                case TaskConst.C_3:
                case TaskConst.C_5:
                case TaskConst.C_9:
                     createPath(TO_COPY, conditon.backupId);
                     break;
            }
        }

        /**根据传入目标点类型&目标点ID -- 创建寻路轨迹
         * @param targetType 目标点类型[TO_NPC , TO_COPY , TO_SCENE]
         * @param targetId   目标点ID  [npcId  , CopyId  , MapId]
         * **/
        public void createPath(short targetType,String targetId) {
            if (targetType < TO_NPC || targetType > TO_SCENE) return;
            if (StringUtils.isEmpty(targetId)) return;

            pathLen = 0;
            curStep = -1;
            isRoading = false;
            bool result = false;
            switch (targetType)
            {
                case TO_NPC:   result = create2NpcPath(targetId);  break;
                case TO_COPY : result = create2CopyPath(targetId); break;
                case TO_SCENE: result = create2MapPath(targetId);  break;
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
        private bool create2NpcPath(String npcId) {
            //NpcVo npcVo = BaseDataMgr.instance.getNpcVo(npcId);
            //String MapId = AppMap.instance.mapParser.MapId;
            //if (MapId == null) return false;    //一种情况mapId=null,即是刚登陆成功时

            //if (MapId.Equals(npcVo.MapId))
            //{//本场景NPC
            //    add2PathList(1, TO_STEP_NPC, npcId);
            //    //Log.info(this, "-create2NpcPath() 追踪到本场景ID:" + MapId + ",NPC:" + npcId + " 跟前!");
            //}
            //else 
            //{//新场景NPC
            //    String mapPointKey=AppMap.instance.mapPointDisplay.key;        //场景跳转点ID
            //    add2PathList(1, TO_STEP_SCENE, mapPointKey);                   //到场景跳转点
            //    add2PathList(2, TO_STEP_WORLDMAP, npcVo.MapId);                //在世界地图UI,记录目标场景ID
            //    add2PathList(3, TO_STEP_NPC, npcId);                           //到新场景NPC跟前
            //    //Log.info(this, "-create2NpcPath() 追踪到新场景ID:" + npcVo.MapId + ",NPC:" + npcId + " 跟前!");
            //}
            return true;
        }

        /**构建到达副本轨迹
         * @param copyId 最终副本ID 目标点
         * **/
        private bool create2CopyPath(String copyId)
        {
            //CopyVo copyVo = BaseDataMgr.instance.getCopyVo(copyId);                 //根据副本ID，找出副本组ID
            //TeleportVo teleportVo = BaseDataMgr.instance.getTeleportVoByCopyGroupId(copyVo.copyGroupId);    //根据副本组ID，找出副本传送点配置信息
            //CopyPointDisplay display = AppMap.instance.getCopyPoint(teleportVo.id);   //根据副本组ID，找副本传送点对象
            //Log.info(this, "-create2CopyPath() copyId:" + ",teleportVo.id:" + teleportVo.id);

            //if (display!=null)
            //{//本场景副本
            //    add2PathList(1, TO_STEP_COPY, teleportVo.id);                                //记录副本组ID     
            //    //Log.info(this, "-create2CopyPath() 追踪到本场景ID:" + MapId + "副本组ID:" + copyGroupId + ",副本ID:" + copyId + " 跟前!");
            //}
            //else
            //{//新场景副本
            //    add2PathList(1, TO_STEP_SCENE, null);                                      //到场景跳转点
            //    add2PathList(2, TO_STEP_WORLDMAP, teleportVo.MapId);                       //在世界地图UI,记录目标场景ID
            //    add2PathList(3, TO_STEP_COPY, teleportVo.id);                                //到新场景NPC跟前
            //    //Log.info(this, "-create2CopyPath() 追踪到新场景ID:" + MapId + "副本组ID:" + copyGroupId + ",副本ID:" + copyId + " 跟前!");
            //}
            return true;
        }

        /**构建到达新场景轨迹
         * @param MapId 目标点场景ID
         * **/
        private bool create2MapPath(String mapId)
        {
            String curMapId = AppMap.Instance.mapParser.MapId.ToString();
            if (curMapId.Equals(mapId))
            {//当前场景
                //Log.info(this, "-create2MapPath() 追踪到本场景ID:" + MapId);
                return false;
            }
            else
            {//新场景
                add2PathList(1, TO_STEP_SCENE, null);           //到场景跳转点
                add2PathList(2, TO_STEP_WORLDMAP, mapId);       //在世界地图UI,记录目标场景ID  
                //Log.info(this, "-create2CopyPath() 追踪新本场景ID:" + MapId );
            }
            return true;
        }

        /**添加阶段轨迹到列表
         * @param index      在轨迹列表中位置下标[1-3]之间
         * @param stepType   到达阶段目标点类型[TO_STEP_NPC,TO_STEP_COPY,TO_STEP_SCENE,TO_STEP_WORLDMAP]
         * @param targetId   到达阶段目标点ID  [npcId，copyId，MapId]
         * **/
        private void add2PathList(int index, int stepType, String targetId)
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
            if (isRoading==false || curStep >= pathLen)
            {
                curStep = -1;
                isRoading = false;
                Log.info(this, "-autoWalk() 自动寻路已结束！");
                //if (battleControler != null) battleControler.isAutoRoad = false;
                return;
            }

            Log.info(this, "-autoWalk() 开始自动寻路！");
            PathStepVo stepVo=pathList[curStep];
            //if (battleControler != null) battleControler.isAutoRoad = true;

            switch (stepVo.stepType)
            {
                case TO_STEP_NPC       :  
                     moveTo( AppMap.Instance.GetNpc(stepVo.targetId) );
                     break;
                case TO_STEP_COPY:
                     //根据副本组ID取得副本传送点   
                     moveTo( AppMap.Instance.GetCopyPoint(stepVo.targetId) );
                     break;
                case TO_STEP_SCENE :
                     moveTo(AppMap.Instance.MapPointDisplay);
                     break;
                case TO_STEP_WORLDMAP  :
                     //worldmapControl.moveTo(stepVo.targetId);
                     break;
            }
        }

        /**执行移动**/
        private bool moveTo(BaseDisplay display) {
            if (display == null) return false;
            if (display.GoBase == null) return false;
            Transform transform = display.GoBase.transform;
            AppMap.Instance.clickVo.SaveClicker(display);
            AppMap.Instance.me.Controller.MoveTo(transform.position.x, transform.position.y);
            return true;
        }
    }
}

