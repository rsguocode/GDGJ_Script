//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SpeechMgr
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.game.vo;
using com.game.consts;

namespace Com.Game.Speech
{
	public class SpeechMgr 
	{		
		public static SpeechMgr Instance = ((null == Instance) ? new SpeechMgr() : Instance);	

		private IDictionary<int, BaseCommand> commandDictionary = new Dictionary<int, BaseCommand>();

		private bool HasCommandRun()
		{
			foreach (KeyValuePair<int, BaseCommand> pair in commandDictionary)
			{
				BaseCommand command = pair.Value;
				if (command.IsPlaying)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsMagicSpeech
		{
			get {return (GameConst.JOB_FASHI == MeVo.instance.job);}
		}

		public bool IsAssassinSpeech
		{
			get {return (GameConst.JOB_CHIKE == MeVo.instance.job);}
		}

		public void PlayEnterCopySpeech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicEnterCopy);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinEnterCopy);
			}
		}

		public void PlayAttackedAndMonster0to4Speech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicAttackedAndMonster0to4);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinAttackedAndMonster0to4);
			}
		}

		public void PlayAttackedAndMonster4to7Speech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicAttackedAndMonster4to7);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinAttackedAndMonster4to7);
			}
		}

		public void PlayKillBossSpeech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicKillBoss);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinKillBoss);
			}
		}

		public void PlayWinSpeech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicWin);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinWin);
			}
		}

		public void PlayFailedSpeech()
		{
			if (IsMagicSpeech)
			{
				PlaySpeech(SpeechConst.MagicClearanceFailed);
			}
			else if (IsAssassinSpeech)
			{
				PlaySpeech(SpeechConst.AssassinClearanceFailed);
			}
		}

		public void PlaySpeech(int commandType)
		{
			//系统中只能同时播放一个语音
			if (HasCommandRun())
			{
				return;
			}

			BaseCommand command = null;

			if (!commandDictionary.ContainsKey(commandType))
			{
				command = SpeechFactory.GetSpeechComand(commandType);

				if (null != command)
				{
					commandDictionary.Add(commandType, command);
				}
			}
			else
			{
				command = commandDictionary[commandType];
			}

			if (null != command)
			{
				command.Run();
			}
		}
		
	}
}
