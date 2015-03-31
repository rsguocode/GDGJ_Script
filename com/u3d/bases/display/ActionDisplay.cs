using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;

/**可活动对象显示逻辑类*/
namespace com.u3d.bases.display
{
    public abstract class ActionDisplay : BaseDisplay
    {
        public BoxCollider2D BoxCollider2D;                              //模型的碰撞体

        /**添加控制脚本**/
        override protected void AddScript(GameObject go)
        {
            //增加控制中心控制脚本
            if (go.GetComponent<ActionControler>() != null) return;
            Controller = go.AddComponent<ActionControler>();
            Controller.Me = this;

            //增加状态控制脚本
            var statuController = go.AddComponent<PlayerStatuController>();
            Controller.StatuController = statuController;

            //增加技能控制脚本
            Controller.SkillController = go.AddComponent<SkillController>();
            Controller.SkillController.MeController = Controller as ActionControler;

            //增加攻击控制脚本
            var attackController = go.AddComponent<AttackControllerBase>();
            attackController.MeController = Controller as ActionControler;
            Controller.AttackController = attackController;

            //增加受击控制脚本
            var beAttackedController = go.AddComponent<BeAttackedControllerBase>();
            beAttackedController.meController = Controller as ActionControler;
            Controller.BeAttackedController = beAttackedController;

            //增加动画事件控制脚本
            Controller.AnimationEventController = GoCloth.GetComponent<AnimationEventController>() ?? GoCloth.AddComponent<AnimationEventController>();
            Controller.AnimationEventController.skillController = Controller.SkillController;
        }
    }
}
