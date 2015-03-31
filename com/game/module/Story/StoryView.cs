//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryView
//文件描述：剧情界面类
//创建者：黄江军
//创建日期：2013-12-10
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.module.map;
using com.game.module.Guide;
using com.game;
using com.u3d.bases.consts;
using com.u3d.bases.display.controler;

namespace Com.Game.Module.Story
{
	public delegate void ClosedCallback();

    public class StoryView : BaseView<StoryView>
    {
        public override string url { get { return "UI/Story/StoryView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.HighLayer; }}
		public override bool playClosedSound { get { return false; } }

		private bool isOpen = false;
		private readonly float maskEffectTime = 1.0f;
		private bool lastRoleAi;

		private UILabel labLeftName;
		private UILabel labLeftWords;
		private UISprite sprLeftNPC;
		private UISprite sprLeftEmotion;

		private UILabel labRightName;
		private UILabel labRightWords;
		private UISprite sprRightNPC;
		private UISprite sprRightEmotion;

		private Button btnSkip;
		private Button btnFull;
		private UISprite sprFullEffect;
		private UISprite sprTopMask;
		private UISprite sprBottomMask;
		private UILabel labAsideText;

		private GameObject talkWindow;
		private GameObject asideWindow;
		private GameObject leftNPC;
		private GameObject rightNPC;

		private Vector3 topMaskOrgPos;
		private Vector3 topMaskDstPos;
		private Vector3 bottomMaskOrgPos;
		private Vector3 bottomMaskDstPos;

		//NPC左
		public GameObject LeftNPC
		{
			get	{return leftNPC;}
		}

		public UILabel LeftNPCName
		{
			get	{return labLeftName;}
		}

		public UILabel LeftNPCWords
		{
			get	{return labLeftWords;}
		}

		public UISprite LeftNPCSprite
		{
			get	{return sprLeftNPC;}
		}

		public UISprite LeftEmotionSprite
		{
			get	{return sprLeftEmotion;}
		}

		//NPC右
		public GameObject RightNPC
		{
			get	{return rightNPC;}
		}

		public UILabel RightNPCName
		{
			get	{return labRightName;}
		}
		
		public UILabel RightNPCWords
		{
			get	{return labRightWords;}
		}
		
		public UISprite RightNPCSprite
		{
			get	{return sprRightNPC;}
		}

		public UISprite RightEmotionSprite
		{
			get	{return sprRightEmotion;}
		}

		public UISprite FullEffectSprite
		{
			get	{return sprFullEffect;}
		}

		public GameObject TalkWindow
		{
			get	{return talkWindow;}
		}

		public GameObject AsideWindow
		{
			get	{return asideWindow;}
		}

		public UILabel AsideText
		{
			get	{return labAsideText;}
		}


		public ClosedCallback ViewClosedCallback 
		{
			get; 
			set;
		}

		protected override void Init()
		{
			asideWindow = gameObject.transform.Find("aside").gameObject;
			labAsideText = FindInChild<UILabel>("aside/words");

			talkWindow = gameObject.transform.Find("play/bottom/talkwindow").gameObject;
			leftNPC = gameObject.transform.Find("play/bottom/talkwindow/talk/left").gameObject;
			rightNPC = gameObject.transform.Find("play/bottom/talkwindow/talk/right").gameObject;

			labLeftName = FindInChild<UILabel>("play/bottom/talkwindow/talk/left/halfbody/name");
			sprLeftNPC = FindInChild<UISprite>("play/bottom/talkwindow/talk/left/halfbody/bust");
			labLeftWords = FindInChild<UILabel>("play/bottom/talkwindow/talk/left/words");
			sprLeftEmotion = FindInChild<UISprite>("play/bottom/talkwindow/talk/left/emotion");

			labRightName = FindInChild<UILabel>("play/bottom/talkwindow/talk/right/halfbody/name");
			sprRightNPC = FindInChild<UISprite>("play/bottom/talkwindow/talk/right/halfbody/bust");
			labRightWords = FindInChild<UILabel>("play/bottom/talkwindow/talk/right/words");
			sprRightEmotion = FindInChild<UISprite>("play/bottom/talkwindow/talk/right/emotion");

			btnSkip = FindInChild<Button>("play/bottom/talkwindow/talk/skip");
			btnFull = FindInChild<Button>("play/bottom/fullButton");
			sprFullEffect = FindInChild<UISprite>("fulleffect/image");
			sprTopMask = FindInChild<UISprite>("mask/topmask/background1");
			sprBottomMask = FindInChild<UISprite>("mask/bottommask/background1");

			topMaskOrgPos = sprTopMask.transform.localPosition;
			topMaskDstPos = topMaskOrgPos + new Vector3(0f, sprTopMask.height, 0f);
			bottomMaskOrgPos = sprBottomMask.transform.localPosition;
			bottomMaskDstPos = bottomMaskOrgPos - new Vector3(0f, sprBottomMask.height, 0f);

			btnSkip.onClick = SkipOnClick;
			btnFull.onClick = FullOnClick;

			//初始化剧情命令管理器
			StoryActionMgr.Instance.EndStoryCallBack = EndStory;

			InitLabel();
		}

		private void InitLabel()
		{
			btnSkip.label.text = LanguageManager.GetWord("StoryView.Skip");
		}

		private void HideAllSubWindows()
		{
			talkWindow.gameObject.SetActive(false);
			asideWindow.gameObject.SetActive(false);
			sprFullEffect.gameObject.SetActive(false);
		}

		private void SkipOnClick(GameObject go)
		{
			StoryActionMgr.Instance.Skip();
			CloseView();
		}

		private void FullOnClick(GameObject go)
		{
			if (!isOpen)
			{
				return;
			}

			StoryActionMgr.Instance.FullOnClick();
		}

		private void ShowMask()
		{
			ShowMaskMoveEffect(sprTopMask, topMaskDstPos, topMaskOrgPos);
			ShowMaskMoveEffect(sprBottomMask, bottomMaskDstPos, bottomMaskOrgPos);
		}

		private void HideMask()
		{
			ShowMaskMoveEffect(sprTopMask, topMaskOrgPos, topMaskDstPos);
			ShowMaskMoveEffect(sprBottomMask, bottomMaskOrgPos, bottomMaskDstPos);
		}

		private void ShowMaskMoveEffect(UISprite sprMask, Vector3 from, Vector3 to)
		{
			TweenPosition tweenPosition = sprMask.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}
			
			tweenPosition = sprMask.gameObject.AddComponent<TweenPosition>();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = maskEffectTime;
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();

			HideAllSubWindows();
			ShowMask();
			vp_Timer.In(maskEffectTime, StartStory, 1, 0);
		}

		private void StartStory()
		{
			StoryActionMgr.Instance.Start();		
			isOpen = true;
		}

		public override void OpenView()
		{
			MapControl.Instance.MyCamera.enabled = false;
			lastRoleAi = AppMap.Instance.me.Controller.AiController.IsAi;
			AppMap.Instance.me.Controller.AiController.SetAi(false);

			isOpen = false;
			StoryActionMgr.Instance.Init();
			base.OpenView();
		}

		//关闭前回调
		public override void CloseView()
		{
			try
			{
				base.CloseView();

				isOpen = false;
				StoryActionMgr.Instance.Stop();				
				MapControl.Instance.MyCamera.enabled = true;
				AppMap.Instance.me.Controller.AiController.SetAi(lastRoleAi);
				(AppMap.Instance.me.Controller as ActionControler).MoveSpeed = Global.ROLE_RUN_SPEED;
			}
			finally
			{
				if (null != ViewClosedCallback)
				{
					ViewClosedCallback();
				}
			}
		}

		public override void Update()
		{
			//窗口关闭不需要处理逻辑
			if (!isOpen)
			{
				return;
			}
			
			StoryActionMgr.Instance.AutoStep();
		}

		private void EndStory()
		{
			isOpen = false;
			HideAllSubWindows();
			HideMask();
			vp_Timer.In(maskEffectTime, CloseView, 1, 0);
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<StoryMode>.Instance.dataUpdated += UpdateForceStopStoryHandler;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<StoryMode>.Instance.dataUpdated -= UpdateForceStopStoryHandler;
		}

		private void UpdateForceStopStoryHandler(object sender, int code)
		{
			if (Singleton<StoryMode>.Instance.FORCE_STOP_STORY == code)
			{
				CloseView();
			}
		}

    }
}
