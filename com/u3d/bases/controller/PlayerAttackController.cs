/**普通攻击控制类
 * @author 骆琦
 * @date  2013-11-6
 * 实现普通攻击的连招控制
 * **/

using System.Collections.Generic;
using com.game.module.fight.vo;
using com.game.module.map;
using com.game.vo;
using com.u3d.bases.consts;

namespace com.u3d.bases.controller
{
    public class PlayerAttackController : AttackControllerBase
    {
        private void Update()
        {
            if (MapMode.StartAutoMove || MeController.StatuController.CurrentStatu == Status.DEATH)
            {
                AttackList.Clear(); //切图的过程中情况攻击列表
            }

            if ((MeController.StatuController.CurrentCombStatu >= MeController.StatuController.MaxComb &&
                 MeController.StatuController.MaxComb > 1) || MeController.StatuController.CurrentStatu == Status.Win)
            {
                if (NeedClear())
                {
                    AttackList.Clear();
                }
            }
            if (AttackList.Count > 0 &&
                (MeController.StatuController.CurrentStatu == Status.IDLE ||
                 MeController.StatuController.CurrentStatu == Status.RUN) &&
                MeController.SkillController.SkillList.Count == 0)
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
                        if (MeController.StatuController.CurrentCombStatu == Status.COMB_0 ||
                            (MeController.StatuController.CurrentCombStatu == MeController.StatuController.MaxComb))
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack1);
                        }
                        else if (MeController.StatuController.CurrentCombStatu == Status.COMB_1)
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack2);
                        }
                        else if (MeController.StatuController.CurrentCombStatu == Status.COMB_2)
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack3);
                        }
                        else if (MeController.StatuController.CurrentCombStatu == Status.COMB_3)
                        {
                            MeController.SkillController.RequestUseSkill(SkillController.Attack4);
                        }
                        break;
                }
            }
        }

        private void RemoveModelEffect()
        {
            MeController.Me.EndModelColorEffect();
        }

        private bool NeedClear()
        {
            foreach (ActionVo actionVo in AttackList)
            {
                if (actionVo.ActionType == Actions.DEATH)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     执行动作添加到队列
        /// </summary>
        /// <param name="vo">动作信息</param>
        /// <param name="isPush">强插入，技能强制插入，普通攻击加入队列</param>
        public override void AddAttackList(ActionVo vo, bool isPush = false)
        {
            var playerVo = MeController.Me.GetVo() as PlayerVo;
            if (playerVo != null && (playerVo.CurHp <= 0 && vo.ActionType != Actions.DEATH)) return;

            if (AttackList == null) AttackList = new List<ActionVo>();

            //死亡立即播放,死亡控制可放在单独的死亡控制类里面
            if (vo.ActionType == Actions.DEATH)
            {
                AttackList.Clear();
                AttackList.Add(vo);
                return;
            }
            //强制插入
            if (isPush)
            {
                if (IsInBattle())
                {
                    return;
                }
                CurVo = null;
                AttackList.Clear();
                AttackList.Add(vo);
                return;
            }
            if (MeController.StatuController.CurrentStatu == Status.IDLE ||
                MeController.StatuController.CurrentStatu == Status.RUN ||
                MeController.StatuController.CurrentStatu == Status.ATTACK1
                || MeController.StatuController.CurrentStatu == Status.ATTACK2 ||
                MeController.StatuController.CurrentStatu == Status.ATTACK3)
            {
                if (AttackList.Count >= MeController.StatuController.MaxComb && MeController.StatuController.MaxComb > 1)
                {
                    return;
                }
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
            return statu == Status.ATTACK1 || statu == Status.ATTACK2 || statu == Status.ATTACK3 ||
                   statu == Status.ATTACK4 || statu == Status.SKILL1
                   || statu == Status.SKILL2 || statu == Status.SKILL3 || statu == Status.SKILL4 ||
                   statu == Status.SKILL5 || statu == Status.SKILL6 || statu == Status.SKILL8;
        }
    }
}