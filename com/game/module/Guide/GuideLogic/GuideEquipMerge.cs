﻿﻿﻿
using Com.Game.Module.Equip;
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/29 07:58:35 
 * function: 装备充灵指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideEquipMerge : GuideBase
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
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_dz, GuideOpenForge, false);
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
            vp_Timer.In(0.2f, GuideEquipMergeTab);
        }

        private void GuideEquipMergeTab()
        {
            if (!Equip1View.Instance.ckb_merge.value)
            {
                SetCurrentGuideToggle(Equip1View.Instance.ckb_merge, AfterClickMergeButton);
                GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【充灵】标签");
            }
            else
            {
                AfterOpenMergeView();
            }
        }

        //点击镶嵌Tab按钮后指引关闭按钮
        private void AfterClickMergeButton()
        {
            RemoveToggleDelegate(Equip1View.Instance.ckb_merge, AfterClickMergeButton);
            AfterOpenMergeView();
        }

        private void AfterOpenMergeView()
        {
            if (SmeltMerge1View.Instance.GuideStoneItemContainer == null)
            {
                AfterClickMergerStone3Button();
                return;
            }
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(SmeltMerge1View.Instance.GuideStoneItemContainer,null, AfterClickStoneItem);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【宝石】");
        }

        private void AfterClickStoneItem()
        {
            GuideView.Instance.CloseView();
            vp_Timer.In(0.3f, GuideMergerStone1Button);
        }

        private void GuideMergerStone1Button()
        {
            if (SmeltMerge1View.Instance.GuideMergeStoneItemContainer == null)
            {
                AfterClickMergerStone3Button();
                return;
            }
            SetCurrentGuideButton(SmeltMerge1View.Instance.GuideMergeStoneItemContainer, null, AfterClickMergerStone1Button);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【充灵宝石】");
        }

        private void AfterClickMergerStone1Button()
        {
            GuideView.Instance.CloseView();
            vp_Timer.In(0.3f, GuideMergerStone2Button);
        }

        private void GuideMergerStone2Button()
        {
            if (SmeltMerge1View.Instance.GuideMergeStoneItemContainer == null)
            {
                AfterClickMergerStone3Button();
                return;
            }
            SetCurrentGuideButton(SmeltMerge1View.Instance.GuideMergeStoneItemContainer, null, AfterClickMergerStone2Button);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【充灵宝石】");
        }

        private void AfterClickMergerStone2Button()
        {
            GuideView.Instance.CloseView();
            vp_Timer.In(0.3f, GuideMergerStone3Button);
        }

        private void GuideMergerStone3Button()
        {
            if (SmeltMerge1View.Instance.GuideMergeStoneItemContainer == null)
            {
                AfterClickMergerStone3Button();
                return;
            }
            SetCurrentGuideButton(SmeltMerge1View.Instance.GuideMergeStoneItemContainer, null, AfterClickMergerStone3Button);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【充灵宝石】");
        }

        private void AfterClickMergerStone3Button()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(SmeltMerge1View.Instance.MergeButton, BeforeClickMergerButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【充灵】");
        }

        private void BeforeClickMergerButton()
        {
            GuideView.Instance.CloseView();
            vp_Timer.In(1, GuideClose);
        }

        private void GuideClose()
        {
            if (!Equip1View.Instance.btn_close.gameObject.activeInHierarchy)return;
            SetCurrentGuideButton(Equip1View.Instance.btn_close, null, BeforeClickEquipCloseBtn);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        //点击关闭按钮后关闭指引面板
        private void BeforeClickEquipCloseBtn()
        {
            GuideView.Instance.CloseView();
        }

    }
}