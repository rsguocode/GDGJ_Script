using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Tips
{
    public class PetEquipTips : BaseView<PetEquipTips>
    {
        public override string url { get { return "UI/Tips/PetEquipTips/PetEquipTips.assetbundle"; } }
        public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }

		public UIWidgetContainer.ClickDelegate mCloseOnClick;

        //private Button equipButton;//装备按钮
        private Button closeButton;//关闭按钮

        private UILabel descLabel;//描述
        private UILabel nameLabel;//名字
        private UILabel numLabel;//拥有数量
        private UILabel lvlLabel;//需要等级
        private List<UILabel> attrList = new List<UILabel>(); //加成属性值 

        private UIWidgetContainer item;//物品
        

        private uint goodsId;//宠物装备id

        protected override void Init()
        {
            closeButton = FindInChild<Button>("info/btn_close");
            closeButton.clickDelegate = CloseView;
            descLabel = FindInChild<UILabel>("info/des");
            lvlLabel = FindInChild<UILabel>("info/level");
            nameLabel = FindInChild<UILabel>("info/name");
            numLabel = FindInChild<UILabel>("info/num");
            base.showTween = FindInChild<TweenPlay>("info");
            FindInChild<UIWidgetContainer>("background").clickDelegate = CloseView;
            item = FindInChild<UIWidgetContainer>("info/item");
            for (int i = 1; i < 6; i++)
            {
                attrList.Add(FindInChild<UILabel>("info/property" + i));
            }
        }
        public void OpenViewByGoodsId(uint goodsId, string leftText, string rightText, UIWidgetContainer.ClickDelegate mLeftOnClick = null,
                                 UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, UIWidgetContainer.ClickDelegate openHandle = null)
        {
            this.goodsId = goodsId;
            OpenView();
        }

        public void OpenViewById(uint uid, string leftText, string rightText, UIWidgetContainer.ClickDelegate mLeftOnClick = null,
                                 UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, UIWidgetContainer.ClickDelegate openHandle = null)
        {
            this.goodsId = GoodsMode.Instance.GetPGoodsById(uid).goodsId;
            OpenView();
        }

		public void OpenPlayerEquipViewById(uint uid, string leftText, string rightText, UIWidgetContainer.ClickDelegate mLeftOnClick = null,
		                         UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, UIWidgetContainer.ClickDelegate openHandle = null)
		{
			this.goodsId = GoodsMode.Instance.GetOtherPGoods(uid).goodsId;
			this.mCloseOnClick = closeOnClick;
			OpenView();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			if (null != mCloseOnClick)
			{
				mCloseOnClick();
			}
		}

        protected override void HandleAfterOpenView()
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
            int count = GoodsMode.Instance.GetCountByGoodsId(goodsId);
            ItemManager.Instance.InitItem(item,goodsId,0);
            nameLabel.text = vo.name;
            descLabel.text = vo.desc;
            numLabel.text = VoUtils.LanguagerFormat("PetEquipTips.NumTips" ,count);
            lvlLabel.text = LanguageManager.GetWord("PetEquipTips.LvlTips") + vo.GetLvl();
            List<string> attrString = vo.GetPetAttrList();
            UILabel label;
            for (int i = 0; i < 5; i++)  //加成属性
            {
                label = attrList[i];
                if (i < attrString.Count)
                {
                    label.text = attrString[i];
                    label.SetActive(true);
                }
                else
                {
                    label.text = string.Empty;
                    label.SetActive(false);
                }
            }
        }

    }

}

