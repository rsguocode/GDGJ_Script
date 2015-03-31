using com.game;
using Com.Game.Module.Role;
using com.game.vo;
using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game.manager;

/**主角自己显示逻辑类*/
namespace com.u3d.bases.display.character
{
    public class MeDisplay : PlayerDisplay
    {
        override protected string SortingLayer { get { return "Player"; } }

        override protected void AddScript(GameObject go)
        {
            if (go.GetComponent<MeControler>() != null) return;
            Controller = go.AddComponent<MeControler>();
            Controller.Me = this;
            Controller.Me.Animator = GoCloth.GetComponent<Animator>();
            //增加状态控制脚本
            var statuController = go.AddComponent<MeStatuController>();
            statuController.MeControler = Controller as ActionControler;
            Controller.StatuController = statuController;

            //增加技能控制脚本
            Controller.SkillController = go.AddComponent<SkillController>();
            Controller.SkillController.MeController = Controller as PlayerControler;

            //增加动画事件控制脚本
            Controller.AnimationEventController = GoCloth.GetComponent<AnimationEventController>() ?? GoCloth.AddComponent<AnimationEventController>();
            Controller.AnimationEventController.skillController = Controller.SkillController;

            //增加投射物控制脚本;
            var projectileController = GoCloth.GetComponent<GraspThrowController>() ?? GoCloth.AddComponent<GraspThrowController>();
            projectileController.MeController = Controller as ActionControler;
            Controller.GraspThrowController = projectileController;
            Controller.AnimationEventController.GraspThrowController = Controller.GraspThrowController;

            //增加动画参数控制脚本
            Controller.AnimationParameter = GoCloth.GetComponent<AnimationParameter>() ?? GoCloth.AddComponent<AnimationParameter>();

            //增加移动控制脚本
            var moveContorller =  go.AddComponent<PlayerMoveController>();
            moveContorller.AnimationEventController = Controller.AnimationEventController;
            Controller.MoveController = moveContorller;

            //增加攻击控制脚本
            var attackController = go.AddComponent<PlayerAttackController>();
            attackController.MeController = Controller as ActionControler;
            Controller.AttackController = attackController;

            //增加受击控制脚本
            var beAttackedController = go.AddComponent<PlayerBeAttackedController>();
            beAttackedController.meController = Controller as ActionControler;
            Controller.BeAttackedController = beAttackedController;

            //增加死亡控制脚本
            var deathController = go.AddComponent<PlayerDeathController>();
            deathController.MeController = Controller as ActionControler;

            //增加AI控制脚本
            var aiController = go.AddComponent<MeAiController>();
            aiController.MeController = Controller as MeControler;
            Controller.AiController = aiController;

			//添加连斩统计脚本
			var continueCutMgr = go.AddComponent<ContinueCutMgr>();
			Controller.ContCutMgr = continueCutMgr;

            BoxCollider2D = GoCloth.GetComponent<BoxCollider2D>();
            BoxCollider2D.enabled = false;
            SetStandClothGoPosition();

            GetMeVoByType<PlayerVo>().Controller = Controller as ActionControler;
        }

        public override void ChangeDire(int dire)
        {
            bool needSyn = dire != CurDire;
            base.ChangeDire(dire);
            if (needSyn && AppMap.Instance.mapParser.NeedSyn)
            {
                RoleMode.Instance.SendStatuChange();  //方向改变同步到服务器
            }
        }

        public new BaseRoleVo GetVo()
        {
            return Vo as BaseRoleVo;
        }
    }
}
