//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldBoxControl
//文件描述：
//创建者：黄江军
//创建日期：2014-02-13
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

namespace Com.Game.Module.GoldBox
{
	public class GoldBoxControl : BaseControl<GoldBoxControl>  
	{		
		public GoldBoxControl() 
		{	
		}	

		//注册Socket数据返回监听
		override protected void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_3_42, Fun_3_42);      //购买金币信息
			AppNet.main.addCMD(CMD.CMD_3_43, Fun_3_43);      //购买金币
		}

		//购买金币信息
		private void Fun_3_42(INetData data)
		{
			RoleGoldBuyInfoMsg_3_42 roleGoldBuyInfo = new RoleGoldBuyInfoMsg_3_42();
			roleGoldBuyInfo.read(data.GetMemoryStream());	

			Singleton<GoldBoxMode>.Instance.SetGoldBuyInfo(roleGoldBuyInfo.times, roleGoldBuyInfo.remainTimes, 
			                                                 roleGoldBuyInfo.diam, roleGoldBuyInfo.gold,
			                                                 roleGoldBuyInfo.batchDiam);
		}

		//购买金币返回码
		private void Fun_3_43(INetData data)
		{
			RoleGoldBuyMsg_3_43 roleGoldBuyCode = new RoleGoldBuyMsg_3_43();
			roleGoldBuyCode.read(data.GetMemoryStream());	

			if (0 == roleGoldBuyCode.code)
			{
				Singleton<GoldBoxMode>.Instance.GoldGetType = roleGoldBuyCode.type;
			}
			else
			{
				ErrorCodeManager.ShowError(roleGoldBuyCode.code);
			}
		}
		
	}
}
