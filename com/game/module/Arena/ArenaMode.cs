using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.game.vo;
using System.IO;
using com.u3d.bases.debug;
using Proto;
using com.game;
using PCustomDataType;
using com.game.manager;
using com.game.data;
using System.Text.RegularExpressions;
using System;
using com.game.utils;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统数据类
 * *******************************************************/
using com.game.module.SystemData;


namespace Com.Game.Module.Arena
{
	public class ArenaMode : BaseMode<ArenaMode> {
		public readonly int UPDATE_ARENA = 1;
		public readonly int UPDATE_CHALLENGERLIST = 2;
		public readonly int UPDATE_ARENA_RANK = 3;
		public readonly int UPDATE_CHALLENG_TIMES = 4;
		public readonly int UPDATE_CD = 5;
		public readonly int UPDATE_MY_RANK = 6;
		public readonly int UPDATE_VSER_HP_MP = 7;
        public readonly int UPDATE_ARENA_RECORD = 8;
		public readonly int UPDATE_MY_BEST_RANK = 9;
		public readonly int UPDATE_RED_POINT = 10;


		private int _selectHeroSub;
		private ArenaVo _myArenaVo = new ArenaVo ();
		private List<ChallengerVo> _challengerList = new List<ChallengerVo> ();
		private List<RankVo> _rankList = new List<RankVo> ();
		private PlayerVo _vsPlayerAttr = new PlayerVo ();   //挑战的玩家的基本属性信息
        private List<PHistory> _history = new List<PHistory>();
		private bool _isGetNewBestRank = false;
		private bool _isGetNewRank = false;
		private bool _isBattleEnd = false;
		private uint _remainCDTime;  //用于小红点
		private bool _arenaRedPoint = false;
        
		public ArenaVo myArenaVo{ get {return _myArenaVo;} }
		public List<ChallengerVo> challengerList {get {return _challengerList;} }
		public List<RankVo> rankList {get {return _rankList;} }
		public uint myRank{get {return _myArenaVo.rank;}}

		public int selectHeroSub{get {return _selectHeroSub;} set{_selectHeroSub = value;}}
		public PlayerVo vsPlayerAttr{get {return _vsPlayerAttr;}}
		public bool IsGetNewBestRank{get {return _isGetNewBestRank;}}
		public bool IsGetNewRank{get {return _isGetNewRank;}}
		public bool ArenaRedPoint{get {return _arenaRedPoint;} 
								  set {_arenaRedPoint = value;  DataUpdate(this.UPDATE_RED_POINT);}}
		public bool IsBattleEnd{get {return _isBattleEnd;} set {_isBattleEnd = value;}}

        public struct Record {
            public int nFail;   //失败= 1，成功 = 0
            public int nRank;  //挑战后排名
            public string sTime;  //多少时间前
            public string sContent; //挑战描述
        }
        public List<Record> record = new List<Record>();

        public List<PHistory> history { get { return _history; } }
		//-------------------------------------  协议请求 -----------------------------------------//
		//客户端请求竞技场信息
		public void ApplyArenaInfo()
		{
			Log.info(this, "发送19-0给服务器请求竞技场信息");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_0 (msdata);
			AppNet.gameNet.send(msdata, 19, 0);
		}

		//客户端请求排名前十玩家信息
		public void ApplyRankInfo()
		{
			Log.info(this, "发送19-1给服务器请求竞技场排名前十玩家信息");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_1 (msdata);
			AppNet.gameNet.send(msdata, 19, 1);
		}

		//客户端请求清除挑战CD
		public void ApplyClearCD()
		{
			Log.info(this, "发送19-3给服务器清除挑战CD时间");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_3 (msdata);
			AppNet.gameNet.send(msdata, 19, 3);
		}

		//客户端请求增加挑战次数
		public void ApplyAddTimes()
		{
			Log.info(this, "发送19-4给服务器增加挑战次数");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_4 (msdata);
			AppNet.gameNet.send(msdata, 19, 4);
		}

		//客户端请求增加挑战次数
		public void ApplyArenaRecord()
		{
			Log.info(this, "发送19-8给服务器请求个人历史挑战记录");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_8 (msdata);
			AppNet.gameNet.send(msdata, 19, 8);
		}

		//客户端发送挑战结果给服务器
		public void SendChallengeResult(byte result)
		{
			if (!_isBattleEnd)
			{
				Log.info(this, "发送19-5给服务器告知挑战结果: " + result);
				MemoryStream msdata = new MemoryStream ();
				Module_19.write_19_5 (msdata, result);
				AppNet.gameNet.send(msdata, 19, 5);
			}
		}

		/// <summary>
		/// 客户端请求开始挑战
		/// </summary>
		/// <param name="challengerId">被挑战者ID.</param>
		/// <param name="challengerRank">被挑战者排名.</param>
		public void ApplyStartChallenge(uint challengerId, ushort challengerRank)
		{
			Log.info(this, "发送19-10给服务器请求开始挑战");
			MemoryStream msdata = new MemoryStream ();
			Module_19.write_19_10 (msdata, challengerId, challengerRank);
			AppNet.gameNet.send(msdata, 19, 10);
		}

		//----------------------------------------  更新数据  ----------------------------------------------//

		//

		public void UpdateMyArenaInfo(HeroesPanelMsg_19_0 heroesPanelMsg_19_0)
		{
			_myArenaVo.rank = heroesPanelMsg_19_0.pos;
			_myArenaVo.challengeRemainTimes = heroesPanelMsg_19_0.restTimes;
			_myArenaVo.cd = heroesPanelMsg_19_0.cd;
			this.ChallengeCDTime(_myArenaVo.cd);    // 只用于小红点

			_myArenaVo.win = heroesPanelMsg_19_0.win;
			_myArenaVo.bestRank = heroesPanelMsg_19_0.best;
			_myArenaVo.buyTimes = heroesPanelMsg_19_0.buyTimes;
			this.GetAwardByRank (_myArenaVo.rank, out _myArenaVo.awardDiamBind);
			Log.info(this, "更新竞技场信息");
			DataUpdate(UPDATE_ARENA);
		}

		//更新可挑战玩家列表
		public void UpdateChallengerList(List<PHeroes> newChallengerList)
		{
			Log.info(this, "清除可挑战玩家信息");
			_challengerList.Clear ();
			foreach(PHeroes hero in newChallengerList)
			{
				ChallengerVo challenger = new ChallengerVo();
				challenger.roleId = hero.userid;
				challenger.rank = hero.pos;
				challenger.name = hero.name;
				challenger.level = hero.lvl;
				challenger.fightNum = hero.fightpoint;
				challenger.job = hero.job;
				challenger.sex = hero.sex;
				_challengerList.Add(challenger);
			}

			Log.info(this, "更新可挑战玩家列表");
			_challengerList.Sort (new ICChallengerVo ());
			for (int i = 0; i < _challengerList.Count; ++i)
			{
				Log.info(this, "rank:" + _challengerList[i].rank.ToString());
				Log.info(this, "job:" + _challengerList[i].job.ToString());
			}
			DataUpdate (UPDATE_CHALLENGERLIST);
		}

		//更新竞技场排行榜信息
		public void UpdateRankInfo(List<PHeroes> top10Heroes)
		{
			_rankList.Clear ();
			foreach(PHeroes hero in top10Heroes)
			{
				RankVo top10Hero = new RankVo();
				top10Hero.rank = hero.pos;
				top10Hero.name = hero.name;
				top10Hero.level = hero.lvl;
				top10Hero.fightNum = hero.fightpoint;
//				top10Hero.diamAward = hero.
				_rankList.Add(top10Hero);
			}
			
			Log.info(this, "更新竞技场排行榜信息");
			DataUpdate (UPDATE_ARENA_RANK);
		}

		//更新CD时间
		public void UpdateCD(uint newCD)
		{
			Log.info (this, "更新挑战cd： " + newCD);
			_myArenaVo.cd = newCD;
			this.ChallengeCDTime(_myArenaVo.cd);    // 只用于小红点
			DataUpdate (UPDATE_CD);
		}

		//更新可挑战次数
		public void UpdateChallengeTimes(ushort restTimes)
		{
			Log.info (this, "更新挑战次数：" + restTimes);
			_myArenaVo.challengeRemainTimes = restTimes;
			DataUpdate (UPDATE_CHALLENG_TIMES);

		}

		//更新可挑战次数
		public void UpdateBuyTimes(ushort times)
		{
			Log.info (this, "更新已购买次数：" + times);
			_myArenaVo.buyTimes = times;
			
		}

		//更新玩家排名
		public void UpdateMyRank(ushort newRank)
		{
			Log.info (this, "更新玩家排名：" + newRank);
			_isGetNewBestRank = newRank < _myArenaVo.bestRank? true: false;
			_isGetNewRank = newRank < _myArenaVo.rank? true: false;
			_myArenaVo.rank = newRank;
			GetAwardByRank (_myArenaVo.rank, out _myArenaVo.awardDiamBind);
			DataUpdate (UPDATE_MY_RANK);
			
		}

		//更新玩家历史最高排名
		public void UpdateMyBestRank(uint newBestRank)
		{
			Log.info (this, "更新玩家历史最高排名：" + newBestRank);
			_myArenaVo.bestRank = newBestRank;
			DataUpdate (UPDATE_MY_BEST_RANK);
			
		}

		//更新被挑战者玩家信息
		public void UpdateVserAttr(List<uint>skills, PBaseAttr recRoleBaseAttr)
		{
			Log.info (this, "更新被挑战者玩家基础信息");
			ChallengerVo challengerInfo = Singleton<ArenaMode>.Instance.challengerList [Singleton<ArenaMode>.Instance.selectHeroSub];
			_vsPlayerAttr = new PlayerVo ();              //需要重新new一个，否则_SysRoleBaseInfoVo不会被置为null，在战斗场景时加载战斗对手时会出问题----add by lixi
			_vsPlayerAttr.job = challengerInfo.job;    
//			_vsPlayerAttr.Id = recRoleAttr.id;
			_vsPlayerAttr.Name = challengerInfo.name;
			_vsPlayerAttr.sex = challengerInfo.sex;
			_vsPlayerAttr.Level = (int)challengerInfo.level;
//			_vsPlayerAttr.exp = recRoleAttr.exp;
//			_vsPlayerAttr.expFull = recRoleAttr.expFull;
//			_vsPlayerAttr.vip = recRoleAttr.vip;
//			_vsPlayerAttr.nation = recRoleAttr.nation;
//			_vsPlayerAttr.vigour = recRoleAttr.vigour;
//			_vsPlayerAttr.vigourFull = recRoleAttr.vigourFull;
//			_vsPlayerAttr.hasCombine = recRoleAttr.hasCombine;
//			_vsPlayerAttr.customFace = recRoleAttr.customFace;
//			_vsPlayerAttr.titleList = recRoleAttr.titleList;
			_vsPlayerAttr.fightPoint = challengerInfo.fightNum;

//			PBaseAttr recRoleBaseAttr = recRoleAttr.attr;
			_vsPlayerAttr.Str = recRoleBaseAttr.str;
			_vsPlayerAttr.Agi = recRoleBaseAttr.agi;
			_vsPlayerAttr.Phy = recRoleBaseAttr.phy;
			_vsPlayerAttr.Wit = recRoleBaseAttr.wit;
			_vsPlayerAttr.CurHp = recRoleBaseAttr.hpCur;

			_vsPlayerAttr.Hp = recRoleBaseAttr.hpFull;
			_vsPlayerAttr.CurMp = recRoleBaseAttr.mpCur;

			_vsPlayerAttr.Mp = recRoleBaseAttr.mpFull;
			_vsPlayerAttr.AttPMin = recRoleBaseAttr.attPMin;
			_vsPlayerAttr.AttPMax = recRoleBaseAttr.attPMax;
			_vsPlayerAttr.AttMMin = recRoleBaseAttr.attMMin;
			_vsPlayerAttr.AttMMax = recRoleBaseAttr.attMMax;
			_vsPlayerAttr.DefP = recRoleBaseAttr.defP;
			_vsPlayerAttr.DefM = recRoleBaseAttr.defM;
			_vsPlayerAttr.Hit = recRoleBaseAttr.hit;
			_vsPlayerAttr.Dodge = (int)recRoleBaseAttr.dodge;
			_vsPlayerAttr.Crit = recRoleBaseAttr.crit;
			_vsPlayerAttr.CritRatio = recRoleBaseAttr.critRatio;
			_vsPlayerAttr.Flex = recRoleBaseAttr.flex;
			_vsPlayerAttr.HurtRe = recRoleBaseAttr.hurtRe;
			_vsPlayerAttr.Speed = recRoleBaseAttr.speed;
			_vsPlayerAttr.Luck = recRoleBaseAttr.luck;

			_vsPlayerAttr.skills = skills;
		}

		public void UpDatePlayerHpMp(int hp, int mp)
		{}

		//根据排名获取奖励信息
		private void GetAwardByRank(uint rank, out long diamBind)
		{
			uint result;
			if (rank > 10000)
				result = 0;
			else if (rank > 2000)
				result = 50 - (rank - 2000) / 400;
			else if (rank > 1000)
				result = 50;
			else if (rank > 100)
				result = 100 - (rank - 100) / 20;
			else if (rank > 50)
				result = 150 - (rank - 50);
			else if (rank > 10)
				result = 250 - (rank - 10) * 2;
			else if (rank > 5)
				result = 350 - (rank - 5) * 10;
			else
				result = (12 - rank) * 50;
			diamBind = result;
//			return result;
		}

		//获取前十名的钻石奖励
		public int GetDiamAwardBelong10(int rank)
		{
			int result = 0;
			if (rank > 5)
				result = 350 - (rank - 5) * 10;
			else
				result = (12 - rank) * 50;
			return result;
		}

		//根据排名获取奖励信息
		private void GetAwardByRankOld(ushort rank, out int prestige, out long gold)
		{
			int level = MeVo.instance.Level;
			if (rank > 10 && rank < 101)
			{
				prestige = 1000 - rank * 5;
				gold = 100000 + level * (1000 - rank * 5);
				return;
			}
			else if (rank > 100 && rank < 1001)
			{
				prestige = rank > 400?500 - rank:100;
				gold = 10000 + (level * (500 - rank)) > (level * 100)?(level * (500 - rank)):(level * 100);
				return;
			}

			switch (rank)
			{
				case 1:
					prestige = 10000;
					gold = 2000000 + level * 10000;
					break;
				case 2: 
					prestige = 5000;
					gold = 1500000 + level * 5000;
					break;
				case 3:
					prestige = 4000;
					gold = 1200000 + level * 4000;
					break;
				case 4:
					prestige = 3000;
					gold = 1100000 + level * 3000;
					break;
				case 5:
					prestige = 2800;
					gold = 1000000 + level * 2800;
					break;
				case 6:
					prestige = 2600;
					gold = 900000 + level * 2600;
					break;
				case 7:
					prestige = 2400;
					gold = 800000 + level * 2400;
					break;
				case 8:
					prestige = 2200;
					gold = 700000 + level * 2200;
					break;
				case 9:
					prestige = 2000;
					gold = 600000 + level * 2000;
					break;
				case 10:
					prestige = 1000;
					gold = 500000 + level * 1000;
					break;
				default:
					prestige = 0;
					gold = 0;
					break;
			}
		}

		//历史最高奖励计算方法
		public uint GetAwardByBestRank(uint hisRank, uint curRank)
		{
			uint result;
			if (hisRank > 1000)
			{
				if (curRank > 1000)
					result = (hisRank - curRank) / 10;
				else if (curRank > 100)
					result = (hisRank - 1001) / 10 + (1000 - curRank);
				else if (curRank > 10)
					result = (hisRank - 1001) / 10 + (1000 - 101) + (100 - curRank) * 10;
				else
					result = (hisRank - 1001) / 10 + (1000 - 101) + (100 - 11) * 10 + (10 - curRank) * 100;
			}
			else if (hisRank > 100)
			{
				if (curRank > 100)
					result = hisRank - curRank;
				else if (curRank > 10)
					result = (hisRank - 101) + (100 - curRank) * 10;
				else
					result = (hisRank - 101) + (100 - 11) * 10 + (10 - curRank) * 100;
			}
			else if (hisRank > 10)
			{
				if (curRank > 10)
					result = (hisRank - curRank) * 10;
				else
					result = (hisRank - 11) * 10 + (10 - curRank) * 100;
			}
			else
			{
				result = (hisRank - curRank) * 100;
			}
			
			return result > 0 ? result : 1;
		}

        //更新个人挑战记录信息
        public void UpdateArenaRecord(List<PHeroesHistory> history)
        {
            _history.Clear();
            record.Clear();
            int recIndex = 0;
            //            public int nFail;   //失败= 1，成功 = 0
            //public int nRank;  //挑战后排名
            //public string sTime;  //多少时间前
            //public string sContent; //挑战描述

            Record rec = new Record();
            DateTime t = new DateTime();
            //string Content = "";
            foreach (PHeroesHistory his in history)
            {
                if (his.type == 0)  //表示我向别人发起挑战
                {
                    rec.sContent = "你挑战了" + his.name + "，" + Result(his.result) + "当前排名：" + his.pos.ToString();
                }
                else
                {
                    rec.sContent = his.name + "挑战了你，你" + Result(his.result) + "当前排名：" + his.pos.ToString();
                }
                rec.nFail = his.result;  //战斗结果
                rec.nRank = (int)his.pos; 
                DateTime t1 = TimeUtil.GetTime(his.time.ToString());   //可能是服务器时间有问题！！！
                //Log.debug(this, "服务器时间: "+t1.ToString());
//                DateTime t2 = new DateTime(DateTime.Now.Ticks);
				DateTime t2 = TimeUtil.GetTime(ServerTime.Instance.Timestamp.ToString());  //当前服务器时间
                TimeSpan t3 = new TimeSpan(t1.Ticks);
                TimeSpan t4 = new TimeSpan(t2.Ticks);
                TimeSpan t5 = t3.Subtract(t4).Duration();
                //Log.debug(this, "本机时间: "+t2.ToString());
                rec.sTime = (t5.Days == 0) ? ((t5.Hours == 0) ? ((t5.Minutes.ToString() + "分钟")) : (t5.Hours.ToString() + "小时")) : (t5.Days.ToString() + "天");
                rec.sTime += "前";

                record.Add(rec); // 添加一条记录
                //执行下一个
                recIndex++;
            }
                DataUpdate(UPDATE_ARENA_RECORD);
        }

        private string Result(int result)
        {
            if (result == 1)
            {
                return "不幸战败！";
            }
            else
            {
                return "获得胜利！";
            }
        }

		//------------------------------------ 小红点使用---------------------------------//
		/// <summary>
		/// 进入竞技场挑战cd倒计时
		/// </summary>
		/// <param name="beginTime">Begin time.</param>
		private void ChallengeCDTime(uint cd)
		{
			this._remainCDTime = cd;
			if(cd > 0 || this._myArenaVo.challengeRemainTimes == 0)
			{
				ArenaRedPoint = false;
			}
			else
			{
				ArenaRedPoint = true;
			}
			vp_Timer.CancelAll("BeginArenaCDTimeSchedule");
			vp_Timer.In(0f, BeginArenaCDTimeSchedule, (int)_remainCDTime, 1f);
		}
		
		private void BeginArenaCDTimeSchedule()
		{
			this._remainCDTime --;

			if(_remainCDTime ==0)
			{
				if (this._myArenaVo.challengeRemainTimes > 0)
				{
					_arenaRedPoint = true;
				}
				vp_Timer.CancelAll("BeginTimeSchedule");
			}
		}
	}
}