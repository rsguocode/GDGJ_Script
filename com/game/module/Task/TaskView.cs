﻿﻿
using System.Collections.Generic;
using com.game.manager;
using com.game.module.test;
using com.game.utils;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/23 12:34:19 
 * function:
 * *******************************************************/

namespace com.game.module.Task
{
    public class TaskView : BaseView<TaskView>
    {
        public override string url { get { return "UI/Task/TaskView.assetbundle"; } }
        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }
        private UILabel _labelAcceptedTask;
        private UILabel _labelCanAcceptedTask;
        private UIToggle _btnAcceptedTask;
        private UIToggle _btnCanAcceptTask;
        private Button _btnClose;
        private GameObject _taskItemGameObject;    //任务Item对应的gameObject
        private List<TaskItem> _taskItems;         //任务Item
        private TaskModel _taskModel;
        private UIGrid _itemsParent;               //任务Item挂载体，任务Item的父项

        protected override void Init()
        {
            _btnAcceptedTask = FindInChild<UIToggle>("Top/BtnAccepted");
            _btnCanAcceptTask = FindInChild<UIToggle>("Top/BtnCanAccept");
            _labelAcceptedTask = FindInChild<UILabel>("Top/BtnAccepted/label");
            _labelCanAcceptedTask = FindInChild<UILabel>("Top/BtnCanAccept/label");
            _btnClose = FindInChild<Button>("TopRight/BtnClose");
            _btnClose.onClick += CloseOnClick;
            _taskItemGameObject = Tools.find(gameObject, "Item").gameObject;
            TaskItemPool.Instance.Init(_taskItemGameObject.transform);
            _itemsParent = FindInChild<UIGrid>("TaskItemPanel/Items");
            _taskItems = new List<TaskItem>();
            _taskModel = TaskModel.Instance;
            UIToggle.current = _btnAcceptedTask;
            _btnAcceptedTask.onChange.Add(new EventDelegate(TopOnClick));
            _btnCanAcceptTask.onChange.Add(new EventDelegate(TopOnClick));
            InitText();
        }

        private void InitText()
        {
            //_labelAcceptedTask.text = LanguageManager.GetWord("MailView.Inbox");
            //_labelCanAcceptedTask.text = LanguageManager.GetWord("MailView.Outbox");
        }

        private void CloseOnClick(GameObject go)
        {
            CloseView();
        }

        private void TopOnClick()
        {
            if (UIToggle.current.value == true)
            {
                UpdateTaskInfo();
            }
        }

        private void UpdateTaskInfo()
        {
            if (UIToggle.current.Equals(_btnAcceptedTask))
            {
                UpdateAcceptedTaskInfo();
            }
            else if (UIToggle.current.Equals(_btnCanAcceptTask))
            {
                UpdateCanAcceptTaskInfo();
            }
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            UIToggle.current = _btnAcceptedTask;
            UpdateTaskInfo();
        }

        /// <summary>
        /// 更新已接任务
        /// </summary>
        private void UpdateAcceptedTaskInfo()
        {
            for (int i = _taskItems.Count - 1; i >= 0; i--)
            {
                TaskItem item = _taskItems[i];
                _taskItems.Remove(item);
                item.Dispose();
            }
            int y = 0;
            var mainTaskVo = _taskModel.CurrentMainTaskVo;
            if (mainTaskVo != null&&mainTaskVo.Statu == TaskStatu.StatuAccepted)
            {
                GameObject taskGameObject = TaskItemPool.Instance.SpawnTaskItem().gameObject;
                var taskItem = taskGameObject.GetComponent<TaskItem>();
                if (taskItem == null)
                {
                    taskItem = taskGameObject.AddComponent<TaskItem>();
                    taskItem.Init();
                }
                taskGameObject.SetActive(false);
                taskGameObject.SetActive(true);
                taskItem.SetData(mainTaskVo);
                _taskItems.Add(taskItem);
                taskGameObject.transform.parent = _itemsParent.transform;
                taskGameObject.transform.localPosition = new Vector3(0, y, 0);
                taskGameObject.transform.localScale = new Vector3(1, 1, 1);
                y -= 195;
            }
            // 屏蔽悬赏任务
            /* var subTaskVo = _taskModel.CurrentSubTaskVo;
            if (subTaskVo != null && subTaskVo.Statu == TaskStatu.StatuAccepted)
            {
                GameObject taskGameObject = TaskItemPool.Instance.SpawnTaskItem().gameObject;
                var taskItem = taskGameObject.GetComponent<TaskItem>();
                if (taskItem == null)
                {
                    taskItem = taskGameObject.AddComponent<TaskItem>();
                    taskItem.Init();
                }
                taskGameObject.SetActive(false);
                taskGameObject.SetActive(true);
                taskItem.SetData(subTaskVo);
                _taskItems.Add(taskItem);
                taskGameObject.transform.parent = _itemsParent.transform;
                taskGameObject.transform.localPosition = new Vector3(0, y, 0);
                taskGameObject.transform.localScale = new Vector3(1, 1, 1);
                y -= 195;
            }*/  
        }

        /// <summary>
        /// 更新可接任务
        /// </summary>
        private void UpdateCanAcceptTaskInfo()
        {
            for (int i = _taskItems.Count - 1; i >= 0; i--)
            {
                TaskItem item = _taskItems[i];
                _taskItems.Remove(item);
                item.Dispose();
            }
            int y = 0;
            var mainTaskVo = _taskModel.CurrentMainTaskVo;
            if (mainTaskVo != null && mainTaskVo.Statu == TaskStatu.StatuUnAccept)
            {
                GameObject taskGameObject = TaskItemPool.Instance.SpawnTaskItem().gameObject;
                var taskItem = taskGameObject.GetComponent<TaskItem>();
                if (taskItem == null)
                {
                    taskItem = taskGameObject.AddComponent<TaskItem>();
                    taskItem.Init();
                }
                taskGameObject.SetActive(false);
                taskGameObject.SetActive(true);
                taskItem.SetData(mainTaskVo);
                _taskItems.Add(taskItem);
                taskGameObject.transform.parent = _itemsParent.transform;
                taskGameObject.transform.localPosition = new Vector3(0, y, 0);
                taskGameObject.transform.localScale = new Vector3(1, 1, 1);
                y -= 195;
            }
            // 屏蔽悬赏任务
            /*var subTaskVo = _taskModel.CurrentSubTaskVo;
            if (subTaskVo == null)
            {
                GameObject taskGameObject = TaskItemPool.Instance.SpawnTaskItem().gameObject;
                var taskItem = taskGameObject.GetComponent<TaskItem>();
                if (taskItem == null)
                {
                    taskItem = taskGameObject.AddComponent<TaskItem>();
                    taskItem.Init();
                }
                taskGameObject.SetActive(false);
                taskGameObject.SetActive(true);
                taskItem.SetWantedTaskData();
                _taskItems.Add(taskItem);
                taskGameObject.transform.parent = _itemsParent.transform;
                taskGameObject.transform.localPosition = new Vector3(0, y, 0);
                taskGameObject.transform.localScale = new Vector3(1, 1, 1);
                y -= 195;
            }*/
        }
    }
}