﻿﻿﻿
using System;
using System.Collections.Generic;
using com.game.module.fight.vo;
using com.game.vo;
using com.u3d.bases.consts;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/06 02:44:02 
 * function: 宠物攻击控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class PetAttackController : AttackControllerBase
    {

        void Update()
        {
            if (AttackList.Count > 0 && (MeController.StatuController.CurrentStatu == Status.IDLE || MeController.StatuController.CurrentStatu == Status.RUN))
            {
                CurVo = AttackList[0];
                AttackList.RemoveAt(0);
                switch (CurVo.ActionType)
                {
                    case Actions.RUN:
                        MeController.MoveTo(CurVo.RunDestination.x, CurVo.RunDestination.y);
                        break;
                    case Actions.DEATH:
                        MeController.StatuController.SetStatu(Status.DEATH);
                        break;
                    case Actions.ATTACK:
                        if (Math.Abs(CurVo.TargetPoint.x) > 0)
                        {
                            int dir = CurVo.TargetPoint.x < MeController.Me.GoBase.transform.position.x ? Directions.Left : Directions.Right;
                            MeController.Me.ChangeDire(dir);
                        }
                        MeController.SkillController.RequestUseSkill(SkillController.Attack1);
                        //宠物暂时只做一次攻击
                        /*if (MeController.StatuController.CurrentCombStatu == Status.COMB_0 || MeController.StatuController.CurrentCombStatu == Status.COMB_2)
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack1);
                        }
                        else if (MeController.StatuController.CurrentCombStatu == Status.COMB_1)
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack2);
                        }*/
                        break;
                }
            }
            if (AttackList.Count > 0 && (MeController.StatuController.CurrentStatu == Status.HURT1 || MeController.StatuController.CurrentStatu == Status.HURT2
                || MeController.StatuController.CurrentStatu == Status.HURT3))
            {
                CurVo = AttackList[0];
                if (CurVo.ActionType == Actions.DEATH)
                {
                    AttackList.Clear();
                    MeController.StatuController.SetStatu(Status.DEATH);
                }
            }
            if (AttackList.Count > 0 && MeController.StatuController.CurrentStatu == Status.HURTDOWN)
            {
                CurVo = AttackList[0];
                if (CurVo.ActionType == Actions.DEATH)
                {
                    AttackList.Clear();
                    Invoke("Death", 0.3f);
                }
            }
        }

        private void Death()
        {
            MeController.StatuController.SetStatu(Status.DEATH);
        }

        /// <summary>
        /// 执行动作添加到队列
        /// </summary>
        /// <param name = "vo">动作信息</param>
        /// <param name = "isPush">强插入</param>
        override public void AddAttackList(ActionVo vo, bool isPush = false)
        {
            var petVo = MeController.Me.GetVo() as PetVo;
            if (petVo != null && (petVo.CurHp <= 0 && vo.ActionType != Actions.DEATH)) return;

            if (AttackList == null) AttackList = new List<ActionVo>();

            //死亡立即播放,死亡控制可放在单独的死亡控制类里面
            if (vo.ActionType == Actions.DEATH)
            {
                AttackList.Clear();
                AttackList.Add(vo);
                return;
            }
            //如果在翻滚状态，则不会受到伤害
            if (MeController.StatuController.CurrentStatu == Status.ROLL && vo.ActionType == Actions.INJURED)
            {
                return;
            }
            //强制插入
            if (isPush)
            {
                CurVo = null;
                AttackList.Clear();
                AttackList.Add(vo);
                return;
            }
            if (MeController.StatuController.CurrentStatu == Status.IDLE || MeController.StatuController.CurrentStatu == Status.RUN)
            {
                //attackList.Clear();
                AttackList.Add(vo);
            }
        }

         
    }
}