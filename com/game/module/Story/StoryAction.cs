//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryAction
//文件描述：
//创建者：黄江军
//创建日期：2014-01-10
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using com.game.data;
using Com.Game.Module.Manager;
using com.game.module.effect;
using Com.Game.Public.EffectCameraManage;
using com.game.module.test;
using com.game.manager;
using com.game.module;
using com.game.vo;
using com.game;
using com.u3d.bases.consts;
using com.game.utils;
using com.u3d.bases.display.controler;
using Com.Game.Module.Role;
using com.game.module.map;
using com.u3d.bases.debug;
using Com.Game.Module.NPCBust;
using com.game.vo;
using com.u3d.bases.display;
using com.u3d.bases.controller;
using com.game.module.loading;

namespace Com.Game.Module.Story
{
	public class BaseAction : IParseNode
	{
		protected bool block;
		protected bool finished;

		protected virtual void Finish()
		{
			finished = true;
		}

		//播放中是否可被打断
		public virtual bool CanInterrupt
		{
			get	{return true;}
		}

		//是否需要手工切换
		public virtual bool MannualSwitch
		{
			get	{return false;}
		}

		//是否可以手工切换
		public virtual bool CanMannualSwitch
		{
			get	{return false;}
		}

		//动作运行
		public virtual void Run()
		{
		}

		//动作停止
		public virtual void Stop()
		{
		}

		//动作切换后处理
		public virtual void AfterPlayed()
		{
		}

		//节点解析
		public virtual void ParseNode(XMLNode node)
		{
		}

		//清理
		public virtual void Clear()
		{
		}
	}

	//对话Action
	public class TalkAction : BaseAction
	{
		protected string npcId;
		protected string words;
		protected float delay;
		private bool bustVisible = false;
		private UISprite sprNPC;
		protected GameObject talkWindow = null;
		private GameObject bust = null;
		private GameObject roleBust = null;

		public override bool CanInterrupt
		{
			get	{return (finished && bustVisible);}
		}

		public override bool MannualSwitch
		{
			get	{return true;}
		}

		public override bool CanMannualSwitch
		{
			get	{return bustVisible;}
		}

		public override void ParseNode(XMLNode node)
		{
			this.npcId = node.GetValue("@id");
			this.words = node.GetValue("@text");
			this.delay = Convert.ToSingle(node.GetValue("@delay"));
		}

		public override void Run()
		{
			UILabel labName;
			UILabel labWords;

			//主角显示在左侧
			if (StoryConst.SELF_ID == this.npcId)
			{
				Singleton<StoryView>.Instance.LeftNPC.SetActive(true);
				Singleton<StoryView>.Instance.RightNPC.SetActive(false);

				sprNPC = Singleton<StoryView>.Instance.LeftNPCSprite;
				labName = Singleton<StoryView>.Instance.LeftNPCName;
				labWords = Singleton<StoryView>.Instance.LeftNPCWords;
				NPCBustMgr.Instance.GetBust(MeVo.instance.BustUrl, RoleBustLoaded);
			}
			//NPC显示在右侧
			else
			{
				Singleton<StoryView>.Instance.LeftNPC.SetActive(false);
				Singleton<StoryView>.Instance.RightNPC.SetActive(true);

				sprNPC = Singleton<StoryView>.Instance.RightNPCSprite;
				labName = Singleton<StoryView>.Instance.RightNPCName;
				labWords = Singleton<StoryView>.Instance.RightNPCWords;
			}

			talkWindow = Singleton<StoryView>.Instance.TalkWindow;
			
			//显示对话框
			if (!talkWindow.activeSelf)
			{
				talkWindow.SetActive(true);
			}

			//打字机不能重复利用，需要先删除再创建
			TypewriterEffect typewriter = labWords.GetComponent<TypewriterEffect>();
			if (null != typewriter)
			{
				GameObject.Destroy(typewriter);
			}
			
			//获得头像、主角名字
			string spriteName = ""; 
			string roleName = "";
			if (StoryConst.SELF_ID == this.npcId)
			{
				//主角头像
				spriteName = Singleton<RoleMode>.Instance.GetPlayerHeadSpriteName(MeVo.instance.job);
				roleName = MeVo.instance.Name;
			}
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(this.npcId);
				if (null != action)
				{
					string resourceId = action.ResourceId;

					//为怪物
					if (action.IsMonster)
					{
						SysMonsterVo vo = BaseDataMgr.instance.getSysMonsterVo(Convert.ToUInt32(resourceId));
						if (null != vo)
						{
							spriteName = vo.icon.ToString();
							roleName = vo.name;
						}

					}
					//为NPC
					else
					{
						SysNpcVo vo = BaseDataMgr.instance.GetNpcVoById(Convert.ToUInt32(resourceId));
						if (null != vo)
						{
							spriteName = vo.halfImgSprite;
							roleName = vo.name;
						}
					}

					bust = action.Bust;
				}
			}

			sprNPC.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
			sprNPC.spriteName = spriteName;
			sprNPC.SetActive(false);
			labName.text = roleName;
			labWords.text = this.words;

			if (null != bust)
			{
				bust.SetActive(true);
				bustVisible = true;
			}

			if (delay > 0f)
			{
				vp_Timer.In(delay, Finish, 1, 0);
			}

			//增加打字效果(测试先屏蔽)
			//typewriter = labWords.gameObject.AddComponent<TypewriterEffect>();
			//typewriter.charsPerSecond = 20;
			//typewriter.DoTypeEnd = Finish;
		}	

		private void RoleBustLoaded(GameObject bustObj)
		{
			roleBust = bustObj;
			roleBust.name = "my_bust";
			roleBust.transform.position = sprNPC.transform.position;	
			MeVo.instance.InitRoleBustPosition(roleBust);
			roleBust.SetActive(true);
			bustVisible = true;
		}

		public override void Clear()
		{
			Finish();
		}

		public override void AfterPlayed()
		{
			talkWindow.SetActive(false);

			if (null != bust)
			{
				bust.SetActive(false);
			}

			if (null != roleBust)
			{
				roleBust.SetActive(false);
			}
		}
	}
	
	//场景特效Action
	public class EffectAction : BaseAction
	{
		private string key;
		private string effectId;
		private float startX;
		private float startY;
		private float time;
		private float targetX;
		private float targetY;

		public override bool CanInterrupt
		{
			get
			{
				return finished;
			}
		}

		public override void ParseNode(XMLNode node)
		{
			this.key = node.GetValue("@key");
			this.startX = Convert.ToSingle(node.GetValue("@startX"));
			this.startY = Convert.ToSingle(node.GetValue("@startY"));
			this.time = Convert.ToSingle(node.GetValue("@time"))/30;
			this.targetX = Convert.ToSingle(node.GetValue("@targetX"));
			this.targetY = Convert.ToSingle(node.GetValue("@targetY"));
			this.effectId = this.key;
		}

		private string GetEffectId()
		{
			switch (this.key)
			{
			case "key1":
				return "100001";

			case "key2":
				return "100002";

			case "key3":
				return "100003";

			default:
				return "";
			}
		}

		public override void Run()
		{
			Vector3 pos = Vector3.zero;
			pos.x = this.startX;
			pos.y = this.startY;
			EffectMgr.Instance.CreateStorySceneEffect(effectId, pos, null, CreatedCallback);
			vp_Timer.In(time, Finish, 1, 0);
		}

		private void CreatedCallback(GameObject effectObj)
		{
			float z = effectObj.transform.localPosition.z;

			TweenPosition tweenPosition = effectObj.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}
			
			tweenPosition = effectObj.AddComponent<TweenPosition>();
			tweenPosition.from = new Vector3(startX, startY, z);
			tweenPosition.to = new Vector3(targetX, targetY, z);
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = time;
		}

		protected override void Finish()
		{
			base.Finish();
			EffectMgr.Instance.RemoveEffect(UrlUtils.GetStorySceneEffectUrl(effectId));
		} 
	}
	
	//全屏特效Action
	public class FullEffectAction : BaseAction
	{
		private string key;
		private float duration;
		private float fadeInTime;
		private float fadeOutTime;
		private float fadeInAlpha;
		private float fadeOutAlpha;
		private UISprite sprFullEffect;

		public override bool CanInterrupt
		{
			get
			{
				return finished;
			}
		}

		private string effectSpriteName
		{
			get
			{
				switch (key)
				{
				case "black":
					return "black";
				case "white":
					return "baise";
				default:
					return "";
				}
			}
		}

		public override void ParseNode(XMLNode node)
		{
			this.key = node.GetValue("@key");
			this.duration = Convert.ToSingle(node.GetValue("@duration"));
			this.fadeInTime = Convert.ToSingle(node.GetValue("@fadeIn"));
			this.fadeOutTime = Convert.ToSingle(node.GetValue("@fadeOut"));
			this.fadeInAlpha = Convert.ToSingle(node.GetValue("@fadeInTo"));
			this.fadeOutAlpha = Convert.ToSingle(node.GetValue("@fadeOutTo"));
		}

		public override void Run()
		{
			sprFullEffect = Singleton<StoryView>.Instance.FullEffectSprite;
			sprFullEffect.spriteName = effectSpriteName;
			sprFullEffect.gameObject.SetActive(true);

			//alpha渐入
			FadeInAlpha();
			//中间延时并进行alpha渐出
			vp_Timer.In(fadeInTime + duration, FadeOutAlpha, 1, 0);
		}

		private void FadeInAlpha()
		{
			ChangeFullEffectAlpha(fadeInTime, 0f, fadeInAlpha);
		}

		private void FadeOutAlpha()
		{
			ChangeFullEffectAlpha(fadeOutTime, fadeInAlpha, fadeOutAlpha);
			vp_Timer.In(fadeOutTime, Finish, 1, 0);
		}

		private void ChangeFullEffectAlpha(float time, float fromAlpha, float toAlpha)
		{			
			TweenAlpha tweenAlpha = sprFullEffect.GetComponent<TweenAlpha>();
			if (null != tweenAlpha)
			{
				GameObject.Destroy(tweenAlpha);
			}
			
			tweenAlpha = sprFullEffect.gameObject.AddComponent<TweenAlpha>();
			tweenAlpha.from = fromAlpha;
			tweenAlpha.to = toAlpha;
			tweenAlpha.style = UITweener.Style.Once;
			tweenAlpha.method = UITweener.Method.QuintEaseInOut;
			tweenAlpha.duration = time;
		}

		public override void AfterPlayed()
		{
			sprFullEffect.SetActive(false);
		}
	}

	//上下坐骑Action
	public class HorseAction : BaseAction
	{
		private string state;
		
		public override void ParseNode(XMLNode node)
		{
			this.state = node.GetValue("@state");
		}
		
		public override void Run()
		{
		}
	}

	//旁白Action
	public class AsideAction : BaseAction
	{
		private string text;
		private float duration;

		private GameObject asideWindow;
		private UILabel labAsideText;
		
		public override bool CanInterrupt
		{
			get
			{
				return finished;
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.text = node.GetValue("@text");
			this.duration = Convert.ToSingle(node.GetValue("@duration"));

			if (duration <= 0f)
			{
				duration = 2f;
			}
		}
		
		public override void Run()
		{
			asideWindow = Singleton<StoryView>.Instance.AsideWindow;
			labAsideText = Singleton<StoryView>.Instance.AsideText;
			labAsideText.text = text;
			asideWindow.SetActive(true);

			vp_Timer.In(duration, Finish, 1, 0);
		}	

		public override void AfterPlayed()
		{
			asideWindow.SetActive(false);
		}
	}

	//播动画片段Action
	public class MovieAction : BaseAction
	{
		private string key;
		
		public override bool CanInterrupt
		{
			get
			{
				return EffectMgr.Instance.IsStoryMovieEffectStopped(key);
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.key = node.GetValue("@key");
		}
		
		public override void Run()
		{
			Vector3 pos = Singleton<EffectCameraMgr>.Instance.CameraPosition;
			EffectMgr.Instance.CreateStoryMovieEffect(this.key, pos);
		}
	}

	//摄像机移动Action
	public class CameraMoveAction : BaseAction
	{
		private float targetX;
		private float targetY;
		private string followId;
		private bool reset;
		private int speed;

		private GameObject role;
		
		public override bool CanInterrupt
		{
			get
			{
				if (block)
				{
					return true;
				}

				StoryCameraMove cameraMove = Camera.main.gameObject.GetComponent<StoryCameraMove>();
				if (null == cameraMove || cameraMove.ExceedBorder)
				{
					return true;
				}

				//摄像机不跟随
				if (null == role)
				{
					return (Camera.main.transform.position.x == targetX);
				}
				//摄像机跟随
				else
				{
					MoveAction moveAction = Singleton<StoryMode>.Instance.GetMoveAction(followId);
					if (null != moveAction)
					{
						return (role.transform.position.x == moveAction.TargetX);
					}
					else
					{
						return true;
					}
				}
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.targetX = Convert.ToSingle(node.GetValue("@targetX"));
			this.targetY = Convert.ToSingle(node.GetValue("@targetY"));
			this.followId = node.GetValue("@followId");
			this.reset = Convert.ToBoolean(node.GetValue("@reset"));
			this.speed = Convert.ToInt32(node.GetValue("@speed"));
			this.block = Convert.ToBoolean(node.GetValue("@block"));
		}
		
		public override void Run()
		{
			//主角
			if (StoryConst.SELF_ID == followId)
			{
				role = AppMap.Instance.me.Controller.gameObject;
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(followId);
				if (null != action)
				{
					if (action.IsMonster)
					{
						role = AppMap.Instance.GetMonster(action.MonsterId).Controller.gameObject;
					}
					else
					{
						role = action.Role;
					}
				}
			}

			//是否重置回Hero
			if (reset)
			{
				targetX = AppMap.Instance.me.Controller.transform.position.x;
			}

			DeleteCameraMoveComponent();
			StoryCameraMove cameraMove = Camera.main.gameObject.AddComponent<StoryCameraMove>();
			cameraMove.FollowTarget = role;
			cameraMove.MoveSpeed = speed;
			cameraMove.TargetPos.x = targetX;
		}

		private void DeleteCameraMoveComponent()
		{
			StoryCameraMove cameraMove = Camera.main.gameObject.GetComponent<StoryCameraMove>();
			if (null != cameraMove)
			{
				GameObject.Destroy(cameraMove);
			}
		}

		public override void Clear()
		{
			DeleteCameraMoveComponent();
		}

	}

	//摄像机震动Action
	public class CameraShockAction : BaseAction
	{
		private int shockType;
		private float duration;

		public override bool CanInterrupt
		{
			get
			{
				return finished;
			}
		}

		public override void ParseNode(XMLNode node)
		{
			this.shockType = Convert.ToInt32(node.GetValue("@shockType"));
			this.duration = Convert.ToSingle(node.GetValue("@duration"));
		}

		public override void Run()
		{
			CameraEffectManager.TweenShakeCamera(0f, duration);
			vp_Timer.In(duration, Finish, 1, 0);
		}
	}

	//摄像机缩放Action
	public class CameraZoomAction : BaseAction
	{
		private float value;
		private float duration;
		
		public override bool CanInterrupt
		{
			get
			{
				return (block || finished);
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.value = Convert.ToSingle(node.GetValue("@value"));
			this.duration = Convert.ToSingle(node.GetValue("@duration"));
			this.block = Convert.ToBoolean(node.GetValue("@block"));

			if (value > 0f)
			{
				value = 1/value;
			}
		}
		
		public override void Run()
		{
			MapControl.Instance.MyCamera.ZoomCamera(value, duration);
			vp_Timer.In(duration, Finish, 1, 0);
		}
	}

	//头顶表情Action
	public class EmotionAction : TalkAction
	{
		private string key;
		private UISprite sprEmotion;
		
		public override void ParseNode(XMLNode node)
		{
			base.ParseNode(node);
			this.key = node.GetValue("@key");
		}
		
		public override void Run()
		{
			base.Run();

			if (StoryConst.SELF_ID == this.npcId)
			{
				sprEmotion = Singleton<StoryView>.Instance.LeftEmotionSprite;
			}
			else
			{
				sprEmotion = Singleton<StoryView>.Instance.RightEmotionSprite;
			}

			sprEmotion.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Chat);
			sprEmotion.spriteName = "bq_" + key;
			sprEmotion.gameObject.SetActive(true);
		}

		public override void Clear()
		{
			Finish();
		}

		protected override void Finish()
		{
			base.Finish();
			sprEmotion.gameObject.SetActive(false);
		} 

		public override void AfterPlayed()
		{
			base.AfterPlayed();
			sprEmotion.gameObject.SetActive(false);
		}
	}

	//角色创建Action
	public class CreateAction : BaseAction
	{
		private string createId;
		private string resourceId;
		private float targetX;
		private float targetY;
		private string resourceType;
		private string dir;

		private GameObject roleGo = null;
		private GameObject bustGo = null;
		private bool modelLoaded = false;
		private bool bustLoaded = false;

		private const string RESOURCE_NPC = "npc";
		private const string RESOURCE_MONSTER = "monster";

		private static uint monsterCreatedNo = 1;

		private uint monsterId;

		public string MonsterId
		{
			get {return monsterId.ToString();}
		}

		public string CreateId
		{
			get {return createId;}
		}

		private string url
		{
			get
			{
				string modelUrl = null;

				if (RESOURCE_NPC == resourceType)
				{
					modelUrl = UrlUtils.npcModeUrl(resourceId);
				}
				else if (RESOURCE_MONSTER == resourceType)
				{
					SysMonsterVo vo = BaseDataMgr.instance.getSysMonsterVo(Convert.ToUInt32(resourceId));
					if (null != vo)
					{
						modelUrl = UrlUtils.monsterModeUrl(vo.res.ToString());
					}
				}

				return modelUrl;
			}
		}

		private string bustUrl
		{
			get
			{
				string bustUrl = null;
				
				if (RESOURCE_NPC == resourceType)
				{
					bustUrl = UrlUtils.npcBustUrl(resourceId);
				}
				else if (RESOURCE_MONSTER == resourceType)
				{
					SysMonsterVo vo = BaseDataMgr.instance.getSysMonsterVo(Convert.ToUInt32(resourceId));
					if (null != vo)
					{
						bustUrl = UrlUtils.monsterBustUrl(vo.res.ToString());
					}
				}
				
				return bustUrl;
			}
		}

		public GameObject Role
		{
			get	{return roleGo;}
		}

		public GameObject Bust
		{
			get	{return bustGo;}
		}

		public string ResourceId
		{
			get	{return resourceId;}
		}

		public bool IsMonster
		{
			get	{return (RESOURCE_MONSTER == resourceType);}
		}

		public override bool CanInterrupt
		{
			get	{return (modelLoaded && bustLoaded);}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.resourceId = node.GetValue("@resourceId");
			this.targetX = Convert.ToSingle(node.GetValue("@targetX"));
			this.targetY = Convert.ToSingle(node.GetValue("@targetY"));
			this.resourceType = node.GetValue("@resourceType");
			this.dir = node.GetValue("@dir");
		}
		
		public override void Run()
		{
			try
			{
				if (IsMonster)
				{
					CreateMonster();
				}
				else
				{
					AssetManager.Instance.LoadAsset<GameObject>(url, RoleLoaded);
				}

				NPCBustMgr.Instance.GetBust(bustUrl, BustLoaded);
			}
			catch (Exception e)
			{
				modelLoaded = true;
				bustLoaded = true;
				Log.error(this, "CreateAction error, exception is: " + e.StackTrace);
			}
		}

		private void CreateMonster()
		{
			MonsterVo monsterVo = new MonsterVo();
			monsterVo.monsterId = Convert.ToUInt32(resourceId);
			monsterId = monsterVo.monsterId * 10 + monsterCreatedNo;
			monsterVo.Id = monsterId;
			monsterVo.Name = resourceType + "_" + resourceId;
			monsterVo.X = targetX;
			monsterVo.Y = targetY;
			monsterVo.CurHp = 100000;
			monsterVo.ModelLoadCallBack = LoadMonsterCallback;
			AppMap.Instance.CreateMonster(monsterVo);
			monsterCreatedNo++;
		}

		private void LoadMonsterCallback(BaseDisplay display)
		{
			if (StoryConst.LEFT == dir)
			{
				display.ChangeDire(Directions.Left);
			}
			else
			{
				display.ChangeDire(Directions.Right);
			}

			MonsterAiController aiController = display.Controller.gameObject.GetComponent<MonsterAiController>();
			if (null != aiController)
			{
				GameObject.Destroy(aiController);
			}

			modelLoaded = true;
		}

		//模型加载回调
		private void RoleLoaded(GameObject obj)
		{
			try
			{
				if (null == obj)
				{
					return;
				}

				GameObject go = GameObject.Instantiate(obj) as GameObject;
				roleGo = new GameObject(resourceType + "_" + resourceId);
				go.transform.parent = roleGo.transform;
				go.AddComponent<StoryRoleEvent>();

				//设置怪物transform
				Vector3 defaultPos = new Vector3(0f, 0f, -30f);
				go.transform.localPosition = defaultPos;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localScale = Vector3.one;
				roleGo.transform.localPosition = new Vector3(targetX, targetY, targetY*5);
				roleGo.transform.localRotation = Quaternion.identity;

				//设置怪物方向
				Vector3 localScale;
				if (StoryConst.LEFT == dir)
				{
					localScale = new Vector3(-1f, 1f, 1f);
				}
				//右转
				else
				{
					localScale = Vector3.one;
				}
				roleGo.transform.localScale = localScale;

				AddNPCFootShadow(go);
			}
			finally
			{
				modelLoaded = true;
			}
		}

		private void AddNPCFootShadow(GameObject npcObj)
		{
			GameObject footShadow =	GameObject.Instantiate(CommonModelAssetManager.Instance.GetFootShadowGameObject()) as GameObject;
			float scaleFactor = 1.5f;
			Vector3 scale = Vector3.one*scaleFactor;
			Vector3 parentScale = npcObj.transform.localScale;

			BoxCollider2D boxCollider = npcObj.GetComponent<BoxCollider2D>();
			if (null != boxCollider)
			{
				scale.x = boxCollider.size.x*parentScale.x*scaleFactor;
				scale.y = boxCollider.size.y*parentScale.y*scaleFactor;
			}

			if (null != footShadow)
			{
				footShadow.transform.parent = npcObj.transform;
				footShadow.transform.localScale = scale;
				footShadow.transform.localPosition = Vector3.zero;
				footShadow.transform.localRotation = Quaternion.identity;
			}
		}

		//半身像加载回调
		private void BustLoaded(GameObject bustObj)
		{
			try
			{
				if (null == bustObj)
				{
					return;
				}

				bustGo = bustObj;
				bustGo.name = resourceType + "_" + resourceId + "_bust";
				bustGo.SetActive(false);
				bustGo.transform.position = Singleton<StoryView>.Instance.RightNPCSprite.transform.position;	

				if (NPCBustMgr.SpecialNPCId == resourceId)
				{
					bustGo.transform.localScale = new Vector3(NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor);
				}
				else
				{
					bustGo.transform.localScale = new Vector3(-NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor, NPCBustMgr.ZoomFactor);
				}
			}
			finally
			{
				bustLoaded = true;
			}
		}
		
		public override void Clear()
		{
			if (null != roleGo)
			{
				GameObject.Destroy(roleGo);
				roleGo = null;
			}

			if (null != bustGo)
			{
				bustGo.SetActive(false);
			}

			if (IsMonster)
			{
				MonsterMgr.Instance.RemoveMonster(MonsterId);
			}
		}
	}

	//角色移动Action
	public class MoveAction : BaseAction
	{
		private string createId;
		private float targetX;
		private float targetY;
		private int speed;

		private GameObject role;
		Vector3 targetPos = Vector3.zero;

		public string CreateId
		{
			get {return createId;}
		}

		public float TargetX
		{
			get {return targetX;}
		}
		
		public override bool CanInterrupt
		{
			get
			{
				//主角
				if (block)
				{
					return true;
				}

				if (StoryConst.SELF_ID == createId)
				{
					return (Status.IDLE == AppMap.Instance.me.Controller.StatuController.CurrentStatu);
				}
				//npc或者monster
				else
				{
					CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);

					if (null == action)
					{
						return true;
					}

					if (action.IsMonster)
					{
						return (Status.IDLE == AppMap.Instance.GetMonster(action.MonsterId).Controller.StatuController.CurrentStatu);
					}
					else
					{
						if (null != role)
						{
							Vector3 rolePos = role.transform.position;
							return (rolePos == targetPos);
						}
						else
						{
							return true;
						}
					}
				}
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.targetX = Convert.ToSingle(node.GetValue("@targetX"));
			this.targetY = Convert.ToSingle(node.GetValue("@targetY"));
			this.speed = Convert.ToInt32(node.GetValue("@speed"));
			this.block = Convert.ToBoolean(node.GetValue("@block"));

			if (this.speed <= 0f)
			{
				speed = Global.ROLE_RUN_SPEED;
			}
		}
		
		public override void Run()
		{
			targetPos = new Vector3(targetX, targetY, 0);

			//主角
			if (StoryConst.SELF_ID == createId)
			{
				(AppMap.Instance.me.Controller as ActionControler).MoveSpeed = speed;
				AppMap.Instance.me.Controller.MoveTo(targetX, targetY, null);
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);

				if (null == action)
				{
					return;
				}

				if (action.IsMonster)
				{
					(AppMap.Instance.GetMonster(action.MonsterId).Controller as ActionControler).MoveSpeed = speed;
					AppMap.Instance.GetMonster(action.MonsterId).Controller.MoveTo(targetPos.x, targetPos.y, null);
				}
				else				
				{
					role = action.Role;
					StoryRoleMove roleMove = role.GetComponent<StoryRoleMove>();

					if (null != roleMove)
					{
						GameObject.Destroy(roleMove);
					}

					roleMove = role.AddComponent<StoryRoleMove>();
					targetPos.z = role.transform.position.z;
					roleMove.TargetPos = targetPos;
					roleMove.MoveSpeed = speed;
				}
			}
		}

		public override void Stop()
		{			
			//主角
			if (StoryConst.SELF_ID == createId)
			{
				(AppMap.Instance.me.Controller as ActionControler).StopWalk();
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);

				if (null == action)
				{
					return;
				}

				if (action.IsMonster)
				{
					(AppMap.Instance.GetMonster(action.MonsterId).Controller as ActionControler).StopWalk();
				}
				else
				{
					role = action.Role;
					StoryRoleMove roleMove = role.GetComponent<StoryRoleMove>();
					if (null != roleMove)
					{
						roleMove.Stop();
					}
				}
			}
		}
	}

	//角色隐藏Action
	public class VisibleAction : BaseAction
	{
		private string createId;
		private bool visible;

		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.visible = Convert.ToBoolean(node.GetValue("@visible"));
		}

		private void ShowMe(bool visible)
		{
			AppMap.Instance.me.Controller.gameObject.SetActive(visible);

			if (Singleton<StoryMode>.Instance.RoleGoActive)
			{
				if (null != AppMap.Instance.me.Controller.GoName)
				{
					AppMap.Instance.me.Controller.GoName.SetActive(visible);
				}
			}

			GameObject autoRoad = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_AutoSerachRoad);
			if (null != autoRoad)
			{
				autoRoad.SetActive(false);
			} 
		}

		public override void Run()
		{			
			//主角
			if (StoryConst.SELF_ID == createId)
			{
				ShowMe(visible);
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);
				if (action.IsMonster)
				{
					AppMap.Instance.GetMonster(action.MonsterId).Controller.gameObject.SetActive(visible);
				}
				else
				{
					action.Role.SetActive(visible);
				}
			}
		}
	}

	//角色移除Action
	public class RemoveAction : BaseAction
	{
		private string createId;
		
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
		}
		
		public override void Run()
		{	
			CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);
			if (null != action)
			{
				action.Clear();
			}
		}
	}

	//角色转向Action
	public class DirectionAction : BaseAction
	{
		private string createId;
		private string dir;
		
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.dir = node.GetValue("@dir");
		}
		
		public override void Run()
		{	
			Vector3 localScale;
			if (StoryConst.LEFT == dir)
			{
				localScale = new Vector3(-1f, 1f, 1f);
			}
			//右转
			else
			{
				localScale = Vector3.one;
			}

			//主角
			if (StoryConst.SELF_ID == createId)
			{
				AppMap.Instance.me.Controller.transform.localScale = localScale;
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);
				if (action.IsMonster)
				{
					AppMap.Instance.GetMonster(action.MonsterId).Controller.transform.localScale = localScale;
				}
				else
				{
					action.Role.transform.localScale = localScale;
				}
			}
		}
	}

	//角色技能Action
	public class SpellAction : BaseAction
	{
		protected string createId;
		protected int skill;
		protected bool repeat;

		public override bool CanInterrupt
		{
			get
			{
				return finished;
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.skill = Convert.ToInt32(node.GetValue("@skill"));
			this.repeat = Convert.ToBoolean(node.GetValue("@repeat"));
		}

		private int GetSkillId(int skill)
		{
			int[] skillIds = {0, 1, 8, 0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15, 16, 7, 18};
			return skillIds[skill];
		}
		
		public override void Run()
		{
			//主角
			if (StoryConst.SELF_ID == createId)
			{
				if ((skill >= Status.ROLL && skill <= Status.SKILL3) || (Status.SKILL4 == skill))
				{
					AppMap.Instance.me.Controller.SkillController.RequestUseSkill(GetSkillId(skill));
				}
				else
				{
					AppMap.Instance.me.Controller.StatuController.SetStatu(skill);
				}
			}
			//npc或者monster
			else
			{
				CreateAction action =  Singleton<StoryMode>.Instance.GetCreateAction(createId);
				if (action.IsMonster)
				{
					if (skill >= Status.ATTACK1 && skill <= Status.ATTACK2)
					{
						AppMap.Instance.GetMonster(action.MonsterId).Controller.SkillController.RequestUseSkill(GetSkillId(skill));
					}
					else
					{
						AppMap.Instance.GetMonster(action.MonsterId).Controller.StatuController.SetStatu(skill);
					}
				}
				else
				{
					StoryRoleSpell roleSpell = action.Role.AddComponent<StoryRoleSpell>();
					roleSpell.Repeat = this.repeat;
					roleSpell.Skill = this.skill;
				}
			}

			vp_Timer.In(0.5f, Finish, 1, 0);
		}
	}

	//角色动作Action
	public class PoseAction : SpellAction
	{
		public override void ParseNode(XMLNode node)
		{
			this.createId = node.GetValue("@id");
			this.skill = Convert.ToInt32(node.GetValue("@pose"));
			this.repeat = false;
		}
	}

	//延时Action
	public class DelayAction : BaseAction
	{
		private float value;
		private float createTime;

		public override bool CanInterrupt
		{
			get
			{
				return (Time.time - createTime >= value);
			}
		}

		public override void ParseNode(XMLNode node)
		{
			this.value = Convert.ToSingle(node.GetValue("@value"));
			createTime = Time.time;
		}
	}

	//切换地图Action
	public class MapChangeAction : BaseAction
	{
		private uint mapId;
		
		public override bool CanInterrupt
		{
			get
			{
				return true;
			}
		}
		
		public override void ParseNode(XMLNode node)
		{
			this.mapId = Convert.ToUInt32(node.GetValue("@target_id"));
		}

		public override void Run()
		{
			Singleton<StartLoadingView>.Instance.OpenView();
			Singleton<StoryMode>.Instance.DataUpdate(Singleton<StoryMode>.Instance.FORCE_STOP_STORY);
			Singleton<MapMode>.Instance.changeScene(mapId, true, 5, 1.5f);
		}
	}


}
