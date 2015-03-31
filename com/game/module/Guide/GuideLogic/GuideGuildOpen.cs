﻿﻿﻿
using Com.Game.Module.Guild;
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:12:47 
 * function: 公会功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideGuildOpen : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenGuild();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_gh, GuideOpenGuild);
            }
        }

        //指引打开公会面板
        private void GuideOpenGuild()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_gh, BeforeClickMainViewGuildButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【公会】");
        }

        //注册点击主UI公会按钮的点击事件执行前处理事件
        private void BeforeClickMainViewGuildButton()
        {
            GuideView.Instance.CloseView();
            GuildView.Instance.AfterOpenGuideDelegate = AfterOpenGuild;
        }

        private void AfterOpenGuild()
        {
            SetCurrentGuideButton(GuildView.Instance.btnClose, BeforeClickGuildCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickGuildCloseButton()
        {
            GuideView.Instance.CloseView();;
        }
    }
}