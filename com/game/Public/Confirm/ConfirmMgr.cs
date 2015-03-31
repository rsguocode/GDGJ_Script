using System;
using System.Collections;
using System.Collections.Generic;
using PCustomDataType;
using UnityEngine;
using com.game.module.test;
using com.game.manager;
using com.game.module;
using com.game.consts;

namespace com.game.Public.Confirm
{
    public class ConfirmMgr
    {
        private static string url { get { return "UI/Mask/MaskView.assetbundle"; } }
        private GameObject maskGo;

        private IList<ConfirmView> viewList;
        private int topViewIndex; 

		public static ConfirmMgr Instance = (Instance == null ? new ConfirmMgr() : Instance);
        public ConfirmView CurrentConfirmView;

		private ConfirmMgr()
		{
			//内部数据初始化
			if (null == viewList)
			{
				viewList = new List<ConfirmView>();
				topViewIndex = -1;
			}
		}

        public void Init()
        {
            //加载mask
            AssetManager.Instance.LoadAsset<GameObject>(url, LoadViewCallBack);
        }

        //UI资源加载成功后回调
		private void LoadViewCallBack(GameObject prefab)
        {
			if (null == maskGo)
            {
				maskGo = NGUITools.AddChild(ViewTree.go, prefab);
				maskGo.SetActive(false);

				UIPanel panel = maskGo.GetComponent<UIPanel>();
				panel.depth = GameConst.ConfirmMaskDepth;
            }
        }

        //窗口关闭后通知
        private void ViewCloseCallback()
        {
            topViewIndex--;
            SetOnlyTopViewVisible();

			//如果没有view显示了，需要隐藏遮罩
			if (topViewIndex < 0)
			{
				maskGo.active = false;
			}
        }

        //获得最顶层view
        private ConfirmView GetNextView(string cmd)
        {
            ConfirmView view;

			//如果有相同类型的窗口在显示，返回此窗口，从而保证同一类型窗口只显示一个
			for (int i = 0; i <= topViewIndex; i++) 
			{
				view = viewList[i];
				if (view.Cmd == cmd)
				{
					return view;
				}
			}

			//如果没有相同类型的窗口，则寻找下一个
            if ((topViewIndex < viewList.Count - 1) && (viewList.Count > 0))
            {
                topViewIndex++;
                view = viewList[topViewIndex];
            }
            else
            {
                view = new ConfirmView();
                view.ViewClosedCallback = ViewCloseCallback;
                viewList.Add(view);
                topViewIndex++;
            }

			//显示遮罩
			if (!maskGo.active)
			{
				maskGo.active = true;
			}

			//显示顶层view
            SetOnlyTopViewVisible();
            CurrentConfirmView = view;
            return view;
        }

        //只显示最顶层view
        private void SetOnlyTopViewVisible()
        {
            if (topViewIndex < 0)
            {
                return;
            }

            //隐藏底层view
            for (int i = 0; i < topViewIndex; i++)
            {
                if (viewList[i].visible)
                {
                    viewList[i].visible = false;
                }
            }

            viewList[topViewIndex].visible = true;
        }

        //显示提示信息  入参 tips ok按钮回调事件 ok按钮文字 cancel回调事件 cancel按钮文字
        //按钮点击后，窗口自动关闭
		public void ShowCommonAlert(string tip = "", string cmd = ConfirmCommands.OK_CANCEL, ClickCallback okFun = null, string txtOk = "",
                                    ClickCallback cancelFun = null, string txtCancel = "")
        {
			ConfirmView view = GetNextView(cmd);
			view.ShowCommonAlert(tip, cmd, okFun, txtOk, cancelFun, txtCancel);
        }

        // 显示普通信息，按钮名字为确定，取消
        // 按钮点击后，窗口自动关闭
		public void ShowOkCancelAlert(string tip = "", string cmd = ConfirmCommands.OK_CANCEL, ClickCallback okFun = null)
        {
			ConfirmView view = GetNextView(cmd);
            view.ShowOkCancelAlert(tip, cmd, okFun);
        }

        //显示Ok提示框，只有Ok按钮，没有取消、关闭按钮
        //按钮点击后，窗口自动关闭
		public void ShowOkAlert(string tip = "", string cmd = ConfirmCommands.OK, ClickCallback okFun = null, string txtOk = "")
        {
			ConfirmView view = GetNextView(cmd);
			view.ShowOkAlert(tip, cmd, okFun, txtOk);
        }

		//显示二者选一窗口,没有关闭按钮
		public void ShowSelectOneAlert(string tip = "", string cmd = ConfirmCommands.SELECT_ONE, ClickCallback okFun = null, string txtOk = "",
		                               ClickCallback cancelFun = null, string txtCancel = "")
		{
			ConfirmView view = GetNextView(cmd);
			view.ShowSelectOneAlert(tip, cmd, okFun, txtOk, cancelFun, txtCancel);
		} 
    }
}

