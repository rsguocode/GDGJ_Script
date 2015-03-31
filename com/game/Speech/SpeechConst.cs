//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SpeechConst
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Speech
{
	public class SpeechConst 
	{	
		//----------------法师语音--------------------------

		//法师连续20秒频繁使用方向移动
		public const int MagicContinueMoveDirection = 1;	

		//法师放技能时候，同屏敌人数量>7
		public const int MagicUseSkillAndMonsterOver7 = 2;	

		//法师被击中时，0<同屏敌人<=4
		public const int MagicAttackedAndMonster0to4 = 3;	

		//法师被击中时，4<同屏敌人<=7
		public const int MagicAttackedAndMonster4to7 = 4;

		//法师一次受击伤害超过最大生命值的10%
		public const int MagicAttackedAndHurtOverPercent10 = 5;

		//法师生命少于5%的一瞬间
		public const int MagicHPBelowPercent5 = 6;

		//法师杀死BOSS
		public const int MagicKillBoss = 7;

		//法师狂按普攻3*3个循环，并且只用普攻
		public const int MagicNormalSkillCycle = 8;

		//法师通关失败
		public const int MagicClearanceFailed = 9;

		//法师进入副本后
		public const int MagicEnterCopy = 10;

		//法师抽到大奖
		public const int MagicGetBigReward = 11;

		//法师女神尖叫
		public const int MagicBeautyCry = 12;

		//法师通关胜利
		public const int MagicWin = 13;

		//----------------刺客语音--------------------------

		//刺客进入副本后
		public const int AssassinEnterCopy = 14;

		//刺客被击中时，0<同屏敌人<=4
		public const int AssassinAttackedAndMonster0to4 = 15;	
		
		//刺客被击中时，4<同屏敌人<=7
		public const int AssassinAttackedAndMonster4to7 = 16;

		//刺客杀死BOSS
		public const int AssassinKillBoss = 17;

		//刺客通关胜利
		public const int AssassinWin = 18;

		//刺客通关失败
		public const int AssassinClearanceFailed = 19;

		//刺客被击倒
		public const int AssassinKnockedDown = 20;

		//刺客死亡
		public const int AssassinDead = 21;

		//刺客普攻第1招式
		public const int AssassinNormalAttack1 = 22;

		//刺客普攻第2招式
		public const int AssassinNormalAttack2 = 23;

		//刺客普攻第3招式
		public const int AssassinNormalAttack3 = 24;

		//刺客普攻第4招式
		public const int AssassinNormalAttack4 = 25;

		//刺客技能1飞镖
		public const int AssassinSkill1 = 26;

		//刺客技能2跳劈
		public const int AssassinSkill2 = 27;

		//刺客技能3来回穿梭
		public const int AssassinSkill3 = 28;

		//刺客技能4飞踢
		public const int AssassinSkill4 = 29;
		
	}
}
