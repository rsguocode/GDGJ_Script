using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.fight.vo;
using com.game.utils;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.u3d.bases.display;
using Com.Game.Utils;
using com.game;
using com.u3d.bases.display.character;
using com.game.consts;
using System.Collections;
using System;

/**普通攻击控制类
 * @author 骆琦
 * @date  2013-11-6
 * 实现普通攻击的连招控制
 * **/

namespace com.u3d.bases.controller
{
    public class AttackControllerBase : MonoBehaviour
    {
        public List<ActionVo> AttackList; //动作列表，暂时定义最多长度2，后来加进来的动作顶替
        protected ActionVo CurVo; //当前攻击信息vo
        public ActionControler MeController;

        // Use this for initialization
        private void Start()
        {
            AttackList = new List<ActionVo>();
        }


        /// <summary>
        ///     执行动作添加到队列
        /// </summary>
        /// <param name="vo">动作信息</param>
        /// <param name="isPush">强行插入</param>
        public virtual void AddAttackList(ActionVo vo, bool isPush = false)
        {
        }

        //获得目标特效
        private string GetTargetEffectId(uint skillId, int index, bool isBullet)
        {
            SysSkillBaseVo skillVo = BaseDataMgr.instance.GetSysSkillBaseVo(skillId);
            if (null == skillVo)
            {
                Log.info(this, "SkillBaseVo表不存在id为" + skillId + "的技能");
                return null;
            }
            SysSkillActionVo actionVo = BaseDataMgr.instance.GetSysSkillActionVo(skillVo.skill_group);
            if (null == actionVo)
            {
                Log.info(this, "Action表不存在id为" + skillVo.skill_group + "的技能组");
                return null;
            }

            string[] effectIds = StringUtils.GetValueListFromString(actionVo.tar_eff);

            if (index < effectIds.Length)
            {
                if (isBullet)
                {
                    return effectIds[index];
                }
                int randomIndex = UnityEngine.Random.Range(0, effectIds.Length);
                return effectIds[randomIndex];
            }
            return string.Empty;
        }

        //显示受击特效
        public void ShowBeAttackedEffect(ActionVo vo, float zoom = 1f, float rotateZ = 0f)
        {
            if (null == vo)
            {
                return;
            }
            string effectId = GetTargetEffectId(vo.SkillId, vo.HurtEffectIndex, vo.IsBullet);
            var effectVo = new Effect
            {
                URL = UrlUtils.GetSkillEffectUrl(effectId),
                Direction =
                    (MeController.transform.position.x > vo.SkillUsePoint.x) ? Directions.Right : Directions.Left,
                BasePosition = MeController.transform.position,
                Offset = new Vector3(0f, 0.55f, 0f),
                Target = vo.Target,
                Zoom = zoom,
                EulerAngles = new Vector3(0f, 0f, rotateZ),
                NeedCache = true
            };
            EffectMgr.Instance.CreateSkillEffect(effectVo);
        }

        /// <summary>
        /// 攻击者攻击后产生的力反馈效果
        /// </summary>
        public void ForceFeedBack(ActionVo vo)
        {
            if (vo.ForceFeedBack > 0)
            {
                var meVo = MeController.GetMeVo();
                meVo.ForceFeedBackTime = vo.ForceFeedBack * 0.001f;
                meVo.IsForceFeedBack = true;
            }
        }


        /// <summary>
        /// 添加抓取投掷;
        /// </summary>
        /// <param name="grapDisplay">被抓取者</param>
        /// <param name="hostController">抓取者</param>
        /// <param name="setTag">PlayConstant中的部位字符串</param>
        /// <param name="autoIfNoTag">如果setTag的部位为null，那么使用display的位置</param>
        public void AddGrapDispaly(BaseDisplay grapDisplay, BaseControler hostController, string setTag, bool autoIfNoTag = true, bool autoThrow = true, Action throwComplete = null)
        {
            if (grapDisplay != null)
            {
                GameObject partGo = PlayUtils.GetPartBonesByHostAndTag(hostController.Me.GetVo().Id.ToString(), setTag);
                Vector3 tmpPos;
                //判断部件位置;
                if (partGo != null)
                {
                    tmpPos = partGo.transform.position;
                }
                else
                {
                    tmpPos = hostController.Me.Controller.transform.position;
                }
                PlayUtils.MakeAFaceToB(hostController.Me, grapDisplay);
                grapDisplay.Pos(tmpPos);
                if (autoThrow)
                {
                    StartCoroutine(ThrowDisplay(grapDisplay, hostController, Directions.GetOpposite(hostController.Me.CurFaceDire), throwComplete));
                }
            }
        }



        public IEnumerator ThrowDisplay(BaseDisplay throwDisplay, BaseControler hostController, int dire = 0, Action throwComplete = null)
        {
            if (throwDisplay != null)
            {
                //Debug.Log("开始投掷*******************************");
                MoveControllerBase mcb = throwDisplay.Controller.MoveController;
                StatuControllerBase scb = throwDisplay.Controller.StatuController;
                float f = 0.0f;
                if (mcb != null)
                {
                    if (throwDisplay is MonsterDisplay)
                    {
                        f = 2 / ((throwDisplay as MonsterDisplay).BoxCollider2D.size.x);
                        (mcb as MonsterMoveController).StartY = throwDisplay.Controller.transform.position.y - 0.1f;
                        yield return StartCoroutine((mcb as MonsterMoveController).MoveByParabola(0.68f * f, 4.0f * f, 32.0f * f, dire));
                    }
                    else if (throwDisplay is PlayerDisplay)
                    {
                        yield return StartCoroutine((throwDisplay.Controller.StatuController as PlayerStatuController).MoveByParabola(throwDisplay.Controller.Me.GoBase, SpeedConst.PlayerHurtBackMoveSpeed, 6.0f, 30.0f));
                    }
                    //Debug.Log("结束投掷****************结束***************");
                }
            }
            if (throwComplete != null)
            {
                throwComplete();
            }
        }

        /// <summary>
        ///     是否在战斗状态
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInBattle()
        {
            int statu = MeController.StatuController.CurrentStatu;
            return statu == Status.ATTACK1 || statu == Status.ATTACK2 || statu == Status.ATTACK3 ||
                   statu == Status.ATTACK4 || statu == Status.SKILL1
                   || statu == Status.SKILL2 || statu == Status.SKILL3;
        }
    }
}