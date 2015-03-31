//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SpeechCommand
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.game.sound;

namespace Com.Game.Speech
{
	//语音基础命令
	public class BaseCommand 
	{
		protected float cd = 10f;
		protected int probability = 100;
		protected float lastRunTime = -1000f;
		protected IList<string> soundList = new List<string>();
		private string curSoundId = string.Empty;

		protected virtual string roleType
		{
			get {return string.Empty;}
		}

		public BaseCommand()
		{
			InitData();
		}

		public bool IsPlaying
		{
			get {return SoundMgr.Instance.IsPlaying(curSoundId);}
		}

		private bool ConditionMatched()
		{
			bool timeMatch = ((Time.time-lastRunTime) >= cd);
			bool probMatch = false;

			if (soundList.Count > 0)
			{
				int randValue = Random.Range(1, 101);
				probMatch = (randValue <= probability);
			}

			return (timeMatch && probMatch);
		}

		protected virtual void InitData()
		{
		}

		public void Run()
		{
			if (!ConditionMatched())
			{
				return;
			}

			if (IsPlaying)
			{
				return;
			}

			int index = Random.Range(0, soundList.Count);
			curSoundId = soundList[index];
			lastRunTime = Time.time;
			SoundMgr.Instance.PlaySpeechAudio(curSoundId, roleType);
		}
	}

	//----------------法师语音--------------------------

	//法师语音基础命令
	public class MagicCommand : BaseCommand
	{
		protected override string roleType
		{
			get {return "Magic";}
		}
	}

	//法师连续20秒频繁使用方向移动
	public class MagicContinueMoveDirectionCommand : MagicCommand
	{
		protected override void InitData()
		{
			cd = 60f;
			probability = 50;
			soundList.Add("20001");
			soundList.Add("20002");
		}
	}

	//法师放技能时候，同屏敌人数量>7
	public class MagicUseSkillAndMonsterOver7Command : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 20f;
			probability = 40;
			soundList.Add("20003");
		}
	}

	//法师被击中时，0<同屏敌人<=4
	public class MagicAttackedAndMonster0to4Command : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 20;
			soundList.Add("20004");
			soundList.Add("20005");
		}
	}

	//法师被击中时，4<同屏敌人<=7
	public class MagicAttackedAndMonster4to7Command : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 10f;
			probability = 50;
			soundList.Add("20006");
			soundList.Add("20007");
		}
	}

	//法师一次受击伤害超过最大生命值的10%
	public class MagicAttackedAndHurtOverPercent10Command : MagicCommand
	{		
		protected override void InitData()
		{
			cd = 20f;
			probability = 40;
			soundList.Add("20008");
			soundList.Add("20009");
		}
	}

	//法师生命少于5%的一瞬间
	public class MagicHPBelowPercent5Command : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 100;
			soundList.Add("20010");
			soundList.Add("20011");
		}
	}

	//法师杀死BOSS
	public class MagicKillBossCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("20012");
			soundList.Add("20013");
			soundList.Add("20014");
			soundList.Add("20015");
		}
	}

	//法师狂按普攻3*3个循环，并且只用普攻
	public class MagicNormalSkillCycleCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 60f;
			probability = 100;
			soundList.Add("20016");
		}
	}

	//法师通关失败
	public class MagicClearanceFailedCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 60f;
			probability = 100;
			soundList.Add("20017");
		}
	}

	//法师法师进入副本后
	public class MagicEnterCopyCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("20018");
			soundList.Add("20019");
		}
	}

	//法师抽到大奖
	public class MagicGetBigRewardCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("20020");
			soundList.Add("20021");
		}
	}

	//法师女神尖叫
	public class MagicBeautiCryCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("20003");
			soundList.Add("20004");
			soundList.Add("20005");
			soundList.Add("20006");
			soundList.Add("20007");
		}
	}

	//法师通关胜利
	public class MagicWinCommand : MagicCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("20022");
		}
	}

	//----------------刺客语音--------------------------

	//刺客语音基础命令
	public class AssassinCommand : BaseCommand
	{
		protected override string roleType
		{
			get {return "Assassin";}
		}
	}

	//刺客进入副本后
	public class AssassinEnterCopyCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 60f;
			probability = 60;
			soundList.Add("30001");
			soundList.Add("30002");
			soundList.Add("30003");
		}
	}

	//刺客被击中时，0<同屏敌人<=4
	public class AssassinAttackedAndMonster0to4Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 50;
			soundList.Add("30004");
			soundList.Add("30005");
		}
	}

	//刺客被击中时，4<同屏敌人<=7
	public class AssassinAttackedAndMonster4to7Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 10f;
			probability = 20;
			soundList.Add("30006");
			soundList.Add("30007");
		}
	}

	//刺客杀死BOSS
	public class AssassinKillBossCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30008");
			soundList.Add("30009");
			soundList.Add("30010");
			soundList.Add("30011");
		}
	}

	//刺客通关胜利
	public class AssassinWinCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30012");
			soundList.Add("30013");
		}
	}

	//刺客通关失败
	public class AssassinClearanceFailedCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30014");
		}
	}

	//刺客被击倒
	public class AssassinKnockedDownCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30015");
			soundList.Add("30016");
			soundList.Add("30017");
		}
	}

	//刺客死亡
	public class AssassinDeadCommand : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30018");
		}
	}

	//刺客普攻第1招式
	public class AssassinNormalAttack1Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30019");
			soundList.Add("30020");
		}
	}

	//刺客普攻第2招式
	public class AssassinNormalAttack2Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30021");
			soundList.Add("30022");
		}
	}

	//刺客普攻第3招式
	public class AssassinNormalAttack3Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30023");
		}
	}

	//刺客普攻第4招式
	public class AssassinNormalAttack4Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 0f;
			probability = 100;
			soundList.Add("30024");
			soundList.Add("30025");
		}
	}

	//刺客技能1飞镖
	public class AssassinSkill1Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 60;
			soundList.Add("30026");
		}
	}

	//刺客技能2跳劈
	public class AssassinSkill2Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 80;
			soundList.Add("30027");
			soundList.Add("30028");
		}
	}

	//刺客技能3来回穿梭
	public class AssassinSkill3Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 60;
			soundList.Add("30030");
			soundList.Add("30031");
		}
	}

	//刺客技能4飞踢
	public class AssassinSkill4Command : AssassinCommand
	{	
		protected override void InitData()
		{
			cd = 30f;
			probability = 50;
			soundList.Add("30029");
		}
	}

}
