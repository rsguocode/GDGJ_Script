﻿﻿﻿
using com.game.module.main;
using Com.Game.Module.Role;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/28 03:12:33 
 * function: 技能开启
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideSkillOpen : GuideBase
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
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_jn, GuideOpenSkill);
            }
        }

        protected virtual void GuideOpenSkill()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_jn, BeforeClickMainViewSkillButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【技能】");
        }

        protected void BeforeClickMainViewSkillButton()
        {
            GuideView.Instance.CloseView();
            SetSkillViewGuideId();
            SkillView.Instance.GuideSkillInitCallback = AfterOpenSkillView;
        }

        protected virtual void SetSkillViewGuideId()
        {
            SkillView.Instance.CurrentGuideId = GuideType.GuideSkillOpen;
        }

        private void AfterOpenSkillView()
        {
            SetCurrentGuideButton(SkillView.Instance.GuideSkillButton, AfterClickGuideSkillButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击技能图标");
        }

        private void AfterClickGuideSkillButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(SkillView.Instance.UpgradeButton, BeforeClickSkillUpgradeButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击学习技能");
        }

        private void BeforeClickSkillUpgradeButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(SkillView.Instance.GuideSkillPositionButton, BeforeClickSkillPositionButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击设置技能位置");
        }

        private void BeforeClickSkillPositionButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(SkillView.Instance.CloseButton, BeforeClickSkillCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickSkillCloseButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}