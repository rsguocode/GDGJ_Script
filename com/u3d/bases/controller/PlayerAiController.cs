/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/28 04:32:44 
 * function: 玩家AI控制器
 * *******************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Arena;
using com.game.module.fight.vo;
using Com.Game.Module.GoldSilverIsland;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using UnityEngine;

namespace com.u3d.bases.controller
{
    public class PlayerAiController : AiControllerBase
    {
        private Animator _animator;
        private PlayerVo _mePlayerVo; //当前脚本所属对象的vo
        private float _stopTime;


        private void Start()
        {
            _mePlayerVo = MeController.Me.GetVo() as PlayerVo;
            BaseRoleVo = _mePlayerVo;
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

            //处理挂机自动攻击
            if (IsAi)
            {
                if (MeController.StatuController.CurrentStatu == Status.IDLE)
                {
                    _stopTime += Time.deltaTime;
                    if (_stopTime > 1)
                    {
                        _stopTime = 0;
                        AiSearchBehaviour();
                    }
                    AiAttackBehaviour();
                }
                else
                {
                    _stopTime = 0;
                }
            }
        }

        /// <summary>
        ///     AI寻路行为          // 移动到 可以攻击敌人的 那个位置
        /// </summary>
        private void AiSearchBehaviour()
        {
            Vector3 point = MeController.Me.GoBase.transform.position;
            float distance = 1000; //1000已经够大了
            Vector3 target = point;
            foreach (ActionDisplay display in GetEnemyDisplay()) // 找出最近的一个敌人
            {
                float temDis = Mathf.Abs(display.GoBase.transform.position.x - point.x);
                if (temDis < distance) 
                {
                    distance = temDis;
                    target = display.GoBase.transform.position;
                }
            }
            const float attackRange = 1f;
            bool needFellow = false;
            if (point.x < target.x - attackRange) // 太近了
            {
                point.x = target.x - attackRange; // 移动到攻击范围的那个点
                needFellow = true;
            }
            else if (point.x > target.x + attackRange) // 太远了
            {
                point.x = target.x + attackRange; // 移动到攻击范围的那个点
                needFellow = true;
            }
            if (needFellow)
            {
                _stopTime = 0;
                var actionVo = new ActionVo {RunDestination = point, ActionType = Actions.RUN};
                MeController.AttackController.AddAttackList(actionVo);
            }
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
            else if (MeVo.instance.mapId == MapTypeConst.ARENA_MAP &&
                     _mePlayerVo.Id == Singleton<ArenaMode>.Instance.vsPlayerAttr.Id)
            {
                Singleton<ArenaMode>.Instance.DataUpdate(Singleton<ArenaMode>.Instance.UPDATE_VSER_HP_MP);
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
                    {// 可以攻击的条件
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