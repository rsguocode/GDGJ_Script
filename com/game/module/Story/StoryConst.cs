//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryConst
//文件描述：
//创建者：黄江军
//创建日期：2014-01-11
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Story
{
	public class StoryConst 
	{
		//触发主要条件
		public const string TRIG_ENTER_SCENE = "enter_scene";
		public const string TRIG_EXIT_SCENE = "exit_scene";
		public const string TRIG_START_STAGE = "start_stage";
		public const string TRIG_FIGHT_STAGE = "fight_stage";
		public const string TRIG_END_STAGE = "end_stage";
		public const string TRIG_TASK_ADD = "quest_add";
		public const string TRIG_TASK_FINISHED = "quest_finished";
		public const string TRIG_LEVEL_UP = "level_up";
		public const string TRIG_HP_CHANGE = "hp_change";
		public const string TRIG_HERO_ARRIVE_AREA = "hero_arrive_area";

		//触发次要条件
		public const string CONDITION_NAME = "condition";

		//脚本行为
		public const string ACTION_TALK = "talk";
		public const string ACTION_EFFECT = "effect";
		public const string ACTION_FULL_EFFECT = "full_effect";
		public const string ACTION_CREATE = "create";
		public const string ACTION_MOVE = "move";
		public const string ACTION_VISIBLE = "visible";
		public const string ACTION_REMOVE = "remove";
		public const string ACTION_DIRECTION = "direction";
		public const string ACTION_POSE = "pose";
		public const string ACTION_SPELL = "spell";
		public const string ACTION_DELAY = "delay";
		public const string ACTION_HORSE = "horse";
		public const string ACTION_MOVIE = "movie";
		public const string ACTION_ASIDE = "aside";
		public const string ACTION_CAMERA_MOVE = "cam_move";
		public const string ACTION_CAMERA_SHOCK = "cam_shock";
		public const string ACTION_CAMERA_ZOOM = "cam_zoom";
		public const string ACTION_EMOTION = "emotion";
		public const string ACTION_MAP_CHANGE = "mapChange";

		//主角ID
		public const string SELF_ID = "-1";
		//怪物类型
		public const string MONSTER = "monster";
		//怪物方向
		public const string LEFT = "1";
		
	}
}
