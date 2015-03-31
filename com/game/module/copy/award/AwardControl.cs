using Proto;
using com.game;
using com.game.cmd;
using com.game.module.test;
using com.net.interfaces;
using com.u3d.bases.debug;
using com.game.module.effect;
using Com.Game.Module.Role;
using Com.Game.Module.Story;
using com.game.module.Task;
using com.game.module.battle;
using com.game.module.map;
using com.game.sound;
using Com.Game.Speech;
using UnityEngine;

namespace Com.Game.Module.Copy
{
	public class AwardControl : BaseControl<AwardControl> 
	{
		DungeonRewardMsg_8_5 curDungenRewardMsg;
		public bool CanInterrupt = true;

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_8_5, Fun_8_5);				//副本奖励
			
		}

		//奖励发放完毕
		public void AwardEnd()
		{
			Log.info (this, "副本奖励统计界面结束,结束副本");
			Singleton<CopyAwardView>.Instance.CloseView ();
			Singleton<CopyControl>.Instance.CopyEnd (true, Singleton<AwardMode>.Instance.award.isFirstPass);
		}

		/// <summary>
		/// 奖励信息
		/// </summary>
		/// <param name="data">Data.</param>
		private void Fun_8_5(INetData data)
		{
			Log.info(this, "收到8-5副本奖励信息协议");
            Debug.Log("****收到8-5副本奖励信息协议");
			DungeonRewardMsg_8_5 dungenRewardMsg = new DungeonRewardMsg_8_5 ();
			dungenRewardMsg.read (data.GetMemoryStream ());
			curDungenRewardMsg = dungenRewardMsg;

			//如果玩家升级了，显示升级特效
			if (Singleton<RoleMode>.Instance.Upgraded)
			{
				CanInterrupt = false;
				EffectMgr.Instance.CreateMainFollowEffect(EffectId.Main_RoleUpgrade, AppMap.Instance.me.Controller.gameObject, Vector3.zero, true, PlayLevelUpStory);
			}
			else
			{
			    StartPlayEndStory();
			}
		}

		private void PlayLevelUpStory()
		{
			if (Singleton<StoryControl>.Instance.PlayLevelUpStory(StartPlayEndStory))
			{
			}
			else
			{
				StartPlayEndStory();
			}
		}

		private void PlayEndStageStory()
		{
			if (Singleton<MapControl>.Instance.CanPlayCopyStory(AppMap.Instance.mapParser.MapId))
			{
				Singleton<BattleView>.Instance.CloseView();
				if (Singleton<StoryControl>.Instance.PlayEndStageStory(AppMap.Instance.mapParser.MapId, MapMode.CUR_MAP_PHASE, PlayExitSceneStory))
				{
				}
				else
				{
					PlayExitSceneStory();
				}
			}
			else
			{
				PlayExitSceneStory();
			}
		}

	    private void StartPlayEndStory()
	    {
			Log.info (this, "-StartPlayEndStory()播放结束剧情");
			CanInterrupt = true;
            //vp_Timer.In(2.5f, PlayEndStageStory);   //1.5S钟后显示副本通关UI，等待胜利动作播放完毕
			PlayEndStageStory ();
	    }


	    private void PlayExitSceneStory()
		{
			if (Singleton<MapControl>.Instance.CurTaskCopyMapId == AppMap.Instance.mapParser.MapId)
			{
				Singleton<BattleView>.Instance.CloseView();
				if (Singleton<StoryControl>.Instance.PlayExitSceneStory(AppMap.Instance.mapParser.MapId, ShowAwardView))
				{
				}
				else
				{
					ShowAwardView();
				}
			}
			else
			{
				ShowAwardView();
			}
		}

		private void ShowAwardView()
		{
			Log.info(this, "打开奖励面板");
			//1.停止场景背景音乐
			string bgMusicId = Singleton<MapControl>.Instance.GetMapBackMusicName(AppMap.Instance.mapParser.MapId);
			SoundMgr.Instance.Stop(bgMusicId);
			//2.播放战斗胜利音乐
			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_BattleWin);

			Singleton<AwardMode>.Instance.UpdateAwardData(curDungenRewardMsg);   //---modify by lixi.先更新数据，再打开UI，否则UI取不到数据
			Singleton<CopyAwardView>.Instance.OpenView ();

			//关闭战斗UI
			Singleton<BattleView>.Instance.CloseView();

			//播放主角通关语音
			SpeechMgr.Instance.PlayWinSpeech();
		}


	}
}
