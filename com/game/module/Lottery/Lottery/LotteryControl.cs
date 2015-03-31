//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LotteryControl
//文件描述：
//创建者：黄江军
//创建日期：2013-12-26
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using Proto;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using com.game.Public.Message;

namespace Com.Game.Module.Lottery
{
	public class LotteryControl : BaseControl<LotteryControl> 
	{		
		public LotteryControl() 
		{	
		}	

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_16_1, Fun_16_1);               //抽奖信息查询反馈
			AppNet.main.addCMD(CMD.CMD_16_2, Fun_16_2);               //抽奖反馈
			AppNet.main.addCMD(CMD.CMD_16_3, Fun_16_3);               //抽奖信息变更反馈
		}

		//抽奖信息查询反馈
		private void Fun_16_1(INetData data)
		{
			ExchangeInfoMsg_16_1 exchangeInfoMsg = new ExchangeInfoMsg_16_1();
			exchangeInfoMsg.read(data.GetMemoryStream());

			Log.info(this, "Fun_16_1");

			for (int i=0; i<exchangeInfoMsg.id.Count; i++)
			{
				uint id = exchangeInfoMsg.id[i];
				uint remain = exchangeInfoMsg.remain[i];
				Singleton<LotteryMode>.Instance.SetLotteryRemain(id, (int)remain); 

				Log.info(this, "抽奖 id：" + id + " 剩余次数：" + remain);
			}
		}

		//抽奖反馈
		private void Fun_16_2(INetData data)
		{
			ExchangeDoMsg_16_2 exchangeDoMsg = new ExchangeDoMsg_16_2();
			exchangeDoMsg.read(data.GetMemoryStream());

			if (0 != exchangeDoMsg.code)
			{
				
				ErrorCodeManager.ShowError(exchangeDoMsg.code);	
			}

			Singleton<LotteryMode>.Instance.SetCurRewardList(exchangeDoMsg.id, exchangeDoMsg.item);
			
			Log.info(this, "抽奖 id：" + exchangeDoMsg.id + " code：" + exchangeDoMsg.code);
		}

		//抽奖信息变更反馈
		private void Fun_16_3(INetData data)
		{
			ExchangeUpdateMsg_16_3 exchangeUpdateMsg = new ExchangeUpdateMsg_16_3();
			exchangeUpdateMsg.read(data.GetMemoryStream());

			uint id = exchangeUpdateMsg.id;
			uint remain = exchangeUpdateMsg.remain;
			Singleton<LotteryMode>.Instance.SetLotteryRemain(id, (int)remain); 

			Log.info(this, "抽奖 id：" + exchangeUpdateMsg.id + " 剩余次数：" + exchangeUpdateMsg.remain);
		}
	}
}
