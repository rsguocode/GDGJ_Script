using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.manager;
using com.game.module;
using com.game.consts;
using com.game.sound;

namespace com.game.Public.Confirm
{
	public delegate void ClickCallback();
	public delegate void ClosedCallback();

    public class ConfirmView 
    {

        private static string url { get { return "UI/Confirm/ConfirmView.assetbundle"; } }
		private static GameObject viewPrefab;

		//对话框命令类型
		private string cmd;

		//Ok按钮的两个位置记录
		private Vector3 okPosForTwoBtn;
		private Vector3 okPosForOneBtn;
        
		private GameObject gameObject;
        
        private string tips;
		private string txtOk;
		private string txtCancel;
		private ClickCallback okClickedCallback;
		private ClickCallback cancelClickedCallback;
        
        private Button btnCancel;
		private Button btnClose;
        private UILabel labTips;
        private UILabel okLabel;
		private UILabel cancelLabel;
		private UISprite sprBack;

        public Button btnOk;
        //设置成静态的，指引专用，所有的ConfirmView公用1个
        public static OpenViewGuideDelegate AfterOpenViewGuideDelegate; 

		public ClosedCallback ViewClosedCallback 
		{
			get; 
			set;
		}

		public enum AlertTypeEnum
		{
			Common,   //普通窗口，有确定、取消、关闭按钮
			Ok,       //确定窗口，只有确定、关闭按钮
			SelectOne //二者选一窗口，只有确定、取消窗口
		}

		//窗口外观类型
		private AlertTypeEnum alertType;

		public bool visible
		{
			get
			{
				if (null != gameObject)
				{
					return gameObject.active;
				}
				else
				{
					return false;
				}
			}

			set
			{
				if (null != gameObject)
				{
					gameObject.active = value;
				}
			}
		}

		public string Cmd
		{
			get
			{
				return cmd;
			}
		}

        private void Init()
        {
			UIPanel panel = gameObject.GetComponent<UIPanel>();
			panel.depth = GameConst.ConfirmMaskDepth + 1;

			btnOk = NGUITools.FindInChild<Button>(gameObject, "btn_ok");
			btnCancel = NGUITools.FindInChild<Button>(gameObject, "btn_cancel");
			btnClose = NGUITools.FindInChild<Button>(gameObject, "btn_close");
			labTips = NGUITools.FindInChild<UILabel>(gameObject, "tips");
			okLabel = NGUITools.FindInChild<UILabel>(gameObject, "btn_ok/label");
			cancelLabel = NGUITools.FindInChild<UILabel>(gameObject, "btn_cancel/label");
			sprBack = NGUITools.FindInChild<UISprite>(gameObject, "background");

            btnOk.onClick = OkOnClick;
            btnCancel.onClick = CancelOnClick;
			btnClose.onClick = CloseOnClick;

			labTips.text = tips;
			if ("" == txtOk) 
			{
				txtOk = LanguageManager.GetWord("ConfirmView.Ok");
			}
			okLabel.text = txtOk;
			if ("" == txtCancel) 
			{
				txtCancel = LanguageManager.GetWord("ConfirmView.Cancel");
			}
			cancelLabel.text = txtCancel;

			//获取ok按钮的两个不同位置
			okPosForTwoBtn = btnOk.gameObject.transform.position;
			okPosForOneBtn = sprBack.gameObject.transform.position;
			okPosForOneBtn.y = okPosForTwoBtn.y;

			SetToLayerTopUI();
        }

		//设置对象层级
		private void SetToLayerTopUI()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI")); 
		}

        //确定按钮事件处理
        private void OkOnClick(GameObject go)
        {
           CloseView();
	       if (null != okClickedCallback)
	       {
	         okClickedCallback();
	       }

			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmOk);
        }

        //取消按钮事件处理
        private void CancelOnClick(GameObject go)
        {
            CloseView();
		    if (null != cancelClickedCallback)
		    {
			   cancelClickedCallback();
		    }

			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmOk);
        }

		//关闭按钮事件处理
		private void CloseOnClick(GameObject go)
		{
			CloseView();
			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmOk);
		}

		//显示窗口
		private void ShowWindow(string tip, string cmd, ClickCallback okFun,  
		                        string textOk, ClickCallback cancelFun, string textCancel)
		{
			this.tips = tip;
			this.cmd = cmd;
			this.txtOk = textOk;
			this.txtCancel = textCancel;
			this.okClickedCallback = okFun;
			this.cancelClickedCallback = cancelFun;

			OpenView();
		}

		//显示提示信息  入参 tips ok按钮回调事件 ok按钮文字 cancel回调事件 cancel按钮文字
		//按钮点击后，窗口自动关闭
		public void ShowCommonAlert(string tip, string cmd, ClickCallback okFun,  
		                            string textOk, ClickCallback cancelFun, string textCancel)
		{
			alertType = AlertTypeEnum.Common;
			ShowWindow(tip, cmd, okFun, textOk, cancelFun, textCancel);
		}

		// 显示普通信息，按钮名字为确定，取消
		// 按钮点击后，窗口自动关闭
		public void ShowOkCancelAlert(string tip, string cmd, ClickCallback okFun)
		{
			alertType = AlertTypeEnum.Common;
			ShowWindow(tip, cmd, okFun, LanguageManager.GetWord("ConfirmView.Ok"), 
			           null, LanguageManager.GetWord("ConfirmView.Cancel")); 
		}

		//显示Ok提示框，只有Ok按钮，没有取消、关闭按钮
		//按钮点击后，窗口自动关闭
		public void ShowOkAlert(string tip, string cmd, ClickCallback okFun, string textOk)
		{
			if ("" == textOk) 
			{
				textOk = LanguageManager.GetWord("ConfirmView.Ok");
			}

			alertType = AlertTypeEnum.Ok;
			ShowWindow(tip, cmd, okFun, textOk, null, "");
		}

		//显示二者选一窗口,没有关闭按钮
		public void ShowSelectOneAlert(string tip, string cmd, ClickCallback okFun, string txtOk,
		                               ClickCallback cancelFun, string txtCancel)
		{
			alertType = AlertTypeEnum.SelectOne;

			if ("" == txtOk) 
			{
				txtOk = LanguageManager.GetWord("ConfirmView.Ok");
			}

			if ("" == txtCancel) 
			{
				txtCancel = LanguageManager.GetWord("ConfirmView.Cancel");
			}

			ShowWindow(tip, cmd, okFun, txtOk, cancelFun, txtCancel);
		} 

		//调整按钮的位置和显示
		private void AdjustButtons()
		{
			//确定框
			if (AlertTypeEnum.Ok == alertType)
			{
				btnCancel.gameObject.SetActive(false);
				btnClose.gameObject.SetActive(false);
				btnOk.gameObject.transform.position = okPosForOneBtn; 
			}
			//两者选一框
			else if (AlertTypeEnum.SelectOne == alertType)
			{
				btnCancel.gameObject.SetActive(true);
				btnClose.gameObject.SetActive(false);
				btnOk.gameObject.transform.position = okPosForTwoBtn; 
			}
			//普通框
			else
			{
				btnCancel.gameObject.SetActive(true);
				btnClose.gameObject.SetActive(true);
				btnOk.gameObject.transform.position = okPosForTwoBtn; 
			}
		}

		private void UpdateInfo()
		{
			labTips.text = tips;
			okLabel.text = txtOk;
			cancelLabel.text = txtCancel;
		}

		//UI资源加载成功后回调
		private void LoadViewCallBack(GameObject prefab)
		{
			if (null == viewPrefab)
			{
				viewPrefab = prefab;
				CreateView();
			}
            if (AfterOpenViewGuideDelegate != null)
            {
                AfterOpenViewGuideDelegate();
                AfterOpenViewGuideDelegate = null;
            }
		}

		//在Hierarchy面板中创建对象
		private void CreateView()
		{
			gameObject = NGUITools.AddChild(ViewTree.go, viewPrefab);
			gameObject.SetActive(true);
			Init();
			UpdateInfo();
			AdjustButtons();
		}

		private void LoadView()
		{
			if (null == viewPrefab)
			{
				AssetManager.Instance.LoadAsset<GameObject>(url, LoadViewCallBack);
			}
			else
			{
				CreateView();
			}
		}

		private void OpenView()
		{
			if (null == gameObject)
			{
				LoadView();				
			}
			else
			{
				gameObject.SetActive(true);
				UpdateInfo();
				AdjustButtons();
                if (AfterOpenViewGuideDelegate != null)
                {
                    AfterOpenViewGuideDelegate();
                    AfterOpenViewGuideDelegate = null;
                }
			}
		   
		}

		private void CloseView()
		{
			gameObject.SetActive(false);

			if (null != ViewClosedCallback)
			{
				ViewClosedCallback();
			}
		}
    }
}

