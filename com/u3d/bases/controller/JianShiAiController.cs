using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.fight.arpg;
using com.game.module.fight.vo;
using Com.Game.Module.GoldSilverIsland;
using com.game.module.map;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.map;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/15 08:51:56 
 * function: 剑士的AI控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class JianShiAiController : AiControllerBase
    {
        private Animator _animator;
        private bool _hasAiTask; //是否有AI任务
        private PlayerVo _mePlayerVo; //当前脚本所属对象的vo
        private Transform _meTransform;
        private float _normalAttackRange;
        private float _stopTime;


        private void Start()
        {
            _mePlayerVo = MeController.Me.GetMeVoByType<PlayerVo>();
            _hasAiTask = false;
            BaseRoleVo = _mePlayerVo;
            _meTransform = transform;
            _normalAttackRange = (2 - Mathf.Abs(2 - _mePlayerVo.job))*1.5f;
        }


        /// <summary>
        ///     执行器（每帧执行）
        /// </summary>
        private void Update()
        {
            // 获取animator的信息并根据animator信息进行相应的业务处理
            if (_animator == null)
            {
                _animator = MeController.Me.Animator;
                return;
            }
            //1. 如果有AI任务或者AI不在待机状态或者死亡的情况下，则不往下走，节省性能消耗
            if (!CanUseAi())
            {
				if (!DangJiTester.role_use_ai) // 根据该变得来设置主角是是否使用AI
                	return;
            }

            //2. 找到最近的敌人
            ActionDisplay nearestEnemy = FindNearestEnemyInMapRange();

            //3. 如果没有敌人,则在场景内左右移动
            if (nearestEnemy == null)
            {
                AiMoveBehaviourNoEnemy();
                return;
            }

            //如果自动切图阶段，则不执行后面的AI
            if (MapMode.StartAutoMove)
            {
                return;
            }

            //4. 尝试使用技能
            TryToUseSkill(nearestEnemy);

            //5. 判断最近敌人和AI的Y值差，若Y值差大于0.5个Unity单位，则纵向移动到同一Y值
            if (Mathf.Abs(nearestEnemy.Controller.transform.position.y - _meTransform.position.y) > 0.5f)
            {
                MoveToTheSameYWithEnemy(nearestEnemy);
                return;
            }

            float disX = Mathf.Abs(nearestEnemy.Controller.transform.position.x - _meTransform.position.x);

            //6. 若AI和最近敌人的距离大于6个Unity单位，则横向移动到离敌人的6个单位内
            if (disX >= 6)
            {
                MoveToAttackRange(nearestEnemy);
                return;
            }

            //7. 若在瞬移范围内，则使用瞬移
            if ((disX < 6 && disX > 5) || (disX < 2.5f && disX > 1.8f))
            {
                if (MeController.SkillController.LearnedSkillList[SkillController.Roll] != 0 &&
                    MeController.SkillController.IsSkillCdReady(SkillController.Roll))
                {
                    MeController.SkillController.RequestUseSkill(SkillController.Roll);
                    return;
                }
            }

            //8. 若在普通攻击范围内，则使用普通攻击
            if (disX < _normalAttackRange && _mePlayerVo.CurHp > 0)
            {
                if (_meTransform.position.x < nearestEnemy.GoBase.transform.position.x)
                {
                    MeController.Me.ChangeDire(Directions.Right);
                }
                else
                {
                    MeController.Me.ChangeDire(Directions.Left);
                }
                var attackVo = new ActionVo
                {
                    ActionType = Actions.ATTACK,
                    TargetPoint = nearestEnemy.GoBase.transform.position
                };
                StartCoroutine(YieldSecordsAutoAttack(0.5f, attackVo));
                return;
            }

            //10. 若以上情况都不是，则继续移动到攻击范围内
            MoveToAttackRange(nearestEnemy);
        }

        private bool CanUseAi()
        {
            if (!IsAi || _hasAiTask)
            {
                return false;
            }
            if (_mePlayerVo.CurHp == 0)
            {
                return false;
            }
            int currentStatu = MeController.StatuController.CurrentStatu;
            if (currentStatu != Status.IDLE && currentStatu != Status.RUN)
            {
                return false;
            }
            int curStatuNameHash = MeController.StatuController.CurStatuNameHash;
            if (curStatuNameHash != Status.NAME_HASH_IDLE && curStatuNameHash != Status.NAME_HASH_RUN)
            {
                return false;
            }
            if (MeController.AttackController.AttackList.Count > 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     找到在当前副本阶段行走区域范围内最近的怪物
        /// </summary>
        /// <returns></returns>
        private ActionDisplay  FindNearestEnemyInMapRange()
        {
            ActionDisplay result = null;
            float dis = 1000000;
            IList<ActionDisplay> tempList = AppMap.Instance.monsterList.Cast<ActionDisplay>().ToList(); /**取得当前怪物列表**/
            if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
            {
                foreach (PlayerDisplay display in AppMap.Instance.playerList) /**取得当前玩家列表**/
                {
                    if (display != AppMap.Instance.me)
                    {
                        tempList.Add(display);
                    }
                }
            }
            else
            {
                tempList.Add(AppMap.Instance.me);
            }
            foreach (ActionDisplay actionDisplay in tempList)
            {
                float x = actionDisplay.GoBase.transform.position.x;
                float y = actionDisplay.GoBase.transform.position.y;
                MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
                if (x < mapRange.MinX || x > mapRange.MaxX || y < mapRange.MinY || y > mapRange.MaxY) // 地图边界外
                {
                    continue;
                }
                if (actionDisplay.GetMeVoByType<BaseRoleVo>().CurHp == 0)
                {
                    continue;
                }
                float curDis = GeteEnemyDistance(actionDisplay);
                if (curDis < dis)
                {
                    result = actionDisplay; 
                    dis = curDis;
                }
            }
            return result;
        }


        /// <summary>
        ///     计算和敌人的距离
        /// </summary>
        /// <param name="enemyDisplay"></param>
        /// <returns></returns>
        private float GeteEnemyDistance(ActionDisplay enemyDisplay)
        {
            Vector3 mePos = _meTransform.position;
            Vector3 enemyPos = enemyDisplay.Controller.transform.position;
            return (mePos.x - enemyPos.x)*(mePos.x - enemyPos.x) + (mePos.y - enemyPos.y)*(mePos.y - enemyPos.y);
        }


        private void TryToUseSkill(ActionDisplay nearestEnemy)
        {
            SkillController skillController = MeController.SkillController;
            Vector3 mePos = MeController.transform.position;
            Vector3 enemyPos = nearestEnemy.Controller.transform.position;
            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                int dir = mePos.x < enemyPos.x ? Directions.Right : Directions.Left; //朝向最近的敌人
                MeController.Me.ChangeDire(dir);
            }
            for (int i = SkillController.Skill1; i <= SkillController.Skill4; i++)
            {
                if (skillController.LearnedSkillList[i] == 0) continue;  //已学会的技能列表
                if (skillController.IsSkillCdReady(i))
                {
                    SysSkillBaseVo skillVo = BaseDataMgr.instance.GetSysSkillBaseVo(skillController.LearnedSkillList[i]);
                    if (DamageCheck.Instance.IsSkillCovered(skillVo, mePos, mePos,
                        nearestEnemy.Controller.transform.position, MeController.Me.CurDire, (MeController.Me as ActionDisplay).BoxCollider2D, nearestEnemy.BoxCollider2D))
                    {
                        skillController.RequestUseSkill(i); //  请求使用技能
                        break;
                    }
                }
            }
        }


        /// <summary>
        ///     没有怪物时的AI移动逻辑
        /// </summary>
        private void AiMoveBehaviourNoEnemy()
        {
            MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            Vector3 targetPoint = _meTransform.position;
            if (MapMode.CanGoToNextPhase)
            {
                MeController.Me.ChangeDire(Directions.Right);
                MeController.MoveByDir(MeController.Me.CurFaceDire);
                return;
            }
            if (targetPoint.x < mapRange.MinX + 1)
            {
                MeController.Me.ChangeDire(Directions.Right);
            }
            else if (targetPoint.x > mapRange.MaxX - 1)
            {
                MeController.Me.ChangeDire(Directions.Left);
            }
            MeController.MoveByDir(MeController.Me.CurFaceDire);
        }

        /// <summary>
        ///     移动和怪物同一Y值
        /// </summary>
        /// <param name="enemyDisplay"></param>
        private void MoveToTheSameYWithEnemy(ActionDisplay enemyDisplay)
        {
            Vector3 targetPoint = _meTransform.position;
            targetPoint.y = enemyDisplay.Controller.transform.position.y;
            var actionVo = new ActionVo { RunDestination = targetPoint, ActionType = Actions.RUN };
            MeController.AttackController.AddAttackList(actionVo);
        }

        /// <summary>
        ///     移动到攻击范围内
        /// </summary>
        /// <param name="enemyDisplay"></param>
        private void MoveToAttackRange(ActionDisplay enemyDisplay)
        {
            Vector3 targetPoint = _meTransform.position;
            if (targetPoint.x < enemyDisplay.Controller.transform.position.x)
            {
                MeController.Me.ChangeDire(Directions.Right);
            }
            else
            {
                MeController.Me.ChangeDire(Directions.Left);
            }
            MeController.MoveByDir(MeController.Me.CurFaceDire);
        }


        /// <summary>
        ///     获取敌人信息
        /// </summary>
        /// <returns>敌人列表</returns>
        private IList<ActionDisplay> GetEnemyDisplay()
        {
            IList<ActionDisplay> tempList = AppMap.Instance.monsterList.Cast<ActionDisplay>().ToList();
            if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
            {
                foreach (PlayerDisplay display in AppMap.Instance.playerList)
                {
                    if (display != AppMap.Instance.me)
                    {
                        tempList.Add(display);
                    }
                }
            }
            else
            {
                tempList.Add(AppMap.Instance.me);
            }
            return tempList;
        }

        /// <summary>
        ///     播放受伤头顶冒伤害数字和暴击文字
        /// </summary>
        /// <param name="isDodge">是否闪避</param>
        /// <param name="isCrit">是否被暴击</param>
        /// <param name="isParry">是否格挡</param>
        /// <param name="cutHp">掉血数量</param>
        /// <param name="curHp">当前血量</param>
        public override void AddHudView(bool isDodge, bool isCrit, bool isParry, int cutHp, int curHp, Color color)
        {
            base.AddHudView(isDodge, isCrit, isParry, cutHp, curHp, color);
            if (_mePlayerVo.Id == MeVo.instance.Id)
            {
                MeVo.instance.DataUpdate(MeVo.DataHpMpUpdate);
            }
            else if (MeVo.instance.mapId == MapTypeConst.GoldSilverIsland_MAP &&
                     _mePlayerVo.Id == Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer.Id)
            {
                Singleton<GoldSilverIslandMode>.Instance.DataUpdate(
                    Singleton<GoldSilverIslandMode>.Instance.UPDATE_PLAYER_ROB_HPMP);
            }
        }


        /// <summary>
        ///     战斗AI行为，如果攻击范围内有怪物则进行攻击
        /// </summary>
        private void AiAttackBehaviour()
        {
            SysMapVo vo = BaseDataMgr.instance.GetMapVo(AppMap.Instance.mapParser.MapId);
            //副本里面的玩家的自动攻击
            if (vo.type == MapTypeConst.COPY_MAP)
            {
                IEnumerable<ActionDisplay> enemyList = GetEnemyDisplay();
                foreach (ActionDisplay display in enemyList)
                {
                    var baseRoleVo = display.GetVo() as BaseRoleVo;
                    if (baseRoleVo != null && baseRoleVo.CurHp <= 0) continue;
                    if (display.Controller == null) continue;
                    if (Mathf.Abs(display.Controller.transform.position.x - MeController.transform.position.x) <= 1.4 &&
                        Mathf.Abs(display.Controller.transform.position.y - MeController.transform.position.y) <= 0.4)
                    {
                        var attackVo = new ActionVo
                        {
                            ActionType = Actions.ATTACK,
                            TargetPoint = display.GoBase.transform.position
                        };
                        StartCoroutine(YieldSecordsAutoAttack(0.5f, attackVo));
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     延迟delay秒进行攻击，弱化AI
        /// </summary>
        /// <param name="delay">攻击之间的间隔</param>
        /// <param name="toActionVo">攻击行为VO</param>
        /// <returns></returns>
        private IEnumerator YieldSecordsAutoAttack(float delay, ActionVo toActionVo)
        {
            yield return new WaitForSeconds(delay);
            MeController.AttackController.AddAttackList(toActionVo);
        }
    }
}