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
 * history:  created by guo ri sheng   2014/6/18 15:00
 * function: 怪物AI控制器
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class MonsterAiController_Far2 : AiControllerBase
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

            
            // run 3 秒就停
            if (MeController.StatuController.CurrentStatu == Status.RUN) 
            {
                _runTime += Time.deltaTime;
                if (_runTime >= 3f)
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

            if (MeController.StatuController.CurrentStatu == Status.IDLE)
            {
                SelfProtected_1();
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
                    point.x = MapControl.Instance.MyCamera.LeftBoundX; // 远程兵跑到屏幕'左边' 
                else // 主角在 左边
                    point.x = MapControl.Instance.MyCamera.RightBoundX;

                MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
                if (Mathf.Abs(point.y - mapRange.MinY) < Mathf.Abs(point.y - mapRange.MaxY))
                {// 离下面比较近，就往上跑
                    point.y = mapRange.MaxY;
                }
                else
                    point.y = mapRange.MinY;

                _runTime = 0;
                var attackVo2 = new ActionVo
                {
                    ActionType = Actions.RUN, // 逃跑动作
                    RunDestination = point
                };
                MeController.AttackController.AddAttackList(attackVo2);
            }
        }          
       
    }
}