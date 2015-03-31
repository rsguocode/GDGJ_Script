/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 02:23:34 
 * function: 任务控制器
 * *******************************************************/
using com.game.cmd;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.Guide.GuideLogic;
using com.game.module.main;
using Com.Game.Module.Story;
using com.game.module.test;
using com.game.Public.Message;
using com.game.sound;
using com.net.interfaces;
using Proto;
using UnityEngine;

namespace com.game.module.Task
{
    public class TaskControl : BaseControl<TaskControl>
    {
        private TaskAcceptMsg_6_2 _curTaskAccetpInfo;
        private TaskCommitMsg_6_4 _curTaskCommitInfo;
        private bool _finishTaskStoryPlaying;
        private bool _receiveTaskStoryPlaying;
        private TaskModel _taskMode;

        protected override void NetListener()
        {
            _taskMode = Singleton<TaskModel>.Instance;
            AppNet.main.addCMD(CMD.CMD_6_1, Fun_6_1); //获取任务信息
            AppNet.main.addCMD(CMD.CMD_6_2, Fun_6_2); //接受任务
            AppNet.main.addCMD(CMD.CMD_6_4, Fun_6_4); //提交任务
            AppNet.main.addCMD(CMD.CMD_6_5, Fun_6_5); //任务进度更新
            AppNet.main.addCMD(CMD.CMD_6_14, Fun_6_14); //请求悬赏任务信息返回
            AppNet.main.addCMD(CMD.CMD_6_15, Fun_6_15); //悬赏任务刷新
            AppNet.main.addCMD(CMD.CMD_6_16, Fun_6_16); //悬赏任务推送
            AppNet.main.addCMD(CMD.CMD_6_17, Fun_6_17); //悬赏任务列表推送
            _taskMode.GetTaskInfo();
        }


        /// <summary>
        ///     获取任务信息
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_1(INetData data)
        {
            var taskInfoMsg = new TaskInfoMsg_6_1();
            taskInfoMsg.read(data.GetMemoryStream());
            if (taskInfoMsg.code != 0)
            {
                ErrorCodeManager.ShowError(taskInfoMsg.code);
                return;
            }
            _taskMode.NextMainTaskId = taskInfoMsg.nextId;
            _taskMode.TaskDoing = taskInfoMsg.taskDoing;
        }

        /// <summary>
        ///     接受任务返回
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_2(INetData data)
        {
            _curTaskAccetpInfo = new TaskAcceptMsg_6_2();
            _curTaskAccetpInfo.read(data.GetMemoryStream());
            if (_curTaskAccetpInfo.code != 0)
            {
                ErrorCodeManager.ShowError(_curTaskAccetpInfo.code);
                return;
            }
            SysTask sysTaskVo = BaseDataMgr.instance.GetSysTaskVo(_curTaskAccetpInfo.taskId);
            if (_curTaskAccetpInfo.taskId == _taskMode.CurrentMainTaskVo.TaskId) //更新主线任务状态
            {
                TaskVo taskVo = _taskMode.CurrentMainTaskVo;
                taskVo.Statu = TaskStatu.StatuAccepted;
                taskVo.CanCommit = _curTaskAccetpInfo.state == 1;
                _taskMode.CurrentMainTaskVo = taskVo;
            }
            else if (sysTaskVo.type == TaskModel.TypeSub)
            {
                if (_taskMode.CurrentSubTaskVo == null)
                {
                    var taskVo = new TaskVo();
                    taskVo.TaskId = _curTaskAccetpInfo.taskId;
                    taskVo.SysTaskVo = sysTaskVo;
                    taskVo.Statu = TaskStatu.StatuAccepted;
                    taskVo.CanCommit = _curTaskAccetpInfo.state == 1;
                    _taskMode.CurrentSubTaskVo = taskVo;
                    Singleton<WantedTaskView>.Instance.CloseView();
                }
                else if(_curTaskAccetpInfo.taskId == _taskMode.CurrentSubTaskVo.TaskId)
                {
                    TaskVo taskVo = _taskMode.CurrentSubTaskVo;
                    taskVo.Statu = TaskStatu.StatuAccepted;
                    taskVo.CanCommit = _curTaskAccetpInfo.state == 1;
                    _taskMode.CurrentSubTaskVo = taskVo;
                }
            }
            Vector3 position = AppMap.Instance.me.GoBase.transform.position;
            position.y = position.y + 2.5f;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_TaskReceive, position);
            //播放接受任务音效
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ReceieTask);

            if (StoryControl.Instance.PlayReceiveTaskStory(_curTaskAccetpInfo.taskId, ReceiveTaskStoryCallback))
            {
                _receiveTaskStoryPlaying = true;
                Singleton<MainView>.Instance.CloseView();
            }
            else
            {
                GuideManager.Instance.TriggerGuide(_curTaskAccetpInfo.taskId, 1); //检查是否会触发指引
            }
        }

        private void ReceiveTaskStoryCallback()
        {
            if (_receiveTaskStoryPlaying)
            {
                _receiveTaskStoryPlaying = false;
                ShowMainView();
                GuideManager.Instance.TriggerGuide(_curTaskAccetpInfo.taskId, 1); //检查是否会触发指引
            }
        }

        private void FinishTaskStoryCallback()
        {
            if (_finishTaskStoryPlaying)
            {
                _finishTaskStoryPlaying = false;
                ShowMainView();
                GuideManager.Instance.TriggerGuide(_curTaskCommitInfo.taskId, 2); //检查是否会触发指引
            }
        }

        private void ShowMainView()
        {
            Singleton<MainView>.Instance.OpenView();
        }

        /// <summary>
        ///     完成任务请求返回
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_4(INetData data)
        {
            _curTaskCommitInfo = new TaskCommitMsg_6_4();
            _curTaskCommitInfo.read(data.GetMemoryStream());
            if (_curTaskCommitInfo.code != 0)
            {
                ErrorCodeManager.ShowError(_curTaskCommitInfo.code);
                return;
            }
            if (_curTaskCommitInfo.taskId == _taskMode.CurrentMainTaskVo.TaskId)
            {
                SysTask sysTaskVo = BaseDataMgr.instance.GetSysTaskVo(_curTaskCommitInfo.nextId);
                var taskVo = new TaskVo();
                taskVo.TaskId = _curTaskCommitInfo.nextId;
                taskVo.SysTaskVo = sysTaskVo;
                taskVo.Statu = TaskStatu.StatuUnAccept;
                _taskMode.CurrentMainTaskVo = taskVo;
            }
            else if (_taskMode.CurrentSubTaskVo!=null&&_curTaskCommitInfo.taskId == _taskMode.CurrentSubTaskVo.TaskId)
            {
                _taskMode.CurrentSubTaskVo = null;
            }
            Vector3 position = AppMap.Instance.me.GoBase.transform.position;
            position.y = position.y + 2.5f;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_TaskFinish, position);
            //播放完成任务音效
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_FinishTask);

            if (StoryControl.Instance.PlayFinishTaskStory(_curTaskCommitInfo.taskId, FinishTaskStoryCallback))
            {
                _finishTaskStoryPlaying = true;
                Singleton<MainView>.Instance.CloseView();
            }
            else
            {
                GuideManager.Instance.TriggerGuide(_curTaskCommitInfo.taskId, 2); //检查是否会触发指引
            }
        }

        /// <summary>
        ///     任务进度更新
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_5(INetData data)
        {
            var taskTrackInfo = new TaskTrackInfoMsg_6_5();
            taskTrackInfo.read(data.GetMemoryStream());
            if (taskTrackInfo.taskId == _taskMode.CurrentMainTaskVo.TaskId)
            {
                TaskVo taskVo = _taskMode.CurrentMainTaskVo;
                taskVo.CanCommit = true;
                _taskMode.CurrentMainTaskVo = taskVo;
            }
            else if (taskTrackInfo.taskId == _taskMode.CurrentSubTaskVo.TaskId)
            {
                TaskVo taskVo = _taskMode.CurrentSubTaskVo;
                taskVo.CanCommit = true;
                _taskMode.CurrentSubTaskVo = taskVo;
            }
        }

        /// <summary>
        ///     悬赏任务详细信息
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_14(INetData data)
        {
            var wantedTaskMsg = new TaskRecruitMsg_6_14();
            wantedTaskMsg.read(data.GetMemoryStream());
            if (wantedTaskMsg.code != 0)
            {
                ErrorCodeManager.ShowError(wantedTaskMsg.code);
            }
            else
            {
                _taskMode.LeftWantedTaskCount = wantedTaskMsg.count;
                _taskMode.WantedTaskList = wantedTaskMsg.tasklist;
                _taskMode.LeftRefreshTime = (int) wantedTaskMsg.time;
            }
        }

        /// <summary>
        ///     刷新悬赏任务返回
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_15(INetData data)
        {
            var wantedTaskInfo = new TaskRecruitRefreshMsg_6_15();
            wantedTaskInfo.read(data.GetMemoryStream());
            if (wantedTaskInfo.code != 0)
            {
                ErrorCodeManager.ShowError(wantedTaskInfo.code);
            }
            else
            {
                _taskMode.WantedTaskList = wantedTaskInfo.tasklist;
            }
        }

        /// <summary>
        ///     玩家接受悬赏任务成功时服务端悬赏任务推送
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_16(INetData data)
        {
            var wantedTaskPushInfo = new TaskRecruitPushMsg_6_16();
            wantedTaskPushInfo.read(data.GetMemoryStream());
            if (wantedTaskPushInfo.code != 0)
            {
                ErrorCodeManager.ShowError(wantedTaskPushInfo.code);
            }
            else
            {
                _taskMode.LeftWantedTaskCount = wantedTaskPushInfo.count;
                for (int i = 0; i < _taskMode.WantedTaskList.Count; i++)
                {
                    if (_taskMode.WantedTaskList[i].id == _taskMode.SelectedWantedTaskId)
                    {
                        _taskMode.WantedTaskList[i] = wantedTaskPushInfo.tasklist[0];
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     服务端悬赏任务列表推送
        /// </summary>
        /// <param name="data"></param>
        private void Fun_6_17(INetData data)
        {
            var taskListInfo = new TaskRecruitListPushMsg_6_17();
            taskListInfo.read(data.GetMemoryStream());
            if (taskListInfo.code != 0)
            {
                ErrorCodeManager.ShowError(taskListInfo.code);
            }
            else
            {
                _taskMode.WantedTaskList = taskListInfo.tasklist;
            }
        }
    }
}