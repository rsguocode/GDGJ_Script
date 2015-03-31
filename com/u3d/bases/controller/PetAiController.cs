using System.Collections.Generic;
using System.Linq;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.map;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/05 04:51:57 
 * function: 宠物AI控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class PetAiController : AiControllerBase
    {
        public const float MaxDistance = 1.2f; //宠物远离主人的最大距离
        public Transform Target;
        private float _idleTime;
        private SysSkillBaseVo _talentSkillVo;
        private Transform _meTransform;
        public const int TargetEnemy = 3;  //作用于敌人

        private void Start()
        {
            Target = MeController.GetMeVoByType<PetVo>().MasterVo.Controller.transform;
            _idleTime = 0;
            var petVo = MeController.GetMeVoByType<PetVo>();
            _talentSkillVo = BaseDataMgr.instance.GetSysSkillBaseVo(petVo.SkillId);
            _meTransform = transform;
        }

        private void Update()
        {
            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                _idleTime += Time.deltaTime;
            }
            DoIdleLogic();
            if (IsAi)
            {
                if (_talentSkillVo.target_type == TargetEnemy)
                {
                    DoAttackLogic();
                }
                else
                {
                    CheckTalentSkill();
                }
            }
        }

        private void DoIdleLogic()
        {
            int mapType = AppMap.Instance.mapParser.MapVo.type;
            if (mapType == MapTypeConst.CITY_MAP)
            {
                FollowMaster();
            }
            else if (mapType == MapTypeConst.COPY_MAP&&_idleTime>1)
            {
                _idleTime = 0;
                var destination = GetMoveDestination();
                MeController.MoveTo(destination.x,destination.y);
            }
        }

        private void DoAttackLogic()
        {
            if (_talentSkillVo.target_type == TargetEnemy && MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                bool isSkillCdReady = MeController.TalentSkillController.IsSkillCdReady();
                if (isSkillCdReady)
                {
                    var result = TryAttack();
                    if (!result)
                    {
                        ActionDisplay nearestEnemy = FindNearestEnemyInMapRange();
                        if (nearestEnemy != null)
                        {
                            MoveToAttackRange(nearestEnemy);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     移动到攻击范围内
        /// </summary>
        /// <param name="enemyDisplay"></param>
        private void MoveToAttackRange(ActionDisplay enemyDisplay)
        {
            Vector3 mePoint = _meTransform.position;
            var pos = enemyDisplay.Controller.transform.position;
            if (mePoint.x < pos.x)
            {
                pos.x = pos.x - _talentSkillVo.cover_width*0.001f + 0.1f;
            }
            else
            {
                pos.x = pos.x + _talentSkillVo.cover_width * 0.001f - 0.1f;
            }
            MeController.MoveTo(pos.x,pos.y);
        }

        /// <summary>
        /// 获取移动目的地，以主人为中心，3个单位为半径的范围内
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMoveDestination()
        {
            Vector3 pos = Target.position;
            float angle = Random.Range(0, 6.28f);
            float radius = Random.Range(0, 3.0f);
            pos.x = pos.x + radius*Mathf.Cos(angle);
            pos.y = pos.y + radius*Mathf.Sin(angle);
            return pos;
        }

        private void FollowMaster()
        {
            Vector3 pos = GetDistination();
            if (Mathf.Abs(pos.x - transform.position.x + pos.y - transform.position.y) > MaxDistance - 0.1f)
            {
                MeController.MoveTo(pos.x, pos.y);
            }
        }


        private void CheckTalentSkill()
        {
            if (MeController.TalentSkillController.CheckCanUseTalentSkill())
            {
                MeController.SkillController.RequestUseSkill(SkillController.Attack1);
            }
        }


        private bool TryAttack()
        {
            return MeController.TalentSkillController.TryTalentAttack();
        }

        private void UpdateDir()
        {
            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                int dir = Target.transform.localScale.x < 0 ? Directions.Left : Directions.Right;
                MeController.Me.ChangeDire(dir);
            }
        }

        private Vector3 GetDistination()
        {
            Vector3 pos = transform.position;
            if (pos.x - Target.position.x > MaxDistance)
            {
                pos.x = Target.position.x + MaxDistance;
            }
            else if (pos.x - Target.position.x < -MaxDistance)
            {
                pos.x = Target.position.x - MaxDistance;
            }
            pos.y = Target.position.y + 0.1f;
            return pos;
        }

        /// <summary>
        ///     找到在当前副本阶段行走区域范围内最近的怪物
        /// </summary>
        /// <returns></returns>
        private ActionDisplay FindNearestEnemyInMapRange()
        {
            ActionDisplay result = null;
            float dis = 1000000;
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
            foreach (ActionDisplay actionDisplay in tempList)
            {
                float x = actionDisplay.GoBase.transform.position.x;
                float y = actionDisplay.GoBase.transform.position.y;
                MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
                if (x < mapRange.MinX || x > mapRange.MaxX || y < mapRange.MinY || y > mapRange.MaxY)
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
            return (mePos.x - enemyPos.x) * (mePos.x - enemyPos.x) + (mePos.y - enemyPos.y) * (mePos.y - enemyPos.y);
        }
    }
}