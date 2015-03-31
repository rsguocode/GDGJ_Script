﻿﻿﻿
using Com.Game.Module.Equip;
using com.game.module.main;
using Com.Game.Module.Role;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/29 02:55:25 
 * function: 装备精炼指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideEquipRefine : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenForge();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_dz, GuideOpenForge,false);
            }
        }

        //指引打开锻造面板
        private void GuideOpenForge()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_dz, BeforeClickMainViewForgeButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【锻造】");
        }

        //注册点击主UI锻造按钮的点击事件执行前处理事件
        private void BeforeClickMainViewForgeButton()
        {
            GuideView.Instance.CloseView();
            Equip1View.Instance.AfterOpenGuideDelegate = AfterOpenEquipStrengthView;
        }

        //打开强化面板后指引精炼按钮
        private void AfterOpenEquipStrengthView()
        {
            vp_Timer.In(0.2f, GuideEquipRefineTab);
        }

        private void GuideEquipRefineTab()
        {
            if (!Equip1View.Instance.ckb_refine.value)
            {
                SetCurrentGuideToggle(Equip1View.Instance.ckb_refine, AfterClickRefineButton);
                GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【精炼】标签");
            }
            else
            {
                AfterOpenRefineView();
            }
        }

        //点击精炼Tab按钮后指引关闭按钮
        private void AfterClickRefineButton()
        {
            RemoveToggleDelegate(Equip1View.Instance.ckb_refine, AfterClickRefineButton);
            AfterOpenRefineView();
        }

        private void AfterOpenRefineView()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(EquipRefineView.Instance.btn_Refine, BeforeClickEquipRefineBtn);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【精炼】");
        }

        private void BeforeClickEquipRefineBtn()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(Equip1View.Instance.btn_close, BeforeClickEquipCloseBtn);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        //点击关闭按钮后关闭指引面板
        private void BeforeClickEquipCloseBtn()
        {
            GuideView.Instance.CloseView();
        }

         
    }
}