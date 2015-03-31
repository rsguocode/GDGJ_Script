﻿﻿﻿
using com.game.module.main;
using Com.Game.Module.Pet;
using Com.Game.Module.Role;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/28 08:39:09 
 * function: 指引培育
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideGrow : GuideBase
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
            vp_Timer.In(0.2f, GuideGrowTab);
        }

        private void GuideGrowTab()
        {
            if (RoleView.Instance.AfterOpenViewGuideDelegate == null)
            {
                return;
            }
            EventDelegate.Remove(RoleView.Instance.tweenPosition.onFinished, RoleView.Instance.AfterOpenViewGuideDelegate);
            if (!RoleView.Instance.ckb_peiyue.value)
            {
                SetCurrentGuideToggle(RoleView.Instance.ckb_peiyue, AfterClickGrowButton);
                GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【培育】标签");
            }
            else
            {
                AfterOpenGrowView();
            }
            RoleView.Instance.AfterOpenViewGuideDelegate = null;
        }

        private void AfterClickGrowButton()
        {
            RemoveToggleDelegate(RoleView.Instance.ckb_peiyue, AfterClickGrowButton);
            GuideView.Instance.CloseView();
            AfterOpenGrowView();
        }

        private void AfterOpenGrowView()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(GrowView.Instance.GuideButton, BeforeClickFreeGrowButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【普通培育】");
        }

        private void BeforeClickFreeGrowButton()
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