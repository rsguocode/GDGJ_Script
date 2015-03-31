﻿﻿﻿
using com.game.module.main;
using Com.Game.Module.Pet;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/29 09:26:23 
 * function: 宠物装备
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuidePetEquip : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenPet();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_cw, GuideOpenPet, false);
            }
        }

        //指引打开宠物面板
        private void GuideOpenPet()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_cw, BeforeClickMainViewPetButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【幻兽】");
        }

        //注册点击主UI宠物按钮的点击事件执行前处理事件
        private void BeforeClickMainViewPetButton()
        {
            GuideView.Instance.CloseView();
            PetView.Instance.GuidePetOpenDelegate = AfterOpenPetView;
        }

        //打开宠物界面后指引宠物激活
        private void AfterOpenPetView()
        {
            SetCurrentGuideButton(PetView.Instance.petsObj[0].GetComponent<Button>(), BeforeClickPetItem);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击打开幻兽信息");
        }

        //点击宠物注册宠物信息弹出事件
        private void BeforeClickPetItem()
        {
            GuideView.Instance.CloseView();
            PetInfoView.Instance.GuideAfterOpenPetInfoView = AfterOpenPetInfoView;
        }

        private void AfterOpenPetInfoView()
        {
            SetCurrentGuideButton(PetInfoView.Instance.GuideEquipButton, BeforeClickPetEquipButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击装备");
        }

        private void BeforeClickPetEquipButton()
        {
            GuideView.Instance.CloseView();
            PetEquipView.Instance.AfterOpenPetEquipView = AfterOpenPetEquipView;
        }

        private void AfterOpenPetEquipView()
        {
            vp_Timer.In(1f,GuideEquipButton);
        }

        private void GuideEquipButton()
        {
            if (PetEquipView.Instance.GuideLeftButton.gameObject.activeInHierarchy && !PetEquipView.Instance.IsClosed)
            {
                SetCurrentGuideButton(PetEquipView.Instance.GuideLeftButton, BeforeClickEquipButton);
                GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【装备】");
            }
            else
            {
                BeforeClickEquipButton();
            }
        }

        private void BeforeClickEquipButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetInfoView.Instance.CloseButton, BeforeClickPetInfoCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickPetInfoCloseButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetView.Instance.CloseButton, BeforeClickPetCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickPetCloseButton()
        {
            GuideView.Instance.CloseView();
        }

         
    }
}