using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统类型库
 * *******************************************************/
namespace Com.Game.Module.Arena
{
	public class ArenaConstVo
	{
		public readonly int CHALLENGE_TIMES_MAX = 10;  //最大挑战次数

	}
	public class ArenaVo
	{
		public uint rank;   //排名
		public uint bestRank;   //历史最高排名
		public ushort challengeRemainTimes;  //剩余挑战次数
		public ushort win;     //连胜次数
		public uint cd;  //挑战冷却时间
		public long awardDiamBind;            //奖励金钱
//		public int awardPrestige;          //奖励声望
//		public uint awardRemainTime;        //奖励领取剩余时间
		public ushort buyTimes;             //购买次数，用于梯级消费
	}

	//对手基本信息
	public class ChallengerVo
	{
		public uint roleId;        //角色ID
		public ushort rank;          //排名
		public string name;        //姓名
		public uint level;         //等级
		public uint fightNum;      //战斗值
		public byte job;           //职业
		public byte sex;           //性别
	}

	public class RankVo
	{
		public ushort rank;         //排名
		public string name;        //姓名
		public uint level;         //等级
		public uint fightNum;      //战斗值
	}

	public enum ChallengeResult : byte
	{
		Success = 0,
		Fail
	}

	public class ICChallengerVo : IComparer<ChallengerVo>
	{
		public int Compare(ChallengerVo x, ChallengerVo y)
		{
			return y.rank.CompareTo(x.rank);
		}
	}
}
