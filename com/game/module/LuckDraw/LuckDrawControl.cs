//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LuckDrawControl
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.cmd;
using com.net.interfaces;
using com.game;
using Proto;
using com.u3d.bases.debug;
using com.game.Public.Message;

namespace Com.Game.Module.LuckDraw
{
	public class LuckDrawControl : BaseControl<LuckDrawControl>  
	{	
		//注册Socket数据返回监听
		override protected void NetListener()
		{
            AppNet.main.addCMD(CMD.CMD_33_1, Fun_33_1);      //抽奖基础信息
            AppNet.main.addCMD(CMD.CMD_33_2, Fun_33_2);      //抽奖
		}

		//抽奖基础信息
		private void Fun_33_1(INetData data)
		{
			ChoujiangInfoMsg_33_1 choujiangInfo = new ChoujiangInfoMsg_33_1();
			choujiangInfo.read(data.GetMemoryStream());	

			if (0 == choujiangInfo.code)
			{
				Singleton<LuckDrawMode>.Instance.SetBaseInfo(choujiangInfo.first, choujiangInfo.freeCd);
			}
			else
			{
				ErrorCodeManager.ShowError(choujiangInfo.code);
			}
		}

		//抽奖
		private void Fun_33_2(INetData data)
		{
			ChoujiangDoMsg_33_2 choujiangDo = new ChoujiangDoMsg_33_2();
			choujiangDo.read(data.GetMemoryStream());	

			if (0 == choujiangDo.code)
			{
				Singleton<LuckDrawMode>.Instance.SetLuckDrawTypeAndReward(choujiangDo.type, choujiangDo.reward);
			}
			else
			{
				Singleton<LuckDrawMode>.Instance.BuyError();
				ErrorCodeManager.ShowError(choujiangDo.code);
			}
		}
		
	}
}
