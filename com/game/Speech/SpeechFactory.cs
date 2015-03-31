//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SpeechFactory
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Speech
{
	public class SpeechFactory 
	{		
		public static BaseCommand GetSpeechComand(int commandType)	
		{
			//法师语音
			if (commandType <= SpeechConst.MagicWin)
			{
				return GetMagicCommand(commandType);
			}
			//刺客语音
			else if (commandType <= SpeechConst.AssassinSkill4)
			{
				return GetAssassinCommand(commandType);
			}
			else
			{
				return null;
			}
		}

		//法师语音
		private static BaseCommand GetMagicCommand(int commandType)
		{
			switch (commandType)
			{
			case SpeechConst.MagicContinueMoveDirection:
				return new MagicContinueMoveDirectionCommand();

			case SpeechConst.MagicUseSkillAndMonsterOver7:
				return new MagicUseSkillAndMonsterOver7Command();

			case SpeechConst.MagicAttackedAndMonster0to4:
				return new MagicAttackedAndMonster0to4Command();

			case SpeechConst.MagicAttackedAndMonster4to7:
				return new MagicAttackedAndMonster4to7Command();

			case SpeechConst.MagicAttackedAndHurtOverPercent10:
				return new MagicAttackedAndHurtOverPercent10Command();

			case SpeechConst.MagicHPBelowPercent5:
				return new MagicHPBelowPercent5Command();

			case SpeechConst.MagicKillBoss:
				return new MagicKillBossCommand();

			case SpeechConst.MagicNormalSkillCycle:
				return new MagicNormalSkillCycleCommand();

			case SpeechConst.MagicClearanceFailed:
				return new MagicClearanceFailedCommand();

			case SpeechConst.MagicEnterCopy:
				return new MagicEnterCopyCommand();

			case SpeechConst.MagicGetBigReward:
				return new MagicGetBigRewardCommand();

			case SpeechConst.MagicBeautyCry:
				return new MagicBeautiCryCommand();

			case SpeechConst.MagicWin:
				return new MagicWinCommand();

			default:
				return null;
			}
		}	

		//刺客语音
		private static BaseCommand GetAssassinCommand(int commandType)
		{
			switch (commandType)
			{
			case SpeechConst.AssassinEnterCopy:
				return new AssassinEnterCopyCommand();
				
			case SpeechConst.AssassinAttackedAndMonster0to4:
				return new AssassinAttackedAndMonster0to4Command();
				
			case SpeechConst.AssassinAttackedAndMonster4to7:
				return new AssassinAttackedAndMonster4to7Command();
				
			case SpeechConst.AssassinKillBoss:
				return new AssassinKillBossCommand();
				
			case SpeechConst.AssassinWin:
				return new AssassinWinCommand();
				
			case SpeechConst.AssassinClearanceFailed:
				return new AssassinClearanceFailedCommand();
				
			case SpeechConst.AssassinKnockedDown:
				return new AssassinKnockedDownCommand();
				
			case SpeechConst.AssassinDead:
				return new AssassinDeadCommand();
				
			case SpeechConst.AssassinNormalAttack1:
				return new AssassinNormalAttack1Command();
				
			case SpeechConst.AssassinNormalAttack2:
				return new AssassinNormalAttack2Command();
				
			case SpeechConst.AssassinNormalAttack3:
				return new AssassinNormalAttack3Command();
				
			case SpeechConst.AssassinNormalAttack4:
				return new AssassinNormalAttack4Command();
				
			case SpeechConst.AssassinSkill1:
				return new AssassinSkill1Command();

			case SpeechConst.AssassinSkill2:
				return new AssassinSkill2Command();

			case SpeechConst.AssassinSkill3:
				return new AssassinSkill3Command();

			case SpeechConst.AssassinSkill4:
				return new AssassinSkill4Command();
				
			default:
				return null;
			}
		}	

	}
}
