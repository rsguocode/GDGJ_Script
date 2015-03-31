using com.game.data;
using com.game.manager;
using com.game.module.main;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/22 11:21:13 
 * function: 指引基类
 * 写指引时需要重点注意事项：
 * 1. 指引的流程逻辑全部写在指引类里面
 * 2. 指引的流程控制主要通过回调实现
 * 3. 回调使用后要特别注意要移除，否则容易出BUG
 * 4. 理解了上面的几点，写指引So Easy
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public delegate void MainKongzhiButtonClickCallBack();

    public class GuideBase
    {
        //当前被指引的按钮对象
        protected UIWidgetContainer CurrentGuideButton;
        //是否是功能开启
        private bool _isFunctionOpen; 
        private vp_Timer.Callback _clickMainKongZhiBtnCallback;
       
        public virtual void BeginGuide()
        {
        }

        /// <summary>
        ///     指引点击主UI右下角的展开按钮
        /// </summary>
        /// <param name="currentGuideButton">当前被指引的按钮</param>
        /// <param name="clickMainKongZhiBtnCallback">展开按钮点击后的回调事件</param>
        /// <param name="isFunctionOpen">是否是功能开启</param>
        protected void GuideMainKongzhiButton(Button currentGuideButton,vp_Timer.Callback clickMainKongZhiBtnCallback,bool isFunctionOpen=true)
        {
            _isFunctionOpen = isFunctionOpen;
            CurrentGuideButton = currentGuideButton;
            _clickMainKongZhiBtnCallback = clickMainKongZhiBtnCallback;
            var mainViewKongZhiToggle = MainBottomRightView.Instance.ckb_kongzhi;
            GuideView.Instance.OpenRightGuide(mainViewKongZhiToggle.transform, "点击展开按钮");
            MainBottomRightView.Instance.BeforeClickKongzhiButton = BeforeClickMainViewCkbKongZhiButton;
        }

        //注册点击主UI右下角展开按钮的点击事件执行前处理事件
        private void BeforeClickMainViewCkbKongZhiButton()
        {
            GuideView.Instance.CloseView();
            if (_isFunctionOpen)
            {
                HideGuideButton();
            }
            MainBottomRightView.Instance.BeforeClickKongzhiButton = null;
            vp_Timer.In(0.35f, _clickMainKongZhiBtnCallback);
        }

        private void HideGuideButton()
        {
            int count = CurrentGuideButton.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                CurrentGuideButton.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     设置当前被指引的对象
        /// </summary>
        /// <param name="currentGuideButton">当前被指引的对象</param>
        /// <param name="beforeClickGuideButtonDelegate">当前被指引的对象被点击前执行的方法</param>
        /// <param name="afterClickGuideButtonDelegate">当前被指引的对象被点击后执行的方法</param>
        protected void SetCurrentGuideButton(UIWidgetContainer currentGuideButton,
            UIWidgetContainer.GuideDelegate beforeClickGuideButtonDelegate = null,
            UIWidgetContainer.GuideDelegate afterClickGuideButtonDelegate = null)
        {
            CurrentGuideButton = currentGuideButton;
            CurrentGuideButton.guideBeforeClickDelegate = beforeClickGuideButtonDelegate;
            CurrentGuideButton.guideAfterClickDelegate = afterClickGuideButtonDelegate;
        }

        /// <summary>
        ///     设置当前被指引的对象
        /// </summary>
        /// <param name="currentGuideToggle">当前被指引对象</param>
        /// <param name="clickCallback">点击后的回调事件</param>
        protected void SetCurrentGuideToggle(UIToggle currentGuideToggle,
            EventDelegate.Callback clickCallback)
        {
            CurrentGuideButton = currentGuideToggle;
            EventDelegate.Add(currentGuideToggle.onChange, clickCallback);
        }


        public void RemoveToggleDelegate(UIToggle currentGuideToggle, EventDelegate.Callback clickCallback)
        {
            EventDelegate.Remove(currentGuideToggle.onChange, clickCallback);
        }

        /// <summary>
        /// 测试时使用的方法，便于快速测试指引，无需走任务，点击某个按钮时调用该方法，可以直接触发对应的指引类型
        /// </summary>
        /// <param name="guideType">需测试的指引类型</param>
        public static void TriggerGuideForTest(int guideType)
        {
            GuideManager.Instance.CurrentTriggeredGuideVo =
                BaseDataMgr.instance.GetDataByTypeAndId<SysGuideVo>(BaseDataConst.SYS_GUIDE_VO, (uint)guideType);
            GuideManager.Instance.GetGuideLogic(guideType).BeginGuide();
        }
    }
}