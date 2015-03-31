using Com.Game.Module.GoldBox;
using com.game.module.main;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/22 10:43:37 
 * function: 黄金宝箱指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideGoldBox : GuideBase
    {
        //第一步，黄金宝箱按钮从屏幕中间飞到原本的位置
        public override void BeginGuide()
        {
            MainTopRightView.Instance.UpdateButtonOpenStatu();
            SetCurrentGuideButton(MainTopRightView.Instance.btn_hjbx, BeforeClickMainViewBoxButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【黄金宝箱】");
        }

        //注册点击主UI右上角黄金宝箱按钮的点击事件执行前处理事件
        private void BeforeClickMainViewBoxButton()
        {
            GuideView.Instance.CloseView();
            GoldBoxView.Instance.AfterOpenGuideDelegate = AfterOpenGoldBoxView;
        }

        //第二步，打开黄金宝箱面板后指引点击打开按钮
        private void AfterOpenGoldBoxView()
        {
            SetCurrentGuideButton(GoldBoxView.Instance.btnOpen, BeforeClickDiamViewOpenButton);
            GuideView.Instance.OpenLeftGuide(CurrentGuideButton.transform, "点击【开启】");
        }

        //注册点击黄金宝箱面板的开启按钮的点击事件执行前处理事件
        private void BeforeClickDiamViewOpenButton()
        {
            GuideView.Instance.CloseView();
            GoldBoxTipsView.Instance.AfterOpenGuideDelegate = AfterOpenGoldBoxTipsView;
        }

        //第三步，打开黄金宝箱使用确定tips面板时指引点击确定按钮
        private void AfterOpenGoldBoxTipsView()
        {
            SetCurrentGuideButton(GoldBoxTipsView.Instance.btnOk, BeforeClickGoldBoxTipsViewOkButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【确定】");
        }

        //注册点击黄金宝箱使用确定tips面板确定按钮的点击事件执行前处理事件
        private void BeforeClickGoldBoxTipsViewOkButton()
        {
            GuideView.Instance.CloseView();
            GoldBoxView.Instance.AfterOpenGuideDelegate = AfterOpenGoldBoxViewSecondTime;
        }

        //第四步，再一次打开黄金宝箱面板时指引关闭按钮
        private void AfterOpenGoldBoxViewSecondTime()
        {
            SetCurrentGuideButton(GoldBoxView.Instance.btnClose, BeforeClickDiamViewCloseButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        //注册黄金宝箱面板关闭按钮点击事件执行前处理事件
        private void BeforeClickDiamViewCloseButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}