//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryCameraMove
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game;
using com.game.module.map;
using com.game.data;
using com.game.manager;
using com.game.consts;

namespace Com.Game.Module.Story
{
	public class StoryCameraMove : MonoBehaviour 
	{		
		public Vector3 TargetPos;
		public float MoveSpeed = 5f;
		public GameObject FollowTarget = null;

		private Transform selfTransform;
		private bool stopped = false;
		private bool exceedBorder = false;
		private Vector3 cameraPos;

		private float cameraGapLen;
		private float borderMin = 0f;
		private float borderMax = 100f;

		private Transform mapFarBg1;          //地图背景层1;
		private Vector3 mapFarBg1Pos;

		public bool ExceedBorder
		{
			get
			{
				return exceedBorder;
			}
		}

		void Start()
		{
            print("****StoryCameraMove  这个脚本有执行吗？");
			selfTransform = gameObject.transform;
			cameraPos = selfTransform.position;
			TargetPos.y = cameraPos.y;
			TargetPos.z = cameraPos.z;

			mapFarBg1 = AppMap.Instance.mapParser.FarLayer1;

			if (null != mapFarBg1)
			{
				mapFarBg1Pos = mapFarBg1.transform.position;
			}

			cameraGapLen = (float)Screen.width / Screen.height * Camera.main.orthographicSize;

			SysMapVo vo = BaseDataMgr.instance.GetMapVo(AppMap.Instance.mapParser.MapId);
			//副本地图摄像机范围为三个阶段
			if (vo.subtype == MapTypeConst.MAIN_COPY 
			    || vo.subtype == MapTypeConst.DAEMONISLAND_COPY
			    || vo.subtype == MapTypeConst.FIRST_BATTLE_COPY)
			{
				int count = AppMap.Instance.mapParser.AccumulatedStagesLength.Count;
				borderMin = cameraGapLen;
				borderMax = AppMap.Instance.mapParser.AccumulatedStagesLength[count-1] - cameraGapLen;
			}
			else
			{
				borderMin = AppMap.Instance.mapParser.PosX + cameraGapLen;
				borderMax = AppMap.Instance.mapParser.EachStageLength[MapMode.CUR_MAP_PHASE - 1] + AppMap.Instance.mapParser.PosX - cameraGapLen;
			}

			if (MoveSpeed <= 0f)
			{
				MoveSpeed = 10f;
			}
		}

		void Update()
		{
			if (stopped || exceedBorder)
			{
				return;
			}

			if (selfTransform.position == TargetPos)
			{				
				return;
			}	

			//不跟随角色
			if (null == FollowTarget)
			{
				cameraPos = Vector3.MoveTowards(selfTransform.position, TargetPos, Time.deltaTime * MoveSpeed);
			}
			else
			{
				cameraPos.x = FollowTarget.transform.position.x;
			}

			if (cameraPos.x < borderMin || cameraPos.x > borderMax)
			{
				exceedBorder = true;
				cameraPos.x = Mathf.Clamp(cameraPos.x, borderMin, borderMax);
			}

			selfTransform.position = cameraPos;

			if (null != mapFarBg1)
			{
				mapFarBg1Pos.x = selfTransform.position.x - cameraGapLen;
				mapFarBg1.position = mapFarBg1Pos;
			}
		}

		public void Stop()
		{
			stopped = true;
		}		
	}
}
