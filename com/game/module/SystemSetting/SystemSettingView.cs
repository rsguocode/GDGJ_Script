//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SystemSettingView
//文件描述：
//创建者：黄江军
//创建日期：2013-12-18
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using com.game.data;
using com.game.module.Guide;
using com.game.module.Guide.GuideLogic;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.consts;

namespace Com.Game.Module.SystemSetting
{
	public class SystemSettingView : BaseView<SystemSettingView> 
	{
		public override string url { get { return "UI/SystemSetting/SystemSettingView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		bool lastCrit; 
		bool lastHide; 
		bool lastMute;
        uint lastSceneVol;
        uint lastEffectVol;

		private UILabel labSet;
		private Button btnClose;
		private UIToggle togCrit;
		private UILabel labCrit;
		private UIToggle togHide;
		private UILabel labHide;
		private UIToggle togMute;
		private UILabel labMute;
        private UIToggle _togShowButton;
		private UILabel labSceneVol;
		private UISlider sldSceneVol;
		private UILabel labEffectVol;
		private UISlider sldEffectVol;
		private UILabel labMusic;
		private UILabel labAudio;
		private UILabel labPlayer;
		private UIToggle togMusic;
		private UIToggle togAudio;
		private UIToggle togPlayer;

		//位移Tween
		//private TweenPosition tweenPosition;
		public SystemSettingView() 
		{	
		}	

		protected override void Init()
		{
			base.showTween = FindInChild<TweenPlay>("sz");
			//tweenPosition = FindInChild<TweenPosition>("sz");
			labSet = FindInChild<UILabel>("sz/title/labsz");
			btnClose = FindInChild<Button>("sz/topright/btn_close");
			togCrit = FindInChild<UIToggle>("sz/chkbj");
			labCrit = FindInChild<UILabel>("sz/chkbj/Label");
			togHide = FindInChild<UIToggle>("sz/chkyc");
			labHide = FindInChild<UILabel>("sz/chkyc/Label");
            _togShowButton = FindInChild<UIToggle>("sz/chkBtn");
			togMute = FindInChild<UIToggle>("sz/chkjy");
			labMute = FindInChild<UILabel>("sz/chkjy/Label");
			labSceneVol = FindInChild<UILabel>("sz/dj/labyy");
			sldSceneVol = FindInChild<UISlider>("sz/dj/yybar");
			labEffectVol = FindInChild<UILabel>("sz/dj/labyx");
			sldEffectVol = FindInChild<UISlider>("sz/dj/yxbar");

			labMusic = FindInChild<UILabel>("sz/music/log/name");
			labAudio = FindInChild<UILabel>("sz/audio/log/name");
			labPlayer = FindInChild<UILabel>("sz/player/log/name");

			togMusic = FindInChild<UIToggle>("sz/music/chkbj");
			togAudio = FindInChild<UIToggle>("sz/audio/chkbj");
			togPlayer = FindInChild<UIToggle>("sz/player/chkbj");
			togPlayer.value = true;

			btnClose.onClick += CloseOnClick;
			togCrit.onChange.Add(new EventDelegate(CritOnClick));
			togHide.onChange.Add(new EventDelegate(HideOnClick));
			togMute.onChange.Add(new EventDelegate(MuteOnClick));
			sldSceneVol.onChange.Add(new EventDelegate(SceneVolOnClick));
			sldEffectVol.onChange.Add(new EventDelegate(EffectVolOnClick));
            _togShowButton.value = false;
            _togShowButton.onChange.Add(new EventDelegate(ShowButtonClick));

			togMusic.onChange.Add(new EventDelegate(MusicOnClick));
			togAudio.onChange.Add(new EventDelegate(AudioOnClick));
			togPlayer.onChange.Add(new EventDelegate(PlayerOnClick));

			InitLabel();

			if (Application.platform != RuntimePlatform.WindowsEditor)
			{
				_togShowButton.SetActive(false);
			}
		}

		private void InitLabel() 
		{
			labSet.text = LanguageManager.GetWord("SystemSettingView.Set");
			labCrit.text = LanguageManager.GetWord("SystemSettingView.CritShake");
			labHide.text = LanguageManager.GetWord("SystemSettingView.HidePlayer");
			labMute.text = LanguageManager.GetWord("SystemSettingView.Mute");
			labSceneVol.text = LanguageManager.GetWord("SystemSettingView.SceneVolumn");
			labEffectVol.text = LanguageManager.GetWord("SystemSettingView.EffectVolumn");

			labMusic.text = LanguageManager.GetWord("SystemSettingView.Music");
			labAudio.text = LanguageManager.GetWord("SystemSettingView.Audio");
			labPlayer.text = LanguageManager.GetWord("SystemSettingView.Player");

			togMusic.FindChild("background/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Close");
			togMusic.FindChild("checkmark/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Open");

			togAudio.FindChild("background/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Close");
			togAudio.FindChild("checkmark/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Open");

			togPlayer.FindChild("background/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Hide");
			togPlayer.FindChild("checkmark/text").GetComponent<UILabel>().text = LanguageManager.GetWord("SystemSettingView.Show");
		}
		
		protected override void HandleAfterOpenView()
		{
			lastCrit = Singleton<SystemSettingMode>.Instance.CritShake;
			lastHide = Singleton<SystemSettingMode>.Instance.HidePlayer;
			lastMute = Singleton<SystemSettingMode>.Instance.Mute;
			lastSceneVol = Singleton<SystemSettingMode>.Instance.SceneVolumn;
			lastEffectVol = Singleton<SystemSettingMode>.Instance.EffectVolumn;

			togCrit.value = lastCrit;
			togHide.value = lastHide;
			togMute.value = lastMute;
			sldSceneVol.value = (float)lastSceneVol/100;
			sldEffectVol.value = (float)lastEffectVol/100;

			togMusic.value = (lastSceneVol > 0f);
			togAudio.value = (lastEffectVol > 0f);

			AdjustTogShow(togMusic);
			AdjustTogShow(togAudio);
			AdjustTogShow(togPlayer);
		    
		    //NGUITools.SetLocalPositionToRayHit(tweenPosition.transform);// 调整动画启动位置为当前鼠标点击的位置
		    //tweenPosition.from = tweenPosition.value;
		}

		protected override void HandleBeforeCloseView()
		{

		}

		//关闭按钮事件函数
		private void CloseOnClick(GameObject go)
		{
			Singleton<SystemSettingMode>.Instance.SaveChangedSystemSettingToServer(lastCrit, lastHide, lastMute, lastSceneVol, lastEffectVol);
			CloseView();
		    //GuideBase.TriggerGuideForTest(GuideType.GuideArenaOpen);
		}

		//暴击震动设置事件
		private void CritOnClick()
		{
			Singleton<SystemSettingMode>.Instance.CritShake = togCrit.value;
		}

		//隐藏玩家设置事件
		private void HideOnClick()
		{
			Singleton<SystemSettingMode>.Instance.HidePlayer = togHide.value;
		}

		//静音设置事件
		private void MuteOnClick()
		{
			Singleton<SystemSettingMode>.Instance.Mute = togMute.value;
		}

		//背景音乐设置事件
		private void SceneVolOnClick()
		{
            Singleton<SystemSettingMode>.Instance.SceneVolumn = (uint)(sldSceneVol.value * 100);
		}

		//游戏音效设置事件
		private void EffectVolOnClick()
		{
            Singleton<SystemSettingMode>.Instance.EffectVolumn = (uint)(sldEffectVol.value * 100);
		}

	    private void ShowButtonClick()
	    {
	        Singleton<SystemSettingMode>.Instance.ShowButton = _togShowButton.value;
	    }

		private void MusicOnClick()
		{
			AdjustTogShow(togMusic);

			if (togMusic.value)
			{
				Singleton<SystemSettingMode>.Instance.SceneVolumn = GameConst.DefaultSceneVolumn;
			}
			else
			{
				Singleton<SystemSettingMode>.Instance.SceneVolumn = 0;
			}
		}

		private void AudioOnClick()
		{
			AdjustTogShow(togAudio);

			if (togAudio.value)
			{
				Singleton<SystemSettingMode>.Instance.EffectVolumn = GameConst.DefaultEffectVolumn;
			}
			else
			{
				Singleton<SystemSettingMode>.Instance.EffectVolumn = 0;
			}
		}

		private void PlayerOnClick()
		{
			AdjustTogShow(togPlayer);

			Singleton<SystemSettingMode>.Instance.HidePlayer = !togPlayer.value;
		}

		private void AdjustTogShow(UIToggle tog)
		{
			if (tog.value)
			{
				tog.FindChild("background").SetActive(false);
			}
			else
			{
				tog.FindChild("background").SetActive(true);
			}
		}
	}
}
