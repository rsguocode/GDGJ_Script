using UnityEngine;
using System.Collections;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/04 09:39:52 
 * function:  家园种植系统类型库
 * *******************************************************/
using System.Collections.Generic;
using com.game.data;


namespace Com.Game.Module.Farm
{
//	public class FarmBaseInfo
//	{
//
//
//	}

	public class FarmInfo
	{
		public uint id;       //农场主ID
		public uint level;    //农场等级
		public uint exp;      //农场经验
		public uint expful;      //农场升级所需经验
//		public List<LandVo> landInfo = new List<LandVo>();
		public Dictionary<byte, LandVo> landInfo = new Dictionary<byte, LandVo> ();
	}

	public class FriendFarmBaseInfo
	{
		public uint id;       //好友ID
		public string name;   //角色名
		public uint level;    //农场等级
		public uint exp;      //农场经验
		public uint expful;      //农场升级所需经验
		public byte status;   //各种状态 二进制表示 第一位可浇水 第二位可除草 第三位可除虫 第四位可收获（偷取）
	}

	public class ICFriendFarmBaseInfo : IComparer<FriendFarmBaseInfo>
	{
		public int Compare(FriendFarmBaseInfo x, FriendFarmBaseInfo y)
		{
			return y.level.CompareTo(x.level);
		}
	}


	public class LandVo
	{
		public byte pos;         //土地编号
		public byte color;       //颜色
		public uint seedId;      //种子ID
		public uint remainTime;  //收获剩余时间
		public uint num;         //数量
		public bool canSteal;    //是否可偷
		public byte status;      //田地状态
	}

	public enum LandStatu : byte
	{
		NORMAL = 0,
		KILL_WORM,
		CUT_GRASS,
		WATER
	}

	public enum OpeType : byte
	{
		GET = 0,
		KILL_WORM,
		CUT_GRASS,
		WATER
	}

	public enum SeedStoreType : byte
	{
		GOLD = 1,
		DIAM,
		HUODONG
	}

	public class FarmLog
	{
		public uint time;      //操作时间
		public uint id;        //操作ID
		public string name;    //角色名
		public byte type;      //操作类型 1：操作种植物 2 种植园升级
		public uint goodsId;   //物品ID 1：经验 2：金币
		public uint num;       //数量
	}

	public class MySeedVo
	{
		public uint id;
		public uint num;
	}

	public class ICSeedMallVo : IComparer<SysSeedMallVo>
	{
		public int Compare(SysSeedMallVo x, SysSeedMallVo y)
		{
			return x.queue.CompareTo(y.queue);
		}
	}
}
