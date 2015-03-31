using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using Proto;


namespace com.game.module.Store
{
	public class StoreMode : BaseMode<StoreMode>
	{
		public readonly int UPDATE_LIMITED   = 1;
		public readonly int UPDATE_GOODS     = 2;
		public readonly int UPDATE_GOLDGOODS = 3;

		private int _limitedRemainTime;  //限购时间
		private List<SysVipMallVo> _goodsList     = new List<SysVipMallVo> ();   //非限购商品信息列表（热卖、钻石、绑钻）
		private List<SysVipMallVo> _goldGoodsList = new List<SysVipMallVo>();    //非限购商品信息列表（金币）
		private SysVipMallVo[] _limitedGoodsArr   = new SysVipMallVo[StoreShopConst.LIMITTIME_3]; //限购商品信息列表
		private LimitGoods[] _limitInfoArr        = new LimitGoods[StoreShopConst.LIMITTIME_3];  //限购信息列表

		public int limitedRemainTime
		{
			get {return _limitedRemainTime;}
		}

		public List<SysVipMallVo> goodsList
		{
			get {return _goodsList;}
		}

		public List<SysVipMallVo> goldGoodsList
		{
			get {return _goldGoodsList;}
		}

		public SysVipMallVo[] limitedGoodsArr
		{
			get {return _limitedGoodsArr;}
		}
	
		public LimitGoods[] limitInfoArr
		{
			get {return _limitInfoArr;}
		}

		//根据商品类别，返回商品信息列表
		public void UpdateGoodsInfoListByType(int type)
		{
			if(type == (int)StoreShopConst.GoodType.Gold)
			{
				_goldGoodsList = BaseDataMgr.instance.GetSysVipMallVoListByType (type);
				_goldGoodsList.Sort(new ICMallVo());
			}else
			{
				_goodsList = BaseDataMgr.instance.GetSysVipMallVoListByType (type);
				_goodsList.Sort (new ICMallVo ());//对GoodsList根据queue值进行排序
				DataUpdate (UPDATE_GOODS);
			}
		}

		//更新限购相关数据列表
		public void UpdateLimitedArr(int pos, LimitGoods limitedGoods)
		{
			limitInfoArr [pos] = limitedGoods;
			limitedGoodsArr [pos] = BaseDataMgr.instance.GetSysVipMallVo (limitedGoods.id, (uint)StoreShopConst.GoodType.Limit);
			limitedGoodsArr [pos].curr_price = (int)limitedGoods.price;
		}

		//更新金币相关数据列表
		public void UpdateGoldGoodsArr(GoldGoods goldGoods)
		{
			foreach(SysVipMallVo mallVo in _goldGoodsList)
			{
				if(mallVo.id == goldGoods.id)
				{
					mallVo.buy_max = (int)goldGoods.remain;
				}
			}
			DataUpdate(UPDATE_GOLDGOODS);
		}

		//以下为协议相关处理数据-----------------------------------------------------------------//
		//购买非限制物品
		public void BuyNormalGoods(SelectedGoods goods)
		{
			MemoryStream msdata = new MemoryStream ();
			Module_15.write_15_1 (msdata, goods.id, goods.num, goods.type, goods.subType);
			AppNet.gameNet.send(msdata, 15, 1);
		}

		//获取限购信息
		public void GetLimitGoodsInfo()
		{
			MemoryStream msdata = new MemoryStream ();
			Module_15.write_15_2 (msdata);
			AppNet.gameNet.send(msdata, 15, 2);
		}

		//购买限购物品
		public void BuyLimitGoods(SelectedGoods goods)
		{
			MemoryStream msdata = new MemoryStream ();
			Module_15.write_15_3 (msdata, goods.id, goods.num);
			AppNet.gameNet.send(msdata, 15, 3);
		}

		//获取金币物品信息
		public void GoldGoodsInfo()
		{
			MemoryStream msdata = new MemoryStream();
			Module_15.write_15_5 (msdata);
			AppNet.gameNet.send(msdata , 15 , 5);
		}

		//数据更新事件相关------------------------------------------------------------//
		//更新限购信息
		public void UpdateLimited(int remainTime)
		{
			_limitedRemainTime = remainTime;
			DataUpdate(UPDATE_LIMITED);
		}
	}
}