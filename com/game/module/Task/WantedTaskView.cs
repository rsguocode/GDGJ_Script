﻿﻿using System.Collections.Generic;
﻿﻿﻿using com.game.consts;
﻿﻿﻿using com.game.data;
﻿﻿﻿using com.game.manager;
﻿﻿﻿using com.game.module.SystemData;
using com.game.module.test;
﻿﻿﻿using com.game.Public.Confirm;
﻿﻿﻿using com.game.utils;
﻿﻿﻿using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/10 05:59:36 
 * function: 悬赏任务面板
 * *******************************************************/

namespace com.game.module.Task
{
    public class WantedTaskView :BaseView<WantedTaskView>
    {

        public override string url { get { return "UI/WantedTask/WantedTaskView.assetbundle"; } }
        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }

        private Button _closeButton;
        private UILabel _processInfoLabel;
        private Button _refreshButton;
        private List<WantedTaskItem> _items;
        private TaskModel _taskModel;
        private UILabel _leftRefreshTimeLabel;

        protected override void Init()
        {
            _taskModel = Singleton<TaskModel>.Instance;
            _closeButton = FindChild("ButtonClose").GetComponent<Button>();
            _closeButton.onClick += OnCloseButtonClick;
            _processInfoLabel = FindChild("Statu/Process").GetComponent<UILabel>();
            _leftRefreshTimeLabel = FindChild("Statu/LeftRefreshTime").GetComponent<UILabel>();
            _refreshButton = FindChild("ButtonRefresh").GetComponent<Button>();
            _refreshButton.label.text = LanguageManager.GetWord("WantedTaskView.Refresh");
            _refreshButton.onClick += OnRefreshWantedTask;
            _items = new List<WantedTaskItem>();
            _taskModel.GetWantedTaskInfo();                     //36级开放悬赏任务
            for (var i = 0; i < 3; i++)
            {
                var name = "Item" + (i + 1);
                var itemObject = FindChild(name);
                var item = itemObject.AddComponent<WantedTaskItem>();
                item.Init();
                _items.Add(item);
            }
        }

        /// <summary>
        /// 点击刷新悬赏任务按钮
        /// </summary>
        /// <param name="button"></param>
        private void OnRefreshWantedTask(GameObject button)
        {
            SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeWantedTaskRefresh);
            string diam = StringUtils.GetValueListFromString(priceVo.diam)[0];
            var costAlter = LanguageManager.GetWord("WantedTaskView.RefreshCost",new[]{diam});
            ConfirmMgr.Instance.ShowCommonAlert(costAlter, ConfirmCommands.WantedTaskRefresh, RefreshWantedTask, LanguageManager.GetWord("ConfirmView.Ok"), null, LanguageManager.GetWord("ConfirmView.Cancel"));
        }

        private void RefreshWantedTask()
        {
            _taskModel.RefreshWantedTask(); //刷新悬赏任务
        }

        private void OnCloseButtonClick(GameObject button)
        {
            CloseView();
        }

        /// <summary>
        /// 注册UI响应事件
        /// </summary>
        public override void RegisterUpdateHandler()
        {
            _taskModel.dataUpdated += WantedTaskDataUpdate;
        }

        /// <summary>
        /// 取消UI响应事件
        /// </summary>
        public override void CancelUpdateHandler()
        {
            _taskModel.dataUpdated -= WantedTaskDataUpdate;
        }

        /// <summary>
        /// 响应悬赏任务数据更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void WantedTaskDataUpdate(object sender, int code)
        {
            switch (code)
            {
                case TaskModel.EventWantedTaskUpdate:
                    UpdateWantedTaskInfo();
                    break;
                case TaskModel.EventWantedCountUpdate:
                    UpdateProcess();
                    break;
                case TaskModel.EventLeftRefreshTimeUpdate:
                    UpdateLeftTime();
                    break;
            }
        }

        /// <summary>
        /// 更新悬赏任务信息
        /// </summary>
        private void UpdateWantedTaskInfo()
        {
            if (_taskModel.WantedTaskList != null)
            {
                for (var i = 0; i < _taskModel.WantedTaskList.Count; i++)
                {
                    var taskId = _taskModel.WantedTaskList[i].id;
                    var taskVo = BaseDataMgr.instance.GetSysTaskVo(taskId);
                    var color = _taskModel.WantedTaskList[i].color;
                    _items[i].SetData(taskVo, color);
                }
            }
            UpdateProcess();
            UpdateLeftTime();
        }

        /// <summary>
        /// 更新悬赏任务进度
        /// </summary>
        private void UpdateProcess()
        {
            var processStr = LanguageManager.GetWord("WantedTaskView.Process");
            _processInfoLabel.text = processStr + " " + _taskModel.LeftWantedTaskCount + "/15";
        }

        /// <summary>
        /// 更新悬赏任务刷新剩余时间
        /// </summary>
        public void UpdateLeftTime()
        {
            ShowLeftRefreshTime();
            vp_Timer.CancelAll("UpdateWantedLeftTime");
            vp_Timer.In(1f, UpdateWantedLeftTime, 100000, 1f);
        }

        /// <summary>
        /// 显示剩余刷新时间
        /// </summary>
        private void ShowLeftRefreshTime()
        {
            int leftTime = 1800 + _taskModel.LeftRefreshTime - ServerTime.Instance.Timestamp;
            int min = leftTime / 60;
            int sec = leftTime % 60;
            var leftTimeStr = LanguageManager.GetWord("WantedTaskView.LeftRefreshTime");
            _leftRefreshTimeLabel.text = leftTimeStr + " "+ min + ":" + sec;
        }

        /// <summary>
        /// 更新剩余时间
        /// </summary>
        private void UpdateWantedLeftTime()
        {
            ShowLeftRefreshTime();
        }

        /// <summary>
        /// 打开面板时刷新悬赏任务信息
        /// </summary>
        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            UpdateWantedTaskInfo();
        }

        /// <summary>
        /// 关闭悬赏任务面板
        /// </summary>
        public override void CloseView()
        {
            base.CloseView();
            vp_Timer.CancelAll("UpdateWantedLeftTime");
        }
    }
}