using com.game;
using com.game.consts;
using com.u3d.bases.consts;
using com.u3d.bases.display.character;
//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：GraspThrowController;
//文件描述：抓投脚本;
//创建者：潘振峰;
//创建日期：2014/6/16 18:54:26;
//////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.u3d.bases.display.controler
{
    public class GraspThrowController : MonoBehaviour
    {
        /// <summary>
        /// 用于间隔检测，后期可能会改为读取配置数据;
        /// </summary>
        private int INTERVAL_CHECK = 5;
        private int countTime = 0;
        public ActionControler MeController;
        public GraspThrowStatus CurStatus = GraspThrowStatus.End;
        private float _grapStartTime = 0.0f;

        /// <summary>
        /// 抓取;
        /// </summary>
        public void Grasp()
        {
            //如果当前已经抓取了，就不让抓了;
            if (MeController.Me.IsGrasp || MeController.Me.IsGrasped)
            {
                return;
            }
            IList<MonsterDisplay> findMonsters = findNearestMonsters();
            BaseDisplay tmpGrapDisplay = null;
            if (findMonsters.Count > 0)
            {
                //搜寻当前状态是否有倒地的;
                foreach (MonsterDisplay md in findMonsters)
                {
                    if ((md.Controller.StatuController.CurrentStatu == Status.HURTDOWN ||
                        md.Controller.StatuController.CurrentStatu == Status.HURT3 ||
                        md.Controller.StatuController.CurrentStatu == Status.HURT4 ) && 
                        md.IsGrasped == false)
                    {
                        tmpGrapDisplay = md;
                        //Debug.Log("找到了倒地的怪物了");
                        break;
                    }
                }
                if (tmpGrapDisplay != null)
                {
                    //设置抓取者;
                    tmpGrapDisplay.SetGraspedController(MeController);
                    //设置被抓取者;
                    MeController.Me.SetGraspController(tmpGrapDisplay.Controller);
                    setActionControllerMoveChangeStatus((tmpGrapDisplay.Controller as ActionControler), false, false);
                    resetGraspState();
                    MeController.AttackController.AddGrapDispaly(
                        tmpGrapDisplay,
                        MeController,
                        PlayConst.POS_NECK,
                        true,
                        false);
                    CurStatus = GraspThrowStatus.Grasp;
                }
            }
        }

        /// <summary>
        /// 扔出去;
        /// </summary>
        public void Throw()
        {
            if (MeController.Me.GrapController != null)
            {
                countTime = 0;
                CurStatus = GraspThrowStatus.Throw;
                (MeController.Me.Controller as ActionControler).StopWalk();
                setActionControllerMoveChangeStatus((MeController.Me.Controller as ActionControler), false, false);
                StartCoroutine(MeController.AttackController.ThrowDisplay(MeController.Me.GrapController.Me, MeController, Directions.GetOpposite(MeController.Me.CurFaceDire), throwComplete));
            }
        }

        public void Update()
        {
            if (CurStatus == GraspThrowStatus.Grasp)
            {
                if ((Time.time - _grapStartTime) > PlayConst.GRASP_RELEASE_AUTO_TIME)
                {
                    graspTimeEnd();
                }
            }
            if (CurStatus == GraspThrowStatus.Throw && MeController.Me.GrapController != null)
            {
                if ((countTime++ % INTERVAL_CHECK) == 0)
                {
                    //检测伤害;
                    if (MeController.Me.GrapController != null)
                    {
                        bool result = MeController.SkillController.CheckDamage(MeController.Me.GrapController.transform.position, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 抓取时间到，没有投掷需要触发的接口;
        /// </summary>
        private void graspTimeEnd()
        {
            setActionControllerMoveChangeStatus((MeController.Me.GrapController as ActionControler), true, true);
            releaseGrapRelation(GraspThrowStatus.TimeEnd);
        }

        private void setActionControllerMoveChangeStatus(ActionControler actller, bool _canMove, bool _canChangeState)
        {
            actller.CanMove = _canMove;
            actller.CanChangeStatus = _canChangeState;
        }

        private void throwComplete()
        {
            setActionControllerMoveChangeStatus((MeController.Me.Controller as ActionControler), true, true);
            if (MeController.Me.GrapController != null)
            {
                setActionControllerMoveChangeStatus((MeController.Me.GrapController as ActionControler), true, true);
            }
            releaseGrapRelation(GraspThrowStatus.End);
        }


        private void resetGraspState()
        {
            _grapStartTime = Time.time;
            MeController.Me.GrapController.Me.clearDirectionKeyCount();
        }

        /// <summary>
        /// 打断抓取状态;
        /// 如果成功，会自动将抓取者，以及被抓取者对象成员置空;
        /// </summary>
        /// <param name="force">是否强制打断;</param>
        public bool InterceptGrasp(bool force = false)
        {
            if((GraspThrowStatus.Grasp == CurStatus) || force)
            {
                setActionControllerMoveChangeStatus((MeController.Me.Controller as ActionControler), true, true);
                if (MeController.Me.GrapController != null)
                {
                    setActionControllerMoveChangeStatus((MeController.Me.GrapController as ActionControler), true, true);
                }
                releaseGrapRelation(GraspThrowStatus.Intercept);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解除抓取关系;
        /// </summary>
        /// <param name="clearGraspQuote">将引用置空;</param>
        /// <param name="status"></param>
        private void releaseGrapRelation(GraspThrowStatus status)
        {
            if (MeController.Me.GrapController != null)
            {
                MeController.Me.GrapController.Me.SetGraspedController(null);
            }
            MeController.Me.SetGraspController(null);
            CurStatus = status;
        }

        /// <summary>
        /// 获取范围内的最近的怪物列表;
        /// </summary>
        /// <returns></returns>
        private IList<MonsterDisplay> findNearestMonsters()
        {
            IList<MonsterDisplay> findMonsters = AppMap.Instance.getMonsterNearestBy(MeController.Me.GetVo().Id.ToString(),
                PlayConst.NORMAL_GRAB_X_OFFSET,
                PlayConst.NORMAL_GRAB_X_2_OFFSET,
                PlayConst.NORMAL_GRAB_Y_OFFSET,
                PlayConst.NORMAL_GRAB_Y_2_OFFSET,
                PlayConst.NORMAL_GRAB_Z_OFFSET,
                PlayConst.NORMAL_GRAB_Z_2_OFFSET
            );
            return findMonsters;
        }

        /// <summary>
        /// 投射物状态;
        /// </summary>
        public enum GraspThrowStatus
        {
            Throw,
            Grasp,
            End,
            TimeEnd,
            Intercept
        }
    }
}
