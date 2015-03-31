﻿﻿﻿
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 09:25:28 
 * function: 世界Boss功能开启
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideWorldBossOpen : GuideBase
    {

        //第一步，世界Boss按钮从屏幕中间飞到原本的位置
        public override void BeginGuide()
        {
            MainTopRightView.Instance.UpdateButtonOpenStatu();
            SetCurrentGuideButton(MainTopRightView.Instance.btn_sjb, BeforeClickWorldBossButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【世界Boss】",false);
        }

        //注册点击世界Boss按钮的点击事件执行前处理事件
        private void BeforeClickWorldBossButton()
        {
            GuideView.Instance.CloseView();
        }

         
    }
}