using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  副本系统类库
 * *******************************************************/
namespace Com.Game.Module.Copy
{
	public class CopyConstVo
	{
		public const int WORLD_POINT_NAME_COVER_ICON = 0;  //世界点名字位置偏移图片最底部的向上偏移量
		public const int GET_REWARD_NEED_STAR_NUM = 3;      //获取宝箱奖励需要的星星数
	}

	public class CopyVo  {
		public uint mapId;      //mapId
		public uint unikey;     //ID即是唯一ID
		public string subIdlist;     //下辖ID列表
		public int parentId;    //父ID
		public string name;    //名字
		public int x;         //X坐标
		public int y;         //Y坐标
		public string icon;  //图标
		public string anim;  //怪物动画
		public string remark;  //备注

		public int lvl;    //要求等级
		public int vigour; //进入地图消耗体力值
		public int goodsId1;   //物品1ID
		public int goodsId2;   //物品1ID
		public int goodsId3;   //物品1ID
		
	}

	public class CopyMapInfo
	{
		public uint curGroupId;
		public uint curWorldId;
		public uint curSubworldId;
		public Dictionary<uint, List<uint>> hasBoxDic = new Dictionary<uint, List<uint>> ();
	}

	public class WorldMapInfo
	{
//		public List<string> worldmapList = new List<string> ();
		public List<CopyVo> WorldPointInfoList = new List<CopyVo>();
	}
//
//	public class SubworldMapInfo
//	{
//		public uint curSubworldId;
//		public Dictionary<uint, List<uint>> hasBoxDic = new Dictionary<uint, List<uint>> ();
//	}

	public class SubworldPointVo
	{
		public uint subworldId;
		public int x;         //X坐标
		public int y;         //Y坐标
		public bool hasBox;
		public bool unLock;
		public string name;

	}

	public class CopyGroupPointVo
	{
		public uint MapId;
		public string icon; 
		public int needVigour;       
		public bool hasBox;
		public bool unLock;
		public List<uint> rewardGoodsIdList = new List<uint>();
		
	}

	public class SelectedWorldInfo
	{
		public uint worldId;
		public string name;
	}

	public class SelectedSubworldInfo
	{
		public uint subworldId;
		public bool isFirst;     //是否第一次进入该分场景点
		public bool isPassed;    //选中分场景点的主线副本是否通关
	}

	public class StarRewardVo
	{
		public uint groupId;
		public uint mapId;
		public ushort grade;
		public bool IsGetReward;    //是否已经领取奖励
	}

//	public enum CopyType
//	{
//		Main = 1,
//		Branch,
//		Pat
//	}

	public class SelectedCopyPoint
	{
		public uint mapId;
		public int copyType;
		public bool isFirst;
	}

	public enum CopyFailReason {
		TIME_OVER,
		DEATH
	}


	//----------------------------------------
	public class CopyPointVo
	{
		public uint id;
		public string icon;
		public string name;

	}

	//------------------------------------new -------------------------------------------
	public enum CopyType : byte
	{
		Common = 0,
		Hard,
		Death
	}

}
