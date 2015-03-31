﻿﻿﻿
using com.game.data;
using com.game.manager;
using com.game.module.test;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/10 06:03:34 
 * function: 悬赏任务Item
 * *******************************************************/

namespace com.game.module.Task
{
    public class WantedTaskItem : MonoBehaviour
    {

        private UILabel _titleLabel;
        private UILabel _taskDetail;
        private UILabel _taskExpRewardLabel;
        private UILabel _taskGoldRewardLabel;
        private Button _acceptTaskButton;
        private TaskModel _taskModel;
        private SysTask _taskVo;
        private int _color;

        public void Init()
        {
            _titleLabel = NGUITools.FindInChild<UILabel>(gameObject,"Title/Label");
            _taskDetail = NGUITools.FindInChild<UILabel>(gameObject, "Content/Label");
            _taskExpRewardLabel = NGUITools.FindInChild<UILabel>(gameObject, "Reward/ExpLabel");
            _taskGoldRewardLabel = NGUITools.FindInChild<UILabel>(gameObject, "Reward/GoldLabel");
            _acceptTaskButton = NGUITools.FindInChild<Button>(gameObject, "ButtonAccept/");
            _acceptTaskButton.onClick += OnAcceptTask;
            _acceptTaskButton.label.text = LanguageManager.GetWord("WantedTaskItem.AcceptTask");
            _taskModel = Singleton<TaskModel>.Instance;
        }

        private void OnAcceptTask(GameObject button)
        {
            _taskModel.SelectedWantedTaskId = _taskVo.taskId;
            _taskModel.AccetpTask((uint)_taskVo.taskId,0);
        }

        public void SetData(SysTask taskVo,int color)
        {
            _color = color;
            _taskVo = taskVo;
            _titleLabel.text = taskVo.name;
            _taskExpRewardLabel.text = taskVo.exp.ToString();
            _taskGoldRewardLabel.text = taskVo.gold.ToString();
            var task = new TaskVo();
            task.Statu = TaskStatu.StatuAccepted;
            task.TaskId = (uint)taskVo.taskId;
            task.SysTaskVo = taskVo;
            task.CanCommit = false;
            _taskDetail.text = TaskUtil.GetTaskDescribe(task);
            switch (_color)
            {
                case 1:
                    _taskDetail.color = Color.white;
                    break;
                case 2:
                    _taskDetail.color = Color.green;
                    break;
                case 3:
                    _taskDetail.color = Color.blue;
                    break;
                case 4:
                    _taskDetail.color = Color.cyan;
                    break;
                case 5:
                    _taskDetail.color = Color.magenta;
                    break;
            }
        }


    }
}