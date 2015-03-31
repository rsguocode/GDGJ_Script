using System.IO;
using UnityEngine;
using System.Collections;
using Proto;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.vo;
using System.Collections.Generic;


namespace Com.Game.Module.Copy
{
	public class AwardMode : BaseMode<AwardMode> {
		public readonly int UPDATE_AWARD = 1;
		private AwardVo _award = new AwardVo ();

		public AwardVo award { get{return _award;} }

		//更新获取到的奖励数据
		public void UpdateAwardData(DungeonRewardMsg_8_5 recDungeonReward)
		{
			this._award.attackAchievement = recDungeonReward.attack > 0 ? true : false;
			this._award.hpAchievement = recDungeonReward.hp > 0 ? true : false;
			this._award.timeAchievement = recDungeonReward.time > 0 ? true : false;
			this._award.siliverReward = recDungeonReward.gold;
			this._award.expReward = recDungeonReward.exp;
			this._award.goodsRewardList = recDungeonReward.box;
			this._award.awardIdType = recDungeonReward.idType;
			this._award.isFirstPass = recDungeonReward.isFirstFinish == 1?true:false;
//			DataUpdate (this.UPDATE_AWARD);
		}
	}

	public class AwardVo
	{
		public bool attackAchievement;   //连斩达成
		public bool hpAchievement;       //生命达成
		public bool timeAchievement;     //时间达成
		public uint expReward;           //经验奖励
		public uint siliverReward;       //金币奖励
		public List<uint> goodsRewardList;  //获取物品列表
		public bool isFirstPass;            //是否首次通关
		public byte awardIdType;
	}
}
