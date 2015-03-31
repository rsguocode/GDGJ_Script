//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryActionMgr
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using com.game.module.test;
using com.u3d.bases.debug;

namespace Com.Game.Module.Story
{
	public delegate void Callback();

	public class StoryActionMgr 
	{		
		public static StoryActionMgr Instance = (Instance == null ? new StoryActionMgr() : Instance);	

		public Callback EndStoryCallBack; 

		private int talkIndex = -1;

		private BaseAction curAction
		{
			get
			{
				if ((talkIndex >= 0) && (talkIndex < Singleton<StoryMode>.Instance.ActionList.Count))
				{
					return Singleton<StoryMode>.Instance.ActionList[talkIndex];
				}
				else
				{
					return null;
				}
			}
		}

		public BaseAction CurAction
		{
			get {return curAction;}
		}

		public void Init()
		{
			talkIndex = -1;
		}

		public void Start()
		{
			Step();
		}

		public void Stop()
		{
			//清理Actions
			if (null != curAction)
			{
				curAction.AfterPlayed();
			}

			Singleton<StoryMode>.Instance.ClearActions();	
		}

		public void Skip()
		{
			if (null != curAction)
			{
				curAction.Stop();
			}
		}
		
		public void FullOnClick()
		{
			if ((null != curAction) 
				&& curAction.MannualSwitch
			    && curAction.CanMannualSwitch)
			{
				Step();
			}
		}

		public void AutoStep()
		{
			if ((null != curAction) 
			    && curAction.CanInterrupt)
			{
				Step();
			}
		}

		//处理下一个步动作
		private void Step()
		{
			if (null != curAction)
			{
				curAction.AfterPlayed();
			}

			if (talkIndex >= Singleton<StoryMode>.Instance.ActionList.Count - 1)
			{
				if (null != EndStoryCallBack)
				{
					EndStoryCallBack();
				}
			}
			else
			{
				RunNextAction();
			}
		}
		
		//运行Action
		private void RunNextAction()
		{
			try
			{
				if (talkIndex >= Singleton<StoryMode>.Instance.ActionList.Count - 1)
				{
					return;
				}
				
				talkIndex++;
				BaseAction actionObj = Singleton<StoryMode>.Instance.ActionList[talkIndex];
				if (null != actionObj)
				{
					actionObj.Run();
				}
			}
			catch(Exception ex)
			{
				Log.warin(this, ex.StackTrace);
			}
		}
		
	}
}
