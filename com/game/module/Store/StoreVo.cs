using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.data;
using com.game.module.test;

namespace com.game.module.Store
{
	public class StoreConstVo
	{

	}

	//选中物品类
	public class SelectedGoods
	{
		public uint id;      //商品ID
		public uint num;     //商品数量
		public ushort type;    //商品分类
		public ushort subType; //商品子分类
	}

	//限时抢购类物品
	public class LimitGoods
	{
		public uint id    = 0;       //商品ID
		public uint sum   = 0;       //数量
		public byte pos   = 0;       //位置
		public uint price = 0;       //价格
		public uint count = 0;       //可购买数量
	}

	//金币物品
	public class GoldGoods
	{
		public uint id     = 0;        //商品ID
		public uint remain = 0;        //剩余购买数量
	}

	public class ICMallVo : IComparer<SysVipMallVo>
	{
		public int Compare(SysVipMallVo x, SysVipMallVo y)
		{
			return x.queue.CompareTo(y.queue);
		}
	}
}