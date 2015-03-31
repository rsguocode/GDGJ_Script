using System;
using System.Collections.Generic;
using AnimationOrTween;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Arena;
using Com.Game.Module.Chat;
using Com.Game.Module.Copy;
using Com.Game.Module.Equip;
using Com.Game.Module.GoldHit;
using Com.Game.Module.GoldSilverIsland;
using com.game.module.Guide;
using Com.Game.Module.Guild;
using Com.Game.Module.Pet;
using Com.Game.Module.Role;
using Com.Game.Module.SystemSetting;
using com.game.module.Task;
using com.game.module.test;
using com.game.sound;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using UnityEngine;

namespace com.game.module.main
{
    public class MainBottomRightView : BaseView<MainBottomRightView>
    {
        public Action BeforeClickKongzhiButton; //按钮动画播放完毕回调
        public bool IsOpen; //右下角是否打开
        private Dictionary<int, Button> _buttonGuideRelation;
        private List<Button> _downRowButtons; //上面一排的Buttons列表
        private List<Button> _openedButtons; //开启了的buttons列表
        private List<Button> _rightButtons; //右边的一排的Buttons列表 
        private List<Button> _upRowButtons; //下面一排的Buttons列表
       
        public Button btn_js; //角色
        public Button btn_jn; //技能
        public Button btn_dz; //锻造
        public Button btn_cw; //宠物
        public Button btn_gh; //公会
        
        public Button btn_jyd; //金银岛
        public Button btn_jscj;//击石成金
        public Button btn_yxb; //英雄榜
        public Button btn_emd; //恶魔岛

        public Button btn_rw; //任务
        public Button btn_xt; //系统

        public UIToggle ckb_kongzhi;
        private bool isOpenHandle;
        private UIPlayTween[] kongzhiPlays;
        private UITweener kongzhiTween;


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
            ckb_kongzhi = FindInChild<UIToggle>("ckb_kongzhi");

            btn_jn = FindInChild<Button>("horizontal/btn_jn");
            btn_js = FindInChild<Button>("horizontal/btn_js");
            btn_dz = FindInChild<Button>("horizontal/btn_dz");
            btn_cw = FindInChild<Button>("horizontal/btn_cw");
            btn_gh = FindInChild<Button>("horizontal/btn_gh");
            btn_jyd = FindInChild<Button>("horizontal/btn_jyd");
            btn_jscj = FindInChild<Button>("horizontal/btn_jscj");
            btn_yxb = FindInChild<Button>("horizontal/btn_yxb");
            btn_emd = FindInChild<Button>("horizontal/btn_emd");

            btn_jn.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Skill");
            btn_js.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Role");
            btn_dz.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Equip");
            btn_cw.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Pet");
            //btn_gh.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Skill");
            btn_jyd.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.GoldSilverIsland");
            btn_jscj.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.GoldHit");
            btn_yxb.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.Arena");
            btn_emd.FindInChild<UILabel>("label").text = LanguageManager.GetWord("MainBottomRightView.DaemonIsland");

            btn_jn.onClick = SkillOnClick;
            btn_js.onClick = JueSeOnClick;
            btn_dz.onClick = DuanZaoOnClick;
            btn_cw.onClick = ChongWuOnClick;
            btn_gh.onClick = GongHuiOnClick;
            btn_jyd.onClick = GoldSilverIslandClick;
            btn_jscj.onClick = GoldHitClick;
            btn_yxb.onClick = YingXiongBOnClick;
            btn_emd.onClick = EMoDaoOnClick;

            btn_rw = FindInChild<Button>("vertial/btn_rw");
            btn_xt = FindInChild<Button>("vertial/btn_xt");
            btn_rw.onClick = RewWuOnClick;
            btn_xt.onClick = SystemSetButtonClick;

            kongzhiTween = FindChild("ckb_kongzhi").GetComponent<UITweener>(); //控制按钮自身的动画
            kongzhiPlays = FindChild("ckb_kongzhi").GetComponents<UIPlayTween>();

            foreach (UIPlayTween play in kongzhiPlays)
            {
                play.trigger = Trigger.None;
            }
            ckb_kongzhi.onChange.Add(new EventDelegate(KongZhiOnClick));
            _openedButtons = new List<Button>();
            _downRowButtons = new List<Button> { btn_cw, btn_dz, btn_jn, btn_js };
            _upRowButtons = new List<Button> {btn_emd, btn_yxb, btn_jscj, btn_jyd};
            RegisterButtonInGuide();
            UpdateButtonOpenStatu();

			GoldSilverIslandMode.Instance.StartTips();
			GoldHitMode.Instance.StartTips();
        }


        private void RegisterButtonInGuide()
        {
            _buttonGuideRelation = new Dictionary<int, Button>();
            //_buttonGuideRelation.Add(GuideType.GuideRoleOpen, btn_js); //注册角色按钮和指引的关系,角色默认开启
            _buttonGuideRelation.Add(GuideType.GuideForgeOpen, btn_dz); //注册锻造按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideSkillOpen,btn_jn); //注册技能按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuidePetOpen, btn_cw); //注册宠物按钮和指引的关系
            //_buttonGuideRelation.Add(GuideType.GuideGuildOpen, btn_gh); //注册公会按钮和指引的关系

            _buttonGuideRelation.Add(GuideType.GuideGoldSilverIslandOpen, btn_jyd); //注册金银岛按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideGoldHitOpen, btn_jscj); //注册击石成金按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideArenaOpen, btn_yxb); //注册英雄榜按钮和指引的关系
            _buttonGuideRelation.Add(GuideType.GuideDaemonIslandOpen, btn_emd); //注册恶魔岛按钮和指引的关系
        }

        private void UpdateButtonOpenStatu()
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
            TaskVo mainTaskVo = TaskModel.Instance.CurrentMainTaskVo;
            if (SystemSettingMode.Instance.ShowButton||mainTaskVo==null)
            {
                _openedButtons.AddRange(_downRowButtons);
                _openedButtons.AddRange(_upRowButtons);
            }
            else
            {
                int mainTaskNum = (int)mainTaskVo.TaskId * 10 + TaskUtil.GetTaskTriggerType(mainTaskVo);
                List<SysGuideVo> guideVoList = BaseDataMgr.instance.GetGuideVoList();
                foreach (SysGuideVo sysGuideVo in guideVoList)
                {
                    //guide_type 1-3为功能开启指引
                    if (sysGuideVo.guide_type > 3 || sysGuideVo.guide_type < 1)
                    {
                        continue;
                    }
                    int guideTaskId = int.Parse(StringUtils.GetValueString(sysGuideVo.condition));
                    int guideTaskNum = sysGuideVo.trigger_type + guideTaskId * 10;
                    if (mainTaskNum >= guideTaskNum)
                    {
                        int guideId = sysGuideVo.guideID;
                        if (_buttonGuideRelation.ContainsKey(guideId))
                        {
                            _openedButtons.Add(_buttonGuideRelation[guideId]);
                        }
                    }
                }
            }
            _openedButtons.Add(btn_js);
        }

        /// <summary>
        ///     根据图标开启的情况设置图标的相对位置
        /// </summary>
        private void SetButtonPosition()
        {
            int index = 0;
            foreach (Button downRowButton in _downRowButtons)
            {
                if (_openedButtons.Contains(downRowButton))
                {
                    downRowButton.SetActive(true);
                    downRowButton.gameObject.GetComponent<TweenPosition>().from = new Vector3(152 - index*98, -50, 0);
                    index++;
                }
                else
                {
                    downRowButton.SetActive(false);
                }
            }
            index = 0;
            foreach (Button upRowButton in _upRowButtons)
            {
                if (_openedButtons.Contains(upRowButton))
                {
                    upRowButton.SetActive(true);
                    upRowButton.gameObject.GetComponent<TweenPosition>().from = new Vector3(152 - index*98, 55, 0);
                    index++;
                }
                else
                {
                    upRowButton.SetActive(false);
                }
            }
            btn_xt.SetActive(true);
            btn_rw.SetActive(false);
            btn_gh.SetActive(false);
        }

        protected override void HandleAfterOpenView()
        {
            //默认是收缩状态……
            isOpenHandle = true;
            ckb_kongzhi.value = true; //默认是收缩状态……
            //更新提示状态
            btn_cw.FindInChild<UISprite>("tips").SetActive(PetMode.Instance.ShowTips);
            btn_jn.FindInChild<UISprite>("tips").SetActive(SkillMode.Instance.ShowTips);
            btn_js.FindInChild<UISprite>("tips").SetActive(RoleMode.Instance.ShowTips);
			Singleton<ArenaMode>.Instance.ApplyArenaInfo ();  //请求竞技场信息，用于判断是否显示小红点
        }


        /// <summary>
        ///     Shrifts the bottom right.  true 收缩  false 打开
        /// </summary>
        public void ShriftBottomRight(bool direction)
        {
            if (ckb_kongzhi != null && ckb_kongzhi.value != direction)
            {
                ckb_kongzhi.value = direction;
            }
        }
        //技能
        private void SkillOnClick(GameObject go)
        {
            SkillView.Instance.OpenView();
        }
        //任务
        private void RewWuOnClick(GameObject go)
        {
            Singleton<TaskView>.Instance.OpenView();
        }


        //击石成金
        private void GoldHitClick(GameObject go)
        {
            Singleton<GoldHitView>.Instance.OpenView();
            Log.debug(this, "请求击石成金的面板信息");
            Singleton<GoldHitMode>.Instance.ApplyGoldHitInfo();
        }

        //金银岛
        private void GoldSilverIslandClick(GameObject go)
        {
            Singleton<IslandMainView>.Instance.OpenView();
        }

        //公会
        private void GongHuiOnClick(GameObject go)
        {
            Singleton<GuildView>.Instance.OpenView();
        }

        //角色
        private void JueSeOnClick(GameObject go)
        {
            Singleton<RoleView>.Instance.OpenGoodsView();
        }


        //锻造
        private void DuanZaoOnClick(GameObject go)
        {
            Singleton<Equip1View>.Instance.OpenStrenView();
        }

        //宠物
        private void ChongWuOnClick(GameObject go)
        {
            Singleton<PetView>.Instance.OpenView();
        }


        //系统设置
        private void SystemSetButtonClick(GameObject go)
        {
            Singleton<SystemSettingView>.Instance.OpenView();
        }


        //英雄帮
        private void YingXiongBOnClick(GameObject go)
        {
            Singleton<ArenaView>.Instance.OpenArenaMainView();
        }


        //恶魔岛
        private void EMoDaoOnClick(GameObject go)
        {
            Singleton<CopyControl>.Instance.OpenDaemonIslandView();
        }


        //点击收缩
        private void KongZhiOnClick()
        {
            UpdateButtonOpenStatu();
            if (BeforeClickKongzhiButton != null)
            {
                BeforeClickKongzhiButton();
            }
            if (UIToggle.current.value && UIToggle.current.Equals(ckb_kongzhi))
            {
                kongzhiTween.PlayForward();
                Singleton<MainBottomLeftView>.Instance.OpenView();
                IsOpen = false;
            }
            else if (UIToggle.current.value == false && UIToggle.current.Equals(ckb_kongzhi))
            {
                Singleton<ChatView>.Instance.CloseView();
                kongzhiTween.PlayReverse();
                Singleton<MainBottomLeftView>.Instance.CloseView();
                IsOpen = true;
            }
            if (isOpenHandle && UIToggle.current.value && UIToggle.current.Equals(ckb_kongzhi))
            {
                ResetTweenPlay();
                isOpenHandle = false;
            }
            else
            {
                foreach (UIPlayTween kongzhiPlay in kongzhiPlays)
                {
                    kongzhiPlay.Play(UIToggle.current.value);
                }
            }
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ButtonFold);

			SetControlButtonTips();
        }


        public void ResetTweenPlay()
        {
            foreach (TweenPosition tweenPosition in gameObject.GetComponentsInChildren<TweenPosition>())
            {
                tweenPosition.value = tweenPosition.to;
            }
            foreach (TweenAlphas tweenAlphas in gameObject.GetComponentsInChildren<TweenAlphas>())
            {
                foreach (UIWidget widget in tweenAlphas.GetComponentsInChildren<UIWidget>())
                {
                    widget.alpha = 0f;
                }
            }
        }

        /// <summary>
        ///     注册UI响应事件
        /// </summary>
        public override void RegisterUpdateHandler()
        {
            Singleton<SystemSettingMode>.Instance.dataUpdated += SystemSettingUpdate;
            PetMode.Instance.dataUpdated += ButtonTipsHandle;
            GoodsMode.Instance.dataUpdated += ButtonTipsHandle;
			GoldSilverIslandMode.Instance.dataUpdated += ButtonTipsHandle;
			GoldHitMode.Instance.dataUpdated += ButtonTipsHandle;
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateArena;
            MeVo.instance.DataUpdated += ButtonTipsHandle;
            SkillMode.Instance.dataUpdated += ButtonTipsHandle;
        }

        public override void CancelUpdateHandler()
        {
            Singleton<SystemSettingMode>.Instance.dataUpdated -= SystemSettingUpdate;
            PetMode.Instance.dataUpdated -= ButtonTipsHandle;
            GoodsMode.Instance.dataUpdated -= ButtonTipsHandle;
			GoldSilverIslandMode.Instance.dataUpdated -= ButtonTipsHandle;
			GoldHitMode.Instance.dataUpdated -= ButtonTipsHandle;
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateArena;

            MeVo.instance.DataUpdated -= ButtonTipsHandle;
            SkillMode.Instance.dataUpdated -= ButtonTipsHandle;
        }
        //示例
        private void ButtonTipsHandle(object sender,int code)
        {
            
            //幻兽模块
            if ((sender.Equals(PetMode.Instance) && code == PetMode.PetList) || (sender.Equals(GoodsMode.Instance) && code == GoodsMode.Instance.UPDATE_PET_GOODS))
            {
				btn_cw.FindInChild<UISprite>("tips").SetActive(PetMode.Instance.ShowTips);
			}
			//角色
            else if (sender.Equals(GoodsMode.Instance) && code == GoodsMode.Instance.UPDATE_TIPS || sender.Equals(GrowMode.Instance) && code == GrowMode.Instance.UPDATE_TIPS)
            {
                btn_js.FindInChild<UISprite>("tips").SetActive(RoleMode.Instance.ShowTips);
            }
			//金银岛
			else if (sender.Equals(GoldSilverIslandMode.Instance) && code == GoldSilverIslandMode.Instance.UPDATE_TIPS)
			{
				btn_jyd.FindInChild<UISprite>("tips").SetActive(GoldSilverIslandMode.Instance.ShowTips);
			}
			//点石成金
			else if (sender.Equals(GoldHitMode.Instance) && code == GoldHitMode.Instance.UPDATE_TIPS)
			{
				btn_jscj.FindInChild<UISprite>("tips").SetActive(GoldHitMode.Instance.ShowTips);
			}
            //技能
            else if ((sender.Equals(SkillMode.Instance) && code == SkillMode.SkillPoint) ||
                (sender.Equals(MeVo.instance) && code == 0))
            {
                btn_jn.FindInChild<UISprite>("tips").SetActive(SkillMode.Instance.ShowTips);
            }

			SetControlButtonTips();
        }

		private void SetControlButtonTips()
		{
			bool visible1 = btn_cw.gameObject.activeSelf && btn_cw.FindInChild<UISprite>("tips").gameObject.activeSelf;
			bool visible2 = btn_js.gameObject.activeSelf && btn_js.FindInChild<UISprite>("tips").gameObject.activeSelf;
			bool visible3 = btn_jyd.gameObject.activeSelf && btn_jyd.FindInChild<UISprite>("tips").gameObject.activeSelf;
			bool visible4 = btn_jscj.gameObject.activeSelf && btn_jscj.FindInChild<UISprite>("tips").gameObject.activeSelf;
			bool visible5 = btn_jn.gameObject.activeSelf && btn_jn.FindInChild<UISprite>("tips").gameObject.activeSelf;
			bool visible6 = btn_yxb.gameObject.activeSelf && btn_yxb.FindInChild<UISprite>("tips").gameObject.activeSelf;
			ckb_kongzhi.FindInChild<UISprite>("tips").SetActive(visible1 || visible2 || visible3 || visible4 || visible5 || visible6);
		}

		private void SystemSettingUpdate(object sender, int code)
        {
            switch (code)
            {
                case SystemSettingMode.ShowButtonUpdate:
                    KongZhiOnClick();
                    break;
            }
        }

		private void UpdateArena(object sender, int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_RED_POINT)
			{
				btn_yxb.FindInChild<UISprite>("tips").SetActive(Singleton<ArenaMode>.Instance.ArenaRedPoint);
			}
		}
    }
}