using com.game.module.effect;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;

/**玩家游戏对象类*/

namespace com.u3d.bases.display.character
{
    public class PlayerDisplay : ActionDisplay
    {
        //无敌特效
        private GameObject _invincibleEffect;

        protected override string SortingLayer
        {
            get { return "Player"; }
        }

        public PetDisplay PetDisplay;

        /// <summary>
        ///     挂载脚本，建立脚本之间的对应关系
        /// </summary>
        /// <param name="go"></param>
        protected override void AddScript(GameObject go)
        {
            if (go.GetComponent<PlayerControler>() != null) return;
            Controller = go.AddComponent<PlayerControler>();
            Controller.Me = this;
            Controller.Me.Animator = Controller.Me.GoCloth.GetComponent<Animator>();

            Controller.SkillController = go.AddComponent<SkillController>();
            Controller.SkillController.MeController = Controller as PlayerControler;

            //增加移动控制脚本
            var moveContorller = go.AddComponent<PlayerMoveController>();
            moveContorller.AnimationEventController = Controller.AnimationEventController;
            Controller.MoveController = moveContorller;

            //增加状态控制脚本
            var statuController = go.AddComponent<PlayerStatuController>();
            statuController.MeControler = Controller as ActionControler;
            Controller.StatuController = statuController;

            //增加攻击控制脚本
            var attackController = go.AddComponent<PlayerAttackController>();
            attackController.MeController = Controller as ActionControler;
            Controller.AttackController = attackController;

            Controller.AnimationEventController = GoCloth.GetComponent<AnimationEventController>() ??
                                                  GoCloth.AddComponent<AnimationEventController>();
            Controller.AnimationEventController.skillController = Controller.SkillController;

            //增加抓投控制脚本;
            var projectileController = GoCloth.GetComponent<GraspThrowController>() ?? GoCloth.AddComponent<GraspThrowController>();
            projectileController.MeController = Controller as ActionControler;
            Controller.GraspThrowController = projectileController;
            Controller.AnimationEventController.GraspThrowController = Controller.GraspThrowController ;

            //增加动画参数控制脚本
            Controller.AnimationParameter = GoCloth.GetComponent<AnimationParameter>() ??
                                            GoCloth.AddComponent<AnimationParameter>();

            //增加受击控制脚本
            var beAttackedController = go.AddComponent<PlayerBeAttackedController>();
            beAttackedController.meController = Controller as ActionControler;
            Controller.BeAttackedController = beAttackedController;

            //增加死亡控制脚本
            var deathController = go.AddComponent<PlayerDeathController>();
            deathController.MeController = Controller as ActionControler;

            //增加AI控制脚本
            var aiController = go.AddComponent<JianShiAiController>();
            aiController.MeController = Controller as ActionControler;
            Controller.AiController = aiController;
            BoxCollider2D = GoCloth.GetComponent<BoxCollider2D>();
            BoxCollider2D.enabled = false;
            SetStandClothGoPosition();
            GetMeVoByType<PlayerVo>().Controller = Controller as ActionControler;
        }

        public new BaseRoleVo GetVo()
        {
            return Vo as BaseRoleVo;
        }

        public void Relive()
        {
            Controller.StatuController.SetStatu(Status.IDLE);
        }

        public void Death()
        {
            if (Controller == null)
            {
                return;
            }
            Controller.StatuController.SetStatu(Status.DEATH);
        }

        /// <summary>
        ///     显示无敌特效
        /// </summary>
        public void ShowInvincibleEffect()
        {
            if (_invincibleEffect == null)
            {
                EffectMgr.Instance.CreateMainFollowEffect(EffectId.Main_Resurrection, GoBase.gameObject, Vector3.zero,
                    false, null, CreateInvincibleEffectBack);
            }
        }

        private void CreateInvincibleEffectBack(GameObject invincibleEffect)
        {
            _invincibleEffect = invincibleEffect;
        }

        /// <summary>
        /// 移除无敌特效
        /// </summary>
        public void RemoveInvincibleEffect()
        {
            if (_invincibleEffect != null)
            {
                EffectMgr.Instance.RemoveEffect(_invincibleEffect.name);
                _invincibleEffect = null;
            }
        }

        override public void Dispose()
        {
            if (PetDisplay != null)
            {
                Object.Destroy(PetDisplay.GoBase);
                PetDisplay = null;
            }
            base.Dispose();
        }
    }
}