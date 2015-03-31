using System.Reflection.Emit;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.SystemData;
using com.game.vo;
using com.u3d.bases.debug;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.IO;
using Proto;
using com.game;
using System.Collections.Generic;
using com.game.Public.Confirm;

namespace Com.Game.Module.Boss
{
	public class BossMode : BaseMode<BossMode> {

		public readonly int UPDATE_BOSS_HP = 1;//boss血量
		public readonly int UPDATE_HURT_RANK = 2;//伤害排行
		public readonly int UPDATE_AWARD_TOTAL = 3;//累计奖励
		public readonly int UPDATE_START = 4;//开启通知
		public readonly int UPDATE_BOSS_DIE = 6;//boss死亡
		public readonly int UPDATE_END = 7;//结束通知
		public readonly int UPDATE_GUWU_INFO = 8;//玩家鼓舞情况登陆时下发
		public readonly int UPDATE_FIGHT_INFO = 9;//战况，玩家登陆是下发
		public readonly int UPDATE_BOSS_FIGHT= 10;//Boss攻击
		public readonly int UPDATE_ATTACK = 11;//角色攻击
		public readonly int UPDATE_BEGIN_TIME = 12;//进入倒计时
	    public readonly int UPDATE_TIPS = 13;//tips提示
		//  22-3 伤害排行榜信息
		public WorldBossRankMsg_22_3 hurtRank = new WorldBossRankMsg_22_3();//伤害排行

	    public const int GuwuMaxTime = 10;//鼓舞最大次数
	    public const int AttackGuwuId = 1;//攻击鼓舞Id
	    public const int CritGuwuId = 2;//暴击鼓舞Id

	    public const int AttackGuWuCostId = 1015;//攻击鼓舞费用Id
	    public const int CritGuWuCostId = 1016;//暴击鼓舞费用Id

	    private SysPriceVo[] guWuCosts;

	    public SysPriceVo[] GuWuCostList
	    {
	        get
	        {
	            if (guWuCosts == null)
	            {
	                guWuCosts = new SysPriceVo[2];
	                guWuCosts[0] = BaseDataMgr.instance.GetDataById<SysPriceVo>((uint) AttackGuWuCostId);
                    guWuCosts[1] = BaseDataMgr.instance.GetDataById<SysPriceVo>((uint)CritGuWuCostId);
	            }
	            return guWuCosts;
	        }
	    }
        /// <summary>
        /// 攻击鼓舞的加成比例
        /// </summary>
	    public float AttackGuwuRate
	    {
	        get { return AttackTime/10f; }
	    }
        /// <summary>
        /// 暴击鼓舞的概率
        /// </summary>
        public float CritGuwuRate
        {
            get { return CritTime / 10f; }
        }

        public uint AttackTime;//攻击鼓舞次数
        public uint CritTime;//暴击鼓舞次数
		// 22-5
		public uint winSiler;//累计金币奖励
		public uint winExtSlier;//额外奖励
		public uint winRepu;//累计声望奖励
		public uint winExtRepu;//累计声望奖励
		// 22-6
		public uint currHp;//当前血量
		public uint fullHp;//总血量

	    private long mEndTimeStamp;//世界Boss剩余时间倒计时间戳
        /// <summary>
        /// 世界Boss剩余时间（单位秒）
        /// </summary>
        public long EndTimeLeft 
        {
            get
            {
                long leftTime = mEndTimeStamp - ServerTime.Instance.Timestamp;  //防止TimeStamp 出现异常
                if (leftTime > 0)
                    return leftTime;
                return 0;
            }
        }

	    private long mRestTimeStamp;//世界Boss开启倒计时

	    public long RestTimeLeft
	    {
            get
            {
                long leftTime = mRestTimeStamp - ServerTime.Instance.Timestamp;  //防止TimeStamp 出现异常
                if (leftTime > 0)
                    return leftTime;
                return 0;
            }
	    }
        
		public bool isDie;//Boss是否死亡

		public uint hurt = 0;//角色伤害值

	    public bool IsOpenRank = false;  //世界Boss 是否弹出奖励界面

	    public string BossName;//世界Boss名字，在mapMode 中创建 世界Boss 时 获得
	    private int bossIcon;//Boss头像图标资源Id
        public int BoosIcon//世界Boss 图标
	    {
	        get{return bossIcon;}
            set
            {
                bossIcon = value; 
                DataUpdate(UPDATE_BOSS_HP);
            }
	    }
        /// <summary>
        /// 是否显示小红点
        /// </summary>
        public override bool ShowTips
        {
            get
            {
                return isShowTips;
            }
        }

	    private bool isShowTips = false;
	    
		/// <summary>
		/// 世界Boss开启
		/// </summary>
		/// <param name="restTime">Rest time.</param>
		public void BossStart(uint restTime)
		{
			this.mRestTimeStamp = restTime + ServerTime.Instance.Timestamp;
		    this.isDie = false;
		    if (restTime == 0)
		    {
		        isShowTips = true; 
		    }
            else
            {
                isShowTips = false;
            }
            Log.info(this,"开启剩余时间： " + restTime);
            vp_Timer.CancelAll("RestTimeSchedule");
            vp_Timer.In(0f, RestTimeSchedule, (int)restTime, 1f);  //延时1秒
            DataUpdate(this.UPDATE_TIPS);
			DataUpdate(this.UPDATE_START);
		}

		private void RestTimeSchedule()
		{
            DataUpdate(this.UPDATE_START);
		    if (this.RestTimeLeft == 0)
		    {
                Log.info(this, "开启剩余时间倒计时： " + RestTimeLeft);
		        vp_Timer.CancelAll("RestTimeSchedule");
		    }
		}

		public void HurtRank(WorldBossRankMsg_22_3 hurtRank)
		{
			this.hurtRank = hurtRank;
			DataUpdate(this.UPDATE_HURT_RANK);
		}
		public void GuWuInfo(WorldBossGuwuMsg_22_4 guwu)
		{
			if(guwu.type == AttackGuwuId)
                this.AttackTime = guwu.count;
			else
                this.CritTime = guwu.count;
            DataUpdate(this.UPDATE_GUWU_INFO);
		}
		public void GuWuBegin(uint attTime,uint damageTime)
		{
			this.AttackTime = attTime;//攻击鼓舞次数
			this.CritTime = damageTime;//暴击鼓舞次数
			DataUpdate(this.UPDATE_GUWU_INFO);
		}
		public void BossDie(WorldBossDieMsg_22_5 bossDie)
		{
			this.winSiler = bossDie.extGold + bossDie.gold;//累计金币奖励
			this.winRepu = bossDie.extRepu + bossDie.repu;//累计声望奖励
			DataUpdate(this.UPDATE_BOSS_DIE);
		}

		//登陆是接收
		public void BossHp(WorldBossHpMsg_22_6 bossHp)
		{
			this.currHp = bossHp.currHp;
			this.fullHp = bossHp.fullHp;
			this.hurt = bossHp.damage;
			DataUpdate(this.UPDATE_ATTACK);
			DataUpdate(this.UPDATE_BOSS_HP);
		}
		public void UpdateBossHp(uint currHp,uint fullHp)
		{
			this.currHp = currHp;
			this.fullHp = fullHp;
			DataUpdate(this.UPDATE_BOSS_HP);
		}

		//战况
		public void FightInfo(WorldBossInfoMsg_22_9 fightInfo)
		{
			this.fullHp = fightInfo.hpfull;
			this.hurt = fightInfo.damage;
			DataUpdate(this.UPDATE_ATTACK);
		}

		public void BossEnd(byte code,uint time)
		{
		    if (code == 0)  //死亡
		    {
                isDie = true;
		        AppMap.Instance.GetMonster()[0].GetMeVoByType<MonsterVo>().CurHp = 0;
		    }
			else if(code == 1)
				isDie = false;
		    this.mEndTimeStamp = time + ServerTime.Instance.Timestamp;
		    if (isDie || EndTimeLeft == 0)
		    {
		        isShowTips = false;
		    }
		    else
		    {
		        isShowTips = true;
		    }
            DataUpdate(this.UPDATE_TIPS);
		    if ((isDie || time == 0) && AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS)
		        IsOpenRank = true;
		    else
		        IsOpenRank = false;
            
            vp_Timer.CancelAll("EndTimeSchedule");
            if (EndTimeLeft != 0)
                vp_Timer.In(0f, EndTimeSchedule, (int)EndTimeLeft, 1f);
			else
				DataUpdate(this.UPDATE_END);
		}
        private void EndTimeSchedule()
        {
            if (EndTimeLeft == 0 )
            {   
                vp_Timer.CancelAll("EndTimeSchedule");
            }
            DataUpdate(this.UPDATE_END);
        }
		/**
     	* 鼓舞，type 鼓舞类型
     	*/
		public void GuWu( byte type)
		{
			MemoryStream msdata = new MemoryStream();
			Module_22.write_22_4(msdata,type);
			AppNet.gameNet.send(msdata, 22, 4);
		}
		/// <summary>
		/// 角色攻击
		/// </summary>
		/// <param name="hurt">伤害值</param>
		public void Attack(uint hurt)
		{
			MemoryStream msdata = new MemoryStream();
			Module_22.write_22_10(msdata,hurt);
			AppNet.gameNet.send(msdata, 22, 10);
		}
	}
}

