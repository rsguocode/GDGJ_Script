using System;
using com.game.module.Guide;
using UnityEngine;
using com.game.module.main;
using com.u3d.bases.debug;
using com.u3d.bases.loader;

using com.game.module.test;
using com.game.module.map;
using com.utils;
/**普通副本UI**/
using com.game.Public.LocalVar;
using System.Collections.Generic;
using com.game.module.effect;
using com.game.utils;
using com.game.consts;
using Com.Game.Module.Role;
using com.game;
using com.u3d.bases.consts;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  世界地图UI
 * *******************************************************/
using com.game.vo;
using com.game.Public.UICommon;
using com.game.module.Task;
using com.game.manager;
using com.game.data;


namespace Com.Game.Module.Copy
{
	public class WorldMapView : BaseView<WorldMapView>
	{
		public override string url { get { return "UI/Copy/WorldMapView.assetbundle"; } }
		public override ViewLayer layerType
		{
			get { return ViewLayer.MiddleLayer; }
		}

		private readonly float MOVE_SPEED = 0.005f;
		private readonly int NORMAL_CITY_DEPTH = 1003;
		private readonly int GRAY_CITY_DEPTH = 1503;
		private Button btn_fanhui;
		private Transform worlds;
		private GameObject model;
		private UICenterOnChild center;
		private Transform centerOnGameObj;
		private UIScrollView scrollPannel;

		private uint targetMapId;
		private List<uint> opendWorld = new List<uint>();
		protected override void Init()
		{
			btn_fanhui = FindInChild<Button>("btn_fanhui");
			worlds = FindInChild<Transform>("WorldPannel/worlds");
			center = FindInChild<UICenterOnChild>("WorldPannel/map");
			centerOnGameObj = FindInChild<Transform>("CenterOnGameObj");
			scrollPannel = FindInChild<UIScrollView>("WorldPannel");
			btn_fanhui.onClick = this.BackOnClick;

			Singleton<CopyMode>.Instance.InitWorldMapInfo();
			this.InitWorldMapView ();
		}

		//初始化世界地图UI
		private void InitWorldMapView()
		{
			Log.info(this, "初始化世界地图");
			
			List<CopyVo> worldMapList = Singleton<CopyMode>.Instance.worldMapInitInfo.WorldPointInfoList;
			Transform world = worlds.FindChild("btn_world_1");
			this.InitWorldPoint(world, worldMapList[0]);
			
			for (int i = worlds.childCount; i < worldMapList.Count; ++i)
			{
				Transform clone = (GameObject.Instantiate(world.gameObject, 
				                                          world.position, world.rotation) as GameObject).transform;
				clone.parent = worlds;
				//					clone.name = "btn_world_" + (i+1).ToString();
				clone.localRotation = world.transform.localRotation;
				clone.localScale = world.transform.localScale;
				
				this.InitWorldPoint(clone, worldMapList[i]);
			}

			//开启任务ID对应的世界点
//			if (Singleton<TaskModel>.Instance.TaskCopyMapId != 0)
//			{
//				string trace = TaskUtil.GetTraceInfo(Singleton<TaskModel>.Instance.CurrentMainTaskVo); //daikon_forge
//				string[] items = trace.Split('.');
//				uint taskWorldId = uint.Parse(items[1]);
//				world = worlds.FindChild("btn_world_" + taskWorldId);
//				world.gameObject.SetActive (true);
//			}

			//默认开启第一个世界点
			string firstWorldId = StringUtils.GetValueListFromString (BaseDataMgr.instance.GetSysDungeonTree (0).list)[0];
			world = worlds.FindChild("btn_world_" + firstWorldId);
			world.gameObject.SetActive (true);
			Log.info(this, "创建人物模型");
			new RoleDisplay().CreateRole(MeVo.instance.job, LoadModelCallBack);  //创建人物模型
			
		}
		
		//初始化世界点
		private void InitWorldPoint(Transform worldPoint, CopyVo worldPointVo)
		{
			Transform name;
			UISprite sprite;
			BoxCollider boxCollider;
			
			worldPoint.name = "btn_world_" + worldPointVo.mapId;
			//初始化世界点的属性
			worldPoint.localPosition = new Vector3 (worldPointVo.x, worldPointVo.y, 0);
			sprite = worldPoint.GetComponent<UISprite> ();
			sprite.spriteName = worldPointVo.icon;
			sprite.MakePixelPerfect(); 
			
			boxCollider = worldPoint.GetComponent<BoxCollider>();
			boxCollider.size = new Vector3 (sprite.width, sprite.height + 30, 0);
			boxCollider.center = new Vector3 (0, - 15, 0);
			
			name = worldPoint.FindChild ("name");
			name.localPosition = new Vector3(0, -sprite.height / 2 + 30, 0);
			name.FindChild("value").GetComponent<UILabel>().text = worldPointVo.name;

			worldPoint.gameObject.SetActive (false);

			worldPoint.GetComponent<Button> ().onClick = this.WorldPointOnClick;
		}


		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<CopyMode>.Instance.dataUpdated += UpdateWorldMapInfo;
		}

		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			float centerY = worlds.FindChild ("btn_world_" + MeVo.instance.mapId).position.y;
//			centerY = centerY < 0 ? centerY : 0;
//			centerY = centerY > -0.1f ? centerY : -0.1f;
			centerOnGameObj.position = new Vector3(0, centerY, 0);

			center.CenterOn (centerOnGameObj);
			if (MeVo.instance.mapId < 10003)
				scrollPannel.SetDragAmount (0.0f, 0.0f, false);
			else if(MeVo.instance.mapId == 10005)
				scrollPannel.SetDragAmount (0.0f, 1.0f, false);
			Singleton<CopyMode>.Instance.ApplyWorldMapInfo();
		}

		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<CopyMode>.Instance.dataUpdated -= UpdateWorldMapInfo;
		}

		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		//更新世界地图信息展示
		private void UpdateWorldMapInfo(object sender,int code)
		{
			if(code == Singleton<CopyMode>.Instance.UPDATE_WORLDMAP)
			{
				Log.info(this, "更新世界地图");
				opendWorld = Singleton<CopyMode>.Instance.openedWorldIdList;
				Transform world;
				for (int i =0; i < opendWorld.Count; ++i)
				{
					world = worlds.FindChild("btn_world_" + opendWorld[i]);
					world.gameObject.SetActive(true);
				}

				CheckNewOpenedWorld();

//				SetNewFlagInLastWorld();  //设置newFlag

				if (model != null) //首次打开地图时出现model尚未加载好，就执行到这里会出错，所以需要加null判断。model首次加载时，在加载回调用调用下述语句
				{
					UpdateModelPos();
					AutoChangeWorldCheck();
				}
			}
		}

		private void CheckNewOpenedWorld()
		{
			string trace = TaskUtil.GetTraceInfo(Singleton<TaskModel>.Instance.CurrentMainTaskVo); //daikon_forge
			string[] items = trace.Split('.');
			if (items.Length > 1)
			{
				uint taskWorldId = uint.Parse(items[1]);
				Transform taskWorld = worlds.FindChild("btn_world_" + taskWorldId);

				if(!opendWorld.Contains(taskWorldId))
				{
					//新开启特许哎
					EffectMgr.Instance.CreateUIEffect(EffectId.UI_NewWorldPoint, taskWorld.position);
				}
				taskWorld.gameObject.SetActive(true);
			}
			else
			{
				Log.error(this, "任务数据异常");
			}
		}

		//设置new标识
		private void SetNewFlagInLastWorld()
		{
			string[] worldIds;
			worldIds = StringUtils.GetValueListFromString (BaseDataMgr.instance.GetSysDungeonTree (0).list);

			Transform world;
			int lastWorldSub = 0;
			for (; lastWorldSub < worldIds.Length; ++lastWorldSub)
			{
				world = worlds.FindChild("btn_world_" + worldIds[lastWorldSub]);
				if (!(world.gameObject.activeSelf) )
					break;
			}
//			worlds.FindChild("btn_world_" + worldIds[lastWorldSub - 1] + "/name/new").gameObject.SetActive(true);
		}

		//更新角色位置
		private void UpdateModelPos()
		{
			model.transform.localRotation = new Quaternion(0,0,0,0);
			model.transform.localPosition = worlds.FindChild ("btn_world_" + AppMap.Instance.mapParser.MapId).localPosition;
			model.transform.localPosition = new Vector3 (model.transform.localPosition.x, 
			                                             model.transform.localPosition.y - model.transform.localScale.y / 2,
			                                             model.transform.localPosition.z);
		}

		//自动切换世界点检测
		private void AutoChangeWorldCheck()
		{
			//自动切换
			if (Singleton<CopyMode>.Instance.autoChangeFlag)
			{
				this.targetMapId = Singleton<CopyMode>.Instance.autoChangeToNextMapId;
				MoveToTargetCity(this.targetMapId);
			}
		}

		//角色模型创建成功回调
		private void LoadModelCallBack(GameObject go)
		{
			Log.info(this, "人物模型创建成功");
			this.model = go;
			//this.model.transform.localScale  =comTest.transform.localScale*0.63f;
			model.transform.parent = worlds.parent;

			model.transform.localScale = new Vector3(100,100,100);
			UpdateModelPos ();
			AutoChangeWorldCheck ();
			
		}

		//移动到指定世界点
		private void MoveToTargetCity(uint targetMapId)
		{
			Log.info (this, "移动到下一个地图点：" + targetMapId);
			Transform targetWorld = worlds.FindChild ("btn_world_" + targetMapId);
			Vector3 targetPos = new Vector3 (targetWorld.localPosition.x, 
			                                 targetWorld.localPosition.y - model.transform.localScale.y / 2, targetWorld.localPosition.z);
			if (model.GetComponent<TweenPosition>() == null)
			{
				model.AddComponent<TweenPosition>();
				model.GetComponent<TweenPosition>().onFinished.Add (new EventDelegate(MoveEnd));
				model.GetComponent<TweenPosition>().style = UITweener.Style.Once;
			}
			TweenPosition modelTween = model.GetComponent<TweenPosition>();
			modelTween.from = model.transform.localPosition;
			modelTween.to = targetPos;
			float distance = (float)Math.Sqrt (Math.Pow (modelTween.to.x - modelTween.from.x, 2) + Math.Pow (modelTween.to.y - modelTween.from.y, 2));
			modelTween.duration = distance * MOVE_SPEED;
			
			model.transform.localRotation = new Quaternion(0,modelTween.to.x > modelTween.from.x ? 0 : 180,0,0);
			
			modelTween.ResetToBeginning ();
			modelTween.Play ();

			model.transform.GetComponentInChildren<Animator> ().SetInteger (Status.STATU, Status.RUN);
			
		}
		
		//		delegate void TweenMoveEnd();
		public void MoveEnd()
		{
			Log.info (this, "到达下一个地图点，开始切换到场景：" + targetMapId);
			Singleton<CopyMode>.Instance.autoChangeFlag = false;
			Singleton<MapMode>.Instance.changeScene(targetMapId, true, 0, 0);
			this.CloseView ();
		}


		
//		//解锁新世界点
//		private void UnlockNewWorldPoint(object sender,int code)
//		{
//			if(code == Singleton<CopyMode>.Instance.UNLOCK_NEW_WORLDPOINT)
//			{
//				Log.info(this, "解锁新世界点");
//				Transform world = worlds.FindChild("btn_world_" + Singleton<CopyMode>.Instance.copyMapInfo.curWorldId);
//				//新开关卡特效
//				EffectMgr.Instance.CreateUIEffect(EffectId.UI_NewWorldPoint, world.position);
//			}
//		}


		//世界地图的返回键被点击
		private void BackOnClick(GameObject go)
		{
			if (!Singleton<CopyMode>.Instance.autoChangeFlag)
			{
				this.CloseView ();

				Singleton<MainBottomLeftView>.Instance.HitMapPoint = true;
				AppMap.Instance.me.Pos(AppMap.Instance.mapParser.WorldTransPosX, 
				                       AppMap.Instance.mapParser.WorldTransPosY - GameConst.HitPointEffectOffH);
				Singleton<MainBottomLeftView>.Instance.RoleLocked = false;
			}
		}

		//世界点被点击
		private void WorldPointOnClick(GameObject go)
		{
			if (!Singleton<CopyMode>.Instance.autoChangeFlag)
			{
				targetMapId = uint.Parse (go.name.Split (new char[]{'_'})[2]);
				MoveToTargetCity(targetMapId);
			}

		}
	}
}
