﻿﻿﻿
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 09:21:43 
 * function: 商城功能开启
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideStoreShopOpen : GuideBase
    {

        //第一步，商城按钮从屏幕中间飞到原本的位置
        public override void BeginGuide()
        {
            MainTopRightView.Instance.UpdateButtonOpenStatu();
            SetCurrentGuideButton(MainTopRightView.Instance.btn_sc, BeforeClickStoreShopButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【商城】",false);
        }

        //注册点击主商城按钮的点击事件执行前处理事件
        private void BeforeClickStoreShopButton()
        {
            GuideView.Instance.CloseView();
        }
         
    }
}