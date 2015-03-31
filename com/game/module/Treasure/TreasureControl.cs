using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using com.game.Public.Message;
using Com.Game.Module.Tips;
using System.Collections.Generic;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/02/25 09:39:52 
 * function:  地宫寻宝系统控制类
 * *******************************************************/
namespace Com.Game.Module.Treasure
{
	public class TreasureControl : BaseControl<TreasureControl> 
	{
		protected override void NetListener ()
		{
			AppNet.main.addCMD(CMD.CMD_27_0, Fun_27_0);				//获取寻宝信息
			AppNet.main.addCMD(CMD.CMD_27_1, Fun_27_1);				//寻宝
			AppNet.main.addCMD(CMD.CMD_27_2, Fun_27_2);				//刷新宝藏点
		}
	
		private void Fun_27_0(INetData data)
		{
			Log.info(this, "-Fun_27_0服务器返回寻宝信息");
			TreasInfoMsg_27_0 treasInfoMsg_27_0 = new TreasInfoMsg_27_0 ();
			treasInfoMsg_27_0.read (data.GetMemoryStream ());

			Singleton<TreasureMode>.Instance.UpdateTreasureInfo (treasInfoMsg_27_0);
		}

		private void Fun_27_1(INetData data)
		{
			Log.info(this, "-Fun_27_1服务器返回寻宝结果");
			TreasTakeMsg_27_1 treasTakeMsg_27_1 = new TreasTakeMsg_27_1 ();
			treasTakeMsg_27_1.read (data.GetMemoryStream ());

			if (treasTakeMsg_27_1.code == 0)
			{
//				Log.info(this, "-Fun_27_1() 寻宝成功，获得道具：" + treasTakeMsg_27_1.goodsId + " 数量：" + treasTakeMsg_27_1.count);
				Singleton<TreasureMode>.Instance.GetNewTreasure(treasTakeMsg_27_1.goodsId, treasTakeMsg_27_1.goodsIdFinal);
			}
			else
			{
				ErrorCodeManager.ShowError(treasTakeMsg_27_1.code);	
				return;
			}
		}

		private void Fun_27_2(INetData data)
		{
			Log.info(this, "-Fun_27_2刷新宝藏请求返回");
			TreasRefreshMsg_27_2 treasRefreshMsg_27_2 = new TreasRefreshMsg_27_2 ();
			treasRefreshMsg_27_2.read (data.GetMemoryStream ());

			if (treasRefreshMsg_27_2.code == 0)
			{
				Log.info(this, "-Fun_27_2() 刷新宝藏信息成功");
			}
			else
			{
				ErrorCodeManager.ShowError(treasRefreshMsg_27_2.code);	
				return;
			}
		}
	}
}
