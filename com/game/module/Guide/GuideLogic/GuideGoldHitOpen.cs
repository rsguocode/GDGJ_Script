﻿﻿﻿
using Com.Game.Module.GoldHit;
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:13:09 
 * function: 击石成金功能指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideGoldHitOpen : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenGoldHit();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_jscj, GuideOpenGoldHit);
            }
        }

        //指引打开击石成金面板
        private void GuideOpenGoldHit()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_jscj, BeforeClickMainViewGoldHitButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【击石成金】");
        }

        //注册点击主UI击石成金按钮的点击事件执行前处理事件
        private void BeforeClickMainViewGoldHitButton()
        {
            GuideView.Instance.CloseView();
            GoldHitLogView.Instance.AfterOpenGuideDelegate = AfterOpenGuideLogin;
        }

        //打开进入UI后指引点击进入按钮
        private void AfterOpenGuideLogin()
        {
            SetCurrentGuideButton(GoldHitLogView.Instance.btn_enter, BeforeClickEnterButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【立即进入】");
        }

        //点击进入按钮后关闭指引界面
        private void BeforeClickEnterButton()
        {
            GuideView.Instance.CloseView();
        }

    }
}