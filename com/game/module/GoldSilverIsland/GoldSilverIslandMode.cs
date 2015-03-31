//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldSilverIslandMode
//文件描述：
//创建者：黄江军
//创建日期：2014-02-19
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using com.game.module.test;
using Proto;
using com.game;
using PCustomDataType;
using com.game.vo;

namespace Com.Game.Module.GoldSilverIsland
{
	public enum AssistReply
	{
		Accept = 0,
		Deny = 1
	}

	public enum RobResult
	{
		Success = 0,
		Failed = 1
	}

	public class RobbedPlayer
	{
		public uint Id;
		public string Name;
		public PlayerVo Vo = new PlayerVo();
	}

	public class GoldSilverIslandMode : BaseMode<GoldSilverIslandMode>
    {
        public readonly byte UPDATE_ROB_REMAIN = 1;
        public readonly byte UPDATE_ASSIST_REMAIN = 2;
        public readonly byte UPDATE_ADVENT_REMAIN = 3;
		public readonly byte UPDATE_PLAYER_LIST = 4;
		public readonly byte UPDATE_ROB_CDTIME = 5;
		public readonly byte UPDATE_GRADE_REFRESH = 6;
		public readonly byte UPDATE_ADVENT_BLAST = 7;
		public readonly byte UPDATE_ADVENT_FINISHED = 8;
		public readonly byte UPDATE_GRADE_ERROR = 9;
		public readonly byte UPDATE_FRIEND_LIST = 10;
		public readonly byte UPDATE_FRIEND_REPLY = 11;
		public readonly byte UPDATE_INVITE_REQUEST = 12;
		public readonly byte UPDATE_PLAYER_DEL = 13;
		public readonly byte UPDATE_ASSIST_INFO = 14;
		public readonly byte UPDATE_PLAYER_ROB_TIMES = 15;
		public readonly byte UPDATE_START_ROB = 16;
		public readonly byte UPDATE_PLAYER_ROB_HPMP = 17;
		public readonly byte UPDATE_ROB_REWARD = 18;
		public readonly byte UPDATE_FRIEND_ASSIST_REMAIN = 19;
		public readonly byte UPDATE_FRIEND_REPLY_FINISHED = 20;
		public readonly byte UPDATE_ROBBED_PLAYER_PREREWARD = 21;
		public readonly byte UPDATE_TIPS = 22;

        private byte robRemainTimes;
        private byte assistRemainTimes;
        private byte adventRemainTimes;
		private ushort robCDTime;
		private byte friendReply = 1;
		private List<PWoodsPlayer> playerList = new List<PWoodsPlayer>();
		private List<PWoodsFriendInfo> friendList = new List<PWoodsFriendInfo>();
		private uint assistId;
		private RobbedPlayer robbedPlayer = new RobbedPlayer();

		private bool canShowTips = false;
		private uint adventRemainTime;

		public override bool ShowTips
		{
			get {return canShowTips && (adventRemainTimes>0) && !AdventInCD();}
		}

		private bool AdventInCD()
		{
			PWoodsPlayer me = GetPlayer(MeVo.instance.Id);
			if (null != me)
			{
				return (me.remainTime>0);
			}

			return false;
		}

		private void AdventRemainTimeCountDown()
		{
			PWoodsPlayer me = GetPlayer(MeVo.instance.Id);
			if (null != me && me.remainTime>0)
			{
				vp_Timer.CancelAll("AdventCountDownCallback");
				vp_Timer.In(me.remainTime, AdventCountDownCallback, 1, 0);
			}
		}

		private void AdventCountDownCallback()
		{
			if (canShowTips)
			{
				GetAdventureRemainCount();
			}
		}

		public void StartTips()
		{
			canShowTips = true;
			GetPlayersInfo();
			GetAdventureRemainCount();
		}

		public void StopTips()
		{
			canShowTips = false;
			DataUpdate(UPDATE_TIPS);
		}

		private void NotifyTips()
		{
			if (canShowTips)
			{
				DataUpdate(UPDATE_TIPS);
			}
		}

		public uint RobbedPlayerPreRep
		{
			get;
			set;
		}

		public uint RobbedPlayerPreGold
		{
			get;
			set;
		}

		public uint SelectAssistId
		{
			get;
			set;
		}

		public uint DelPlayerId
		{
			get;
			set;
		}

		public RobbedPlayer RobbedPlayer
		{
			get
			{
				return robbedPlayer;
			}
		}

		public uint RobRewardGold
		{
			get;
			set;
		}

		public uint RobRewardRep
		{
			get;
			set;
		}

		public void SetRobbedPlayerPreReward(uint gold, uint rep)
		{
			this.RobbedPlayerPreGold = gold;
			this.RobbedPlayerPreRep = rep;
			DataUpdate(UPDATE_ROBBED_PLAYER_PREREWARD);
		}

		public void SetRobReward(uint gold, uint rep)
		{
			this.RobRewardGold = gold;
			this.RobRewardRep = rep;
			DataUpdate(UPDATE_ROB_REWARD);
		}

		public void SetRobbedPlayerInfo(uint id, string name, byte job, byte lvl, PBaseAttr attr, List<uint> skills)
		{
			robbedPlayer.Id = id;
			robbedPlayer.Name = name;
			SetRobberPlayerVo(robbedPlayer.Vo, attr);
			robbedPlayer.Vo.Id = id;
			robbedPlayer.Vo.Name = name;
			robbedPlayer.Vo.job = job;
			robbedPlayer.Vo.Level = lvl;
			robbedPlayer.Vo.skills = skills;

			DataUpdate(UPDATE_START_ROB);
		}

		private void SetRobberPlayerVo(PlayerVo robberPlayerVo, PBaseAttr recRoleBaseAttr)
		{
			robberPlayerVo.Str = recRoleBaseAttr.str;
			robberPlayerVo.Agi = recRoleBaseAttr.agi;
			robberPlayerVo.Phy = recRoleBaseAttr.phy;
			robberPlayerVo.Wit = recRoleBaseAttr.wit;
			robberPlayerVo.CurHp = recRoleBaseAttr.hpCur;
			
			robberPlayerVo.Hp = recRoleBaseAttr.hpFull;
			robberPlayerVo.CurMp = recRoleBaseAttr.mpCur;
			
			robberPlayerVo.Mp = recRoleBaseAttr.mpFull;
			robberPlayerVo.AttPMin = recRoleBaseAttr.attPMin;
			robberPlayerVo.AttPMax = recRoleBaseAttr.attPMax;
			robberPlayerVo.AttMMin = recRoleBaseAttr.attMMin;
			robberPlayerVo.AttMMax = recRoleBaseAttr.attMMax;
			robberPlayerVo.DefP = recRoleBaseAttr.defP;
			robberPlayerVo.DefM = recRoleBaseAttr.defM;
			robberPlayerVo.Hit = recRoleBaseAttr.hit;
			robberPlayerVo.Dodge = (int)recRoleBaseAttr.dodge;
			robberPlayerVo.Crit = recRoleBaseAttr.crit;
			robberPlayerVo.CritRatio = recRoleBaseAttr.critRatio;
			robberPlayerVo.Flex = recRoleBaseAttr.flex;
			robberPlayerVo.HurtRe = recRoleBaseAttr.hurtRe;
			robberPlayerVo.Speed = recRoleBaseAttr.speed;
			robberPlayerVo.Luck = recRoleBaseAttr.luck;
		}

		public void SetBlast()
		{
			DataUpdate(UPDATE_ADVENT_BLAST);
		}

		public void SetGradeRefreshError()
		{
			DataUpdate(UPDATE_GRADE_ERROR);
		}

		public uint AssistId
		{
			get
			{
				return assistId;
			}

			set
			{
				assistId = value;
				DataUpdate(UPDATE_ASSIST_INFO);
			}
		}

		public byte FriendReply
		{
			get
			{
				return friendReply;
			}
			
			set
			{
				friendReply = value;
				DataUpdate(UPDATE_FRIEND_REPLY);
			}
		}

		public ushort RobCDTime
		{
			get
			{
				return robCDTime;
			}
			
			set
			{
				robCDTime = value;
				DataUpdate(UPDATE_ROB_CDTIME);
			}
		}

		public List<PWoodsFriendInfo> FriendList
		{
			get
			{
				return friendList;
			}
			
			set
			{
				friendList = value;
				DataUpdate(UPDATE_FRIEND_LIST);
			}
		}

		public PWoodsFriendInfo GetAssist(uint assistId)
		{
			foreach (PWoodsFriendInfo item in friendList)
			{
				if (assistId == item.id)
				{
					return item;
				}
			}
			
			return null;
		}

		public void UpdateFriendAssistRemain(PWoodsFriendInfo assist)
		{
			foreach (PWoodsFriendInfo item in friendList)
			{
				if (assist.id == item.id)
				{
					item.remainTimes = assist.remainTimes;
					item.fightPoint = assist.fightPoint;
					DataUpdate(UPDATE_FRIEND_ASSIST_REMAIN);
				}
			}
		}

		public List<PWoodsPlayer> PlayerList
		{
			get
			{
				return playerList;
			}
			
			set
			{
				playerList = value;
				DataUpdate(UPDATE_PLAYER_LIST);
				NotifyTips();
				AdventRemainTimeCountDown();
			}
		}
		
		public PWoodsPlayer GetPlayer(uint playerId)
		{
			foreach (PWoodsPlayer item in playerList)
            {
                if (playerId == item.id)
                {
                    return item;
                }
            }

            return null;
        }

		public void DelPlayer(uint playerId)
		{
			foreach (PWoodsPlayer item in playerList)
			{
				if (playerId == item.id)
				{
					DelPlayerId = item.id;
					playerList.Remove(item);
					DataUpdate(UPDATE_PLAYER_DEL);
					return;
				}
			}
		}

		public void SetPlayerRobbedTimes(uint playerId, byte robTimes)
		{
			PWoodsPlayer palyer = GetPlayer(playerId);

			if (null != palyer)
			{
				palyer.robTimes = robTimes;
				DataUpdate(UPDATE_PLAYER_ROB_TIMES);
			}
		}

		public void AddPlayers(List<PWoodsPlayer> playerList)
		{
			foreach (PWoodsPlayer item in playerList)
			{
				this.playerList.Add(item);
			}

			DataUpdate(UPDATE_PLAYER_LIST);
		}
		
		public int GetPlayerIndex(PWoodsPlayer player) 
		{
			if (null != playerList)
			{
				return playerList.IndexOf(player);
			}
			else
			{
				return -1;
			}
		}

        public byte RobRemainTimes
        {
            get
            {
                return robRemainTimes;
            }

            set
            {
                robRemainTimes = value;
                DataUpdate(UPDATE_ROB_REMAIN);
            }
        }

        public byte AssistRemainTimes
        {
            get
            {
                return assistRemainTimes;
            }

            set
            {
                assistRemainTimes = value;
                DataUpdate(UPDATE_ASSIST_REMAIN);
            }
        }

        public byte AdventRemainTimes
        {
            get
            {
                return adventRemainTimes;
            }

            set
            {
                adventRemainTimes = value;
                DataUpdate(UPDATE_ADVENT_REMAIN);
				NotifyTips();
            }
        }

		public byte Grade
		{
			get;
			set;
		}

		public byte RefreshTimes
		{
			get;
			set;
		}

		public uint RepuGets
		{
			get;
			set;
		}

		public uint GoldGets
		{
			get;
			set;
		}

		public uint InviterId
		{
			get;
			set;
		}

		public string InviterName
		{
			get;
			set;
		}

		public byte InviterGrade
		{
			get;
			set;
		}

		public void SetAdventFinished(uint repu, uint gold)
		{
			this.RepuGets = repu;
			this.GoldGets = gold;
			DataUpdate(UPDATE_ADVENT_FINISHED);
		}

		public void SetGradeAndRefreshTimes(byte grade, byte refreshTimes)
		{
			this.Grade = grade;
			this.RefreshTimes = refreshTimes;
			DataUpdate(UPDATE_GRADE_REFRESH);
		}

		public void SetInviteInfo(uint inviterId, string inviterName, byte inviterGrade)
		{
			this.InviterId = inviterId;
			this.InviterName = inviterName;
			this.InviterGrade = inviterGrade;
			DataUpdate(UPDATE_INVITE_REQUEST);
		}

        //获得冒险玩家信息请求
        public void GetPlayersInfo()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_0(msdata);
            AppNet.gameNet.send(msdata, 26, 0);
        }

        //获得剩余打劫次数
        public void GetRobRemainCount()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_1(msdata);
            AppNet.gameNet.send(msdata, 26, 1);
        }

        //获得剩余协助次数
        public void GetAssistRemainCount()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_2(msdata);
            AppNet.gameNet.send(msdata, 26, 2);
        }

        //获得剩余冒险次数
        public void GetAdventureRemainCount()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_3(msdata);
            AppNet.gameNet.send(msdata, 26, 3);
        }

        //获得打劫cd
        public void GetRobCD()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_4(msdata);
            AppNet.gameNet.send(msdata, 26, 4);
        }

        //清除打劫cd
        public void ClearRobCD()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_5(msdata);
            AppNet.gameNet.send(msdata, 26, 5);
        }

        //获得当前品质和刷新次数
        public void GetRefreshCount()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_6(msdata);
            AppNet.gameNet.send(msdata, 26, 6);
        }

        //获得当前的协助玩家
        public void GetCurAssistInfo()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_7(msdata);
            AppNet.gameNet.send(msdata, 26, 7);
        }


        //邀请协助者
        public void InviteAssist(uint friendId)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_11(msdata, friendId);
            AppNet.gameNet.send(msdata, 26, 11);
        }

        //推送邀请应答
        public void ReplyInvite(byte result, uint id)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_12(msdata, result, id);
            AppNet.gameNet.send(msdata, 26, 12);
        }

        //随机品质
        public void RandomGrade(byte type)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_13(msdata, type);
            AppNet.gameNet.send(msdata, 26, 13);
        }

        //开始冒险
        public void StartAdventure()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_14(msdata);
            AppNet.gameNet.send(msdata, 26, 14);
        }

        //抢劫
        public void StartRob(uint id)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_15(msdata, id);
            AppNet.gameNet.send(msdata, 26, 15);
        }

        //疾风术
        public void Blast()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_16(msdata);
            AppNet.gameNet.send(msdata, 26, 16);
        }

        //好友剩余协助次数列表
        public void GetFriendAssitRemainCountList()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_18(msdata);
            AppNet.gameNet.send(msdata, 26, 18);
        }        

        //打劫结果
        public void RobResult(byte result, uint derid)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_20(msdata, result, derid);
            AppNet.gameNet.send(msdata, 26, 20);
        }

        //完成冒险
        public void FinishAdventure()
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_21(msdata);
            AppNet.gameNet.send(msdata, 26, 21);
        }

        //图标打开状态
        public void IconOpen(byte status)
        {
            MemoryStream msdata = new MemoryStream();
            Module_26.write_26_24(msdata, status);
            AppNet.gameNet.send(msdata, 26, 24);
        }

		//打劫预览
		public void RobRewardPreview(uint playerId)
		{
			MemoryStream msdata = new MemoryStream();
			Module_26.write_26_26(msdata, playerId);
			AppNet.gameNet.send(msdata, 26, 26);
		}
		
	}
}
