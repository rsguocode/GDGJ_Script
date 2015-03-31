﻿﻿﻿
using Com.Game.Module.GoldSilverIsland;
using com.game.module.main;
using com.game.Public.Confirm;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:13:02 
 * function: 金银岛功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideGoldSilverIslandOpen : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenGoldSilverIsland();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_jyd, GuideOpenGoldSilverIsland);
            }
        }

        //指引打开金银岛面板
        private void GuideOpenGoldSilverIsland()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_jyd, BeforeClickMainViewGoldSilverIslandButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【金银岛】");
        }

        //注册点击主UI金银岛按钮的点击事件执行前处理事件
        private void BeforeClickMainViewGoldSilverIslandButton()
        {
            GuideView.Instance.CloseView();
            BoatRefreshView.Instance.AfterOpenGuideDelegate = AfterOpenBoatRefresh;
        }

        private void AfterOpenBoatRefresh()
        {
            SetCurrentGuideButton(BoatRefreshView.Instance.btnShipRefresh,BeforeClickRefreshButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【刷新】");
        }

        private void BeforeClickRefreshButton()
        {
            GuideView.Instance.CloseView();
            ConfirmView.AfterOpenViewGuideDelegate = AfterOpenConfirmView;
        }

        private void AfterOpenConfirmView()
        {
            SetCurrentGuideButton(ConfirmMgr.Instance.CurrentConfirmView.btnOk, BeforeClickOkButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【确定】");
        }

        private void BeforeClickOkButton()
        {
            GuideView.Instance.CloseView();
            BoatRefreshView.Instance.RefreshFinishCallback = RefreshFinishCallback;
        }

        private void RefreshFinishCallback()
        {
            SetCurrentGuideButton(BoatRefreshView.Instance.btnStartSail, BeforeClickStartSailButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【出发】");
        }

        private void BeforeClickStartSailButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(IslandMainView.Instance.btnBack, BeforeClickBackButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【返回】");
        }

        public void BeforeClickBackButton()
        {
            GuideView.Instance.CloseView();
        }

    }
}