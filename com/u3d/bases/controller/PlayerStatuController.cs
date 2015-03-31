using System.Collections;
using com.game;
using Com.Game.Module.Arena;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game.consts;

/**玩家状态控制器
 * @author 骆琦
 * @date   2013-11-3
 * 基于animator的玩家状态控制器
 * **/

namespace com.u3d.bases.controller
{
    public class PlayerStatuController : StatuControllerBase
    {
        protected PlayerVo _playerVo;

        protected void Awake()
        {
            MeControler = GetComponent<ActionControler>();
            _playerVo = MeControler.Me.GetVo() as PlayerVo;
            switch (_playerVo.job)
            {
                case 1:
                case 3:
                    MaxComb = 4;
                    break;
                case 2:
                    MaxComb = 3;
                    break;
                default:
                    MaxComb = 3;
                    break;
            }
        }

        private void Start()
        {
            PreStatuNameHash = Status.NAME_HASH_IDLE;
        }

        protected override void DoStatuTransfered()
        {
            base.DoStatuTransfered();
            if (PreStatuNameHash == Status.NAME_HASH_DEATH_END)
            {
                if (_playerVo == null)
                {
                    _playerVo = MeControler.Me.GetVo() as PlayerVo;
                }
                _playerVo.IsUnbeatable = true; //设置玩家复活时3S钟的无敌状态
                Invoke("ResetUnbeatableStatu", 3.0f);
            }
        }

        protected override void DoStatuChange()
        {
            base.DoStatuChange();
            switch (CurrentStatu)
            {
                case Status.DEATH:
                    ArenaControl.Instance.OneHeroDeath(MeControler.GetMeVo().Id);
                    break;
                    /*case Status.HURT2:
                    if (!MeControler.GetMeVo().NeedKeep)
                    {
                        //StartCoroutine(MoveByParabola(MeControler.Me.GoBase, SpeedConst.PlayerHurtBackMoveSpeed, 6.0f, 30.0f));
                        int dir = MeControler.Me.CurDire == Directions.Left ? 1 : -1;
                        float endX = MeControler.Me.GoBase.transform.position.x + dir*2;
                        //StartCoroutine(MoveBack(MeControler.Me.GoBase, 7.0f, endX));
                    }
                    break;*/
            }
            if (MeVo.instance.Id != MeControler.GetMeVo().Id && AppMap.Instance.mapParser.NeedSyn)
            {
                if (CurrentStatu == Status.RUN)
                {
                    MeControler.IsSynMove = true;
                }
                else
                {
                    MeControler.IsSynMove = false;
                }
            }
        }

        /// <summary>
        ///     模拟抛物线运动
        /// </summary>
        /// <param name="xSpeed">X轴初始速度</param>
        /// <param name="ySpeed">Y轴初始速度</param>
        /// <param name="accelerate">重力加速度</param>
        /// <returns></returns>
        public IEnumerator MoveByParabola(GameObject gameObj, float xSpeed, float ySpeed, float accelerate)
        {
            Vector3 position = gameObj.transform.position;
            float startY = position.y;
            int dir = MeControler.Me.CurFaceDire == Directions.Left ? 1 : -1;
            while (position.y >= startY)
            {
                position = gameObj.transform.position;
                float x = position.x + xSpeed*dir*Time.deltaTime;
                x = AppMap.Instance.mapParser.GetFinalMonsterX(x); //控制不让出界
                position.x = x;
                position.y += ySpeed*Time.deltaTime - accelerate*Time.deltaTime*Time.deltaTime*0.5f;
                ySpeed -= accelerate*Time.deltaTime;
                gameObj.transform.position = position;
                yield return 0;
            }
            MeControler.StatuController.SetStatu(Status.IDLE);
            yield return 0;
        }

        protected IEnumerator MoveBack(GameObject gameObj, float xSpeed, float endX)
        {
            Vector3 position = gameObj.transform.position;
            int dir = MeControler.Me.CurFaceDire == Directions.Left ? 1 : -1;
            endX = AppMap.Instance.mapParser.GetFinalMonsterX(endX); //边界处理
            while ((Directions.Left == MeControler.Me.CurFaceDire && (position.x < endX)) ||
                   (MeControler.Me.CurFaceDire == Directions.Right && position.x > endX))
            {
                position = gameObj.transform.position;
                float x = position.x + xSpeed*dir*Time.deltaTime;
                x = AppMap.Instance.mapParser.GetFinalMonsterX(x); //控制不让出界
                position.x = x;
                gameObj.transform.position = position;
                yield return 0;
            }
            MeControler.StatuController.SetStatu(Status.IDLE);
            yield return 0;
        }

        private void ResetUnbeatableStatu()
        {
            _playerVo.IsUnbeatable = false;
        }

        protected override void judgeNormalCombIfLongkey()
        {
            if (MeVo.instance.Id == MeControler.GetMeVo().Id && GameConst.IS_NORMAL_COMBO_BY_PRESS_LONG && Input.GetKey(KeyCode.J))
            {
                AppMap.Instance.MeControler().AttackController.AttackList.Clear();
                AppMap.Instance.MeControler().addNormalAttack(false);
            }
        }
    }
}