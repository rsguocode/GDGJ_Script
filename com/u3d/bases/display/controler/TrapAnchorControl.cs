﻿﻿﻿
using System;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.debug;
using com.u3d.bases.display.character;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/02/26 04:21:38 
 * function: 铁锚陷阱行为控制控制
 * *******************************************************/

namespace com.u3d.bases.display.controler
{
    public class TrapAnchorControl : MonoBehaviour
    {
        public ActionControler ActionControler;
        public TrapVo ThisTrapVo;
        public TrapDisplay TrapMeDisplay;

        // Use this for initialization
        private void Start()
        {
            ActionControler.StatuController.SetStatu(Status.IDLE);
            TrapMeDisplay = ActionControler.Me as TrapDisplay;
            ThisTrapVo = TrapMeDisplay.GetTrapVo();
            vp_Timer.In(ThisTrapVo.SysTrapVo.AttackInterval * 0.001f, ActionAttack);
        }

        private void ActionAttack()
        {
            if (ActionControler == null)
            {
                return;
            }
            ActionControler.SkillController.RequestUseSkill(SkillController.Attack1);
            vp_Timer.In(ThisTrapVo.SysTrapVo.AttackInterval * 0.001f, ActionAttack);
        }

    }
}