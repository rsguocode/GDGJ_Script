﻿﻿﻿using com.game.manager;
using com.game.module.test;
using com.u3d.bases.display;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/23 12:34:42 
 * function:
 * *******************************************************/

namespace com.game.module.Task
{
    public class TaskItem : MonoBehaviour
    {
        private Button _btnGuide;
        private UILabel _taskTitleLabel;
        private UILabel _taskDescribeLabel;
        private UILabel _taskRewardLabel;
        private UILabel _taskExpLabel;
        private UILabel _taskGoldLabel;
        private TaskVo _taskVo;
        private GameObject _taskReward;

        public void Init()
        {
            _btnGuide = NGUITools.FindInChild<Button>(gameObject, "BtnGuide");
            _taskTitleLabel = NGUITools.FindInChild<UILabel>(gameObject, "Content/TaskTitle");
            _taskDescribeLabel = NGUITools.FindInChild<UILabel>(gameObject, "Content/TaskDescribe");
            _taskRewardLabel = NGUITools.FindInChild<UILabel>(gameObject, "Content/TaskReward");
            _taskExpLabel = NGUITools.FindInChild<UILabel>(gameObject, "Reward/ExpLabel");
            _taskGoldLabel = NGUITools.FindInChild<UILabel>(gameObject, "Reward/GoldLabel");
            _taskReward = NGUITools.FindChild(gameObject, "Reward");
            _btnGuide.onClick += OnGuideTask;
        }

        private void OnGuideTask(GameObject button)
        {
            string trace;
            if (_taskVo != null)
            {
                trace = TaskUtil.GetTraceInfo(_taskVo);
            }
            else
            {
                trace = "n.10001.20000";
            }
            BaseDisplay display = TaskUtil.GetTaskDisplay(trace);
            MoveTo(display);
            Singleton<TaskView>.Instance.CloseView();
        }

        /// <summary>
        /// 任务寻路处理
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        private bool MoveTo(BaseDisplay display)
        {
            if (display == null) return false;
            if (display.GoBase == null) return false;
            Transform target = display.GoBase.transform;
            AppMap.Instance.clickVo.SaveClicker(display);
            AppMap.Instance.me.Controller.MoveTo(target.position.x, target.position.y);
            return true;
        }

        public void SetData(TaskVo taskVo)
        {
            _taskVo = taskVo;
            _taskReward.SetActive(true);
            _taskExpLabel.text = taskVo.SysTaskVo.exp + "";
            _taskGoldLabel.text = taskVo.SysTaskVo.gold + "";
            _taskTitleLabel.text = taskVo.SysTaskVo.name;
            _taskDescribeLabel.text = LanguageManager.GetWord("MainLeftView.mainTask") + TaskUtil.GetTaskDescribe(taskVo);
            _taskRewardLabel.text = "任务奖励：";
        }

        public void SetWantedTaskData()
        {
            _taskVo = null;
            _taskTitleLabel.text = "[支]悬赏任务";
            _taskDescribeLabel.text = "完成悬赏任务可获得大量的经验和金币";
            _taskReward.SetActive(false);
            _taskRewardLabel.text = "剩余悬赏次数：" + TaskModel.Instance.LeftWantedTaskCount;
        }

        /// <summary>
        /// 销毁操作
        /// </summary>
        public void Dispose()
        {
            //Singleton<MailMode>.Instance.dataUpdated -= MailDataUpdateHandle;
            //Destroy(gameObject);
            TaskItemPool.Instance.DeSpawn(gameObject.transform);
        }
    }
}