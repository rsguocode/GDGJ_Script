//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryRoleMove
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.u3d.bases.consts;

namespace Com.Game.Module.Story
{
	public class StoryRoleMove : MonoBehaviour 
	{	
		public Vector3 TargetPos;
		public float MoveSpeed = Global.ROLE_RUN_SPEED;

		private Animator animator;
		private Transform selfTransform;

		private bool stopped = false;

		void Start()
		{
			selfTransform = gameObject.transform;
			animator = gameObject.GetComponentInChildren<Animator>();

			if (null != animator)
			{
				animator.SetInteger(Status.STATU, Status.RUN);
			}

			//处理方向
			//右转
			if (selfTransform.position.x < TargetPos.x)
			{
				selfTransform.localScale = Vector3.one;
			}
			//左转
			else
			{
				selfTransform.localScale = new Vector3(-1, 1, 1);
			}

			if (MoveSpeed <= 0f)
			{
				MoveSpeed = Global.ROLE_RUN_SPEED;
			}
		}

		void Update()
		{
			if (stopped)
			{
				return;
			}

			if (selfTransform.position == TargetPos)
			{
				if (null != animator)
				{
					animator.SetInteger(Status.STATU, Status.IDLE);
				}

				return;
			}

			if (null != animator)
			{
				animator.SetInteger(Status.STATU, Status.RUN);
			}

			selfTransform.position = Vector3.MoveTowards(selfTransform.position, TargetPos, Time.deltaTime * MoveSpeed);
		}

		public void Stop()
		{
			stopped = true;
		}
		
	}
}
