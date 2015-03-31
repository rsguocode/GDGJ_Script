using com.game.consts;
using Com.Game.Module.Medal;
using Com.Game.Module.Role.Deed;
using com.game.vo;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;
using com.game.module.test;
using com.game.manager;
using com.game.module.Guide;

namespace Com.Game.Module.Role
{
    public class RoleView : BaseView<RoleView>
    {
        public override string url { get { return "UI/Role/RoleView.assetbundle"; } }
        public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}
		public override ViewType viewType { get { return ViewType.CityView; }}

        public UIToggle ckb_shuxing;
        public UIToggle ckb_beibao;
        public UIToggle ckb_peiyue;
        public UIToggle ckb_medal;//勋章
       
        public Button btn_close;

        private int currentId ; 
		private SpinWithMouse modelSpin;
        private GameObject roleBackground;
        private GameObject repuGameObject;

		public TweenPosition tweenPosition;
        public EventDelegate.Callback AfterOpenViewGuideDelegate;

		private int currentClickNavID;
		private enum NAVIGATION_TYPE
		{
			NAV_BEIBAO   = 1,
			NAV_SHUXING  = 2,
			NAV_XUNZHANG = 3,
			NAV_PEIYU    = 4,
		};
        
        private UIToggle lastTab;
        private UIToggle currentTab;
        protected override void Init()
        {
			base.showTween = FindInChild<TweenPlay>();
            roleBackground = FindChild("rolebackground");
            repuGameObject = FindChild("Medal/repu");
            modelSpin = NGUITools.FindInChild<SpinWithMouse>(roleBackground,"rolebackgroundl");
            EventDelegate.Add(base.showTween.onFinished,GoodsView.Instance.ResetGird);
			tweenPosition = FindInChild<TweenPosition>();
            ckb_shuxing = FindInChild<UIToggle>("top/ckb_shuxing");
			ckb_shuxing.FindInChild<UILabel>("label").text = LanguageManager.GetWord("RoleView.PropertyTab");
			ckb_beibao = FindInChild<UIToggle>("top/ckb_beibao");
			ckb_beibao.FindInChild<UILabel>("label").text = LanguageManager.GetWord("RoleView.BagTab");
            ckb_peiyue = FindInChild<UIToggle>("top/ckb_peiyu");
			ckb_peiyue.FindInChild<UILabel>("label").text = LanguageManager.GetWord("RoleView.GrowTab");
            ckb_medal = FindInChild<UIToggle>("top/ckb_medal");
            ckb_medal.FindInChild<UILabel>("label").text = LanguageManager.GetWord("RoleView.MedalTab");

            btn_close = FindInChild<Button>("topright/btn_close");
           
            btn_close.clickDelegate += CloseView;

            EventDelegate.Add(ckb_medal.onChange,TabViewOnClick);
            EventDelegate.Add(ckb_shuxing.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_peiyue.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_beibao.onChange, TabViewOnClick);

            Singleton<GoodsView>.Instance.gameObject = FindChild("Goods");
            Singleton<RolePropView>.Instance.gameObject = FindChild("Property");
            Singleton<GrowView>.Instance.gameObject = FindChild("Grow");
            MedalView.Instance.gameObject = FindChild("Medal");

        }

        /// <summary>
        /// 打开背包
        /// </summary>
        public void OpenGoodsView()
        {
            currentId = 1;
            OpenView();
        }
        
        /// <summary>
        /// 打开属性
        /// </summary>
        public void OpenProView()
        {
            currentId = 2;
            OpenView();
        }
        /// <summary>
        /// 打开技能
        /// </summary>
        public void OpenSkillView()
        {
            currentId = 3;
            OpenView();
        }

        public RoleDisplay RoleModel;
        protected override void HandleAfterOpenView()
        {
			TabViewHelp();
            if (RoleModel == null)
            {
                RoleModel = new RoleDisplay();
                RoleModel.CreateRole((int)MeVo.instance.job, LoadCallBack);
            }
			TweenUtils.AdjustTransformToClick(tweenPosition);
            if (AfterOpenViewGuideDelegate != null)
            {
                EventDelegate.Add(tweenPosition.onFinished, AfterOpenViewGuideDelegate);
            }
        }
        public override void RegisterUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated += TabTipsHandle;
            GrowMode.Instance.dataUpdated += TabTipsHandle;
        }

        private void TabTipsHandle(object sender, int code)
        {
            if (sender.Equals(GoodsMode.Instance) && code == GoodsMode.Instance.UPDATE_TIPS )
            {
                UpdateBeibaoTips();
            }
            else if (sender.Equals(GrowMode.Instance) && code == GrowMode.Instance.UPDATE_TIPS)
            {
                UpdatePeiyuTips();
            }
        }

        private void UpdateBeibaoTips()
        {
            if (GoodsMode.Instance.ShowTips && currentClickNavID != (int)NAVIGATION_TYPE.NAV_BEIBAO)
                ckb_beibao.FindInChild<UISprite>("tips").SetActive(true);
            else
            {
                ckb_beibao.FindInChild<UISprite>("tips").SetActive(false);
            }
        }

        private void UpdatePeiyuTips()
        {
            if (GrowMode.Instance.ShowTips && currentClickNavID != (int)NAVIGATION_TYPE.NAV_PEIYU)
                ckb_peiyue.FindInChild<UISprite>("tips").SetActive(true);
            else
            {
                ckb_peiyue.FindInChild<UISprite>("tips").SetActive(false);
            }
        }
        public override void CancelUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated -= TabTipsHandle;
            GrowMode.Instance.dataUpdated -= TabTipsHandle;
        }

        public void LoadCallBack(GameObject go)
        {
            modelSpin.target = RoleModel.GoBase.transform.GetChild(0);
            modelSpin.speed = 3f;
            SetModelPosition();
        }

        private void DisableModel()
        {
            roleBackground.SetActive(false);
            if(RoleModel != null && RoleModel.GoBase != null)
                NGUITools.SetActive(RoleModel.GoBase,false);
        }
		protected override void HandleBeforeCloseView()
		{
            SetToggleFalseHelp(ckb_medal);
            SetToggleFalseHelp(ckb_beibao);
            SetToggleFalseHelp(ckb_peiyue);
            SetToggleFalseHelp(ckb_shuxing);
            DisableModel();
		}

        private void SetToggleFalseHelp(UIToggle toggle)
        {
            toggle.optionCanBeNone = true;
		    toggle.value = false;
		    toggle.optionCanBeNone = false;
        }
        private bool isfirst = false;
		private void TabViewHelp()
		{
			if (currentId == (int)NAVIGATION_TYPE.NAV_BEIBAO)
		    {
                currentTab = ckb_beibao;
                ckb_beibao.value = true;
		        
		    }
			if (currentId == (int)NAVIGATION_TYPE.NAV_SHUXING)
		    {
                currentTab = ckb_shuxing;
		        ckb_shuxing.value = true;
		    }
			if (currentId == (int)NAVIGATION_TYPE.NAV_XUNZHANG)
			{
                currentTab = ckb_medal;
			    ckb_medal.value = true;
			}
			if (currentId == (int)NAVIGATION_TYPE.NAV_PEIYU)
		    {
                currentTab = ckb_peiyue;
		        ckb_peiyue.value = true;
		    }
		}

        private void TabViewOnClick()
        {
            UIToggle current = UIToggle.current;
            
            if (current.Equals(ckb_beibao))
            {
	            if (current.value == false)
                {
                    Singleton<GoodsView>.Instance.CloseView();
                }
                else
				{
					currentClickNavID = (int)NAVIGATION_TYPE.NAV_BEIBAO;
					Singleton<GoodsView>.Instance.OpenView();
					SetModelPosition();

                }
            }
            else if (current.Equals(ckb_shuxing))
            {
                if (UIToggle.current.value == false)
                {
                    Singleton<RolePropView>.Instance.CloseView();
                }
                else
				{
					currentClickNavID = (int)NAVIGATION_TYPE.NAV_SHUXING;
                    Singleton<RolePropView>.Instance.OpenView();
                    SetModelPosition();
                }
            }
            else if (UIToggle.current.Equals(ckb_medal))
            {
                if (UIToggle.current.value == false)
                {
                    MedalView.Instance.CloseView();
                }
                else
                {
					bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideMedal , MeVo.instance.Level);
					if(isLimit)
					{
						setClickNavState(currentClickNavID);
						return;
					}
					currentClickNavID = (int)NAVIGATION_TYPE.NAV_XUNZHANG;
                    repuGameObject.SetActive(true);
                    MedalView.Instance.OpenView();
                    DisableModel();
                }
            }
            else if (current.Equals(ckb_peiyue))
            {
                if (current.value == false)
                {
                    Singleton<GrowView>.Instance.CloseView();
                }
                else
                {
					bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideGrow , MeVo.instance.Level);
					if(isLimit)
					{
						setClickNavState(currentClickNavID);
						return;
					}
					currentClickNavID = (int)NAVIGATION_TYPE.NAV_PEIYU;
                    Singleton<GrowView>.Instance.OpenView();
                    SetModelPosition();
                }
            }
            if (current.value) //调整页签颜色
            {
                current.FindInChild<UILabel>("label").color = Color.white;
                //SetOtherFalse(current);
            }
            else
            {
                current.FindInChild<UILabel>("label").color = ColorConst.FONT_GRAY;
            }
            if (current.Equals(ckb_medal))
            {
                roleBackground.SetActive(false);
                repuGameObject.SetActive(true);
            }
            else
            {
                repuGameObject.SetActive(false);
                roleBackground.SetActive(true);
            }
            UpdateBeibaoTips();
            UpdatePeiyuTips();
        }
		private void SetModelPosition()
		{
            roleBackground.SetActive(true);
		    if (RoleModel != null && RoleModel.GoBase != null)
		    {
                NGUITools.SetActive(RoleModel.GoBase, true);
                RoleModel.GoBase.transform.localPosition = new Vector3(-248.8286f, -162f, 0);
		    }
		    
		}

		private void setClickNavState(int type)
		{
			switch(type)
			{
				case (int)NAVIGATION_TYPE.NAV_BEIBAO:
					ckb_beibao.value = true;
					break;
				case (int)NAVIGATION_TYPE.NAV_SHUXING:
					ckb_shuxing.value = true;
					break;
				case (int)NAVIGATION_TYPE.NAV_XUNZHANG:
					ckb_medal.value = true;
					break;
				case (int)NAVIGATION_TYPE.NAV_PEIYU:
					ckb_peiyue.value = true;
					break;
			}
		}
    }
}

