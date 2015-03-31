﻿﻿﻿
using com.game.data;
using com.game.vo;
using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/02/26 01:54:22 
 * function: 陷阱
 * *******************************************************/

namespace com.u3d.bases.display.character
{
    public class TrapDisplay : ActionDisplay
    {
        override protected string SortingLayer { get { return "Player"; } }

        public TrapVo GetTrapVo()
        {
            return Vo as TrapVo;
        }

        /**添加控制脚本**/
        override protected void AddScript(GameObject go)
        {
            //增加控制中心控制脚本
            if (go.GetComponent<ActionControler>() != null) return;
            Controller = go.AddComponent<ActionControler>();
            Controller.Me = this;
            Controller.Me.Animator = Controller.Me.GoCloth.GetComponent<Animator>();
            //增加状态控制脚本
            var statuController = go.AddComponent<StatuControllerBase>();
            Controller.StatuController = statuController;
            statuController.MeControler = Controller as ActionControler;

            //增加移动控制脚本
            var monsterMoveController = go.AddComponent<MoveControllerBase>();
            monsterMoveController.AnimationEventController = Controller.AnimationEventController;
            monsterMoveController.MeController = Controller as ActionControler;
            Controller.MoveController = monsterMoveController;
            monsterMoveController.AnimationParameter = Controller.AnimationParameter;

            //增加攻击控制脚本
            var attackController = go.AddComponent<AttackControllerBase>();
            attackController.MeController = Controller as ActionControler;
            Controller.AttackController = attackController;

            //增加技能控制脚本
            SkillController skillController = Controller.SkillController = go.AddComponent<SkillController>();
            skillController.MeController = Controller as ActionControler;

            Controller.AnimationEventController = GoCloth.GetComponent<AnimationEventController>() ?? GoCloth.AddComponent<AnimationEventController>();
            Controller.AnimationEventController.skillController = Controller.SkillController;

            SysTrap sysTrapVo = GetTrapVo().SysTrapVo;
            switch (sysTrapVo.Type)
            {
                case 1:  //铁锚类陷阱
                    var trapAnchorControl = GoCloth.AddComponent<TrapAnchorControl>();
                    trapAnchorControl.ActionControler = Controller as ActionControler;
                    break;
                case 2:  //触碰接触伤害类陷阱
                    var trapColliderControl = GoCloth.AddComponent<TrapCollideControl>();
                    trapColliderControl.ActionControler = Controller as ActionControler;
                    break;
            }

            BoxCollider2D = GoCloth.GetComponent<BoxCollider2D>();
        }


         
    }
}