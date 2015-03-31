using System.Collections.Generic;
using System.Net.Mime;
using com.game.consts;
using com.game.manager;
using Com.Game.Module.Role;
using Com.Game.Module.VIP;
using com.game.Public.Message;
using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.data;

namespace Com.Game.Module.Medal
{
    public class MedalView : BaseView<MedalView>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        private UILabel repuValue;//声望值
        private List<GameObject> attrItemList = new List<GameObject>(); //加成属性值显示
        private UILabel medalLevel;
        private Button btnUp;//升级
        private UILabel repuCost;//升级声望花费
        private UILabel goldCost;//升级金币花费


        public ItemContainer GuideItemContainer;
        
        
        protected override void Init()
        {
            repuValue = FindInChild<UILabel>("repu/value");
            medalLevel = FindInChild<UILabel>("level/value");
            btnUp = FindInChild<Button>("up/btn_up");
            btnUp.clickDelegate = MedalControl.Instance.MedalUp;
            repuCost = FindInChild<UILabel>("up/cost/repuvalue");
            goldCost = FindInChild<UILabel>("up/cost/goldvalue");

            for (int i = 1; i < 9; i++)
            {
                attrItemList.Add(FindChild("attrs" + i));
            }

        }

        protected override void HandleAfterOpenView()
        {
            repuValue.text =LanguageManager.GetWord("RoleView.RepuLabel") +MeVo.instance.repu;
            MedalControl.Instance.RequestMedalInfo();
        }

        public override void RegisterUpdateHandler()
        {
            MedalMode.Instance.dataUpdated += UpdateMedalHandle;
            RoleMode.Instance.dataUpdated += UpdateRepuHandle;
        }

        private void UpdateMedalHandle(object sender, int code)
        {
            if (code == MedalMode.Instance.UPDATE_MEDAL_INFO)
            {
                UpdateMedalInfo();
            }
            else if (code == MedalMode.Instance.UPDATE_MEDAL_UP)
            {
                UpdateMedalUp();
            }
        }

        private void UpdateMedalInfo()
        {
            int medalId = MedalMode.Instance.UpgradeId;
            medalLevel.text = "Lv." + medalId;
            if (MedalMode.Instance.IsMaxLevel())
            {
                medalId = MedalMode.MaxMedalLvel;
            }
            else
            {
                medalId = medalId + 1;
            }
            SysMedalVo medalVo = BaseDataMgr.instance.GetDataById<SysMedalVo>((uint) medalId);
            List<int> attrList = medalVo.GetAddAttrs();
            GameObject tempItem;
            for (int i = 0; i < 8; i++)
            {
                tempItem = attrItemList[i];
                NGUITools.FindInChild<UILabel>(tempItem, "value").text = "+" + MeVo.instance.GetAttrValueById(attrList[2 * i]);
                NGUITools.FindInChild<UILabel>(tempItem, "name").text = VoUtils.GetRoleAttr(attrList[2*i]);
                NGUITools.FindInChild<UILabel>(tempItem, "add").text = "+" + attrList[2*i + 1];
            }
            if (MedalMode.Instance.IsMaxLevel())
            {
                repuCost.text = "声望：" + string.Format(ColorConst.YELLOW_FORMAT, 0);
                goldCost.text = "金币：" + string.Format(ColorConst.YELLOW_FORMAT, 0);
            }
            else
            {
                string colorString = ColorConst.YELLOW_FORMAT;
                if (!MedalMode.Instance.CanMedalUp(medalId))  //不能升级
                    colorString = ColorConst.RED_FORMAT;
                repuCost.text = "声望：" + string.Format(colorString, medalVo.repu);
                goldCost.text = "金币：" + string.Format(colorString, medalVo.gold);
            }
            
        }

        private void UpdateMedalUp()
        {
            //升级特效
        }
        private void UpdateRepuHandle(object sender, int code)
        {
            if(code == RoleMode.Instance.UPDATE_ROLE_ATTR)
                repuValue.text = LanguageManager.GetWord("RoleView.RepuLabel") + MeVo.instance.repu;
        }
        public override void CancelUpdateHandler()
        {
            MedalMode.Instance.dataUpdated -= UpdateMedalHandle;
            RoleMode.Instance.dataUpdated -= UpdateRepuHandle;
        }
    }

}

