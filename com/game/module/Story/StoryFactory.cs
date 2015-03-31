//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryFactory
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Story
{
	public class StoryFactory 
	{	
		//创建Entry
		public static ScriptBaseEntry GetEntry(string type)
		{
			switch(type)
			{
			case StoryConst.TRIG_ENTER_SCENE:
				return new ScriptEnterSceneEntry();

			case StoryConst.TRIG_EXIT_SCENE:
				return new ScriptExitSceneEntry();

			case StoryConst.TRIG_START_STAGE:
				return new ScriptStartStageEntry();

			case StoryConst.TRIG_FIGHT_STAGE:
				return new ScriptFightStageEntry();

			case StoryConst.TRIG_END_STAGE:
				return new ScriptEndStageEntry();

			case StoryConst.TRIG_TASK_ADD:
				return new ScriptReveiveTaskEntry();

			case StoryConst.TRIG_TASK_FINISHED:
				return new ScriptFinishTaskEntry();

			case StoryConst.TRIG_LEVEL_UP:
				return new ScriptLevelUpEntry();

			case StoryConst.TRIG_HP_CHANGE:
				return new ScriptHpChangeEntry();

			case StoryConst.TRIG_HERO_ARRIVE_AREA:
				return new ScriptHeroArriveAreaEntry();

			default:
				return null;
			}
		}

		//创建Action
		public static BaseAction GetAction(string type)
		{
			switch (type)
			{
			case StoryConst.ACTION_TALK:
				return new TalkAction();

			case StoryConst.ACTION_EFFECT:
				return new EffectAction();

			case StoryConst.ACTION_FULL_EFFECT:
				return new FullEffectAction();

			case StoryConst.ACTION_CREATE:
				return new CreateAction();

			case StoryConst.ACTION_MOVE:
				return new MoveAction();

			case StoryConst.ACTION_VISIBLE:
				return new VisibleAction();

			case StoryConst.ACTION_REMOVE:
				return new RemoveAction();

			case StoryConst.ACTION_DIRECTION:
				return new DirectionAction();

			case StoryConst.ACTION_POSE:
				return new PoseAction();

			case StoryConst.ACTION_SPELL:
				return new SpellAction();

			case StoryConst.ACTION_DELAY:
				return new DelayAction();

			case StoryConst.ACTION_HORSE:
				return new HorseAction();

			case StoryConst.ACTION_MOVIE:
				return new MovieAction();

			case StoryConst.ACTION_ASIDE:
				return new AsideAction();

			case StoryConst.ACTION_CAMERA_MOVE:
				return new CameraMoveAction();

			case StoryConst.ACTION_CAMERA_SHOCK:
				return new CameraShockAction();

			case StoryConst.ACTION_CAMERA_ZOOM:
				return new CameraZoomAction();

			case StoryConst.ACTION_EMOTION:
				return new EmotionAction();

			case StoryConst.ACTION_MAP_CHANGE:
				return new MapChangeAction();

			default:
				return null;
			}
		}
		
	}
}
