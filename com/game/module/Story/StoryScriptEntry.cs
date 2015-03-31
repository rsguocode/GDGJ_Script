//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryScriptEntry
//文件描述：
//创建者：黄江军
//创建日期：2014-01-10
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using com.game.vo;
using com.game.module.map;
using com.game.utils;

namespace Com.Game.Module.Story
{
	public interface IParseNode
	{
		void ParseNode(XMLNode node);
	}

	//Base Entry
	public class ScriptBaseEntry : IParseNode
	{
		public string ScriptName;
		private string comparator;
		private string value;

		public virtual void ParseNode(XMLNode node)
		{
			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				string comparator = conditionNode.GetValue("@comparator");
				string value = conditionNode.GetValue("@value");	
				
				if (null != comparator)
				{
					this.comparator = comparator;
				}
				
				if (null != value)
				{
					this.value = value;
				}
			}
		}

		protected bool LevelMatch()
		{
			bool comparatorInvalid = (null == comparator || string.Empty == comparator);
			bool valueInvalid = (null == value || string.Empty == value);
			
			if (comparatorInvalid || valueInvalid)
			{
				return true;
			}

			int curLevel = MeVo.instance.Level;
			int goalLevel = Convert.ToInt32(value);

			if (">" == comparator)
			{
				return curLevel > goalLevel; 
			}
			else if ("<" == comparator)
			{
				return curLevel < goalLevel; 
			}
			else if (">=" == comparator)
			{
				return curLevel >= goalLevel; 
			}
			else if ("<=" == comparator)
			{
				return curLevel <= goalLevel; 
			}
			else if ("=" == comparator)
			{
				return curLevel == goalLevel; 
			}
			else
			{
				return false;
			}
		}

		public override bool Equals(object other)
		{
			ScriptBaseEntry otherBaseEntry = other as ScriptBaseEntry;
			if (null != otherBaseEntry)
			{				
				return LevelMatch();
			}
			else
			{
				return false;
			}
		}
	}

	//Base Scene 
	public class ScriptBaseSceneEntry : ScriptBaseEntry
	{
		public string SceneId;
		
		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);

			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				string scenneId = conditionNode.GetValue("@sceneId");

				if (null != scenneId)
				{
					this.SceneId = scenneId;
				}
			}
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				ScriptBaseSceneEntry otherBaseSceneEntry = other as ScriptBaseSceneEntry;
				if (null != otherBaseSceneEntry)
				{
					bool sceneIdEqual = (this.SceneId == otherBaseSceneEntry.SceneId);					
					return sceneIdEqual;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	//入场景Entry
	public class ScriptEnterSceneEntry : ScriptBaseSceneEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptEnterSceneEntry);
			}
			else
			{
				return false;
			}
		}
	}

	//出场景Entry
	public class ScriptExitSceneEntry : ScriptBaseSceneEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptExitSceneEntry);
			}
			else
			{
				return false;
			}
		}
	}

	//在副本某阶段Base Stage
	public class ScriptBaseStageEntry : ScriptBaseSceneEntry
	{
		public string StageId;

		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);

			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				string stageId = conditionNode.GetValue("@stageId");
				
				if (null != stageId)
				{
					this.StageId = stageId;
				}
			}
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				ScriptBaseStageEntry otherBaseStageEntry = other as ScriptBaseStageEntry;
				if (null != otherBaseStageEntry)
				{
					return (this.StageId == otherBaseStageEntry.StageId);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	//副本某阶段开始
	public class ScriptStartStageEntry : ScriptBaseStageEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptStartStageEntry);
			}
			else
			{
				return false;
			}
		}
	}

	//副本某阶段战斗
	public class ScriptFightStageEntry : ScriptBaseStageEntry
	{
		public string MonsterId;

		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);
			
			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				string monsterId = conditionNode.GetValue("@monsterId");
				
				if (null != monsterId)
				{
					this.MonsterId = monsterId;
				}
			}
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				ScriptFightStageEntry otherFightStageEntry = other as ScriptFightStageEntry;
				if (null != otherFightStageEntry)
				{
					return (this.MonsterId == otherFightStageEntry.MonsterId);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	//副本某阶段结束
	public class ScriptEndStageEntry : ScriptBaseStageEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptEndStageEntry);
			}
			else
			{
				return false;
			}
		}
	}
	
	//Base Task 
	public class ScriptBaseTaskEntry : ScriptBaseEntry
	{
		public string TaskId;
		
		public override void ParseNode(XMLNode node)
		{
			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				this.TaskId = conditionNode.GetValue("@value");
			}
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				ScriptBaseTaskEntry otherBaseTaskEntry = other as ScriptBaseTaskEntry;
				if (null != otherBaseTaskEntry)
				{
					return (this.TaskId == otherBaseTaskEntry.TaskId);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	//接受任务Entry
	public class ScriptReveiveTaskEntry : ScriptBaseTaskEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptReveiveTaskEntry);
			}
			else
			{
				return false;
			}
		}
	}
	
	//完成任务Entry
	public class ScriptFinishTaskEntry : ScriptBaseTaskEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptFinishTaskEntry);
			}
			else
			{
				return false;
			}
		}
	}

	//玩家升级Entry
	public class ScriptLevelUpEntry : ScriptBaseEntry
	{
		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				return (other is ScriptLevelUpEntry);
			}
			else
			{
				return false;
			}
		}
	}

	//玩家血量更改Entry
	public class ScriptHpChangeEntry : ScriptBaseEntry
	{
		public string RoleType = String.Empty;
		public string Id = String.Empty;
		private string hpComparator;
		private string hpValue;

		private bool HpChangeMatch()
		{
			bool comparatorInvalid = (null == hpComparator || string.Empty == hpComparator);
			bool valueInvalid = (null == hpValue || string.Empty == hpValue);

			if (comparatorInvalid || valueInvalid)
			{
				return true;
			}

			uint curHp = MeVo.instance.CurHp;
			uint goalHp = Convert.ToUInt32(hpValue);

			if (RoleType.Equals(StoryConst.MONSTER))
			{
				MonsterVo monsterVo = MonsterMgr.Instance.GetMonsterWithConfigId(Id);
				if (null != monsterVo)
				{
					curHp = monsterVo.CurHp;
				}
				else
				{
					return false;
				}
			}			

			//比较
			if (">" == hpComparator)
			{
				return curHp > goalHp; 
			}
			else if ("<" == hpComparator)
			{
				return curHp < goalHp; 
			}
			else if (">=" == hpComparator)
			{
				return curHp >= goalHp; 
			}
			else if ("<=" == hpComparator)
			{
				return curHp <= goalHp; 
			}
			else if ("=" == hpComparator)
			{
				return curHp == goalHp; 
			}
			else
			{
				return false;
			}			
		}

		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);
			
			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				string comparator = conditionNode.GetValue("@comparator");
				string value = conditionNode.GetValue("@value");
				string roleType = conditionNode.GetValue("@roleType");
				string id = conditionNode.GetValue("@id");
				
				if (null != comparator)
				{
					this.hpComparator = comparator;
				}

				if (null != value)
				{
					this.hpValue = value;
				}

				if (null != roleType)
				{
					this.RoleType = roleType;
				}

				if (null != id)
				{
					this.Id = id;
				}
			}
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{		
				ScriptHpChangeEntry otherHpChangeEntry = other as ScriptHpChangeEntry;
				if (null != otherHpChangeEntry)
				{
					bool roleTypeEqual = (RoleType.Equals(otherHpChangeEntry.RoleType));
					bool idEqual = (Id.Equals(otherHpChangeEntry.Id));
					bool result = (roleTypeEqual && idEqual && HpChangeMatch());

					return result;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	//玩家到达某区域Entry
	public class ScriptHeroArriveAreaEntry : ScriptBaseSceneEntry
	{
		public Vector3 Position;
		private string value;
		private float bottomLeftX;
		private float bottomLeftY;
		private float topRightX;
		private float topRightY;

		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);

			foreach (XMLNode conditionNode in node.GetNodeList(StoryConst.CONDITION_NAME))
			{
				this.value = conditionNode.GetValue("@value");
			}

			ParseCoordinate();
		}

		private void ParseCoordinate()
		{
			string[] coords = StringUtils.GetValueListFromString(value, '|');

			if (coords.Length < 2)
			{
				return;
			}

			string[] bottomLeftCoords = StringUtils.GetValueListFromString(coords[0]);
			string[] topRightCoords = StringUtils.GetValueListFromString(coords[1]);

			bottomLeftX = Convert.ToSingle(bottomLeftCoords[0]);
			bottomLeftY = Convert.ToSingle(bottomLeftCoords[1]);

			topRightX = Convert.ToSingle(topRightCoords[0]);
			topRightY = Convert.ToSingle(topRightCoords[1]);
		}

		private bool InArea(Vector3 pos)
		{
			if (pos.x < bottomLeftX || pos.x > topRightX)
			{
				return false;
			}

			if (pos.y < bottomLeftY || pos.y > topRightY)
			{
				return false;
			}

			return true;
		}

		public override bool Equals(object other)
		{
			if (base.Equals(other))
			{
				ScriptHeroArriveAreaEntry heroArriveAreaEntry = other as ScriptHeroArriveAreaEntry;
				if (null != heroArriveAreaEntry)
				{
					return InArea(heroArriveAreaEntry.Position);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

}
