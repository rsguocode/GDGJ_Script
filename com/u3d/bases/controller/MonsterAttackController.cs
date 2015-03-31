using System;
using System.Collections.Generic;
using com.game.module.fight.vo;
using com.game.vo;
using com.u3d.bases.consts;

/**普通攻击控制类
 * @author 骆琦
 * @date  2013-11-6
 * 实现普通攻击的连招控制
 * **/

namespace com.u3d.bases.controller
{
    public class MonsterAttackController : AttackControllerBase
    {
        private void Update()
        {
            if (AttackList.Count > 0 &&
                ((MeController.StatuController.CurrentStatu == Status.IDLE &&
                  MeController.StatuController.CurStatuNameHash == Status.NAME_HASH_IDLE) ||
                 (MeController.StatuController.CurrentStatu == Status.RUN ||
                  MeController.StatuController.CurStatuNameHash == Status.NAME_HASH_RUN)))
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
                            int dir = CurVo.TargetPoint.x < MeController.Me.GoBase.transform.position.x
                                ? Directions.Left
                                : Directions.Right;
                            MeController.Me.ChangeDire(dir);
                        }
                        //怪物只有两连击
                        if (MeController.StatuController.CurrentCombStatu == Status.COMB_0 ||
                            MeController.StatuController.CurrentCombStatu == Status.COMB_2)
                        {
                            print("****SkillController.Attack1");
                            MeController.SkillController.RequestUseSkill(SkillController.Attack1);
                        }
                        else if (MeController.StatuController.CurrentCombStatu == Status.COMB_1)
                        {
                            print("****SkillController.Attack2");
                            MeController.SkillController.RequestUseSkill(SkillController.Attack2);
                        }
                        break;
                }
            }
            if (AttackList.Count > 0 &&
                (MeController.StatuController.CurrentStatu == Status.HURT1 ||
                 MeController.StatuController.CurrentStatu == Status.HURT2
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
        ///     执行动作添加到队列
        /// </summary>
        /// <param name="vo">动作信息</param>
        /// <param name="isPush">强插入</param>
        public override void AddAttackList(ActionVo vo, bool isPush = false)
        {
            var monsterVo = MeController.Me.GetVo() as MonsterVo;
            if (monsterVo != null && (monsterVo.CurHp <= 0 && vo.ActionType != Actions.DEATH)) return;

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
            if (MeController.StatuController.CurrentStatu == Status.IDLE ||
                MeController.StatuController.CurrentStatu == Status.RUN)
            {
                AttackList.Add(vo);
            }
        }

        /// <summary>
        ///     是否在战斗状态
        /// </summary>
        /// <returns></returns>
        public override bool IsInBattle()
        {
            int statu = MeController.StatuController.CurrentStatu;
            return statu == Status.ATTACK1 || statu == Status.ATTACK2;
        }
    }
}