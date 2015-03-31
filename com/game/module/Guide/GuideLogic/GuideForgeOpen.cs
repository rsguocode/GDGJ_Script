﻿﻿﻿
using Com.Game.Module.Equip;
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:07:18 
 * function: 锻造功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideForgeOpen : GuideBase
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
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_dz, GuideOpenForge);
            }
        }

        //指引打开锻造面板
        private void GuideOpenForge()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_dz, BeforeClickMainViewForgeButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【锻造】");
        }

        //注册点击主UI锻造按钮的点击事件执行前处理事件
        private void BeforeClickMainViewForgeButton()
        {
            GuideView.Instance.CloseView();
            EquipStren1View.Instance.AfterOpenGuideDelegate = AfterOpenEquipStrengthView;
        }

        //打开强化面板后指引强化按钮
        private void AfterOpenEquipStrengthView()
        {
            SetCurrentGuideButton(EquipStren1View.Instance.strenButton, BeforeClickStrengthBtn);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【强化】");
        }

        //点击强化按钮后指引关闭按钮
        private void BeforeClickStrengthBtn()
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