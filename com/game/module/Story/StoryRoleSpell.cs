//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoryRoleSpell
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
	public class StoryRoleSpell : MonoBehaviour 
	{	
		public bool Repeat;
		public int Skill;

		private Animator animator;
		protected AnimatorStateInfo stateInfo; 

		private int[] stateVector = {
									Status.NAME_HASH_IDLE, 
                                    Status.NAME_HASH_RUN, 
                                    Status.NAME_HASH_ROLL, 
									Status.NAME_HASH_ATTACK1, 
                                    Status.NAME_HASH_ATTACK2, 
                                    Status.NAME_HASH_ATTACK3, 
									Status.NAME_HASH_ATTACK4, 
                                    Status.NAME_HASH_SKILL1, 
                                    Status.NAME_HASH_SKILL2,
									Status.NAME_HASH_SKILL3,
                                    Status.NAME_HASH_HURT1,
                                    Status.NAME_HASH_DEATH, 
									Status.NAME_HASH_HURT2, 
                                    Status.NAME_HASH_HURT3, 
                                    Status.NAME_HASH_HURT4, 
									Status.NAME_HASH_HURTDOWN, 
                                    Status.NAME_HASH_STANDUP, 
                                    Status.NAME_HASH_SKILL4,
                                    Status.NAME_HASH_Win,
                                    Status.NAME_HASH_SKILL5,
                                    Status.NAME_HASH_SKILL6,
                                    Status.NAME_HASH_SKILL7,
                                    Status.NAME_HASH_SKILL8,
                                    Status.NAME_HASH_ATTACK5,
                                    Status.NAME_HASH_ATTACK6
									};
        
		void Start()
		{
			animator = gameObject.GetComponentInChildren<Animator>();

			if (null != animator)
			{
				animator.SetInteger(Status.STATU, Skill);
			}
		}

		void Update()
		{
			//重复播放不需要处理
			if (Repeat)
			{
				return;
			}

			stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.nameHash == stateVector[Skill])
			{
				ProcessState();
			}
		}

		private void ProcessState()
		{
			if (stateInfo.normalizedTime > 0.8)
			{
				animator.SetInteger(Status.STATU, Status.IDLE);
			}
		}
		
	}
}
