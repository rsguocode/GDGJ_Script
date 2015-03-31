//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：CodeView
//文件描述：激活码界面
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

using com.game.module.test;

namespace com.game.module.LoginAward
{
	public class CodeView : Singleton<CodeView>
	{
		private GameObject codeViewGameObject;

		private UIInput inputLabel;
		private Button btn_receive;

		public void Init(GameObject obj)
		{
			codeViewGameObject = obj;
			initView();
			initClick();
		}

		private void initView()
		{
			inputLabel = NGUITools.FindInChild<UIInput>(codeViewGameObject.gameObject , "inp_input");
			btn_receive = NGUITools.FindInChild<Button>(codeViewGameObject.gameObject , "btn_receive");
		}

		private void initClick()
		{
			btn_receive.onClick = getCodeReceiveOnClick;
		}

		private void getCodeReceiveOnClick(GameObject go)
		{
			if(inputLabel.value == "")
			{
				return;
			}
			Singleton<LoginAwardMode>.Instance.ApplyActivationCode((ushort) LoginAwardConst.GiftType.ActivationCode , inputLabel.value);
		}

		//成功领取激活码
		public void GetCodeSuccess()
		{
			inputLabel.value = "";
		}
	}
}
