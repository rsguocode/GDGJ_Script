﻿﻿﻿
using com.game.module.main;
using Com.Game.Module.Medal;
using Com.Game.Module.Role;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/29 10:55:42 
 * function: 勋章指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideMedal : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenRole();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_js, GuideOpenRole, false);
            }
        }

        //指引打开角色面板
        private void GuideOpenRole()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_js, BeforeClickMainViewRoleButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【角色】");
        }

        //注册点击主UI角色按钮的点击事件执行前处理事件
        private void BeforeClickMainViewRoleButton()
        {
            GuideView.Instance.CloseView();
            RoleView.Instance.AfterOpenViewGuideDelegate = AfterOpenRole;
        }

        private void AfterOpenRole()
        {
            vp_Timer.In(0.2f, GuideMedalTab);
        }

        private void GuideMedalTab()
        {
            if (RoleView.Instance.AfterOpenViewGuideDelegate == null)
            {
                return;
            }
            EventDelegate.Remove(RoleView.Instance.tweenPosition.onFinished, RoleView.Instance.AfterOpenViewGuideDelegate);
            if (!RoleView.Instance.ckb_peiyue.value)
            {
                SetCurrentGuideToggle(RoleView.Instance.ckb_medal, AfterClickMedalButton);
                GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【勋章】标签");
            }
            else
            {
                AfterOpenMedalView();
            }
            RoleView.Instance.AfterOpenViewGuideDelegate = null;
        }

        private void AfterClickMedalButton()
        {
            RemoveToggleDelegate(RoleView.Instance.ckb_medal, AfterClickMedalButton);
            GuideView.Instance.CloseView();
            AfterOpenMedalView();
        }

        private void AfterOpenMedalView()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(MedalView.Instance.GuideItemContainer, BeforeClickMedalButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【勋章图标】");
        }

        private void BeforeClickMedalButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(RoleView.Instance.btn_close, BeforeClickRoleCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickRoleCloseButton()
        {
            GuideView.Instance.CloseView();
        }

         
    }
}