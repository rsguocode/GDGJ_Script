using System.Collections.Generic;
using System.Globalization;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.AutoShieldView;
using Com.Game.Module.Boss;
using Com.Game.Module.Copy;
using Com.Game.Module.GoldBox;
using com.game.module.Guide;
using Com.Game.Module.LuckDraw;
using Com.Game.Module.Manager;
using com.game.module.map;
using com.game.module.Store;
using Com.Game.Module.SystemSetting;
using com.game.module.Task;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.display;
using UnityEngine;

namespace com.game.module.main
{
    public class MainTopRightView : BaseView<MainTopRightView>
    {
        private const int MaxGuideTaskId = 100020; //最大指引的任务ID，大于等于这个ID的不指引
        public bool AutoRoad = false;
        private List<Button> _allButtons; //所有点buttons列表
        private Dictionary<int, Button> _buttonGuideRelation;
        private UISprite _mainFaceSprite;
        private GameObject _mainTask;
        private Button _mainTaskButton;
        private UILabel _mainTaskLabel;
        private List<Button> _openedButtons; //开启了的buttons列表
        //private UISprite _stateSprite; //任务的状态图标—— 感叹号
        private GameObject _subTask;
        private Button _subTaskButton;
        private UILabel _subTaskLabel;
        //private GameObject _tanhao;
        //private TweenScale _tanhaoPlay;
        private GameObject _taskGuide;
        private TaskModel _taskMode;
        public Button btn_hjbx; //黄金宝箱
        public Button btn_mcxl; //萌宠献礼
        public Button btn_sc; //商城
        public Button btn_sjb; //世界BOSS


        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        protected override void Init()
        {
            btn_sc = FindInChild<Button>("btn_sc");
            btn_mcxl = FindInChild<Button>("btn_mcxl");
            btn_hjbx = FindInChild<Button>("btn_hjbx");
            btn_sjb = FindInChild<Button>("btn_sjb");

            btn_sc.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.Store");
            btn_mcxl.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.PetPresent");
            btn_hjbx.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.GoldBox");
            btn_sjb.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.WorldBoss");
            btn_sjb.FindInChild<UILabel>("label").pivot = UIWidget.Pivot.Top;

            btn_sc.onClick = StoreOnClick;
            btn_mcxl.onClick = MengChongXianliOnClick;
            btn_hjbx.onClick = GoldBoxBtnClick;
            btn_sjb.onClick = WorldBossClick;
            /*_stateSprite = FindInChild<UISprite>("MainTask/Statu");
            _stateSprite.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
            _stateSprite.spriteName = "zhuizong_tanhao";*/
            _taskMode = Singleton<TaskModel>.Instance;
            _mainTaskLabel = FindChild("MainTask/Label").GetComponent<UILabel>();
            _subTaskLabel = FindChild("SubTask/Label").GetComponent<UILabel>();
            _mainTaskButton = FindChild("MainTask/Button").GetComponent<Button>();
            _subTaskButton = FindChild("SubTask/Button").GetComponent<Button>();
            _mainFaceSprite = FindChild("MainTask/NpcFace").GetComponent<UISprite>();
            _taskGuide = FindChild("MainTask/TaskGuide");
            //_tanhao = FindChild("MainTask/Statu");
            //_tanhao.transform.rotation = new Quaternion(0, 0, 0, 0);
            //_tanhaoPlay = _tanhao.GetComponent<TweenScale>();
            _mainTaskButton.onClick += OnMainTaskButtonClick;
            _subTaskButton.onClick += OnSubTaskButtonClick;
            _mainTask = FindChild("MainTask");
            _subTask = FindChild("SubTask");
            InitTaskInfo();
            showTween = FindInChild<TweenPlay>();
            InitButtonOpen();

			LuckDrawMode.Instance.StartTips();
        }

        private void InitButtonOpen()
        {
            _openedButtons = new List<Button>();
            _allButtons = new List<Button> {btn_sc, btn_mcxl, btn_hjbx, btn_sjb};
            RegisterButtonInGuide();
            UpdateButtonOpenStatu();
        }


        private void RegisterButtonInGuide()
        {
            _buttonGuideRelation = new Dictionary<int, Button>();
            _buttonGuideRelation.Add(GuideType.GuideShopOpen, btn_sc); //注册商城按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideLuckDraw, btn_mcxl); //注册萌宠献礼按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideGoldBoxOpen, btn_hjbx); //注册黄金宝箱按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideWorldBoss, btn_sjb); //注册世界BOSS按钮和指引的关系
        }

        public void UpdateButtonOpenStatu()
        {
            CalculateOpenedButton();
            SetButtonPosition();
        }

        /// <summary>
        ///     计算图标开启情况
        /// </summary>
        private void CalculateOpenedButton()
        {
            _openedButtons.Clear();
            if (SystemSettingMode.Instance.ShowButton)
            {
                _openedButtons.AddRange(_allButtons);
            }
            else
            {
				GameObject loginObj = GameObject.Find("driver/uiroot(Clone)/viewtree/LoginView(Clone)");
				if (loginObj)
					loginObj.SetActive(false);
				/*
                TaskVo mainTaskVo = TaskModel.Instance.CurrentMainTaskVo;
                int mainTaskNum = (int) mainTaskVo.TaskId*10 + TaskUtil.GetTaskTriggerType(mainTaskVo);
                List<SysGuideVo> guideVoList = BaseDataMgr.instance.GetGuideVoList();
                foreach (SysGuideVo sysGuideVo in guideVoList)
                {
                    //guide_type 1-3为功能开启指引
                    if (sysGuideVo.guide_type > 3 || sysGuideVo.guide_type < 1)
                    {
                        continue;
                    }
                    int guideTaskId = int.Parse(StringUtils.GetValueString(sysGuideVo.condition));
                    int guideTaskNum = sysGuideVo.trigger_type + guideTaskId*10;
                    if (mainTaskNum >= guideTaskNum)
                    {
                        int guideId = sysGuideVo.guideID;
                        if (_buttonGuideRelation.ContainsKey(guideId))
                        {
                            _openedButtons.Add(_buttonGuideRelation[guideId]);
                        }
                    }
                }*/
            }
        }

        /// <summary>
        ///     根据图标开启的情况设置图标的相对位置
        /// </summary>
        private void SetButtonPosition()
        {
            int index = 0;
            foreach (Button button in _allButtons)
            {
                if (_openedButtons.Contains(button))
                {
                    button.SetActive(true);
                    button.transform.localPosition = new Vector3(-350 + 100*index, 92, 0);
                    index++;
                }
                else
                {
                    button.SetActive(false);
                }
            }
        }

        //商城
        private void StoreOnClick(GameObject go)
        {
            Singleton<StoreShopView>.Instance.OpenView();
        }

        //萌宠献礼
        private void MengChongXianliOnClick(GameObject go)
        {
            Singleton<LuckDrawView>.Instance.OpenView();
        }


        /// <summary>
        ///     黄金宝箱按钮
        /// </summary>
        /// <param name="go"></param>
        private void GoldBoxBtnClick(GameObject go)
        {
            Singleton<GoldBoxView>.Instance.OpenView();
        }

        //世界Boss
        private void WorldBossClick(GameObject go)
        {
            Singleton<BossControl>.Instance.EnterBossScene();
        }

        /// <summary>
        ///     主线任务引导
        /// </summary>
        /// <param name="button"></param>
        private void OnMainTaskButtonClick(GameObject button)
        {
            if (GuideModel.Instance.IsShowGuide)
            {
                return; //触发了指引则不响应任务引导
            }
            int needLevel = StringUtils.GetStringToInt(_taskMode.CurrentMainTaskVo.SysTaskVo.level)[0];
            if (needLevel > MeVo.instance.Level)
            {
                MessageManager.Show("请升级到" + needLevel + "级再来接取");
                return;
            }
            string trace = TaskUtil.GetTraceInfo(_taskMode.CurrentMainTaskVo);
            string[] items = trace.Split('.');
            uint taskTargetMapId = uint.Parse(items[1]);
            if (taskTargetMapId == AppMap.Instance.mapParser.MapId)
            {
                MapMode.Instance.NeedGuideMainTask = false;
                BaseDisplay display = TaskUtil.GetTaskDisplay(trace);
                MoveTo(display);
            }
            else
            {
                MapMode.Instance.NeedGuideMainTask = true;
                CopyControl.Instance.AutoChangeWorld(taskTargetMapId);
            }
            vp_Timer.CancelAll("ShowTaskGuide");
            _taskGuide.SetActive(false);
        }

        /// <summary>
        ///     支线任务引导
        /// </summary>
        /// <param name="button"></param>
        private void OnSubTaskButtonClick(GameObject button)
        {
            string trace = TaskUtil.GetTraceInfo(_taskMode.CurrentSubTaskVo);
            BaseDisplay display = TaskUtil.GetTaskDisplay(trace);
            MoveTo(display);
        }

        /// <summary>
        ///     获取任务信息
        /// </summary>
        private void InitTaskInfo()
        {
            TaskVo mainTaskVo = _taskMode.CurrentMainTaskVo;
            if (mainTaskVo != null)
            {
                _mainTask.SetActive(true);
                _mainTaskLabel.text = LanguageManager.GetWord("MainLeftView.mainTask") + GetTaskDescribe(mainTaskVo);
                _mainFaceSprite.atlas = AtlasManager.Instance.GetAtlas(AtlasManager.Header);
                _mainFaceSprite.spriteName = _taskMode.CurrentMainTaskVo.TaskIcon.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                _mainTask.SetActive(false);
            }
            TaskVo subTaskVo = _taskMode.CurrentSubTaskVo;
            if (subTaskVo != null)
            {
                _subTask.SetActive(true);
                _subTaskLabel.text = LanguageManager.GetWord("MainLeftView.subTask") + GetTaskDescribe(subTaskVo);
            }
            else
            {
                _subTask.SetActive(false);
            }
        }

        //提交任务叹号提示
        public void ShakeTanhao()
        {
            /*TaskVo taskvo = TaskModel.Instance.CurrentMainTaskVo;
            if (taskvo.Statu == TaskStatu.StatuAccepted && taskvo.CanCommit)
            {
                if (_tanhaoPlay)
                {
                    _tanhaoPlay.enabled = true;
                    vp_Timer.In(6f, EndPlay);
                }
            }*/
            UpdateTaskGuideStatu();
        }

        public void EndPlay()
        {
            //_tanhaoPlay.enabled = false;
        }

        /// <summary>
        ///     解析任务描述
        /// </summary>
        /// <param name="taskVo"></param>
        /// <returns></returns>
        private string GetTaskDescribe(TaskVo taskVo)
        {
            return TaskUtil.GetTaskDescribe(taskVo);
        }


        /// <summary>
        ///     任务寻路处理
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        private void MoveTo(BaseDisplay display)
        {
            if (display == null) return;
            if (display.GoBase == null) return;
            AutoShieldView.Instance.OpenView();
            Transform target = display.GoBase.transform;
            AppMap.Instance.clickVo.SaveClicker(display);
            AppMap.Instance.me.Controller.MoveTo(target.position.x, target.position.y);
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            InitTaskInfo();
            if (MapMode.Instance.NeedGuideMainTask)
            {
                vp_Timer.In(1, AutoGuideMainTask);
            }
            UpdateTaskGuideStatu();
            btn_sjb.FindInChild<UISprite>("tips").SetActive(BossMode.Instance.ShowTips);
        }


        /// <summary>
        ///     注册UI响应事件
        /// </summary>
        public override void RegisterUpdateHandler()
        {
            Singleton<TaskModel>.Instance.dataUpdated += TaskDataUpdate;
            Singleton<SystemSettingMode>.Instance.dataUpdated += SystemSettingUpdate;
            Singleton<GuideModel>.Instance.dataUpdated += GuideDataUpdate;
			LuckDrawMode.Instance.dataUpdated += ButtonTipsHandle;
            BossMode.Instance.dataUpdated += ButtonTipsHandle;
        }


        public override void CancelUpdateHandler()
        {
            Singleton<TaskModel>.Instance.dataUpdated -= TaskDataUpdate;
            Singleton<SystemSettingMode>.Instance.dataUpdated -= SystemSettingUpdate;
            Singleton<GuideModel>.Instance.dataUpdated -= GuideDataUpdate;
			LuckDrawMode.Instance.dataUpdated -= ButtonTipsHandle;
            BossMode.Instance.dataUpdated -= ButtonTipsHandle;
        }

        /// <summary>
        ///     响应任务数据中心的数据改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void TaskDataUpdate(object sender, int code)
        {
            switch (code)
            {
                case TaskModel.EventMainTaskIdUpdate:
                    InitTaskInfo();
                    break;
                case TaskModel.EventSubTaskIdUpdate:
                    InitTaskInfo();
                    break;
                case TaskModel.EventMainTaskStatuUpdate: //任务状态改变
                {
                    //_tanhaoPlay.enabled = false;
                    ShakeTanhao();
                }
                    break;
                case TaskModel.EventMainTaskAutoGuide:
                    OnMainTaskButtonClick(null);
                    break;
            }
        }

        /// <summary>
        ///     响应引导数据中心的数据改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void GuideDataUpdate(object sender, int code)
        {
            switch (code)
            {
                case GuideModel.ShowGuideStatuUpdate:
                    UpdateTaskGuideStatu();
                    break;
            }
        }

        /// <summary>
        ///     更新指引状态
        /// </summary>
        private void UpdateTaskGuideStatu()
        {
			/* grsyh
            TaskVo taskvo = TaskModel.Instance.CurrentMainTaskVo;
            vp_Timer.CancelAll("ShowTaskGuide");
            if (GuideModel.Instance.IsShowGuide)
            {
                _taskGuide.SetActive(false);
            }
            else
            {
                if (taskvo.TaskId < MaxGuideTaskId || taskvo.Statu == TaskStatu.StatuAccepted)
                {
                    vp_Timer.In(0.5f, ShowTaskGuide);
                }
            }
*/
        }

        private void ShowTaskGuide()
        {
            _taskGuide.SetActive(true);
        }

        private void SystemSettingUpdate(object sender, int code)
        {
            switch (code)
            {
                case SystemSettingMode.ShowButtonUpdate:
                    UpdateButtonOpenStatu();
                    break;
            }
        }

        private void AutoGuideMainTask()
        {
            OnMainTaskButtonClick(null);
        }

		private void ButtonTipsHandle(object sender,int code)
		{
			//萌宠献礼
			if (sender.Equals(LuckDrawMode.Instance) && code == LuckDrawMode.Instance.UPDATE_TIPS)
			{
				btn_mcxl.FindInChild<UISprite>("tips").SetActive(LuckDrawMode.Instance.ShowTips);
			}
            //世界Boss
            else if (sender.Equals(BossMode.Instance) && code == BossMode.Instance.UPDATE_TIPS)
            {
                btn_sjb.FindInChild<UISprite>("tips").SetActive(BossMode.Instance.ShowTips);
            }
            //世界Boss开启时间倒计时
            else if (sender.Equals(BossMode.Instance) && code == BossMode.Instance.UPDATE_START)
            {
                float restTimeLeft = BossMode.Instance.RestTimeLeft;
                if (restTimeLeft == 0)
                    btn_sjb.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.WorldBoss");
                else
                {
                    vp_TimeUtility.Units timeU = vp_TimeUtility.TimeToUnits(restTimeLeft);
                    string timeStr = string.Format("{0:D2}:{1:D2}", timeU.minutes, timeU.seconds);
                    btn_sjb.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainTopRightView.WorldBoss")
                        + "\n"+ string.Format(ColorConst.RED_FORMAT,timeStr);
                
                }

            }
		}
    }
}