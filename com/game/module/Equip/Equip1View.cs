using com.game.consts;
using UnityEngine;

using Com.Game.Module.Role;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.module.Guide;
using com.game.vo;

namespace Com.Game.Module.Equip
{
    public class Equip1View :BaseView<Equip1View> 
    {
		public override string url {get {return "UI/Equip/Equip3View.assetbundle";}}
        public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }
		public UIToggle ckb_stren;  //强化页签
        public UIToggle ckb_refine;   //精炼
        public UIToggle ckb_inlay;//镶嵌
        public UIToggle ckb_inherit;//继承
        public UIToggle ckb_destroy;//分解
        public UIToggle ckb_merge;//充灵

		public Button btn_close;//关闭按钮
	    private int currentId;  //标记当前页面的Id

        public GameObject RightTips;//没有选择装备的提示信息显示
        public UILabel TipsSprite;//文字

		private int currentClickNavID;
		private enum NAVIGATION_TYPE
		{
			NAV_STREN   = 1,//强化
			NAV_INHERIT = 2,//继承
			NAV_DESTORY = 3,//分解
			NAV_REFINED = 4,//精炼
			NAV_INLAY   = 5,//镶嵌
			NAV_MERGE   = 6,//充灵
		};

		protected override void Init()	
		{
			ckb_inlay = FindInChild<UIToggle>("ckb_inlay");
			ckb_merge = FindInChild<UIToggle>("ckb_merge");
			ckb_inherit = FindInChild<UIToggle>("ckb_inherit");
			ckb_stren = FindInChild<UIToggle>("ckb_stren");
            ckb_destroy = FindInChild<UIToggle>("ckb_destroy");
            ckb_refine = FindInChild<UIToggle>("ckb_refine");

            RightTips = FindChild("tips");
            TipsSprite = NGUITools.FindInChild<UILabel>(RightTips, "label");
            EventDelegate.Add(ckb_refine.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_destroy.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_stren.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_inherit.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_merge.onChange, TabViewOnClick);
            EventDelegate.Add(ckb_inlay.onChange, TabViewOnClick);

			btn_close = FindInChild<Button>("btn_close");
		    btn_close.clickDelegate = CloseView;

		    Singleton<EquipLeftView>.Instance.gameObject = FindChild("left");
		    Singleton<EquipInheritView>.Instance.gameObject =FindChild("EquipInherit");
            Singleton<EquipDestroyView>.Instance.gameObject =FindChild("EquipDestroy");
            Singleton<EquipRefineView>.Instance.gameObject =FindChild("EquipRefine");
            Singleton<EquipStren1View>.Instance.gameObject = FindChild("EquipStren");
            Singleton<SmeltInlay1View>.Instance.gameObject = FindChild("EquipInlay");
            Singleton<SmeltMerge1View>.Instance.gameObject = FindChild("EquipMerge");

		}
		private void TabViewOnClick()
		{
            RightTips.SetActive(false);
			UIToggle current = UIToggle.current;
			if(current.Equals(ckb_stren))//强化
			{
				clickNavStren(current);
			}else if(current.Equals(ckb_inherit))//继承
			{
				clickNavInherit(current);
			}else if(current.Equals(ckb_destroy))//分解
			{
				clickNavDestory(current);
			}else if(current.Equals(ckb_refine))//精炼
			{
				clickNavRefine(current);
			}else if(current.Equals(ckb_inlay))//镶嵌
			{
				clickNavInlay(current);
			}else if(current.Equals(ckb_merge))//充灵
			{
				clickNavMerge(current);
			}
		    if (current.value == true)
			{
		        current.FindInChild<UILabel>("label").color = Color.white;
		    }else
		    {
                current.FindInChild<UILabel>("label").color = ColorConst.FONT_GRAY;
		    }
            
		}

		//点击强化标签
		private void clickNavStren(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<EquipStren1View>.Instance.CloseView();
			}else
			{
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_STREN;
				Singleton<EquipStren1View>.Instance.OpenView();
			}
		}

		//点击继承标签
		private void clickNavInherit(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<EquipInheritView>.Instance.CloseView();
			}else
			{
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_INHERIT;
				Singleton<EquipInheritView>.Instance.OpenView();
			}
		}

		//点击分解标签
		private void clickNavDestory(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<EquipDestroyView>.Instance.CloseView();
			}else
			{
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_DESTORY;
				Singleton<EquipDestroyView>.Instance.OpenView();
			}
		}

		//点击精炼标签
		private void clickNavRefine(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<EquipRefineView>.Instance.CloseView();
			}else
			{
				bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideEquipRefine , MeVo.instance.Level);
				if(isLimit)
				{
					setClickNavState(currentClickNavID);
					return;
				}
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_REFINED;
				Singleton<EquipRefineView>.Instance.OpenView();
			}
		}

		//点击镶嵌标签
		private void clickNavInlay(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<SmeltInlay1View>.Instance.CloseView();
			}else
			{
				bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideEquipInlay , MeVo.instance.Level);
				if(isLimit)
				{
					setClickNavState(currentClickNavID);
					return;
				}
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_INLAY;
				Singleton<SmeltInlay1View>.Instance.OpenView();
			}
		}

		//点击充灵标签
		private void clickNavMerge(UIToggle current)
		{
			if(current.value == false)
			{
				Singleton<SmeltMerge1View>.Instance.CloseView();
			}else
			{
				bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideEquipMerge , MeVo.instance.Level);
				if(isLimit)
				{
					setClickNavState(currentClickNavID);
					return;
				}
				currentClickNavID = (int)NAVIGATION_TYPE.NAV_REFINED;
				Singleton<SmeltMerge1View>.Instance.OpenView();
			}
		}

        public void OpenViewFromBag(int repos,uint uid,int op)
        {
            OpenStrenView();
        }
        /// <summary>
        /// 打开强化面板
        /// </summary>
	    public void OpenStrenView()
	    {
			bool isLimit = OpenLevelLimitManager.checkLeveByGuideLimitID(GuideType.GuideForgeOpen , MeVo.instance.Level);
			if(isLimit)
			{
				setClickNavState(currentClickNavID);
				return;
			}
			currentId = (int)NAVIGATION_TYPE.NAV_STREN;
            OpenView();
	    }
        /// <summary>
        /// 打开继承面板
        /// </summary>
	    public void OpenInheritView()
	    {
			currentId = (int)NAVIGATION_TYPE.NAV_INHERIT;
            OpenView();
	    }
        /// <summary>
        /// 打开镶嵌面板
        /// </summary>
	    public void OpenInlayView()
	    {
			currentId = (int)NAVIGATION_TYPE.NAV_INLAY;
            OpenView();
	    }
        /// <summary>
        /// 打开充灵面板
        /// </summary>
	    public void OpenMergeView()
	    {
			currentId = (int)NAVIGATION_TYPE.NAV_MERGE;
            OpenView();
	    }
        /// <summary>
        /// 分解
        /// </summary>
        public void OpenDestroyView()
        {
			currentId = (int)NAVIGATION_TYPE.NAV_DESTORY;
            OpenView();
        }
        /// <summary>
        /// 精炼
        /// </summary>
        public void OpenRefineView()
        {
            currentId = (int)NAVIGATION_TYPE.NAV_REFINED;
            OpenView();
        }

        private bool isfirst = false;
	    protected override void HandleAfterOpenView()
	    {
            Singleton<EquipLeftView>.Instance.OpenView();
			if (currentId == (int)NAVIGATION_TYPE.NAV_STREN)
	        {
	            ckb_stren.value = true;
	        }
			else if (currentId == (int)NAVIGATION_TYPE.NAV_REFINED)
	            ckb_refine.value = true;
			else if (currentId == (int)NAVIGATION_TYPE.NAV_INHERIT)
	            ckb_inherit.value = true;
			else if (currentId == (int)NAVIGATION_TYPE.NAV_DESTORY)
	            ckb_destroy.value = true;
			else if (currentId == (int)NAVIGATION_TYPE.NAV_INLAY)
	            ckb_inlay.value = true;
			else if (currentId == (int)NAVIGATION_TYPE.NAV_MERGE)
	            ckb_merge.value = true;
            
	    }

        protected override void HandleBeforeCloseView()
        {
            SetToggleFalseHelp(ckb_stren);
            SetToggleFalseHelp(ckb_refine);
            SetToggleFalseHelp(ckb_merge);
            SetToggleFalseHelp(ckb_inlay);
            SetToggleFalseHelp(ckb_inherit);
            SetToggleFalseHelp(ckb_destroy);
        }

        private void SetToggleFalseHelp(UIToggle toggle)
        {
            toggle.optionCanBeNone = true;
            toggle.value = false;
            toggle.optionCanBeNone = false;
        }

		private void setClickNavState(int type)
		{
			switch(type)
			{
				case (int)NAVIGATION_TYPE.NAV_STREN:
					ckb_stren.value = true;
						break;
				case (int)NAVIGATION_TYPE.NAV_REFINED:
					ckb_refine.value = true;
						break;
				case (int)NAVIGATION_TYPE.NAV_INHERIT:
					ckb_inherit.value = true;
						break;
				case (int)NAVIGATION_TYPE.NAV_DESTORY:
					ckb_destroy.value = true;
						break;
				case (int)NAVIGATION_TYPE.NAV_INLAY:
					ckb_inlay.value = true;
						break;
				case (int)NAVIGATION_TYPE.NAV_MERGE:
					ckb_merge.value = true;
						break;
			}
		}
    }
}