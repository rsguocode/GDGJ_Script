using com.game.module.effect;
using com.game.module.fight.vo;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.character;
using UnityEngine;
using System.Collections;

/**受击控制类
 * @author 骆琦
 * @date  2013-11-13
 * 实现怪物受击控制处理
 * **/

namespace com.u3d.bases.controller
{
    public class MonsterBeAttackedController : BeAttackedControllerBase
    {
        private const float Hurt1Time = 0.8f;
        private const float Hurt2Time = 0.8f;
        private const float Hurt3Time = 0.8f;

        private void ResetCombHurtStatu()
        {
            meController.StatuController.SetStatu(Status.IDLE);
        }

        private void RemoveModelEffect()
        {
            meController.Me.EndModelColorEffect();
        }

        public override void BeAttacked(ActionVo actionVo)
        {
            base.BeAttacked(actionVo);
            actionVo.Target = meController.Me.GoCloth; // 显示特效的目标对象
            float zoom = Random.Range(5, 10)/5f;
            float rotateZ = Random.Range(0, 360);
            meController.AttackController.ShowBeAttackedEffect(actionVo, zoom, rotateZ); //只要受击了就显示受击特效，怪物受击特效大小随机
            meController.Me.StartShowModelBeAttackedEffect();
            var monsterVo = meController.GetMeVoByType<MonsterVo>();
            if (meController.StatuController.CurrentStatu == Status.ATTACK1 ||
                meController.StatuController.CurrentStatu == Status.ATTACK2)
            {
                if (meController.AnimationParameter.IsWarning/*!meController.AnimationParameter.IsWarning*/)
                {
                    return;
                }
                
            }

            //浮空处理
            /*
            if (monsterVo.LeftFloatingTime > 0 && monsterVo.CurrentFloatingNumber < BaseRoleVo.MaxFloatingNumber)
            {
                monsterVo.CurrentFloatingNumber++;
                monsterVo.LeftFloatingTime = monsterVo.CurrentFloatingNumber == BaseRoleVo.MaxFloatingNumber
                    ? 0.6f
                    : 0.5f;
                return;
            }
            if (actionVo.FloatingValue > monsterVo.MonsterVO.Floating_Resist)
            {
                if (monsterVo.LeftFloatingTime > 0)
                {
                    monsterVo.CurrentFloatingNumber = 1;
                    monsterVo.LeftFloatingTime = 0.5f;
                }
                else
                {
                    monsterVo.IsFloating = true;
                }
            }*/
            //受击后按照受击方向产生击飞
            if(actionVo.Velocity_Origin > 0)
            {
                monsterVo.FloatingXSpeed = actionVo.Velocity_Origin * Mathf.Cos(actionVo.Angle * Mathf.PI / 180f);
                monsterVo.FloatingYSpeed = actionVo.Velocity_Origin * Mathf.Sin(actionVo.Angle * Mathf.PI / 180f);
                monsterVo.AtkerDirection = actionVo.FaceDirection;
                SetHurt4(actionVo);
                return;
            }
            if (meController.IsFloating())
            {
                SetHurt4(actionVo);
                return;
            }
            
            //受击处理
            /*
            if (monsterVo.HurtResist >= 100)
                        {
                            return;
                        }
                        if (monsterVo.HurtResist > 0)
                        {
                            monsterVo.HurtResist += 5;
                        }
                        if (monsterVo.HurtResist >= 100)
                        {
                            if (meController.Me.DefenceEffect == null)
                            {
                                Vector3 pos = Vector3.zero;
                                BoxCollider2D boxCollider2D = meController.GetMeByType<MonsterDisplay>().BoxCollider2D;
                                float y = (boxCollider2D.center.y + boxCollider2D.size.y/2)*
                                          meController.Me.GoCloth.transform.localScale.y;
                                pos.y = y + 0.6f;
                                GameObject monsterFranticEffect =
                                    EffectMgr.Instance.GetSkillEffectGameObject(EffectId.Skill_MonsterFrantic);
                                meController.Me.DefenceEffect = Instantiate(monsterFranticEffect) as GameObject;
                                if (meController.Me.DefenceEffect != null)
                                {
                                    meController.Me.DefenceEffect.transform.parent = meController.Me.GoBase.transform;
                                    meController.Me.DefenceEffect.transform.localPosition = pos;
                                    meController.Me.DefenceEffect.SetActive(true);
                                }
                            }
                            else
                            {
                                Vector3 pos = Vector3.zero;
                                BoxCollider2D boxCollider2D = meController.GetMeByType<MonsterDisplay>().BoxCollider2D;
                                float y = (boxCollider2D.center.y + boxCollider2D.size.y/2)*
                                          meController.Me.GoCloth.transform.localScale.y;
                                pos.y = y + 0.6f;
                                meController.Me.DefenceEffect.transform.localPosition = pos;
                            }
                        }*/
            
            int dir = actionVo.SkillUsePoint.x < meController.Me.GoBase.transform.position.x
                ? Directions.Left
                : Directions.Right;
            meController.Me.ChangeDire(dir);

            //meController.gameObject.transform.position = actionVo.HurtDestination;//不需要
            switch (actionVo.HurtType)
            {
                    //根据原因来执行相应的受击表现
                case Actions.HurtNormal:
                    /*
                    if (meController.StatuController.CurrentStatu == Status.HURT1 || meController.StatuController.CurrentStatu == Status.Hurt1Recover)
                                        {
                                            meController.StatuController.SetStatu(Status.HURT2);
                                        }
                                        else
                                        {
                                            meController.StatuController.SetStatu(Status.HURT1);
                                        }*/
                    monsterVo.ProtectValue += actionVo.ProtectValue;
                    if(!monsterVo.IsProtecting)
                    {
                        if(actionVo.HurtAnimation == 1)
                        {
                            SetHurt1(actionVo);
                        }
                        else if (actionVo.HurtAnimation == 2)
                        {
                            SetHurt2(actionVo);
                        }
                    }
                    else
                    {
                        SetHurt3(actionVo);
                    }
                    break;
                case Actions.HurtFly:
                    if (monsterVo.HurtDownResist >= 100)
                    {/*
                    
                                            meController.StatuController.SetStatu(Status.HURT4);*/
                        SetHurt4(actionVo);
                        meController.GetMeVo().HurtDownResist = (uint) monsterVo.MonsterVO.hurt_down_resist;
                    }
                    else
                    {
                        monsterVo.HurtDownResist += 5;
                        /*
                        if (meController.StatuController.CurrentStatu == Status.HURT1)
                                                {
                                                    meController.StatuController.SetStatu(Status.HURT2);
                                                }
                                                else
                                                {
                                                    meController.StatuController.SetStatu(Status.HURT1);
                                                }*/
                        meController.StatuController.SetStatu(Status.HURT1);
                    }
                    break;
            }
        }
    }
}