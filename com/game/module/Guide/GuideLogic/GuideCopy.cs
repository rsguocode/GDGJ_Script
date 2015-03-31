﻿﻿﻿
using Com.Game.Module.Copy;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/28 10:55:26 
 * function:
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideCopy : GuideBase
    {

        //第一步，指引点击任务副本
        public override void BeginGuide()
        {
            SetCurrentGuideButton(CopyPointView.Instance.GuideCopyButton, BeforeClickCopyPointButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【任务副本】");
        }

        private void BeforeClickCopyPointButton()
        {
            GuideView.Instance.CloseView();
            CopyDetailView.Instance.AfterOpenGuideDelegate = AfterOpenCopyDetail;
        }

        private void AfterOpenCopyDetail()
        {
            SetCurrentGuideButton(CopyDetailView.Instance.btn_play, BeforeClickEnterCopyButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【挑战】");
        }

        private void BeforeClickEnterCopyButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}