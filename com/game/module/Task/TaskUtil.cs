﻿﻿﻿
using System.Runtime.InteropServices;
using com.game.manager;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.effect;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/08 04:51:22 
 * function: 任务工具类
 * *******************************************************/

namespace com.game.module.Task
{
    public class TaskUtil
    {
        /// <summary>
        /// 根据任务表字符串返回任务的说明
        /// </summary>
        /// <returns>任务说明</returns>
        public static string GetTaskDescribe(TaskVo taskVo)
        {
            string trace = GetTraceInfo(taskVo);
            string result = "";
            string[] items = trace.Split('.');
            switch (items[0])
            {
                case "n":
                    result = GetFindNpcTaskDescribe(items);
                    break;
                case "fb":
                    result = GetFindCopyTaskDescribe(items);
                    break;
            }
            return result;
        }

        private static string GetFindNpcTaskDescribe(string[] items)
        {
            var sysNpcVo = BaseDataMgr.instance.GetNpcVoById(uint.Parse(items[2]));
            return string.Format("{0}{1}", "找到", sysNpcVo.name);
        }

        private static string GetFindCopyTaskDescribe(string[] items)
        {
            var sysMapVo = BaseDataMgr.instance.GetMapVo(uint.Parse(items[2]));
            return string.Format("{0}{1}", "通关", sysMapVo.name);
        }


        public static BaseDisplay GetTaskDisplay(string trace)
        {
            string[] items = trace.Split('.');
            switch (items[0])
            {
                case "n":
                    return GetNpcDisplay(items);
                    break;
                case "fb":
                    return GetMapPointDisplay(items);
                    break;
            }
            return null;
        }

        private static NpcDisplay GetNpcDisplay(string[] items)
        {
            uint npcId = uint.Parse(items[2]);
            return AppMap.Instance.GetNpcDisplay(npcId);
        }

        private static MapPointDisplay GetMapPointDisplay(string[] items)
        {
            return AppMap.Instance.MapPointDisplay;
        }

        /// <summary>
        /// 获取追踪信息
        /// </summary>
        /// <param name="taskVo"></param>
        /// <returns></returns>
        public static string GetTraceInfo(TaskVo taskVo)
        {
            string trace = "";
            switch (taskVo.Statu)
            {
                case TaskStatu.StatuUnAccept:
                    trace = taskVo.SysTaskVo.trace_accept;
                    break;
                case TaskStatu.StatuAccepted:
                    if (taskVo.CanCommit)
                    {
                        trace = taskVo.SysTaskVo.trace_com;
                    }
                    else
                    {
                        trace = taskVo.SysTaskVo.trace_uncom;
                    }
                    break;
                case TaskStatu.StatuFinished:
                    trace = taskVo.SysTaskVo.trace_com;
                    break;
            }
            return trace;
        }

        /// <summary>
        /// 获取任务的触发类型
        /// </summary>
        /// <param name="taskVo"></param>
        /// <returns></returns>
        public static int GetTaskTriggerType(TaskVo taskVo)
        {
            int result = 0;
            if (taskVo.Statu == TaskStatu.StatuAccepted)
            {
                result = taskVo.CanCommit ? 2 : 1;
            }
            return result;
        }
    }
}