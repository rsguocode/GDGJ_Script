﻿﻿﻿
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:13:26 
 * function: 恶魔岛功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideDaemonIslandOpen : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenDaemonIsland();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_emd, GuideOpenDaemonIsland);
            }
        }

        //指引打开恶魔岛面板
        private void GuideOpenDaemonIsland()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_emd, BeforeClickMainViewDaemonIslandButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【恶魔岛】");
        }

        //注册点击主UI恶魔岛按钮的点击事件执行前处理事件
        private void BeforeClickMainViewDaemonIslandButton()
        {
            GuideView.Instance.CloseView();
        }

         
    }
}