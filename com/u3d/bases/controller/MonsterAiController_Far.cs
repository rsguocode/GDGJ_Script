using System;
using com.game;
using com.game.consts;
using com.game.module.fight.vo;
using com.game.module.map;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.character;
using com.u3d.bases.map;
using UnityEngine;
using Random = UnityEngine.Random;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/28 04:02:34 
 * function: 怪物AI控制器
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class MonsterAiController_Far : AiControllerBase
    {
        private Animator _animator;
        private bool _checkNeedMoveAfterAttack;
        private bool _isMoveAfterAttack;
        private MonsterVo _meEnemyVo; //当前脚本所属对象的vo
        private Transform _selfTransform;
        private float _stopTime;
        private float _runTime;

        // Use this for initialization
        private void Start()
        {
            _meEnemyVo = MeController.Me.GetVo() as MonsterVo;
            BaseRoleVo = _meEnemyVo;
            _selfTransform = MeController.transform;
        }

        // Update is called once per frame
        private void Update()
        {
            // 获取animator的信息并根据animator信息进行相应的业务处理
            if (_animator == null)
            {
                _animator = MeController.Me.Animator;
                return;
            }

            if (!MonsterMgr.CanSetAi)
            {
                return;
            }

            
            // run 1 秒就停
            if (MeController.StatuController.CurrentStatu == Status.RUN) 
            {
                _runTime += Time.deltaTime;
                if (_runTime >= 2f)
                {
                    MeController.StatuController.SetStatu(Status.IDLE);
                }
            }


            if (MeController.StatuController.CurrentStatu == Status.IDLE &&
                MeController.StatuController.CurStatuNameHash == Status.NAME_HASH_IDLE)
            {
                if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS)
                {
                    Vector3 point;
                    MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange; // 地图边界
                    if (_selfTransform.position.x < MapControl.Instance.MyCamera.LeftBoundX + 1) // 使怪物移动不超出地图边界
                    {
                        float x = Mathf.Max(MapControl.Instance.MyCamera.LeftBoundX + 1, mapRange.MinX + 1);
                        point = new Vector3(Random.Range(x, x + 1), Random.Range(mapRange.MinY, mapRange.MaxY), 0);
                        var actionVo = new ActionVo {RunDestination = point, ActionType = Actions.RUN}; // 怪物将要run动作添加进attacklist
                        _runTime = 0;
                        MeController.AttackController.AddAttackList(actionVo);

                        return;
                    }
                    if (_selfTransform.position.x > MapControl.Instance.MyCamera.RightBoundX - 1) // 使怪物移动不超出地图边界
                    {
                        float x = Mathf.Min(MapControl.Instance.MyCamera.RightBoundX - 1, mapRange.MaxX - 1);
                        point = new Vector3(Random.Range(x - 1, x), Random.Range(mapRange.MinY, mapRange.MaxY), 0);
                        var actionVo = new ActionVo { RunDestination = point, ActionType = Actions.RUN };
                        _runTime = 0;
                        MeController.AttackController.AddAttackList(actionVo);
                        return;
                    }
                }
            }

            if (!IsAi)
            {
                return;
            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.nameHash == Status.NAME_HASH_ATTACK1 || stateInfo.nameHash == Status.NAME_HASH_ATTACK2)
            {
                if (!_checkNeedMoveAfterAttack)
                {
                    _checkNeedMoveAfterAttack = true;
                    int random = Random.Range(0, 1000);
                    if (_meEnemyVo != null && random < _meEnemyVo.MonsterVO.attackMovePro) // attackMovePro; 攻击后游走概率
                    {
                        _isMoveAfterAttack = true;
                    }
                }
            }
            else
            {
                _checkNeedMoveAfterAttack = false;
            }

            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                _stopTime += Time.deltaTime;
                if (_meEnemyVo != null && _stopTime > _meEnemyVo.MonsterVO.searchStop * 0.001) // searchStop; 寻路停顿
                {
                    AiSearchBehaviour_Far();
                    _stopTime = 0;
                    return;
                }
                if (_isMoveAfterAttack)
                {
                    _isMoveAfterAttack = false;
                    AiCruiseBehaviour();
                    return;
                }
            }
            else
            {
                _stopTime = 0;
            }

            if (Random.Range(0, 100) < 10)
            {
                //控制攻击频率，即直接退出 不会进行下面的攻击
                return;
            }

            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                AiAttackBehaviour();
            }
        }

        // 远程兵近身自我保护，逃跑
        //
        private void SelfProtected_1()
        {
            Vector3 playerPosition = AppMap.Instance.me.GoBase.transform.position; // 主角位置
            Vector3 point = _selfTransform.position;
            float nearDefend = _meEnemyVo.MonsterVO.near_defend * 0.1f;

            if (Math.Abs(playerPosition.x - _selfTransform.position.x) <= nearDefend)
            {
                if (_selfTransform.position.x < playerPosition.x) // 主角在 右边
                    point.x -= (nearDefend + 0.2f); // 远程兵跑到'左边' 
                else // 主角在 左边
                    point.x += (nearDefend + 0.2f);

                MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
                if (Mathf.Abs(point.y - mapRange.MinY) < Mathf.Abs(point.y - mapRange.MaxY))
                {// 离下面比较近，就往上跑
                    point.y += 0.02f;
                }
                else
                    point.y -= 0.02f;

                var attackVo2 = new ActionVo
                {
                    ActionType = Actions.ATTACK2, // 逃跑动作
                    TargetPoint = point
                };
                MeController.AttackController.AddAttackList(attackVo2);
            }
        }


        //
        // 远程兵近身自我保护，不逃跑(跑到攻击范围处进行攻击)
        //
        private void SelfProtected_2()
        {
            Vector3 playerPosition = AppMap.Instance.me.GoBase.transform.position; // 主角位置
            Vector3 point = _selfTransform.position;
            float attackRange = _meEnemyVo.MonsterVO.attack_range * 0.1f;

            if (Math.Abs(playerPosition.x - _selfTransform.position.x) <= attackRange)
            {
                if (_selfTransform.position.x < playerPosition.x) // 主角在 右边                
                    point.x -= _meEnemyVo.MonsterVO.attack_range; // 远程兵跑到'左边'攻击范围处                 
                else
                    point.x += _meEnemyVo.MonsterVO.attack_range; // 远程兵跑到'右边'攻击范围处 

                point.y = point.y + Random.Range(-300, 300) * 0.001f;
                _runTime = 0;
                var attackVo2 = new ActionVo
                {
                    ActionType = Actions.RUN, // 跑到攻击范围
                    RunDestination = point
                };
                MeController.AttackController.AddAttackList(attackVo2);
            }            

        }
        /// <summary>
        ///     AI攻击行为       主要是：ActionType = Actions.ATTACK ，    MeController.AttackController.AddAttackList(actionVo);
        /// </summary>
        private void AiAttackBehaviour()
        {
            if (_meEnemyVo == null) return;
            foreach (PlayerDisplay display in AppMap.Instance.playerList)
            {
                BaseRoleVo baseRoleVo = display.GetVo();
                if (baseRoleVo != null && baseRoleVo.CurHp <= 0) continue;
                Vector3 playerPosition = display.Controller.transform.position; // 主角位置;
                Vector3 monsterPosition = _selfTransform.position; // 怪本身位置;

                float attackRange = _meEnemyVo.MonsterVO.attack_range * 0.1f;

                if (Math.Abs(playerPosition.x - monsterPosition.x) < attackRange && _meEnemyVo.CurHp > 0
                    && Math.Abs(playerPosition.y - monsterPosition.y) < 0.4f) // 增加y轴的判断,应该还要增加一个 面向 的判断
                {
                    if (_meEnemyVo.CurHp < _meEnemyVo.MonsterVO.blood_change_state) 
                    {// 当血量小于‘值’时换套攻击动作，目前该动作还没添加 new 
                        var attackVo2 = new ActionVo
                        {
                            ActionType = Actions.ATTACK2,
                            TargetPoint = display.GoBase.transform.position
                        };
                        print("****新攻击动作");
                        MeController.AttackController.AddAttackList(attackVo2);
                        break;
                    }
                    else
                    {
                        var attackVo = new ActionVo
                        {
                            ActionType = Actions.ATTACK, // 动作的行为，比如attack，或者run
                            TargetPoint = display.GoBase.transform.position  //GoBase 游戏模型的挂载体,即怪物要攻击的主角;
                        };
                        print("****攻击");
                        MeController.AttackController.AddAttackList(attackVo);
                        break;
                    }

                }
            }
        }

        /// <summary>
        ///     攻击后游走行为       主要是：ActionType = Actions.RUN ，    MeController.AttackController.AddAttackList(actionVo);
        /// </summary>
        private void AiCruiseBehaviour()
        {
            if (_meEnemyVo == null) return;
            Vector3 point = AppMap.Instance.me.GoBase.transform.position; // GoBase,怪是MonsterDisplay，me 是MeDisplay
            bool needFellow = false;
            if (_selfTransform.position.x < point.x) // 主角在右边， MeController = MonsterDisplay
            {
                point.x = MeController.transform.position.x - _meEnemyVo.MonsterVO.searchLength; // searchLength 寻路步长, 
                float maxX = MapControl.Instance.MyCamera.RightBoundX - 3;
                if (point.x > maxX) // 不能超出‘右边’屏幕范围
                {
                    point.x = maxX;
                }
                point.y = point.y + Random.Range(-150, 150) * 0.01f;
                needFellow = true;
            }
            else if (_selfTransform.position.x > point.x) // 主角在左边
            {
                point.x = _selfTransform.position.x + _meEnemyVo.MonsterVO.searchLength; // 被打了就往右走，即离开
                float minX = MapControl.Instance.MyCamera.LeftBoundX + 3;
                if (point.x < minX) // 不能超出‘左边’屏幕范围
                {
                    point.x = minX;
                }
                point.y = point.y + Random.Range(-150, 150) * 0.01f;
                needFellow = true;
            }
            if (needFellow)
            {
                _stopTime = 0;
                var attackVo = new ActionVo { RunDestination = point, ActionType = Actions.RUN };
                MeController.AttackController.AddAttackList(attackVo, true);
            }
        }

        /// <summary>
        ///     AI巡逻行为  a    主要是：ActionType = Actions.RUN ，    MeController.AttackController.AddAttackList(actionVo);
        ///     函数功能：使怪物靠近 主角：
        ///         1. 在attack2攻击范围‘外’，则向主角方向移动 searchLength 距离
        ///         2. 主角在attack1攻击范围‘外’,attack2‘内，则 移动 searchLength 或  -= attackRange1，看概率
        ///         3. 主角在attack1攻击范围‘内’
        /// </summary>
        private void AiSearchBehaviour_Far()
        {
            if (_meEnemyVo == null || _meEnemyVo.MonsterVO.attack_range == 0) return;
            Vector3 PlayerPoint = AppMap.Instance.me.GoBase.transform.position; // 主角位置
            Vector3 targetPoint = _selfTransform.position;
            float attackRange = _meEnemyVo.MonsterVO.attack_range * 0.1f;
            bool needFellow = false;

            float n = Random.Range(0, 10f);
            
            if (n < 5)
            {
                targetPoint.x = PlayerPoint.x - attackRange; // 移动到主角左边 攻击范围处;
            }
            else
            {
                targetPoint.x = PlayerPoint.x + attackRange; // 移动到主角右边 攻击范围处;
            }

            /*
            if (_selfTransform.position.x < PlayerPoint.x) // 主角在 右边
            {
                if (_selfTransform.position.x + attackRange < PlayerPoint.x)
                {
                    targetPoint.x = PlayerPoint.x - attackRange; // 移动到攻击范围处;
                }
                else
                {
                    targetPoint.x = _selfTransform.position.x;
                }
                needFellow = true;           
            }
            else // 主角在 左边
            {
                if (_selfTransform.position.x - attackRange > PlayerPoint.x)
                {
                    targetPoint.x = PlayerPoint.x - attackRange; // 移动到攻击范围处;
                }
                else
                {
                    targetPoint.x = _selfTransform.position.x;
                } 
                needFellow = true;
            }
            */
            targetPoint.y = PlayerPoint.y + Random.Range(-40, 40) * 0.01f;
            _runTime = 0;

            _stopTime = 0;
            var actionVo = new ActionVo { RunDestination = targetPoint, ActionType = Actions.RUN };
            MeController.AttackController.AddAttackList(actionVo);            
        }      

        private void AiMoveToScreenCenter()
        {
            Vector3 targetPoint = _selfTransform.position;
            MyCamera myCamera = MapControl.Instance.MyCamera;
            if (targetPoint.x < myCamera.LeftBoundX + 1.5f)
            {
                targetPoint.x = Random.Range(myCamera.LeftBoundX + 2.1f, myCamera.LeftBoundX + 3.5f);
            }
            else if (targetPoint.x > myCamera.RightBoundX - 2.1f)
            {
                targetPoint.x = Random.Range(myCamera.RightBoundX - 2.1f, myCamera.RightBoundX - 3.5f);
            }
            var actionVo = new ActionVo {RunDestination = targetPoint, ActionType = Actions.RUN};
            MeController.AttackController.AddAttackList(actionVo);
        }
    }
}