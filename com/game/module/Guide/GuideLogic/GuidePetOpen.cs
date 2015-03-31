using com.game.module.main;
using Com.Game.Module.Pet;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 07:09:42 
 * function: 宠物功能开启指引
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public delegate void GuidePetOpenDelegate();

    public class GuidePetOpen : GuideBase
    {
        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenPet();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_cw, GuideOpenPet);
            }
        }

        //指引打开宠物面板
        private void GuideOpenPet()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_cw, BeforeClickMainViewPetButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【幻兽】");
        }

        //注册点击主UI宠物按钮的点击事件执行前处理事件
        private void BeforeClickMainViewPetButton()
        {
            GuideView.Instance.CloseView();
            PetView.Instance.GuidePetOpenDelegate = AfterOpenPetView;
        }

        //打开宠物界面后指引宠物激活
        private void AfterOpenPetView()
        {
            SetCurrentGuideButton(PetView.Instance.petsObj[0].GetComponent<Button>(), BeforeClickPetItem);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击激活幻兽");
        }

        //点击宠物注册宠物信息弹出事件
        private void BeforeClickPetItem()
        {
            GuideView.Instance.CloseView();
            PetTipsView.Instance.AfterOpenViewGuideDelegate = AfterOpenPetTipsView;
        }

        //打开宠物Tips面板指引点击确定按钮
        private void AfterOpenPetTipsView()
        {
            EventDelegate.Remove(PetTipsView.Instance.tipsPlay.onFinished, AfterOpenPetTipsView);
            PetTipsView.Instance.AfterOpenViewGuideDelegate = null;
            SetCurrentGuideButton(PetTipsView.Instance.CloseButton, BeforeClickPetActiveBtn);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【确定】");
        }


        //点击激活按钮后指引关闭宠物面板
        private void BeforeClickPetActiveBtn()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetView.Instance.petsObj[0].GetComponent<Button>(), BeforeClickPetItemSecondTime);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击打开幻兽信息");
        }

        private void BeforeClickPetItemSecondTime()
        {
            GuideView.Instance.CloseView();
            PetInfoView.Instance.GuideAfterOpenPetInfoView = AfterOpenPetInfoView;
        }

        private void AfterOpenPetInfoView()
        {
            vp_Timer.In(1f, GuideClickFight);
        }

        private void GuideClickFight()
        {
            SetCurrentGuideButton(PetInfoView.Instance.FightButton, BeforeClickPetFight);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击【出战】");
        }

        private void BeforeClickPetFight()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetInfoView.Instance.CloseButton, BeforeClickPetInfoCloseBtn);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }


        private void BeforeClickPetInfoCloseBtn()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetView.Instance.CloseButton, BeforeClickPetCloseBtn);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        //关闭宠物面板后关闭指引面板
        private void BeforeClickPetCloseBtn()
        {
            GuideView.Instance.CloseView();
        }
    }
}