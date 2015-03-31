using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Assets.Scripts.com.game.consts;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Arena;
using Com.Game.Module.Chat;
using Com.Game.Module.Copy;
using com.game.module.effect;
using com.game.module.fight.vo;
using com.game.module.GameSetting;
using Com.Game.Module.GoldHit;
using com.game.module.main;
using Com.Game.Module.Manager;
using com.game.module.map;
using Com.Game.Module.Role;
using Com.Game.Module.Story;
using com.game.module.SystemData;
using com.game.module.test;
using Com.Game.Speech;
using com.game.ui;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.debug;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/24 02:41:53 
 * function: 战斗控制UI
 * *******************************************************/

namespace com.game.module.battle
{
    public class BattleView : BaseView<BattleView>
    {
        private bool[] _isIncd = new bool[5];
        private bool[] _canUse = new bool[5];
        public UISprite BtnChatBg;
        private UISprite _arrowLeft; //向左的箭头提示
        private UISprite _arrowRight; //向右的箭头提示
        private GameObject _autoSystemEffect; //自动挂机特效
        private bool _hasLoadAutoSystemEffect;
        private BattleMode _battleMode;
        private Button _btnAttack;
        private Button _btnChat;
        private Button _btnSkill0;
        private Button _btnSkill1;
        private Button _btnSkill2;
        private Button _btnSkill3;
        private Button _btnSkill4;
        private Button _btnStop;
        private GameObject _currentGuideSkillBtn;
        private Button _entrustBtn;
        private GameObject _guideJoy;
        private GameObject _guideSkill;
        private int _guideSkillIndex;
        private UILabel _healthLabel;
        private bool _heroHpChangeStoryPlaying;
        private bool _isAttack; //是否按住了攻击键
        private UISprite _joystickHighLight;
        private float _lastAttackCommandTime; //上一次攻击指令的时间
        private UILabel _leftNum; //左边剩余怪物
        private int _leftSecond; //剩余时间
        private UILabel _leftTimeLabel;
        private UILabel _magicLabel;
        private MeAiController _meAiController;
        private bool _monsterHpChangeStoryPlaying;
        private int _nLeftNum;
        private int _nRightNum;
        private NGUIJoystick _nguiJoystick;
        private UISprite _normalAttackIcon;
        private UISprite _playerHeadSprite;
        private UILabel _playerLevelLabel;
        private UILabel _playerNameLabel;
        private UILabel _rightNum; //右边剩余怪物
        private GameObject _skill0Cd;
        private UILabel _skill0CdTimeLabel;
        private UISprite _skill0Icon;
        private GameObject _skill1Cd;
        private UILabel _skill1CdTimeLabel;
        private UISprite _skill1Icon;
        private GameObject _skill2Cd;
        private UILabel _skill2CdTimeLabel;
        private UISprite _skill2Icon;
        private GameObject _skill3Cd;
        private UILabel _skill3CdTimeLabel;
        private UISprite _skill3Icon;
        private GameObject _skill4Cd;
        private UILabel _skill4CdTimeLabel;
        private UISprite _skill4Icon;
        private GameObject[] _skillCdGameObjects;
        private Button[] _skillButtons;
        private UILabel[] _skillCdTimeLabels;
        private UISprite[] _skillIcons;
        private UIBloodBar _sldBossHp;
        private UISlider _sldHp;
        private UISlider _sldMagic;
        private UIBloodBar _sldMonsterHp;
        private Transform _time;
        private GameObject _topGameObject;
        private GameObject _topLeftGameObject;
        private GameObject _topRightGameObject;
        private UIAtlas _skillAtlas; //技能图集
        private UISprite _monsterIcon;

        public override string url
        {
            get { return "UI/Battle/BattleView.assetbundle"; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        public override ViewLayer layerType
        {
            get { return ViewLayer.LowLayer; }
        }

        public override bool waiting
        {
            get { return false; }
        }

        protected override void Init()
        {
            //这里是 竞技场 View 
            ArenaFightView.Instance.gameObject = FindChild("ArenaFightView");
            ArenaFightView.Instance.gameObject.SetActive(false);

            _topLeftGameObject = Tools.find(gameObject, "TopLeft");
            _topRightGameObject = Tools.find(gameObject, "TopRight");
            _topGameObject = Tools.find(gameObject, "Top");
            _sldBossHp = Tools.find(gameObject, "TopRight/SldBossHp").GetComponent<UIBloodBar>();
            _sldBossHp.fillDirection = UIBloodBar.FillDirection.RightToLeft;
            _sldMonsterHp = Tools.find(gameObject, "TopRight/SldMonsterHp").GetComponent<UIBloodBar>();
            _sldMonsterHp.fillDirection = UIBloodBar.FillDirection.RightToLeft;
            _playerLevelLabel = Tools.find(gameObject, "TopLeft/PlayerInfo/PlayerLevelLabel").GetComponent<UILabel>();
            _playerNameLabel = Tools.find(gameObject, "TopLeft/PlayerInfo/PlayerNameLabel").GetComponent<UILabel>();
            _playerHeadSprite = Tools.find(gameObject, "TopLeft/PlayerInfo/PlayerHeadIcon").GetComponent<UISprite>();
            _sldHp = Tools.find(gameObject, "TopLeft/SldHp").GetComponent<UISlider>();
            _sldMagic = Tools.find(gameObject, "TopLeft/SldMagic").GetComponent<UISlider>();
            _btnStop = Tools.find(gameObject, "TopLeft/BtnStop").GetComponent<Button>();
            _entrustBtn = Tools.find(gameObject, "TopLeft/BtnEntrust").GetComponent<Button>();
            _arrowLeft = Tools.find(gameObject, "Left").GetComponent<UISprite>();
            _arrowRight = Tools.find(gameObject, "Right").GetComponent<UISprite>();
            _leftNum = Tools.find(gameObject, "Left/num").GetComponent<UILabel>();
            _rightNum = Tools.find(gameObject, "Right/num").GetComponent<UILabel>();

            _btnSkill0 = Tools.find(gameObject, "BottomRight/BtnSkill0").GetComponent<Button>();
            _btnSkill1 = Tools.find(gameObject, "BottomRight/BtnSkill1").GetComponent<Button>();
            _btnSkill2 = Tools.find(gameObject, "BottomRight/BtnSkill2").GetComponent<Button>();
            _btnSkill3 = Tools.find(gameObject, "BottomRight/BtnSkill3").GetComponent<Button>();
            _btnSkill4 = Tools.find(gameObject, "BottomRight/BtnSkill4").GetComponent<Button>();
            _btnAttack = Tools.find(gameObject, "BottomRight/BtnAttack").GetComponent<Button>();
            _btnChat = Tools.find(gameObject, "BottomLeft/BtnChat").GetComponent<Button>();
            BtnChatBg = Tools.find(gameObject, "BottomLeft/BtnChat/background").GetComponent<UISprite>();
            _healthLabel = Tools.find(gameObject, "TopLeft/SldHp/Label").GetComponent<UILabel>();
            _magicLabel = Tools.find(gameObject, "TopLeft/SldMagic/Label").GetComponent<UILabel>();

            _skill0Cd = Tools.find(gameObject, "BottomRight/BtnSkill0/CD");
            _skill1Cd = Tools.find(gameObject, "BottomRight/BtnSkill1/CD");
            _skill2Cd = Tools.find(gameObject, "BottomRight/BtnSkill2/CD");
            _skill3Cd = Tools.find(gameObject, "BottomRight/BtnSkill3/CD");
            _skill4Cd = Tools.find(gameObject, "BottomRight/BtnSkill4/CD");

            _skill0CdTimeLabel = Tools.find(gameObject, "BottomRight/BtnSkill0/Number").GetComponent<UILabel>();
            _skill1CdTimeLabel = Tools.find(gameObject, "BottomRight/BtnSkill1/Number").GetComponent<UILabel>();
            _skill2CdTimeLabel = Tools.find(gameObject, "BottomRight/BtnSkill2/Number").GetComponent<UILabel>();
            _skill3CdTimeLabel = Tools.find(gameObject, "BottomRight/BtnSkill3/Number").GetComponent<UILabel>();
            _skill4CdTimeLabel = Tools.find(gameObject, "BottomRight/BtnSkill4/Number").GetComponent<UILabel>();

            _skill0Icon = Tools.find(gameObject, "BottomRight/BtnSkill0/Icon").GetComponent<UISprite>();
            _skill1Icon = Tools.find(gameObject, "BottomRight/BtnSkill1/Icon").GetComponent<UISprite>();
            _skill2Icon = Tools.find(gameObject, "BottomRight/BtnSkill2/Icon").GetComponent<UISprite>();
            _skill3Icon = Tools.find(gameObject, "BottomRight/BtnSkill3/Icon").GetComponent<UISprite>();
            _skill4Icon = Tools.find(gameObject, "BottomRight/BtnSkill4/Icon").GetComponent<UISprite>();
            _normalAttackIcon = Tools.find(gameObject, "BottomRight/BtnAttack/Icon").GetComponent<UISprite>();

            _leftTimeLabel = Tools.find(gameObject, "Top/LeftTime/LeftTimeLabel").GetComponent<UILabel>(); //剩余时间

            _time = Tools.find(gameObject, "Top/LeftTime").GetComponent<Transform>();
            _nguiJoystick = Tools.find(gameObject, "BottomLeft/Joystick/Button").AddComponent<NGUIJoystick>();
            _nguiJoystick.radius = 80;
            _nguiJoystick.IsBattleJoystick = true;
            _joystickHighLight = Tools.find(gameObject, "BottomLeft/Joystick/Background").GetComponent<UISprite>();
            _btnChat.onClick += OnChatBtnClick;
            _btnSkill0.onClick += CallSkill0;
            _btnSkill1.onClick += CallSkill1;
            _btnSkill2.onClick += CallSkill2;
            _btnSkill3.onClick += CallSkill3;
            _btnSkill4.onClick += CallSkill4;
            _entrustBtn.onClick += CallEntrust;
            _btnAttack.onPress += CallAttack;
            _btnStop.onClick += StopGame;
            UpdateInfo();
            InitPlayerInfo();
            _sldMonsterHp.gameObject.SetActive(false);
            InitSkillIcon();
            _skillCdGameObjects = new[] {_skill1Cd, _skill2Cd, _skill3Cd, _skill4Cd, _skill0Cd};
            _skillButtons = new[] {_btnSkill1, _btnSkill2, _btnSkill3, _btnSkill4,_btnSkill0};
            _battleMode = Singleton<BattleMode>.Instance;
            _skillCdTimeLabels = new[]
            {_skill1CdTimeLabel, _skill2CdTimeLabel, _skill3CdTimeLabel, _skill4CdTimeLabel, _skill0CdTimeLabel};
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                _btnChat.SetActive(false);
            }
            _guideJoy = Tools.find(gameObject, "BottomLeft/GuideJoy");
            _guideSkill = Tools.find(gameObject, "BottomRight/GuideSkill");
        }

        private void InitSkillIcon()
        {
            _skillIcons = new[] {_skill0Icon, _skill1Icon, _skill2Icon, _skill3Icon, _skill4Icon};
            for (int i = 0; i < _skillIcons.Length; i++)
            {
                string name = MeVo.instance.job + "0" + i;
                _skillIcons[i].spriteName = name;
            }
            _normalAttackIcon.spriteName = MeVo.instance.job + "0";
        }


        private void InitPlayerInfo()
        {
            _playerLevelLabel.text = MeVo.instance.Level + "";
            _playerNameLabel.text = MeVo.instance.Name + "";
            _playerHeadSprite.spriteName = MeVo.instance.job + "001";
            _playerHeadSprite.MakePixelPerfect();
        }

        public override void RegisterUpdateHandler()
        {
            MeVo.instance.DataUpdated += UpdateHpMpHandler;
            MeVo.instance.DataUpdatedWithParam += UpdateMonsterHpHandlerWithParam;
            Singleton<MapMode>.Instance.dataUpdated += UpdateLeftTimeHandler;
            Singleton<BattleMode>.Instance.dataUpdated += UpdateBattleMode;
            Singleton<MonsterMgr>.Instance.dataUpdated += UpdateBossInfo;
            if (MeVo.instance.mapId == MapTypeConst.FirstCopy) 
            {
                Singleton<SkillMode>.Instance.dataUpdated += UpdateSkillPos;
            }
        }

        public override void CancelUpdateHandler()
        {
            MeVo.instance.DataUpdated -= UpdateHpMpHandler;
            MeVo.instance.DataUpdatedWithParam -= UpdateMonsterHpHandlerWithParam;
            Singleton<MapMode>.Instance.dataUpdated -= UpdateLeftTimeHandler;
            Singleton<BattleMode>.Instance.dataUpdated -= UpdateBattleMode;
            if (MeVo.instance.mapId == MapTypeConst.FirstCopy) 
            {
                Singleton<SkillMode>.Instance.dataUpdated -= UpdateSkillPos;
            }
        }

        /// <summary>
        ///     新消息提示显示
        /// </summary>
        /// <param name="liaotianBg1">聊天图标</param>
        /// <param name="alarm">true，有提示。false，恢复正常</param>
        public void NewMsgAlarm(UISprite liaotianBg1, bool alarm)
        {
            if (alarm)
            {
                Log.debug(this, "should highlight");
                liaotianBg1.spriteName = "btn_lt_highlight";
                liaotianBg1.GetComponent<Animator>().enabled = true;
            }
            else
            {
                Log.debug(this, "should not highlight");
                liaotianBg1.spriteName = "btn_lt_background";
                liaotianBg1.GetComponent<Animator>().enabled = false;
                liaotianBg1.transform.localScale = Vector3.one;
            }
        }

        private void OnChatBtnClick(GameObject go)
        {
            Singleton<ChatView>.Instance.OpenView();
            Singleton<MainBottomLeftView>.Instance.NewMsgAlarm(BtnChatBg, false);
        }

        private void UpdateBossInfo(object sender, int code)
        {
            if (code == MonsterMgr.EventBossUpdate)
            {
                //注释掉该需求
                //UpdateMonsterHpInfo();
            }
        }


        private void UpdateMonsterHpInfo()
        {
            MonsterVo boss = MonsterMgr.Instance.BossVo;
            if (boss == null || boss.CurHp == 0)
            {
                _sldBossHp.gameObject.SetActive(false);
                _sldMonsterHp.gameObject.SetActive(false);
            }
            else
            {
                int count = boss.MonsterVO.hp_count;
                int perValue = Mathf.CeilToInt((float) boss.Hp/count);
                int leftCount = (int) boss.CurHp/perValue;
                var left = (int) (boss.CurHp%perValue);
                var last = (int) (boss.lastHp%perValue);
                if (left != 0)
                {
                    leftCount = leftCount + 1;
                }
                _sldBossHp.gameObject.SetActive(true);
                string hpStr = boss.CurHp + "/" + boss.Hp;
                float rate = (float) left/perValue;
                float lastRate = (float) last/perValue;
                if (boss.CurHp == boss.Hp)
                {
                    rate = 1;
                    lastRate = 1;
                }
                _sldBossHp.SetValue(boss.MonsterVO.name, hpStr, rate, lastRate, leftCount, boss.Level);
            }
        }

        private void UpdateBattleMode(object sender, int code)
        {
            if (BattleMode.UpdataAutoSystemStatu == code)
            {
                UpdateAutoSystemStatu();
            }
            else if (BattleMode.UpdateTopRightHpBar == code)
            {
                UpdateTopRightHpBar();
            }
            else if (BattleMode.UpdateMonsterDeath == code)
            {
                RemoveMonsterBloodUi();
            }
        }

        private void UpdateSkillPos(object sender, int code) //技能位置变化处理
        {
            if (sender == Singleton<SkillMode>.Instance && code == SkillMode.SkillPos)
            {
                SetSkillIcon();
            }

            if (MeVo.instance.mapId == MapTypeConst.FirstCopy) 
            {
                uint[] skills = Singleton<SkillMode>.Instance.GetUsedSkill();
                if (skills[0] != 0 && skills[1] == 0)
                {
                    var icon = _skillButtons[0].transform.FindChild("Icon");
                    icon.GetComponent<TweenPlay>().PlayReverse(); //第一个技能位置，播放技能动画
                }
            }
        }


        private void RemoveMonsterBloodUi()
        {
            _sldBossHp.gameObject.SetActive(false);
            _sldMonsterHp.gameObject.SetActive(false);
        }

        private void UpdateAutoSystemStatu()
        {
            if (Singleton<BattleMode>.Instance.IsAutoSystem)
            {
                if (_autoSystemEffect == null)
                {
                    if (!_hasLoadAutoSystemEffect)
                    {
                        _hasLoadAutoSystemEffect = true;
                        EffectMgr.Instance.CreateUIEffect(EffectId.UI_AutoBattle, new Vector3(0.5f,0.5f,0), null, true,
                            CreateAutoBattleEffectBack);
                    }
                }
                else
                {
                    _autoSystemEffect.SetActive(true);
                }
                _entrustBtn.highLight.SetActive(true);
            }
            else
            {
                if (_autoSystemEffect != null)
                {
                    _autoSystemEffect.SetActive(false);
                }
                _entrustBtn.highLight.SetActive(false);
            }
        }

        private void CreateAutoBattleEffectBack(GameObject autoBattleEffect)
        {
            _autoSystemEffect = autoBattleEffect;
            if (Singleton<BattleMode>.Instance.IsAutoSystem)
            {
                _autoSystemEffect.SetActive(true);
            }
            else
            {
                _autoSystemEffect.SetActive(false);
            }
        }

        private void UpdateTopRightHpBar()
        {
            if (_battleMode.MonsterType == MonsterType.TypeBoss)
            {
                _sldMonsterHp.gameObject.SetActive(false);
                _sldBossHp.gameObject.SetActive(true);
                _sldBossHp.SetValue(_battleMode.MonsterName, _battleMode.HpString, _battleMode.CurrentHpRate,
                    _battleMode.PreviousHpRate, _battleMode.LeftCount, _battleMode.MonsterLvl);
                _monsterIcon = _sldBossHp.FindInChild<UISprite>("MonsterIcon");
                _monsterIcon.atlas = Singleton<AtlasManager>.Instance.GetAtlas("MonsterHeadAtlas");
                _monsterIcon.spriteName = _battleMode.MonsterIcon + "";
                _monsterIcon.MakePixelPerfect();
                _monsterIcon.transform.localScale = new Vector3(1.3f,1.3f,1);
            }
            else
            {
                _sldBossHp.gameObject.SetActive(false);
                _sldMonsterHp.gameObject.SetActive(true);
                _sldMonsterHp.SetValue(_battleMode.MonsterName, _battleMode.HpString, _battleMode.CurrentHpRate,
                    _battleMode.PreviousHpRate, _battleMode.LeftCount, _battleMode.MonsterLvl);
                _monsterIcon = _sldMonsterHp.FindInChild<UISprite>("MonsterIcon");
                _monsterIcon.atlas = Singleton<AtlasManager>.Instance.GetAtlas("MonsterHeadAtlas");
                _monsterIcon.spriteName = _battleMode.MonsterIcon + "";
                _monsterIcon.MakePixelPerfect();
                _monsterIcon.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            }
        }

        private void UpdateLeftTimeHandler(object sender, int code)
        {
            if (MapMode.EVENT_CODE_UPDATE_LEFTTIME == code)
            {
                UpdateLeftTime();
            }
            else if (MapMode.EVENT_CODE_STOP_LEFTTIME == code)
            {
                StopLeftTime();
            }
        }

        public override void CloseView()
        {
            Singleton<BattleMode>.Instance.dataUpdated -= UpdateBattleMode;
            MeVo.instance.DataUpdated -= UpdateHpMpHandler;
            base.CloseView();
            vp_Timer.CancelAll("UpdateBattleViewLeftTime");
        }

        // 更新副本剩余时间
        public void UpdateLeftTime()
        {
            if (MapMode.expire == 999999)
            {
                _time.gameObject.SetActive(false);
            }
            else
            {
                _time.gameObject.SetActive(true);
                SetLeftTimeValue();
            }
            vp_Timer.CancelAll("UpdateBattleViewLeftTime");
            int seconds = MapMode.EndTimestamp - ServerTime.Instance.Timestamp;
            vp_Timer.In(1f, UpdateBattleViewLeftTime, seconds, 1f);
        }

        public void StopLeftTime()
        {
            vp_Timer.CancelAll("UpdateBattleViewLeftTime");
        }

        /// <summary>
        ///     战斗UI剩余时间更新
        /// </summary>
        private void UpdateBattleViewLeftTime()
        {
            SetLeftTimeValue();
        }

        private void SetLeftTimeValue()
        {
            _leftSecond = MapMode.EndTimestamp - ServerTime.Instance.Timestamp;
            if (_leftSecond < 0) _leftSecond = 0;
            int min = _leftSecond/60;
            string minstr = min < 10 ? "0" + min : min + "";
            int sec = _leftSecond%60;
            string secstr = sec < 10 ? "0" + sec : sec + "";
            _leftTimeLabel.text = minstr + ":" + secstr;
        }

        private void UpdateHpMpHandler(object sender, int code)
        {
            if (MeVo.DataHpMpUpdate == code)
            {
                UpdateInfo();
            }

            if (MeVo.DataHpUpdate == code)
            {
                if (MapControl.CanPlayHeroHpChangeStory)
                {
                    PlayHeroHpChangeStory();
                }
                if ((float) MeVo.instance.CurHp/MeVo.instance.Hp <= 0.05f)
                {
                    if (SpeechMgr.Instance.IsMagicSpeech)
                    {
                        SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicHPBelowPercent5);
                    }
                }
            }
            if (MeVo.DataHurtOverPercent10Update == code)
            {
                if (SpeechMgr.Instance.IsMagicSpeech)
                {
                    SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicAttackedAndHurtOverPercent10);
                }
            }
        }

        private void UpdateMonsterHpHandlerWithParam(object sender, int code, object param)
        {
            if (MeVo.MonsterDataHpUpdateWithParam == code)
            {
                if (MapControl.CanPlayMonsterHpChangeStory)
                {
                    var paramStruct = (MonsterParamStruct) param;
                    PlayMonsterHpChangeStory(paramStruct.MonsterId.ToString());
                }
            }
        }

        /// <summary>
        ///     更新信息
        /// </summary>
        public void UpdateInfo()
        {
            UpdateHealth((int) MeVo.instance.CurHp, (int) MeVo.instance.Hp);
            UpdateMagic((int) MeVo.instance.CurMp, (int) MeVo.instance.Mp);
        }

        private void PlayHeroHpChangeStory()
        {
            if (Singleton<StoryControl>.Instance.PlayHeroHpChangeStory(HeroHpChangeCallback))
            {
                _heroHpChangeStoryPlaying = true;
                MapControl.CanPlayHeroHpChangeStory = false;
                MapMode.InStory = true;
                AppMap.Instance.StopAllMonstersAi();
                CloseView();
                Singleton<CopyMode>.Instance.PauseCopy();
				AppMap.Instance.me.Controller.ContCutMgr.PauseAttack();
            }
			else
			{
				HeroHpChangeCallback();
			}
        }

        private void HeroHpChangeCallback()
        {
            if (_heroHpChangeStoryPlaying)
            {
                _heroHpChangeStoryPlaying = false;
                MapMode.InStory = false;
                OpenView();
                Singleton<CopyMode>.Instance.ResumeCopy();
                AppMap.Instance.StartAllMonstersAi();
				AppMap.Instance.me.Controller.ContCutMgr.ResumeAttack();
            }
        }

        private void PlayMonsterHpChangeStory(string monsterId)
        {
            if (Singleton<StoryControl>.Instance.PlayMonsterHpChangeStory(monsterId, MonsterHpChangeCallback))
            {
                _monsterHpChangeStoryPlaying = true;
                MapControl.CanPlayMonsterHpChangeStory = false;
                MapMode.InStory = true;
                AppMap.Instance.StopAllMonstersAi();
                CloseView();
                Singleton<CopyMode>.Instance.PauseCopy();
				AppMap.Instance.me.Controller.ContCutMgr.PauseAttack();
            }
			else
			{
				MonsterHpChangeCallback();
			}
        }

        private void MonsterHpChangeCallback()
        {
            if (_monsterHpChangeStoryPlaying)
            {
                _monsterHpChangeStoryPlaying = false;
                MapMode.InStory = false;
                OpenView();
                Singleton<CopyMode>.Instance.ResumeCopy();
                AppMap.Instance.StartAllMonstersAi();
				AppMap.Instance.me.Controller.ContCutMgr.ResumeAttack();
            }
        }

        private void StopGame(GameObject go)
        {
            if (Singleton<AwardControl>.Instance.CanInterrupt) //正在播放升级特效时屏蔽掉该功能----modify by lixi
            {
                Singleton<GameSettingView>.Instance.OpenView();
            }
        }

        private void CallEntrust(GameObject go)
        {
            _meAiController.SetAi(!Singleton<BattleMode>.Instance.IsAutoSystem);
        }

        private void CallSkill0(GameObject go)
        {
            if (MapMode.DisableInput||!_canUse[4])
            {
                return; //自动切图的时候不响应用户输入
            }
            _meAiController.SetAi(false);
            AppMap.Instance.me.Controller.SkillController.RequestUseSkill(SkillController.Roll);
            if (_currentGuideSkillBtn != null && go == _currentGuideSkillBtn.gameObject)
            {
                ProcessSkillGuide();
            }
        }

        private void CallSkill1(GameObject go)
        {
            if (MapMode.DisableInput||!_canUse[0])
            {
                return; //自动切图的时候不响应用户输入
            }
            _meAiController.SetAi(false);
            AppMap.Instance.me.Controller.SkillController.RequestUseSkill(SkillController.Skill1);
            if (_currentGuideSkillBtn != null && go == _currentGuideSkillBtn.gameObject)
            {
                ProcessSkillGuide();
            }
        }

        private void CallSkill2(GameObject go)
        {
            if (MapMode.DisableInput||!_canUse[1])
            {
                return; //自动切图的时候不响应用户输入
            }
            _meAiController.SetAi(false);
            AppMap.Instance.me.Controller.SkillController.RequestUseSkill(SkillController.Skill2);
            if (_currentGuideSkillBtn != null && go == _currentGuideSkillBtn.gameObject)
            {
                ProcessSkillGuide();
            }
        }

        private void CallSkill3(GameObject go)
        {
            if (MapMode.DisableInput||!_canUse[2])
            {
                return; //自动切图的时候不响应用户输入
            }
            _meAiController.SetAi(false);
            AppMap.Instance.me.Controller.SkillController.RequestUseSkill(SkillController.Skill3);
            if (_currentGuideSkillBtn != null && go == _currentGuideSkillBtn.gameObject)
            {
                ProcessSkillGuide();
            }
        }

        private void CallSkill4(GameObject go)
        {
            if (MapMode.DisableInput||!_canUse[3])
            {
                return; //自动切图的时候不响应用户输入
            }
            _meAiController.SetAi(false);
            AppMap.Instance.me.Controller.SkillController.RequestUseSkill(SkillController.Skill4);
            if (_currentGuideSkillBtn != null && go == _currentGuideSkillBtn.gameObject)
            {
                ProcessSkillGuide();
            }
        }

        private void CallAttack(GameObject go, bool flag)
        {
            if (MapMode.DisableInput)
            {
                _isAttack = false;
                return; //自动切图的时候不响应用户输入
            }
            _isAttack = flag;
            if (flag && _guideSkillIndex == 1)
            {
                ProcessSkillGuide();
            }
        }

        private void GetCanUseSkillBtn()
        {
            float[] cdTimes = AppMap.Instance.me.Controller.SkillController.LeftCdTimes;
            if (MapMode.CUR_MAP_PHASE == 1)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (cdTimes[i + 3] <= 0)
                    {
                        _currentGuideSkillBtn = _skillButtons[i - 1].gameObject;
                        return;
                    }   
                }
                if (cdTimes[5 + 3] <= 0)
                {
                    _currentGuideSkillBtn = _skillButtons[5 - 1].gameObject;
                }
            }
            else
            {
                for (int i = 3; i < 6; i++)
                {
                    if (cdTimes[i + 3] <= 0)
                    {
                        _currentGuideSkillBtn = _skillButtons[i - 1].gameObject;
                        return;
                    }
                }
            }
        }

        public override void Update()
        {
            //下面处理箭头指示
            BossDirectionTip(); //处理怪物位置的提示

            if (AppMap.Instance.me == null || AppMap.Instance.me.Controller == null)
            {
                return;
            }
            if (_meAiController == null)
            {
                _meAiController = AppMap.Instance.me.Controller.AiController as MeAiController;
            }
            //CD显示
            float[] cdTimes = AppMap.Instance.me.Controller.SkillController.LeftCdTimes;
            float[] totalTimes = AppMap.Instance.me.Controller.SkillController.TotalCdTimes;
            int[] skillMpCost = AppMap.Instance.me.Controller.SkillController.SkillMpCost;
            _skill1Cd.GetComponent<UISprite>().fillAmount = cdTimes[SkillController.Skill1]/
                                                            totalTimes[SkillController.Skill1];
            _skill2Cd.GetComponent<UISprite>().fillAmount = cdTimes[SkillController.Skill2]/
                                                            totalTimes[SkillController.Skill2];
            _skill3Cd.GetComponent<UISprite>().fillAmount = cdTimes[SkillController.Skill3]/
                                                            totalTimes[SkillController.Skill3];
            _skill4Cd.GetComponent<UISprite>().fillAmount = cdTimes[SkillController.Skill4]/
                                                            totalTimes[SkillController.Skill4];
            _skill0Cd.GetComponent<UISprite>().fillAmount = cdTimes[SkillController.Roll]/
                                                            totalTimes[SkillController.Roll];

            for (int i = 0; i < 5; i++)
            {
                if (cdTimes[i + 4] > totalTimes[i + 4] - 0.2 && totalTimes[i + 4] > 0)
                {
                    _isIncd[i] = true;
                }
                if (_isIncd[i] && cdTimes[i + 4] <= 0)
                {
                    _isIncd[i] = false;
                    //显示技能CD
                    EffectMgr.Instance.CreateUIEffect(EffectId.UI_SkillIcon, _skillCdGameObjects[i].transform.position);
                }
                if (cdTimes[i + 4] > 0)
                {
                    _skillCdTimeLabels[i].gameObject.SetActive(true);
                    _skillCdTimeLabels[i].text = (int) (cdTimes[i + 4] + 1) + "";
                }
                else
                {
                    _skillCdGameObjects[i].GetComponent<UISprite>().fillAmount = skillMpCost[i + 4] > MeVo.instance.CurMp
                        ? 1
                        : 0;
                    _skillCdTimeLabels[i].gameObject.SetActive(false);
                }
            }

            Vector2 position = _nguiJoystick.position;
            if (position.x > 0 || position.y > 0 || position.x < 0 || position.y < 0)
            {
                var playerControler = AppMap.Instance.me.Controller as PlayerControler;
                if (playerControler != null)
                {
                    int dir = Directions.GetDirByVector2(position);
                    playerControler.MoveByDir(dir);
                    _joystickHighLight.spriteName = "Joystick2";
                }
                if (position.x > 0.8f)
                {
                    if (_guideJoy.activeInHierarchy)
                    {
                        _guideJoy.SetActive(false); //移除指引
                        _guideSkill.SetActive(true);
                        _guideSkillIndex = 0;
                        ProcessSkillGuide();
                    }
                }
            }
            else
            {
                _joystickHighLight.spriteName = "Joystick1";
            }


            if (_isAttack)
            {
                if (Time.time - _lastAttackCommandTime > 0.12f && MeVo.instance.CurHp > 0)
                {
                    var vo = new ActionVo {ActionType = Actions.ATTACK};
                    if (_meAiController != null) _meAiController.SetAi(false);
                    AppMap.Instance.me.Controller.AttackController.AddAttackList(vo);
                    _lastAttackCommandTime = Time.time;
                }
            }
        }

        public void BossDirectionTip() //提示左边还是右边有怪物存在
        {
            _nLeftNum = 0;
            _nRightNum = 0;
            //死亡一个怪物就要判断当前是不是屏幕之外还有怪物
            IList<MonsterDisplay> list = AppMap.Instance.monsterList;
            bool bRight = false;
            bool bLeft = false;

            foreach (MonsterDisplay monster in list)
            {
                if (ReferenceEquals(Camera.main,null))
                {
                    return;
                }
                Vector3 pos1 = Camera.main.WorldToScreenPoint(monster.Controller.transform.position);
                Vector3 pos2 = Camera.main.ScreenToViewportPoint(pos1);
                Animator animator = monster.Animator;
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                bool death = stateInfo.normalizedTime > 0.05 && stateInfo.nameHash == Status.NAME_HASH_DEATH;
                if (pos2.x > 1.2f && (!death)) // || pos2.x < 0)  //有怪物出现在屏幕之外
                {
                    bRight = true;
                    _nRightNum++;
                }
                else if (pos2.x < -0.2f && (!death))
                {
                    bLeft = true;
                    _nLeftNum++;
                }
            }
            if (_nLeftNum == 0)
                _arrowLeft.gameObject.SetActive(false);
            if (_nRightNum == 0)
                _arrowRight.gameObject.SetActive(false);

            if (bLeft)
            {
                _leftNum.text = _nLeftNum.ToString();
                _arrowLeft.gameObject.SetActive(true);
            }
            if (bRight)
            {
                _rightNum.text = _nRightNum.ToString();
                _arrowRight.gameObject.SetActive(true);
            }
        }


        /// <summary>
        ///     更新玩家血量
        /// </summary>
        /// <param name="curHp">当前血量</param>
        /// <param name="hp">总血量</param>
        public void UpdateHealth(int curHp, int hp)
        {
            if (curHp < 0) curHp = 0;
            _healthLabel.text = curHp + "/" + hp;
            TweenSlider.Begin(_sldHp.gameObject, 0.5f, (float) curHp/hp);
        }


        /// <summary>
        ///     更新玩家魔法
        /// </summary>
        /// <param name="curMp">当前魔法</param>
        /// <param name="mp">总魔法</param>
        public void UpdateMagic(int curMp, int mp)
        {
            if (curMp < 0) curMp = 0;
            _magicLabel.text = curMp + "/" + mp;
            TweenSlider.Begin(_sldMagic.gameObject, 0.5f, (float) curMp/mp);
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            UpdateLeftTime();
            UpdateInfo();
            UpdateAutoSystemStatu();
            ResetUi();
            AdjustUiByMapId();
            SetGuideInfo();
            InitPlayerInfo();
            LoadSkillIcon();
            if (MapMode.InStory)
            {
                CloseView();
            }
        }

        /// <summary>
        /// 重置UI状态
        /// </summary>
        private void ResetUi()
        {
            _sldBossHp.gameObject.SetActive(false);
            _sldMonsterHp.gameObject.SetActive(false);
            _btnAttack.onPress(_btnAttack.gameObject, false);
            _joystickHighLight.spriteName = "Joystick1";
            _nguiJoystick.Reset();
        }


        /// <summary>
        /// 根据不同的地图id调整UI显示的内容
        /// </summary>
        private void AdjustUiByMapId()
        {
            if (AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS)
            {
                OpenViewByBoss();
            }
            else if (AppMap.Instance.mapParser.MapId == MapTypeConst.ARENA_MAP ||
                     AppMap.Instance.mapParser.MapId == MapTypeConst.GoldSilverIsland_MAP)
            {
                CloseTopView();
                //打开竞技场 View 
                Singleton<ArenaFightView>.Instance.OpenView();
            }
            else
            {
                OpenTopView();
            }
            if (AppMap.Instance.mapParser.MapId == MapTypeConst.GoldHit_MAP)
            {
                //Singleton<GoldHitView>.Instance.OpenView();
            }
            else if (AppMap.Instance.mapParser.MapId != MapTypeConst.GoldHit_MAP)
            {
                Singleton<GoldHitView>.Instance.CloseView();
            }
            //第一场战斗不显示副本暂停按钮和挂机按钮
            if (AppMap.Instance.mapParser.MapId == MapTypeConst.FirstFightMap)
            {
                _btnStop.SetActive(false);
                _entrustBtn.SetActive(false);
            }
            else
            {
                _btnStop.SetActive(true);
                _entrustBtn.SetActive(true);
            }
        }

        /// <summary>
        /// 设置指引状态
        /// </summary>
        private void SetGuideInfo()
        {
            if (MeVo.instance.mapId == MapTypeConst.FirstFightMap)
            {
                _guideJoy.SetActive(true);
                _guideSkill.SetActive(false);
            }
            else
            {
                _guideJoy.SetActive(false);
                _guideSkill.SetActive(false);
            }
        }


        private void LoadSkillIcon()
        {
            if (_skillAtlas == null)
            {
                Singleton<AtlasManager>.Instance.LoadAtlasHold(AtlasUrl.SkillIconHold, AtlasUrl.SkillIconNormal, LoadAtlas, true);
            }
            else
            {
                SetSkillIcon();
            }
        }

        private void LoadAtlas(UIAtlas atlas)
        {
            if (_skillAtlas == null)
            {
                _skillAtlas = atlas;
                SetSkillIcon();
            }
        }

        private void SetSkillIcon()
        {
            uint[] learnedSkillIds = SkillMode.Instance.GetUsedSkill();
            if (MeVo.instance.mapId == MapTypeConst.FirstFightMap)
            {
                string[] skillIds = StringUtils.GetValueListFromString(MeVo.instance.SysRoleBaseInfo.SkillList);
                for (int i = 0; i < 5; i++)
                {
                    learnedSkillIds[i] = uint.Parse(skillIds[i + 6]);
                }
            }
            var learnedSkillList = AppMap.Instance.me.Controller.SkillController.LearnedSkillList;
            for (int i = 6; i <= 10; i++)
            {
                learnedSkillList[i] = learnedSkillIds[i - 6];
            }
            for (int i=0;i<_skillButtons.Length;i++)
            {
                var uiSprites = _skillButtons[i].transform.GetComponentsInChildren<UISprite>(true);
                foreach (var uiSprite in uiSprites)
                {
                    uiSprite.atlas = _skillAtlas;
                }
                var icon = _skillButtons[i].transform.FindChild("Icon").GetComponent<UISprite>();
                if (learnedSkillIds[i] != 0)
                {
                    var skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(learnedSkillIds[i]);
                    icon.spriteName = skill.icon.ToString(CultureInfo.InvariantCulture);
                    _canUse[i] = true;
                }
                else
                {
                    _canUse[i] = false;
                    icon.spriteName = "suo231";
                }
            }
            AppMap.Instance.me.Controller.SkillController.InitTotalCdTime();
        }

        /// <summary>
        ///     是否要隐藏聊天按钮，
        /// </summary>
        /// <param name="hind">ture，隐藏</param>
        private void HindChatBtn(bool hind)
        {
            if (hind)
            {
                _btnChat.SetActive(false);
            }
            else
            {
                _btnChat.SetActive(true);
            }
        }

        private void CloseTopView()
        {
            _topLeftGameObject.SetActive(false);
            _topGameObject.SetActive(false);
            _topRightGameObject.SetActive(false);
            Singleton<BattleMode>.Instance.dataUpdated -= UpdateBattleMode;
            MeVo.instance.DataUpdated -= UpdateHpMpHandler;
        }

        private void OpenTopView()
        {
            _topLeftGameObject.SetActive(true);
            _topGameObject.SetActive(true);
            _topRightGameObject.SetActive(true);
            Singleton<BattleMode>.Instance.dataUpdated += UpdateBattleMode;
            MeVo.instance.DataUpdated += UpdateHpMpHandler;
        }

        public int GetDugeonTime()
        {
            return (int) MapMode.expire;
        }

        public int GetLeftTime()
        {
            return _leftSecond;
        }

        #region interface for worldboss

        private void OpenViewByBoss()
        {
            _topLeftGameObject.SetActive(false);
            _topGameObject.SetActive(false);
            _topRightGameObject.SetActive(true);
            _sldBossHp.gameObject.SetActive(true);
            Singleton<BattleMode>.Instance.dataUpdated += UpdateBattleMode;
        }

        private void CloseViewByBoss()
        {
            _topLeftGameObject.SetActive(true);
            _topGameObject.SetActive(true);
            Singleton<BattleMode>.Instance.dataUpdated -= UpdateBattleMode;
        }

        #endregion

        #region Battle Guide

        private void ProcessSkillGuide()
        {
            if (_guideSkill.activeInHierarchy)
            {
                if (_guideSkillIndex > 8)
                {
                    _guideSkill.SetActive(false);
                    return;
                }
                if (_guideSkillIndex == 0)
                {
                    _guideSkill.transform.position = _btnAttack.transform.position;
                }
                else
                {
                    _guideSkill.SetActive(false);
                    vp_Timer.In(1, ShowNextSkillGuide);
                }
                _guideSkillIndex++;
            }
        }

        private void ShowNextSkillGuide()
        {
            _guideSkill.SetActive(true);
            GetCanUseSkillBtn();
            if (_currentGuideSkillBtn != null)
            {
                _guideSkill.transform.position = _currentGuideSkillBtn.transform.position;
            }
        }

        #endregion
    }
}