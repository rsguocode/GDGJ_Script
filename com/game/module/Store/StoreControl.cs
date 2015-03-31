using UnityEngine;
using System.Collections;
using com.game;
using com.game.Public.Message;
using com.game.cmd;
using com.game.module.test;
using Proto;
using com.net.interfaces;
using com.u3d.bases.debug;
using PCustomDataType;
using Com.Game.Module.Waiting;

namespace com.game.module.Store
{
	public class StoreControl : BaseControl<StoreControl> 
	{
		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_15_1, Fun_15_1);				//购买非限制物品
			AppNet.main.addCMD(CMD.CMD_15_2, Fun_15_2);				//限购信息
			AppNet.main.addCMD(CMD.CMD_15_3, Fun_15_3);				//限购物品
			AppNet.main.addCMD(CMD.CMD_15_5, Fun_15_5);             //金币物品
		}

		//更新受限商品信息
		public void UpdateLimitedGoods()
		{
			Singleton<StoreMode>.Instance.GetLimitGoodsInfo ();
		}

		//购买非限制物品返回
		private void Fun_15_1(INetData data)
		{
			MallBuyGoodsMsg_15_1 buyGoodsMsg = new MallBuyGoodsMsg_15_1 ();
			buyGoodsMsg.read (data.GetMemoryStream ());
			if (buyGoodsMsg.code != 0)
			{
				ErrorCodeManager.ShowError(buyGoodsMsg.code);	
				return;
			}
			MessageManager.Show("物品购买成功");
			Singleton<StoreShopAlertView>.Instance.closeAlertAndInputView();
		}

		//限购信息返回
		private void Fun_15_2(INetData data)
		{
			MallLimitInfoMsg_15_2 limitInfoMsg = new MallLimitInfoMsg_15_2 ();
			limitInfoMsg.read (data.GetMemoryStream ());
			foreach (PLimitGoods pLimitGoods in limitInfoMsg.limit)
			{
				LimitGoods limitGoods = new LimitGoods ();
				limitGoods.pos        = pLimitGoods.pos;
				limitGoods.id         = pLimitGoods.id;
				limitGoods.price      = pLimitGoods.price;
				limitGoods.sum        = pLimitGoods.sum;
				limitGoods.count      = pLimitGoods.count;
				Singleton<StoreMode>.Instance.UpdateLimitedArr(limitGoods.pos - 1, limitGoods);
			}
			Singleton<StoreMode>.Instance.UpdateLimited ((int)limitInfoMsg.remainTime);
		}

		//购买限购物品返回
		private void Fun_15_3(INetData data)
		{
			MallBuyLimitGoodsMsg_15_3 buyLimitGoodsMsg = new MallBuyLimitGoodsMsg_15_3 ();
			buyLimitGoodsMsg.read (data.GetMemoryStream ());
			if (buyLimitGoodsMsg.code != 0)
			{
				ErrorCodeManager.ShowError(buyLimitGoodsMsg.code);	
				return;
			}
			MessageManager.Show("限购物品购买成功");
			Singleton<StoreShopAlertView>.Instance.closeAlertAndInputView();
		}

		//金币物品信息返回
		private void Fun_15_5(INetData data)
		{
			MallDiamInfoMsg_15_5 goldInfoMsg = new MallDiamInfoMsg_15_5();
			goldInfoMsg.read (data.GetMemoryStream());
			foreach (PDiamGoods pGoldGoods in goldInfoMsg.goods)
			{
				GoldGoods goldGoods = new GoldGoods();
				goldGoods.id        = pGoldGoods.id;
				goldGoods.remain    = pGoldGoods.remain;
				Singleton<StoreMode>.Instance.UpdateGoldGoodsArr(goldGoods);
			}
		}
	}
}