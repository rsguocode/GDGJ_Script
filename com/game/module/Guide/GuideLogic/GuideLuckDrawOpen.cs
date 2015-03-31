﻿﻿﻿
using Com.Game.Module.LuckDraw;
using com.game.module.main;
using com.game.Public.Confirm;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 09:27:03 
 * function: 萌宠献礼功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideLuckDrawOpen : GuideBase
    {

        //第一步，萌宠献礼按钮从屏幕中间飞到原本的位置
        public override void BeginGuide()
        {
            MainTopRightView.Instance.UpdateButtonOpenStatu();
            SetCurrentGuideButton(MainTopRightView.Instance.btn_mcxl, BeforeClickLuckDrawButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【萌宠献礼】");
        }

        //注册点击萌宠献礼按钮的点击事件执行前处理事件
        private void BeforeClickLuckDrawButton()
        {
            GuideView.Instance.CloseView();
            LuckDrawView.Instance.AfterOpenGuideDelegate = AfterOpenLuckDrawView;
        }

        //打开萌宠献礼面板后指引购买一个按钮
        private void AfterOpenLuckDrawView()
        {
            SetCurrentGuideButton(LuckDrawView.Instance.BtnOnce,BeforeClickLuckOnceBtn);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform,"点击【购买】");
        }

        //设置弹出确认面板指引代理
        private void BeforeClickLuckOnceBtn()
        {
            GuideView.Instance.CloseView();
            //ConfirmView.AfterOpenViewGuideDelegate = AfterOpenConfirmView;
        }

        //指引弹出面板的确定按钮
        private void AfterOpenConfirmView()
        {
            SetCurrentGuideButton(ConfirmMgr.Instance.CurrentConfirmView.btnOk, BeforeClickOkButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【确定】");
        }

        //关闭指引
        private void BeforeClickOkButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}