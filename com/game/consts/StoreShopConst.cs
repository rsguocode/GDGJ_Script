//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：StoreShopConst
//文件描述：商城枚举类
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace com.game.consts
{
	public class StoreShopConst
	{
		//限时抢购最大数 = 3
		public static int LIMITTIME_3 = 3;

		//每页显示商品数量 = 9
		public static int ITEMNUMBER_9 = 9;

		//输入框0-9
		public static int INPUT_NUMBER = 10;

		//最大购买数量 = 99
		public static int BUY_MAX = 99;

		//标签类型
		public enum GoodType
		{
			Hot = 0,
			Diamond,
			BindingDiamond,
			Gold,
			Limit
		}
	}
}