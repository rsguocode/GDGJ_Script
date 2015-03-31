using com.game.module.main;
using Com.Game.Module.Pet;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/29 09:25:59 
 * function: 宠物技能
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuidePetSkill : GuideBase
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
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_cw, GuideOpenPet, false);
            }
        }

        //指引打开宠物面板
        private void GuideOpenPet()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_cw, BeforeClickMainViewPetButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【幻兽】");
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
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击打开幻兽信息");
        }

        //点击宠物注册宠物信息弹出事件
        private void BeforeClickPetItem()
        {
            GuideView.Instance.CloseView();
            PetInfoView.Instance.GuideAfterOpenPetInfoView = AfterOpenPetInfoView;
        }

        private void AfterOpenPetInfoView()
        {
            vp_Timer.In(1.5f, GuideClickPetSkillItem);
        }

        private void GuideClickPetSkillItem()
        {
            SetCurrentGuideButton(PetInfoView.Instance.GuideSkillButton, BeforeClickPetSkillItem);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击升级技能");
        }

        private void BeforeClickPetSkillItem()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetInfoView.Instance.CloseButton, BeforeClickPetInfoCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickPetInfoCloseButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(PetView.Instance.CloseButton, BeforeClickPetCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickPetCloseButton()
        {
            GuideView.Instance.CloseView();
        }
    }
}