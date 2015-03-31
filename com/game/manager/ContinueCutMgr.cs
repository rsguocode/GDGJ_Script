using System;
using System.Collections;
using com.game.vo;
using UnityEngine;
using com.u3d.bases.debug;
using Com.Game.Module.ContinueCut;
using com.game.module.test;

namespace com.game.manager
{
    public class ContinueCutMgr : MonoBehaviour
    {
        //连斩统计间隔(秒为单位)，如果超过此间隔没有斩杀怪物，则连斩数目清零
        private int cutTimeGap = 30;
        //当前连斩计数器
        private int curCutCounter = 0;
        //上一次斩杀时间
        private float lastCutTime;

		//连击相关定义
		private int curAttackCounter = 0;
		private int maxAttackCounter = 0;
		private float lastAttackTime;
		private bool attackPause = false;
		private float attackPauseTime;

		//每隔一秒显示剩余时间
		private const float SHOW_LEFT_INTERVAL = 0.1f;
		//上次记录剩余时间的时间点
		private float lastRecordTime; 

		public int AttackCounter {get {return maxAttackCounter;}} 

		private float attackTimeGap
		{
			get
			{
				if (curAttackCounter < 50)
				{
					return 5f;
				}
				else if (curAttackCounter < 100)
				{
					return 4f;
				}
				else if (curAttackCounter < 150)
				{
					return 3f;
				}
				else if (curAttackCounter < 200)
				{
					return 2f;
				}
				else
				{
					return 1f;
				}
			}
		}

        //增加连斩数目
        public void AddCutNum(int cutNum = 1)
        {
            curCutCounter += cutNum;
            lastCutTime = Time.time;

			ShowCut();
        }

		//显示连击数量
		public void AddAttackNum(int attackNum = 1)
		{
			curAttackCounter += attackNum;

			if (maxAttackCounter < curAttackCounter)
			{
				maxAttackCounter = curAttackCounter;
			}

			lastAttackTime = Time.time;
		    MeVo.instance.SeqAttack = (uint) curAttackCounter/100;
		    StartCoroutine(ShowAttack());
		}

		//显示连斩统计
		private void ShowCut()
		{
			if (curCutCounter >= 1)
			{
				Log.info(this, "********************万夫莫敌，连斩" + curCutCounter);
			}
		}

		//显示连击统计
		private IEnumerator ShowAttack()
		{
		    yield return 0;   //隔一帧再显示，避免一帧耗时过长，引起卡顿
			if (curAttackCounter >= 1)
			{
				Singleton<ContinueCutView>.Instance.ShowNum(curAttackCounter);
			}
		}

        //停止连斩统计
        private void StopCut()
        {
            curCutCounter = 0;
			Log.info(this, "********************停止连斩统计");
        }

		//停止连击统计
		private void StopAttack()
		{
			curAttackCounter = 0;
			attackPause = false;
			Singleton<ContinueCutView>.Instance.CloseView();
		}

		//停止连斩、连击统计
		public void StopAll()
		{
			maxAttackCounter = 0;
			StopCut();
			StopAttack();
		}

		//暂停连击统计
		public void PauseAttack()
		{
			if (Singleton<ContinueCutView>.Instance.gameObject.activeSelf)
			{
				attackPause = true;
				attackPauseTime = Time.time;
			}
		}

		//恢复连击统计
		public void ResumeAttack()
		{
			if (attackPause)
			{
				lastAttackTime += (Time.time-attackPauseTime);
				attackPause = false;
				UpdateAttackNumber();
			}
		}

        protected void Update()
        {
			UpdateCutNumber();     
			UpdateAttackNumber();
        }

		//更新连斩统计
		private void UpdateCutNumber()
		{
			if (curCutCounter <= 0)
			{
				return;
			}
			
			//如果在时间允许范围内没有继续斩杀怪物，则停止连斩统计
			if (Time.time - lastCutTime > cutTimeGap)
			{
				StopCut();
				return;
			}
		}

		//更新连击统计
		private void UpdateAttackNumber()
		{
			if (attackPause || curAttackCounter <= 0)
			{
				return;
			}
			
			//如果在时间允许范围内没有继续攻击怪物，则停止连击统计
			if (Time.time - lastAttackTime > attackTimeGap)
			{
				StopAttack();
				return;
			}
			
			//每隔一秒显示连斩剩余时间
			if (Time.time - lastRecordTime > SHOW_LEFT_INTERVAL)
			{
				lastRecordTime = Time.time;
				float leftTime = attackTimeGap - (Time.time - lastAttackTime);
				Singleton<ContinueCutView>.Instance.ShowLeftTime(leftTime / attackTimeGap);
			}
		}

        /// <summary>
        /// 获得连斩加成
        /// </summary>
        /// <returns></returns>
        public float GetCutAddition()
        {
            return curAttackCounter * 0.1f;
        }
    }
}