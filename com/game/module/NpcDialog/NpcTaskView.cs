﻿﻿﻿
using System.Collections.Generic;
using com.game.manager;
using com.game.module.Task;
using com.game.module.test;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/08 11:47:49 
 * function: Npc面板任务信息
 * *******************************************************/
using com.game.Public.Message;
using UnityEngine;

namespace com.game.module.NpcDialog
{
    public class NpcTaskView : BaseView<NpcTaskView>
    {
        private UILabel _npcSpeakLabel;
        private UILabel _taskInfoLabel;
        private Button _taskButton;
        private uint _currentNpcId;
        private NpcDialogModel _npcDialogModel;
        private TaskModel _taskModel;
        private TaskVo _currentTaskVo;
        private List<string> _wordsList;
        private int _talkIndex;
        private bool _isDialog;  //是否是在对话

        public new void Init()
        {
            _npcSpeakLabel = FindChild("NpcSpeak/Label").GetComponent<UILabel>();
            _taskInfoLabel = FindChild("TaskInfo/Label").GetComponent<UILabel>();
            _taskButton = FindChild("Button").GetComponent<Button>();
            _npcDialogModel = Singleton<NpcDialogModel>.Instance;
            _taskModel = Singleton<TaskModel>.Instance;
            _wordsList = new List<string>();
            _talkIndex = 0;
            _taskButton.onClick += OnTaskButtonClick;
        }

        private void OnTaskButtonClick(GameObject button)
        {
            if (!_isDialog)
            {
                switch (_currentTaskVo.Statu)
                {
                    case TaskStatu.StatuUnAccept:
                        _taskModel.AccetpTask(_currentTaskVo.TaskId, _npcDialogModel.CurrentNpcId);
                        break;
                    case TaskStatu.StatuAccepted:
                        if (_currentTaskVo.CanCommit)
                        {
                            _taskModel.CommitTask(_currentTaskVo.TaskId, _npcDialogModel.CurrentNpcId);
                        }
                        else
                        {
                            MessageManager.Show("任务还没完成哦");
                        }
                        break;
                }
                Singleton<NpcDialogView>.Instance.CloseView();
            }
            else
            {
                _talkIndex += 1;
                ShowRoleDialog();
            }
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            _currentNpcId = _npcDialogModel.CurrentNpcId;
            ShowTaskInfo();
        }

        private void ShowTaskInfo()
        {
            AnalysWords();
            _taskButton.SetActive(true);
            ShowRoleDialog();
        }

        private void ShowTaskStatu()
        {
            var taskReward = "任务奖励：";
            if (_currentTaskVo.SysTaskVo.exp > 0)
            {
                taskReward += "经验：" + _currentTaskVo.SysTaskVo.exp + "    ";
            }
            if (_currentTaskVo.SysTaskVo.gold > 0)
            {
                taskReward += "金币：" + _currentTaskVo.SysTaskVo.gold;
            }
            _taskInfoLabel.text = taskReward;
            switch (_currentTaskVo.Statu)
            {
                case TaskStatu.StatuUnAccept:
                    _taskButton.label.text = "接受任务";
                    break;
                case TaskStatu.StatuAccepted:
                    if (_currentTaskVo.CanCommit)
                    {
                        _taskButton.label.text = "完成任务";
                    }
                    else
                    {
                        _taskButton.SetActive(false);
                    }
                    break;
            }
        }

        private void ShowRoleDialog()
        {
            var currentWords = _wordsList[_talkIndex];
            var currentList = currentWords.Replace("#r", "@").Split('@');
            _npcSpeakLabel.text = currentList[0];
            if (currentList.Length > 1)
            {
                _isDialog = true;
                _taskInfoLabel.text = currentList[1];
                _taskButton.label.text = "继续对话";
            }
            else
            {
                _isDialog = false;
                ShowTaskStatu();
            }
        }

        private void AnalysWords()
        {
            _wordsList.Clear();
            _currentTaskVo = _npcDialogModel.CurrentTaskVo;
            string words = "";
            switch (_currentTaskVo.Statu)
            {
                case TaskStatu.StatuUnAccept:
                    words = _currentTaskVo.SysTaskVo.talk_accept;
                    break;
                case TaskStatu.StatuAccepted:
                    if (_currentTaskVo.CanCommit)
                    {
                        words = _currentTaskVo.SysTaskVo.talk_com;
                    }
                    else
                    {
                        words = _currentTaskVo.SysTaskVo.talk_uncom;
                    }
                    break;
            }
            string[] result = words.Replace("#n", "@").Split('@');
            foreach (var s in result)
            {
                if (s.Length > 0)
                {
                    _wordsList.Add(s);
                }
            }
        }

    }
}