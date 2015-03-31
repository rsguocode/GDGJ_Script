using System;
using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.main;
using Com.Game.Module.Manager;
using com.game.module.Task;
using com.game.module.test;
using com.game.Public.Message;
using com.game.vo;
using UnityEngine;
using Com.Game.Module.NPCBust;
using com.game.utils;
using Object = UnityEngine.Object;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 08:23:12 
 * function: Npc对话框
 * *******************************************************/

namespace com.game.module.NpcDialog
{
    public class NpcDialogView : BaseView<NpcDialogView>
    {
        public override string url { get { return "UI/NpcDialog/NpcDialogView.assetbundle"; } }
        public override ViewLayer layerType
        {
            get { return ViewLayer.HighLayer; }
        }

        private const int NpcSpeaker = 1;
        private const int PlayerSpeaker = 2;
        private NpcDialogModel _npcDialogModel;
        //private NpcTaskView _npcTaskView; //Npc任务信息
        //private NpcFunctionView _npcFunctionView; //NPC功能信息
        private GameObject _npcFunctionItem; //FunctionItem模版
        private TaskModel _taskModel;
        private uint _currentNpcId; //当前打开面板的NPCid
        private Button _dialogButton;
        private TaskVo _currentTaskVo;
        private List<string> _wordsList;
        private int _talkIndex;
        private UILabel _dialogPlayerLabel;
		private UILabel _dialogNpcLabel;
        private UILabel _npcNameLabel;
        private UILabel _playerNameLabel;
        private bool _isDialog;  //是否是在对话
        private int _currentSpeak;
        private UISprite _speakBackground;
        private GameObject _npcHead;
        private GameObject _playerHead;
        private bool _hasTask;
        private SysNpcVo _currentSysNpcVo;
        private UISprite _npcSprite;
        private UISprite _playerSprite;
        private bool _isType;  //是否是在打字

		private GameObject roleBust;
		private GameObject npcBust;

		private bool canSwitch = false;

		private UILabel _dialogLabel
		{
			get
			{
				if (_currentSpeak == NpcSpeaker)
				{
					return _dialogNpcLabel;
				}
				else
				{
					return _dialogPlayerLabel;
				}
			}
		}

        protected override void Init()
        {
            _npcDialogModel = Singleton<NpcDialogModel>.Instance;
            _dialogButton = FindChild("Buttom/Button").GetComponent<Button>();
            _dialogButton.onClick += OnDialogButtonClick;
			_dialogPlayerLabel = FindChild("Buttom/Content/leftLabel").GetComponent<UILabel>();
			_dialogNpcLabel = FindChild("Buttom/Content/rightLabel").GetComponent<UILabel>();
            _speakBackground = FindChild("Buttom/Content/Background").GetComponent<UISprite>();
            _npcHead = FindChild("Buttom/Npc");
            _npcNameLabel = FindChild("Buttom/Npc/Name").GetComponent<UILabel>();
            _npcSprite = FindChild("Buttom/Npc/FaceIcon").GetComponent<UISprite>();
            _playerHead = FindChild("Buttom/Player");
			_playerNameLabel = FindChild("Buttom/Player/Name").GetComponent<UILabel>();
			_playerSprite = FindChild("Buttom/Player/FaceIcon").GetComponent<UISprite>();
            /*_npcTaskView = Singleton<NpcTaskView>.Instance;
            _npcTaskView.gameObject = FindChild("NpcTaskView");
            _npcTaskView.transform = _npcTaskView.gameObject.transform;
            _npcTaskView.Init();*/
            /*_npcFunctionView = Singleton<NpcFunctionView>.Instance;
            _npcFunctionView.gameObject = FindChild("FunctionView");
            _npcFunctionView.transform = _npcFunctionView.gameObject.transform;
            _npcFunctionView.Init();*/
            _taskModel = Singleton<TaskModel>.Instance;
            _npcDialogModel = Singleton<NpcDialogModel>.Instance;
            _wordsList = new List<string>();
        }

        /// <summary>
        /// 点击对话框区域处理
        /// </summary>
        /// <param name="button"></param>
        private void OnDialogButtonClick(GameObject button)
        {
            if (_isType)
            {
                return;
            }

			if (!canSwitch)
			{
				return;
			}

            if (_hasTask)
            {
                HandleTask();
            }
            else
            {
                HandleFunction();
            }
        }

        /// <summary>
        /// 处理任务
        /// </summary>
        private void HandleTask()
        {
            if (_talkIndex >= _wordsList.Count)
            {
                EndDialog();
            }
            else
            {
                if (_currentSpeak == PlayerSpeaker)
                {
                    _currentSpeak = NpcSpeaker;
                    _talkIndex += 1;
                }
                else
                {
                    _currentSpeak = PlayerSpeaker;
                }
                ShowRoleDialog();
            }
        }

        private void EndDialog()
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
                        MessageManager.Show(LanguageManager.GetWord("NpcDialogView.NotFinished"));
                    }
                    break;
            }
            CloseView();
            Singleton<MainView>.Instance.OpenView();
        }

        private void HandleFunction()
        {
            CloseView();
            Singleton<MainView>.Instance.OpenView();
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            _currentNpcId = _npcDialogModel.CurrentNpcId;
            _currentSysNpcVo = BaseDataMgr.instance.GetNpcVoById(_currentNpcId);
            ShowRightInfo();
            Singleton<MainView>.Instance.CloseView();
        }

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();

			HideAllBust();
		}

        /// <summary>
        /// 显示NPC对话面板右边的信息
        /// </summary>
        private void ShowRightInfo()
        {
            if (_taskModel.CurrentMainTaskVo!=null&&_taskModel.CurrentMainTaskVo.TargetNpcId == _currentNpcId)
            {
                _npcDialogModel.CurrentTaskVo = _taskModel.CurrentMainTaskVo;
                ShowTaskInfo();
            }
            else if (_taskModel.CurrentSubTaskVo != null && _taskModel.CurrentSubTaskVo.TargetNpcId == _currentNpcId)
            {
                _npcDialogModel.CurrentTaskVo = _taskModel.CurrentSubTaskVo;
                ShowTaskInfo();
            }
            else
            {
                ShowFunctionInfo();
            }
        }

        /// <summary>
        /// 显示任务信息
        /// </summary>
        private void ShowTaskInfo()
        {
            _hasTask = true;
            AnalysWords();
            _currentSpeak = NpcSpeaker;
            ShowRoleDialog();
        }

        /// <summary>
        /// 对话解析
        /// </summary>
        private void AnalysWords()
        {
            _wordsList.Clear();
            _talkIndex = 0;
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
            const char ch = (char) 1;
            string[] result = words.Replace("#n", ch.ToString()).Split(ch);
            foreach (var s in result)
            {
                if (s.Length > 0)
                {
                    _wordsList.Add(s);
                }
            }
        }

		private void HideAllBust()
		{
			if (null != roleBust)
			{
				roleBust.SetActive(false);
			}
			
			if (null != npcBust)
			{
				npcBust.SetActive(false);
			}
		}

        private void ShowRoleDialog()
        {
            if (_talkIndex >= _wordsList.Count)
            {
                EndDialog();
                return;
            }
            var currentWords = _wordsList[_talkIndex];
            const char ch = (char)1;
            currentWords = currentWords.Replace("[playername]", "[00ff00]"+MeVo.instance.Name+"[-]");  //替换玩家名字显示
            var currentList = currentWords.Replace("#r", ch.ToString()).Split(ch);

			HideAllBust();

			canSwitch = false;

            if (_currentSpeak == NpcSpeaker)
            {
                _playerHead.gameObject.SetActive(false);
                _npcHead.gameObject.SetActive(true);

				_dialogPlayerLabel.SetActive(false);
				_dialogNpcLabel.SetActive(true);

                SetDialogTypeWriter(20);
                _dialogLabel.supportEncoding = true;
				_dialogLabel.text = currentList[0];
                var scale = _speakBackground.transform.localScale;
                scale.x = 1;
                _speakBackground.transform.localScale = scale;
                _npcNameLabel.text = _currentSysNpcVo.name;
                _npcSprite.atlas = AtlasManager.Instance.GetAtlas(AtlasManager.Header);
                _npcSprite.spriteName = _currentSysNpcVo.npcId.ToString();
				_npcSprite.SetActive(false);

				NPCBustMgr.Instance.GetBust(UrlUtils.npcBustUrl(_currentSysNpcVo.model.ToString()), NPCBustLoaded);
            }
            else if (_currentSpeak == PlayerSpeaker)
            {
                if (currentList.Length > 1)
                {
                    _npcHead.gameObject.SetActive(false);
                    _playerHead.gameObject.SetActive(true);

					_dialogPlayerLabel.SetActive(true);
					_dialogNpcLabel.SetActive(false);

                    SetDialogTypeWriter(20);
                    _dialogLabel.supportEncoding = true;
					_dialogLabel.text = currentList[1];
                    var scale = _speakBackground.transform.localScale;
                    scale.x = -1;
                    _speakBackground.transform.localScale = scale;
                    _playerNameLabel.text = MeVo.instance.Name;
                    _playerSprite.atlas = AtlasManager.Instance.GetAtlas(AtlasManager.Header);
                    _playerSprite.spriteName = "101";
					_playerSprite.SetActive(false);

					NPCBustMgr.Instance.GetBust(MeVo.instance.BustUrl, RoleBustLoaded);
                }
                else
                {
                    EndDialog();
                }
            }
        }

		private void RoleBustLoaded(GameObject bustObj)
		{
			roleBust = bustObj;
			roleBust.name = "my_bust";
			roleBust.transform.position = _playerSprite.transform.position;	
			MeVo.instance.InitRoleBustPosition(roleBust);
			roleBust.SetActive(true);
			canSwitch = true;
		}

		private void NPCBustLoaded(GameObject bustObj)
		{
			npcBust = bustObj;
			npcBust.name = "npc_" + _currentSysNpcVo.npcId + "_bust";
			npcBust.transform.position = _npcSprite.transform.position;	

			if (NPCBustMgr.SpecialNPCId == _currentSysNpcVo.model.ToString())
			{
				npcBust.transform.localScale = new Vector3(NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor);
			}
			else
			{
				npcBust.transform.localScale = new Vector3(-NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor);
			}

			npcBust.SetActive(true);
			canSwitch = true;
		}

        private void SetDialogTypeWriter(int charsPerSecond)
        {
			var typewriter = _dialogLabel.GetComponent<TypewriterEffect>();
            if (null != typewriter)
            {
                Object.Destroy(typewriter);
            }
//            _isType = true;
//            typewriter = _dialogLabel.gameObject.AddComponent<TypewriterEffect>();
//            typewriter.DoTypeEnd = ResetIsTypeStatu;
//            typewriter.charsPerSecond = charsPerSecond;
        }

        private void ResetIsTypeStatu()
        {
            _isType = false;
        }

        private void ShowTaskStatu()
        {
            var taskReward = LanguageManager.GetWord("NpcDialogView.TaskReward");
            if (_currentTaskVo.SysTaskVo.exp > 0)
            {
                taskReward += LanguageManager.GetWord("NpcDialogView.Exp") + " [00ff00]" + _currentTaskVo.SysTaskVo.exp + "[-]  ";
            }
            if (_currentTaskVo.SysTaskVo.gold > 0)
            {
                taskReward += LanguageManager.GetWord("NpcDialogView.Gold") + " [00ff00]" + _currentTaskVo.SysTaskVo.gold + "[-] ";
            }
            _dialogLabel.supportEncoding = true;
            _dialogLabel.text = taskReward;
        }

        /// <summary>
        /// 显示功能
        /// </summary>
        private void ShowFunctionInfo()
        {
			canSwitch = false;
            _hasTask = false;
			_currentSpeak = NpcSpeaker;
            _playerHead.gameObject.SetActive(false);
            _npcHead.gameObject.SetActive(true);
			_npcSprite.SetActive(false);
			_dialogPlayerLabel.SetActive(false);
			_dialogNpcLabel.SetActive(true);
            var scale = _speakBackground.transform.localScale;
            scale.x = 1;
            _speakBackground.transform.localScale = scale;
            _dialogLabel.supportEncoding = true;
            //_dialogLabel.text = "[00ff00]" + _currentSysNpcVo.name + ":[-]\n" + _currentSysNpcVo.fixspeak;
			_dialogLabel.text = _currentSysNpcVo.fixspeak;
            _npcSprite.atlas = AtlasManager.Instance.GetAtlas(AtlasManager.Header);
            _npcSprite.spriteName = _currentSysNpcVo.npcId.ToString();
            _npcNameLabel.text = _currentSysNpcVo.name;
			NPCBustMgr.Instance.GetBust(UrlUtils.npcBustUrl(_currentSysNpcVo.model.ToString()), NPCBustLoaded);
        }

        private void OpenTaskInfo()
        {
//            _npcFunctionView.CloseView();
//            _npcTaskView.OpenView();
        }

        private void OpenNpcFunctionView()
        {
//            _npcTaskView.CloseView();
//            _npcFunctionView.OpenView();
        }
    }
}