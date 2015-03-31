//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：LoginAwardView
//文件描述：奖励界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

using com.game.manager;
using com.game.module.test;

namespace com.game.module.LoginAward
{
	public class AwardView : BaseView<AwardView>
	{
		public override string url { get { return "UI/LoginAward/LoginAwardView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.HighLayer ; }}
		public override bool IsFullUI { get { return true ; }}
		public override bool isDestroy { get { return true; } }

		private Button btn_close;

		private UILabel btnLoginLabel;
		private UILabel btnCodeLabel;

		private UIToggle btn_login;
		private UIToggle btn_code;
		private UIToggle btn_sign;

		private GameObject loginViewObj;
		private GameObject inputCodeViewObj;
		private GameObject signViewObj;
		private bool isInstantiationChildView;

		protected override void Init()
		{
			initView();
			initClick();
			initTextLanguage();
		}

		private void initView()
		{
			btn_close = FindInChild<Button>("btn_close");

			loginViewObj     = NGUITools.FindChild(gameObject , "loginView");
			inputCodeViewObj = NGUITools.FindChild(gameObject , "inputCodeView");
			signViewObj      = NGUITools.FindChild(gameObject , "signView");

			btnLoginLabel = FindInChild<UILabel>("leftNavigation/btn_login/label");
			btnCodeLabel  = FindInChild<UILabel>("leftNavigation/btn_code/label");

			btn_login = FindInChild<UIToggle>("leftNavigation/btn_login");
			btn_code  = FindInChild<UIToggle>("leftNavigation/btn_code");
			btn_sign  = FindInChild<UIToggle>("leftNavigation/btn_sign");
		}

		private void initClick()
		{
			btn_close.onClick = closeOnClick;

			EventDelegate.Add(btn_login.onChange , loginOnClick);
			EventDelegate.Add(btn_code.onChange , codeOnClick);
			EventDelegate.Add(btn_sign.onChange , signOnClick);
		}

		private void initTextLanguage()
		{
			btnLoginLabel.text = LanguageManager.GetWord("AwardView.loginAward");
			btnCodeLabel.text  = LanguageManager.GetWord("AwardView.code");
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			Singleton<LoginAwardMode>.Instance.ApplyDayInfo();
			Singleton<LoginAwardMode>.Instance.StopTips();
			Singleton<LoginAwardMode>.Instance.ApplySignInfo();
			btn_login.value = true;
			instanticationChildView();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
		}

		public override void Destroy()
		{
			base.Destroy();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<LoginAwardMode>.Instance.dataUpdated += updateLoginAwardHandler;
			Singleton<LoginAwardMode>.Instance.dataUpdated += updateGetAwardHanderl;
			Singleton<LoginAwardMode>.Instance.dataUpdated += updateGetCodeHandler;
			Singleton<LoginAwardMode>.Instance.dataUpdated += updateSignHandler;
		}

		public override void CancelUpdateHandler()
		{
			Singleton<LoginAwardMode>.Instance.dataUpdated -= updateLoginAwardHandler;
			Singleton<LoginAwardMode>.Instance.dataUpdated -= updateGetAwardHanderl;
			Singleton<LoginAwardMode>.Instance.dataUpdated -= updateGetCodeHandler;
			Singleton<LoginAwardMode>.Instance.dataUpdated -= updateSignHandler;
		}

		private void closeOnClick(GameObject go)
		{
			isInstantiationChildView = false;
			CloseView();
		}

		//实例化子界面
		private void instanticationChildView()
		{
			if(!isInstantiationChildView)
			{
				Singleton<LoginAwardView>.Instance.Init(loginViewObj);
				Singleton<CodeView>.Instance.Init(inputCodeViewObj);
				Singleton<SignView>.Instance.Init(signViewObj);
			}
			isInstantiationChildView = true;
		}

		//登陆奖励
		private void loginOnClick()
		{
			if (UIToggle.current.Equals(btn_login) && UIToggle.current.value)
			{
				loginViewObj.SetActive(true);
				inputCodeViewObj.SetActive(false);
				signViewObj.SetActive(false);
			}
		}

		//激活码
		private void codeOnClick()
		{
			if (UIToggle.current.Equals(btn_code) && UIToggle.current.value)
			{
				loginViewObj.SetActive(false);
				inputCodeViewObj.SetActive(true);
				signViewObj.SetActive(false);
			}
		}

		//签到奖励
		private void signOnClick()
		{
			if (UIToggle.current.Equals(btn_sign) && UIToggle.current.value)
			{
				loginViewObj.SetActive(false);
				inputCodeViewObj.SetActive(false);
				signViewObj.SetActive(true);
			}
		}

		//登陆奖励
		private void updateLoginAwardHandler(object sender , int code)
		{
			if(code == Singleton<LoginAwardMode>.Instance.UPDATE_AWARD_INFO)
			{
				int day    = Singleton<LoginAwardMode>.Instance.dayInfo.day;
				int status = Singleton<LoginAwardMode>.Instance.dayInfo.status;
				Singleton<LoginAwardView>.Instance.SetCurrentDayState(day , status);
			}
		}

		//成功领取奖励
		private void updateGetAwardHanderl(object sender , int code)
		{
			if(code == Singleton<LoginAwardMode>.Instance.UPDATE_GET_AWARD)
			{
				int day    = Singleton<LoginAwardMode>.Instance.dayInfo.day;
				int status = Singleton<LoginAwardMode>.Instance.dayInfo.status = (int)LoginAwardConst.GetStatus.HaveReveive;
				Singleton<LoginAwardView>.Instance.SetCurrentDayState(day , status);
			}
		}

		//成功领取激活码
		private void updateGetCodeHandler(object sender , int code)
		{
			if(code == Singleton<LoginAwardMode>.Instance.UPDATE_CODE_SUCCESS)
			{
				Singleton<CodeView>.Instance.GetCodeSuccess();
			}
		}

		//签到信息
		private void updateSignHandler(object sender , int code)
		{
			if(code == Singleton<LoginAwardMode>.Instance.UPDATE_SIGN)
			{
				Singleton<SignView>.Instance.updateCurrentSign(Singleton<LoginAwardMode>.Instance.signInfo);
			}
		}
	}
}