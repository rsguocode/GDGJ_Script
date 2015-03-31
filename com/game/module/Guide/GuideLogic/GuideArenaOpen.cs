﻿﻿﻿
using Com.Game.Module.Arena;
using com.game.module.main;
using com.game.module.test;
using com.game.Public.Confirm;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:13:16 
 * function: 英雄榜开启功能指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideArenaOpen : GuideBase
    {

        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenArena();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_yxb, GuideOpenArena);
            }
        }

        //指引打开英雄榜面板
        private void GuideOpenArena()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_yxb, BeforeClickMainViewArenaButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【英雄榜】");
        }

        //注册点击主UI英雄榜按钮的点击事件执行前处理事件
        private void BeforeClickMainViewArenaButton()
        {
            GuideView.Instance.CloseView();
            ArenaMainView.Instance.ChallengeUpdateCallback = ChallengeUpdateCallback;
        }

        private void ChallengeUpdateCallback()
        {
            var challengers = Singleton<ArenaMode>.Instance.challengerList;
            //如果没有可以挑战的玩家，只有自己，则不指引挑战，直接指引关闭面板
            if (challengers.Count == 0)
            {
                AfterOpenConfirmView();
            }
            else
            {
                SetCurrentGuideButton(ArenaMainView.Instance.Hero1, BeforeClickHero1Button);
                GuideView.Instance.OpenLeftGuide(CurrentGuideButton.transform, "点击【挑战】");
            }
        }

        //点击了被挑战对象后，指引弹出面板
        private void BeforeClickHero1Button()
        {
            GuideView.Instance.CloseView();
            ResetHeroLayer();
            ConfirmView.AfterOpenViewGuideDelegate = AfterOpenConfirmView;
        }


        //恢复被挑战对象的层次
        private void ResetHeroLayer()
        {
            var display = ArenaMainView.Instance.Hero1.FindChild("RoleDisplay");
            NGUITools.SetLayer(display, LayerMask.NameToLayer("Mode"));
        }

        //指引弹出面板的确定按钮
        private void AfterOpenConfirmView()
        {
            SetCurrentGuideButton(ConfirmMgr.Instance.CurrentConfirmView.btnOk, BeforeClickOkButton);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【确定】");
        }

        //关闭指引
        private void BeforeClickOkButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}