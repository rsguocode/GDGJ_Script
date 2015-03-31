using System.Collections.Generic;
﻿﻿﻿using System.IO;
﻿﻿﻿using com.game.manager;
﻿﻿﻿using com.game.module.test;
using PCustomDataType;
﻿﻿﻿using Proto;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 02:24:02 
 * function: 任务管理数据中心
 * *******************************************************/

namespace com.game.module.Task
{
    public class TaskModel :BaseMode<TaskModel>
    {

        public const int EventMainTaskIdUpdate = 1; //主线任务ID改变事件
        public const int EventSubTaskIdUpdate = 2; //支线任务ID改变事件
        public const int EventWantedTaskUpdate = 3; //悬赏任务更新事件
        public const int EventWantedCountUpdate = 4;//悬赏任务数量更新
        public const int EventLeftRefreshTimeUpdate = 5;//悬赏任务剩余刷新时间更新
        public const int EventMainTaskStatuUpdate = 6; //主线任务任务状态更新
        public const int EventMainTaskAutoGuide = 7; //主线任务自动引导
        public const int TypeMain = 1;//主线任务
        public const int TypeSub = 2;//支线任务
        private uint _nextMainTaskId;//下一个主线任务Id
        private List<PTaskDoing> _taskDoing;
        private TaskVo _currentMainTaskVo;
        private TaskVo _currentSubTaskVo;
        private List<PRecruit> _wantedTaskList;
        private int _leftWantedTaskCount; //剩余悬赏任务数量
        private int _leftRefreshTime; //剩余刷新时间
        public int SelectedWantedTaskId; //准备接受的悬赏任务Id
        public uint TaskCopyMapId;           //任务副本id
        

        public List<PTaskDoing> TaskDoing
        {
            get { return _taskDoing; }
            set
            {
                _taskDoing = value;
                foreach (var pTaskDoing in _taskDoing)
                {
                    var sysTaskVo = BaseDataMgr.instance.GetSysTaskVo(pTaskDoing.taskId);
                    if (sysTaskVo.type == TypeMain)
                    {
                        var taskVo = new TaskVo();
                        taskVo.TaskId = pTaskDoing.taskId;
                        taskVo.SysTaskVo = sysTaskVo;
                        taskVo.Statu = TaskStatu.StatuAccepted;
                        taskVo.CanCommit = pTaskDoing.state!=0;
                        CurrentMainTaskVo = taskVo;
                    }
                    else if (sysTaskVo.type == TypeSub)
                    {
                        var taskVo = new TaskVo();
                        taskVo.TaskId = pTaskDoing.taskId;
                        taskVo.SysTaskVo = sysTaskVo;
                        taskVo.Statu = TaskStatu.StatuAccepted;
                        taskVo.CanCommit = pTaskDoing.state != 0;
                        CurrentSubTaskVo = taskVo;
                    }
                }
                if (CurrentMainTaskVo == null)
                {
                    var sysTaskVo = BaseDataMgr.instance.GetSysTaskVo(NextMainTaskId);
                    if(sysTaskVo==null)return;
                    if (sysTaskVo.type == TypeMain)
                    {
                        var taskVo = new TaskVo();
                        taskVo.TaskId = NextMainTaskId;
                        taskVo.SysTaskVo = sysTaskVo;
                        taskVo.Statu = TaskStatu.StatuUnAccept;
                        CurrentMainTaskVo = taskVo;
                    }
                }
            }
        }





        public TaskVo CurrentMainTaskVo
        {
            get { return _currentMainTaskVo; }
            set
            {
                _currentMainTaskVo = value;
                SetTaskCopyId();
                DataUpdate(EventMainTaskIdUpdate);
                DataUpdate(EventMainTaskStatuUpdate);
            }
        }

        private void SetTaskCopyId()
        {
            string trace = TaskUtil.GetTraceInfo(_currentMainTaskVo);
            string[] items = trace.Split('.');
            switch (items[0])
            {
                case "n":
                    TaskCopyMapId = 0;
                    break;
                case "fb":
                    TaskCopyMapId = uint.Parse(items[2]);
                    break;
            }
        }

        public TaskVo CurrentSubTaskVo
        {
            get { return _currentSubTaskVo; }
            set
            {
                _currentSubTaskVo = value;
                DataUpdate(EventSubTaskIdUpdate);
            } 
        }

        public uint NextMainTaskId
        {
            get { return _nextMainTaskId; }
            set { _nextMainTaskId = value; }
        }

        public List<PRecruit> WantedTaskList
        {
            get { return _wantedTaskList; }
            set
            {
                _wantedTaskList = value;
                DataUpdate(EventWantedTaskUpdate);
            }
        }

        public int LeftWantedTaskCount
        {
            get { return _leftWantedTaskCount; }
            set
            {
                _leftWantedTaskCount = value; 
                DataUpdate(EventWantedCountUpdate);
            }
        }

        public int LeftRefreshTime
        {
            get { return _leftRefreshTime; }
            set
            {
                _leftRefreshTime = value;
                DataUpdate(EventLeftRefreshTimeUpdate);
            }
        }

        /// <summary>
        /// 主线任务自动引导
        /// </summary>
        public void AutoGuideMainTask()
        {
            DataUpdate(EventMainTaskAutoGuide);
        }

        //--------------------------------------华丽的分割线，以下是协议请求操作---------------------------------

        /// <summary>
        /// 请求任务信息
        /// </summary>
        public void GetTaskInfo()
        {
            var msdata = new MemoryStream();
            Module_6.write_6_1(msdata);
            AppNet.gameNet.send(msdata, 6, 1);
        }

        /// <summary>
        /// 接任务
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="npcId">NpcId</param>
        public void AccetpTask(uint taskId, uint npcId)
        {
            var msdata = new MemoryStream();
            Module_6.write_6_2(msdata,taskId,npcId);
            AppNet.gameNet.send(msdata, 6, 2);
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="npcId"></param>
        public void CommitTask(uint taskId, uint npcId)
        {
            var msdata = new MemoryStream();
            Module_6.write_6_4(msdata, taskId, npcId);
            AppNet.gameNet.send(msdata, 6, 4);
        }

        /// <summary>
        /// 第一次进入场景时请求悬赏任务信息
        /// </summary>
        public void GetWantedTaskInfo()
        {
            var msdata = new MemoryStream();
            Module_6.write_6_14(msdata);
            AppNet.gameNet.send(msdata, 6, 14);
        }

        public void RefreshWantedTask()
        {
            var msdata = new MemoryStream();
            Module_6.write_6_15(msdata);
            AppNet.gameNet.send(msdata, 6, 15);
        }
    }
}