using System;
﻿﻿﻿using com.game.module.fight.vo;
﻿﻿﻿using com.u3d.bases.consts;
using UnityEngine;
using com.game;
using Com.Game.Speech;
using com.game.vo; 

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/22 05:39:42 
 * function: 玩家被攻击控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class PlayerBeAttackedController : BeAttackedControllerBase
    {
        private float _beAttackedTimes=0;
        private float _lastBeAttackedInterval=0;
        private const float BeAttackedCheckInterval = 0;
        private const int MaxHurtActionInInterval = 3;
        private const float BeAttackedMinInterval = 1.5f;

        override public void BeAttacked(ActionVo actionVo)
        {
            base.BeAttacked(actionVo);
            actionVo.Target = meController.Me.GoBase;        // 显示特效的目标对象
            meController.AttackController.ShowBeAttackedEffect(actionVo);  //只要受击了就显示受击特效
            meController.Me.StartShowModelBeAttackedEffect();
            /*var statu = meController.StatuController.CurrentStatu;
            if (statu == Status.IDLE || statu == Status.RUN)
            {
                int dir = actionVo.SkillUsePoint.x < meController.Me.GoBase.transform.position.x ? Directions.Left : Directions.Right;
                meController.Me.ChangeDire(dir);  //攻击过程中受击不转身
            }*/
            //meController.gameObject.transform.position = actionVo.HurtDestination;//不需要
            if(actionVo.HurtAnimation == Actions.Hurt1)
            {
                SetHurt1(actionVo);
            }
            else if (actionVo.HurtAnimation == Actions.Hurt2)
            {
                SetHurt2(actionVo);
            }

            /*播放受击动画间隔（不需要）
            if (Time.realtimeSinceStartup - _lastBeAttackedInterval > BeAttackedCheckInterval)
            {
                _beAttackedTimes = 1;
            }
            else
            {
                if (_beAttackedTimes > MaxHurtActionInInterval)
                {
                    return;
                }
            }
            if (Time.realtimeSinceStartup - _lastBeAttackedInterval < BeAttackedCheckInterval)
            {
                return;
            }
            if (meController.StatuController.CurrentStatu == Status.HURT1)
            {
                meController.StatuController.SetStatu(Status.HURT2);
            }
            else
            {
                meController.StatuController.SetStatu(Status.HURT1);
            }
            _beAttackedTimes++;
            _lastBeAttackedInterval = Time.realtimeSinceStartup;*/
			int monsterNumber = AppMap.Instance.MonsterNumber;
			if (monsterNumber>0 && monsterNumber<=4)
			{
				SpeechMgr.Instance.PlayAttackedAndMonster0to4Speech();
			}
			else if (monsterNumber>4 && monsterNumber<=7)
			{
				SpeechMgr.Instance.PlayAttackedAndMonster4to7Speech();
			}
        }
    }
}