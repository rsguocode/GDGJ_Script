//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryControl
//文件描述：剧情控制类
//创建者：黄江军
//创建日期：2013-12-12
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;

namespace Com.Game.Module.Story
{
	public class StoryControl : BaseControl<StoryControl> 
	{
		private ClosedCallback closedCallback;

		//播放入场景剧情
		public bool PlayEnterSceneStory(uint mapId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;

			ScriptEnterSceneEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_ENTER_SCENE) as ScriptEnterSceneEntry;
			entry.SceneId = mapId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放出场景剧情
		public bool PlayExitSceneStory(uint mapId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;

			ScriptExitSceneEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_EXIT_SCENE) as ScriptExitSceneEntry;
			entry.SceneId = mapId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放阶段开始剧情
		public bool PlayStartStageStory(uint mapId, uint stageId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptStartStageEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_START_STAGE) as ScriptStartStageEntry;
			entry.SceneId = mapId.ToString();
			entry.StageId = stageId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放阶段战斗剧情
		public bool PlayFightStageStory(uint mapId, uint stageId, uint monsterId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptFightStageEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_FIGHT_STAGE) as ScriptFightStageEntry;
			entry.SceneId = mapId.ToString();
			entry.StageId = stageId.ToString();
			entry.MonsterId = monsterId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放阶段结束剧情
		public bool PlayEndStageStory(uint mapId, uint stageId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptEndStageEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_END_STAGE) as ScriptEndStageEntry;
			entry.SceneId = mapId.ToString();
			entry.StageId = stageId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放接受任务后剧情
		public bool PlayReceiveTaskStory(uint taskId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptReveiveTaskEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_TASK_ADD) as ScriptReveiveTaskEntry;
			entry.TaskId = taskId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放完成任务后剧情
		public bool PlayFinishTaskStory(uint taskId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptFinishTaskEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_TASK_FINISHED) as ScriptFinishTaskEntry;
			entry.TaskId = taskId.ToString();
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}

			return result;
		}

		//播放玩家升级后剧情
		public bool PlayLevelUpStory(ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptLevelUpEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_LEVEL_UP) as ScriptLevelUpEntry;
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}
			
			return result;
		}

		//播放主角HP更改剧情
		public bool PlayHeroHpChangeStory(ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptHpChangeEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_HP_CHANGE) as ScriptHpChangeEntry;
			entry.RoleType = string.Empty;
			entry.Id = StoryConst.SELF_ID;
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}
			
			return result;
		}

		//播放怪物HP更改剧情
		public bool PlayMonsterHpChangeStory(string monsterId, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptHpChangeEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_HP_CHANGE) as ScriptHpChangeEntry;
			entry.RoleType = StoryConst.MONSTER;
			entry.Id = monsterId;
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}
			
			return result;
		}

		//播放主角走到某区域剧情
		public bool PlayHeroArriveStory(uint mapId, Vector3 pos, ClosedCallback callBack)
		{
			bool result = false;
			closedCallback = callBack;
			
			ScriptHeroArriveAreaEntry entry = StoryFactory.GetEntry(StoryConst.TRIG_HERO_ARRIVE_AREA) as ScriptHeroArriveAreaEntry;
			entry.SceneId = mapId.ToString();
			entry.Position = pos;
			result = Singleton<StoryMode>.Instance.StoryExits(entry);

			if (result)
			{
				Singleton<StoryMode>.Instance.LoadActionData(entry, LoadDataCallback);
			}
			
			return result;
		}

		//剧情相关数据加载完毕后开始播放剧情
		private void LoadDataCallback()
		{
			//如果有配剧情，则播放剧情
			if (Singleton<StoryMode>.Instance.LoadedOk)
			{
				Singleton<StoryView>.Instance.ViewClosedCallback = closedCallback;
				Singleton<StoryView>.Instance.OpenView();
			}
			else
			{
				if (null != closedCallback)
				{
					closedCallback();
				}
			}
		}
	}
}
