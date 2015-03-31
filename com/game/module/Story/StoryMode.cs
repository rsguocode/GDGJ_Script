//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryMode
//文件描述：剧情模型类
//创建者：黄江军
//创建日期：2013-12-12
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager; 
using com.game.data;
using com.game.utils;
using com.game;

namespace Com.Game.Module.Story
{
	public delegate void LoadActionData();

	public class StoryMode : BaseMode<StoryMode>  
	{
		public readonly int FORCE_STOP_STORY = 1;  //异常情况下强制关闭剧情

		private LoadActionData loadActionDataCallback;
		private ScriptBaseEntry curScriptEntry;
		private bool loadingData = false;
		private IList<ScriptBaseEntry> scriptEntryList;
		private Dictionary<string, TextAsset> scriptDict;
		private IList<BaseAction> actionList;

		private bool roleGoActive = true;

		public bool RoleGoActive
		{
			get {return roleGoActive;}
		}

		public IList<BaseAction> ActionList
		{
			get
			{
				return actionList;
			}
		}

		public bool LoadedOk
		{
			get
			{
				return (null != curScriptEntry);
			}
		}
 
		public StoryMode() 
		{
			scriptEntryList = new List<ScriptBaseEntry>();
			actionList = new List<BaseAction>();
			scriptDict = new Dictionary<string, TextAsset>();
		}

		private string GetNPCAtlasUrl(string atlasName)
		{
			return "Atlas/Npc/" + atlasName + ".assetbundle";
		}
		
		private string GetBgAtlasUrl(string atlasName)
		{
			return "Atlas/StoryGround/" + atlasName + ".assetbundle";
		}

		public void Init()
		{
			string scriptTemplateUrl = UrlUtils.GetStoryScriptUrl("script_t");
			AssetManager.Instance.LoadAsset<TextAsset>(scriptTemplateUrl, ScriptTemplateLoaded);
		}

		public void ClearActions()
		{
			//Action清理动作
			foreach (BaseAction action in actionList)
			{
				action.Clear();
			}
			actionList.Clear();
		}

		public bool StoryExits(ScriptBaseEntry baseEntry)
		{
			foreach(ScriptBaseEntry entry in scriptEntryList)
			{
				if (entry.Equals(baseEntry))
				{
					return true;
				}
			}

			return false;

		}

		//获得脚本Action数据
		public void LoadActionData(ScriptBaseEntry baseEntry, LoadActionData loadDataCallback)
		{
			//如果正在加载剧情数据，则不能播放下一个剧情
			if (loadingData)
			{
				if (null != loadDataCallback)
				{
					loadDataCallback();
				}

				return;
			}

			loadingData = true;
			curScriptEntry = null;

			foreach(ScriptBaseEntry entry in scriptEntryList)
			{
				if (entry.Equals(baseEntry))
				{
					curScriptEntry = entry;
					actionList.Clear();

					//加载Action数据
					this.loadActionDataCallback = loadDataCallback;
					string scriptUrl = UrlUtils.GetStoryScriptUrl(entry.ScriptName);
					AssetManager.Instance.LoadAsset<TextAsset>(scriptUrl, ScriptLoaded);
					break;
				}
			}

			//处理脚本不存在情况
			if (null == curScriptEntry)
			{
				loadingData = false;

				if (null != loadDataCallback)
				{
					loadDataCallback();
				}
			}
		}

		//剧情脚本模板加载回调
		private void ScriptTemplateLoaded(TextAsset textObj)
		{
			if (null == textObj)
			{
				return;
			}

			string triggerName = "triggerTable>0>trigger";

			XMLNode rootNode  = XMLParser.Parse(textObj.ToString());
			foreach (XMLNode triggerNode in rootNode.GetNodeList(triggerName))
			{
				string scriptName = triggerNode.GetValue("@scriptName");
				string type = triggerNode.GetValue("@type");

				ScriptBaseEntry baseEntry = StoryFactory.GetEntry(type);
				if (null != baseEntry)
				{
					baseEntry.ScriptName = scriptName;
					baseEntry.ParseNode(triggerNode);
					scriptEntryList.Add(baseEntry);
				}
			}
		}

		//剧情加载回调
		private void ScriptLoaded(TextAsset textObj)
		{
			try
			{
				if (!scriptDict.ContainsKey(curScriptEntry.ScriptName))
				{
					if (null != textObj)
					{
						scriptDict.Add(curScriptEntry.ScriptName, textObj);
					}
				}

				//判断文件是否存在
				if (!scriptDict.ContainsKey(curScriptEntry.ScriptName))
				{
					return;
				}

				if (null != AppMap.Instance.me.Controller.GoName)
				{
					roleGoActive = AppMap.Instance.me.Controller.GoName.activeSelf;
				}

				//解析剧情数据
				string actionName = "script>0>action";	

				XMLNode rootNode  = XMLParser.Parse(scriptDict[curScriptEntry.ScriptName].ToString());
				if (null != rootNode)
				{
					foreach (XMLNode actionNode in rootNode.GetNodeList(actionName))
					{
						string type = actionNode.GetValue("@type");

						BaseAction baseAction = StoryFactory.GetAction(type);	
						if (null != baseAction)
						{
							baseAction.ParseNode(actionNode);
							actionList.Add(baseAction);
						}
					}
				}
			}
			finally
			{
				loadingData = false;

				if (null != loadActionDataCallback)
				{
					loadActionDataCallback();
				}
			}
		}

		//获得CreateAction对象
		public CreateAction GetCreateAction(string createId)
		{
			foreach (BaseAction action in actionList)
			{
				if (action is CreateAction)
				{
					CreateAction createAction = action as CreateAction;
					if (createAction.CreateId == createId)
					{
						return createAction;
					}
				}
			}

			return null;
		}

		//获得MoveAction对象
		public MoveAction GetMoveAction(string createId)
		{
			foreach (BaseAction action in actionList)
			{
				if (action is MoveAction)
				{
					MoveAction moveAction = action as MoveAction;
					if (moveAction.CreateId == createId)
					{
						return moveAction;
					}
				}
			}
			
			return null;
		}
		
	}
}
