using com.game.module.main;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/23 11:06:30 
 * function: 角色面板功能开启
 * *******************************************************/
using Com.Game.Module.Role;
using Com.Game.Module.Tips;

namespace com.game.module.Guide.GuideLogic
{
    public class GuideRoleOpen : GuideBase
    {
        //第一步，看是直接指引打开面板还是先指引展开主UI按钮
        public override void BeginGuide()
        {
            if (MainBottomRightView.Instance.IsOpen)
            {
                GuideOpenRole();
            }
            else
            {
                GuideMainKongzhiButton(MainBottomRightView.Instance.btn_js, GuideOpenRole);
            }
        }

        //指引打开角色面板
        private void GuideOpenRole()
        {
            SetCurrentGuideButton(MainBottomRightView.Instance.btn_js, BeforeClickMainViewRoleButton);
            GuideView.Instance.OpenFlyButtonGuide(CurrentGuideButton.transform, "点击【角色】");
        }

        //注册点击主UI角色按钮的点击事件执行前处理事件
        private void BeforeClickMainViewRoleButton()
        {
            GuideView.Instance.CloseView();
            GoodsView.Instance.ResetPositionDelegate = AfterOpenGoodsView;
        }

        private void AfterOpenGoodsView()
        {
            GoodsView.Instance.ResetPositionDelegate = null;
            var weaponItem = GoodsView.Instance.GetWeaponItem();
            if (weaponItem == null)
            {
                return;   //没有装备则终止这次指引
            }
            SetCurrentGuideButton(weaponItem, BeforeClickWeaponItem);
            GuideView.Instance.OpenUpGuide(CurrentGuideButton.transform, "点击装备");
        }

        private void BeforeClickWeaponItem()
        {
            GuideView.Instance.CloseView();
            EquipTips.Instance.AfterOpenGuideDelegate = AfterOpenEquipTips;
        }

        private void AfterOpenEquipTips()
        {
            SetCurrentGuideButton(EquipTips.Instance.btn_right, BeforeClickEquipButton);
            GuideView.Instance.OpenDownGuide(CurrentGuideButton.transform, "点击【装备】");
        }

        private void BeforeClickEquipButton()
        {
            GuideView.Instance.CloseView();
            SetCurrentGuideButton(RoleView.Instance.btn_close, BeforeClickCloseButton);
            GuideView.Instance.OpenRightGuide(CurrentGuideButton.transform, "点击【关闭】");
        }

        private void BeforeClickCloseButton()
        {
            GuideView.Instance.CloseView();
        }

    }
}