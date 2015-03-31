﻿﻿﻿
using com.game.module.main;
using Com.Game.Module.Role;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/05/07 05:29:12 
 * function: 指引技能4学习
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideSkill4Learn : GuideSkillOpen
    {
        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenSkill();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_jn, GuideOpenSkill, false);
            }
        }

        override protected void GuideOpenSkill()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_jn, BeforeClickMainViewSkillButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【技能】");
        }

        override protected void SetSkillViewGuideId()
        {
            SkillView.Instance.CurrentGuideId = GuideType.GuideSkill4Learn;
        }
    }
}