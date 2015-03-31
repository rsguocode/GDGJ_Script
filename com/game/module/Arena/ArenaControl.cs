using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using Proto;
using com.game.Public.Message;
using com.u3d.bases.debug;
using com.game.module.map;
using PCustomDataType;
using com.game.consts;
using com.game.vo;
using com.game.module.effect;
using Com.Game.Module.GoldSilverIsland;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统控制类
 * *******************************************************/
using Com.Game.Module.Copy;


namespace Com.Game.Module.Arena
{
	public class ArenaControl : BaseControl<ArenaControl> {

		protected override void NetListener ()
		{
			AppNet.main.addCMD(CMD.CMD_19_0, Fun_19_0);				//获取竞技场信息返回
			AppNet.main.addCMD(CMD.CMD_19_1, Fun_19_1);				//获取竞技场排名前十玩家信息返回
			AppNet.main.addCMD(CMD.CMD_19_2, Fun_19_2);				//获取可挑战玩家列表返回
			AppNet.main.addCMD(CMD.CMD_19_3, Fun_19_3);				//清除挑战CD时间返回
			AppNet.main.addCMD(CMD.CMD_19_4, Fun_19_4);				//增加挑战次数返回
			AppNet.main.addCMD(CMD.CMD_19_5, Fun_19_5);				//发送挑战结果返回
            AppNet.main.addCMD(CMD.CMD_19_8, Fun_19_8);				//个人历史挑战记录但会，暂时屏蔽
			AppNet.main.addCMD(CMD.CMD_19_10, Fun_19_10);				//挑战申请返回

		}

		//服务器返回竞技场信息
		private void Fun_19_0(INetData data)
		{
			Log.info(this, "服务器返回19_0竞技场信息给客户端");
			HeroesPanelMsg_19_0 heroesPanelMsg_19_0 = new HeroesPanelMsg_19_0 ();
			heroesPanelMsg_19_0.read (data.GetMemoryStream ());
			if (heroesPanelMsg_19_0.code == 0)
			{

				Singleton<ArenaMode>.Instance.UpdateMyArenaInfo(heroesPanelMsg_19_0);
			}
			else
			{
				ErrorCodeManager.ShowError(heroesPanelMsg_19_0.code);	
				return;
			}
		}

		//服务器返回竞技场排名前十玩家信息
		private void Fun_19_1(INetData data)
		{
			Log.info(this, "服务器返回19_1 竞技场排名前十玩家信息");
			HeroesTop10Msg_19_1 heroesTop10Msg_19_1 = new HeroesTop10Msg_19_1 ();
			heroesTop10Msg_19_1.read (data.GetMemoryStream ());
			Singleton<ArenaMode>.Instance.UpdateRankInfo (heroesTop10Msg_19_1.list);
		}

		//服务器返回可挑战玩家列表
		private void Fun_19_2(INetData data)
		{
			Log.info(this, "服务器返回19_2排名接近的玩家 供挑战使用");
			HeroesNearMsg_19_2 heroesNearMsg_19_2 = new HeroesNearMsg_19_2 ();
			heroesNearMsg_19_2.read (data.GetMemoryStream ());
			Singleton<ArenaMode>.Instance.UpdateChallengerList (heroesNearMsg_19_2.list);
		}

		//清除挑战CD时间返回
		private void Fun_19_3(INetData data)
		{
			Log.info(this, "服务器返回19_3清除挑战CD时间请求");
			HeroesClearCdMsg_19_3 heroesClearCdMsg_19_3 = new HeroesClearCdMsg_19_3 ();
			heroesClearCdMsg_19_3.read (data.GetMemoryStream ());
			if (heroesClearCdMsg_19_3.code == 0)
			{
				
				Singleton<ArenaMode>.Instance.UpdateCD(0);
			}
			else
			{
				ErrorCodeManager.ShowError(heroesClearCdMsg_19_3.code);	
				return;
			}
		}

		//增加挑战次数请求返回
		private void Fun_19_4(INetData data)
		{
			Log.info(this, "服务器返回19_4购买挑战信息返回");
			HeroesAddTimesMsg_19_4 heroesAddTimesMsg_19_4 = new HeroesAddTimesMsg_19_4 ();
			heroesAddTimesMsg_19_4.read (data.GetMemoryStream ());
			if (heroesAddTimesMsg_19_4.code == 0)
			{
				Singleton<ArenaMode>.Instance.UpdateChallengeTimes(heroesAddTimesMsg_19_4.restTimes);
				Singleton<ArenaMode>.Instance.UpdateBuyTimes(heroesAddTimesMsg_19_4.buyTimes);
//				Singleton<ArenaMode>.Instance.myArenaVo.buyTimes = heroesAddTimesMsg_19_4.buyTimes;
			}
			else
			{
				ErrorCodeManager.ShowError(heroesAddTimesMsg_19_4.code);	
				return;
			}
		}

		//客户端发送挑战结果返回
		private void Fun_19_5(INetData data)
		{
			Log.info(this, "服务端返回19_5.确认收到挑战结果");
			HeroesChallengeMsg_19_5 heroesChallengeMsg_19_5 = new HeroesChallengeMsg_19_5 ();
			heroesChallengeMsg_19_5.read (data.GetMemoryStream ());
			if (heroesChallengeMsg_19_5.code == 0)
			{
				Singleton<ArenaMode>.Instance.UpdateCD(heroesChallengeMsg_19_5.cd);
				Singleton<ArenaMode>.Instance.UpdateMyRank(heroesChallengeMsg_19_5.pos);
				Singleton<ArenaMode>.Instance.IsBattleEnd = true;
				ReciveChallengeResult(heroesChallengeMsg_19_5.result);

			}
			else
			{
				ErrorCodeManager.ShowError(heroesChallengeMsg_19_5.code);
				return;
			}
		}

		//服务器返回个人历史挑战记录
        private void Fun_19_8(INetData data)
        {
            Log.info(this, "服务器返回19_8个人历史挑战记录返回");
            HeroesHistoryPeronalMsg_19_8 heroesHistoryPeronalMsg_19_8 = new HeroesHistoryPeronalMsg_19_8();
            heroesHistoryPeronalMsg_19_8.read(data.GetMemoryStream());
            Singleton<ArenaMode>.Instance.UpdateArenaRecord(heroesHistoryPeronalMsg_19_8.history);
        }

		//申请挑战返回
		private void Fun_19_10(INetData data)
		{
			Log.info(this, "服务器返回19_10申请挑战信息返回");
			HeroesChallengeBeginMsg_19_10 heroesChallengeBeginMsg_19_10 = new HeroesChallengeBeginMsg_19_10 ();
			heroesChallengeBeginMsg_19_10.read (data.GetMemoryStream ());
			if (heroesChallengeBeginMsg_19_10.code == 0)
			{

				Singleton<ArenaMode>.Instance.UpdateVserAttr(heroesChallengeBeginMsg_19_10.skills, heroesChallengeBeginMsg_19_10.role[0]);
//				this.StartChallenge ();
			}
			else
			{
				ErrorCodeManager.ShowError(heroesChallengeBeginMsg_19_10.code);	
				return;
			}
		}

		private void StartChallenge()
		{
			Singleton<MapMode>.Instance.changeScene(MapTypeConst.ARENA_MAP, false, 5, 1.5f);
//			this.CloseArenaView ();
		}

		public void CloseArenaView()
		{
			Singleton<ArenaVsView>.Instance.CloseView ();
			Singleton<ArenaMainView>.Instance.CloseView();
			Singleton<ArenaRankView>.Instance.CloseView();
			Singleton<ArenaRecordView>.Instance.CloseView();
			Singleton<ArenaView>.Instance.CloseView ();
//			Singleton<ArenaMainView>.Instance.CloseView();
		}

		//挑战失败
		public void ChallengeFail()
		{
//			Log.info
			if (MeVo.instance.mapId == MapTypeConst.ARENA_MAP)
			{
				Singleton<ArenaMode>.Instance.SendChallengeResult ((byte)ChallengeResult.Fail);
			}
			else if (MeVo.instance.mapId == MapTypeConst.GoldSilverIsland_MAP)
			{
				uint robbedPlayerId = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer.Id;
				Singleton<GoldSilverIslandMode>.Instance.RobResult((byte)RobResult.Failed, robbedPlayerId);
				Singleton<GoldSilverIslandControl>.Instance.ShowRobFailed();
			}

		}

		//挑战过程中有英雄死亡
		public void OneHeroDeath(uint deathRoleId)
		{
			//竞技场
			if (MeVo.instance.mapId == MapTypeConst.ARENA_MAP)
			{
				if (MeVo.instance.Id == deathRoleId)
				{
					Singleton<ArenaMode>.Instance.SendChallengeResult ((byte)ChallengeResult.Fail);
				}
				else
				{
					Singleton<ArenaMode>.Instance.SendChallengeResult ((byte)ChallengeResult.Success);
				}
			}
			//金银岛
			else if (MeVo.instance.mapId == MapTypeConst.GoldSilverIsland_MAP)
			{
				uint robbedPlayerId = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer.Id;
				if (MeVo.instance.Id == deathRoleId)
				{
					Singleton<GoldSilverIslandMode>.Instance.RobResult((byte)RobResult.Failed, robbedPlayerId);
					Singleton<GoldSilverIslandControl>.Instance.ShowRobFailed();
				}
				else
				{
					Singleton<GoldSilverIslandMode>.Instance.RobResult((byte)RobResult.Success, robbedPlayerId);
				}
			}
		}

		//显示挑战结果
		private void ReciveChallengeResult(byte result)
		{
			if (result == 0)
			{
				Log.info(this, "挑战成功");
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_ChallengeOk,Vector3.zero, ChallengeSuccessResultAnimCallBack);
			}
			else
			{
				Log.info(this, "挑战失败");
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_ChallengeFail,Vector3.zero, ChallengeFailResultAnimCallBack);
			}
		}

		private void ChallengeSuccessResultAnimCallBack()
		{
			ChallengeEnd();
//			ushort bestRank = Singleton<ArenaMode>.Instance.myArenaVo.bestRank;
//			ushort newRank = Singleton<ArenaMode>.Instance.myArenaVo.rank;
//			if (newRank < bestRank)
//			{
//				Singleton<ArenaMainView>.Instance.OpenNewBestRankTips();
//			}
//			else
//			{
//				ChallengeEnd();
//			}

		}
		
		private void ChallengeFailResultAnimCallBack()
		{
			if (MeVo.instance.CurHp > 0)
			{
				ChallengeEnd();
			}
			else
			{
				Singleton<CopyFailView>.Instance.ReviveReturnCity ();
				Singleton<ArenaFightView>.Instance.CloseView();
			}
		}

		public void ChallengeEnd()
		{
//			Singleton<MapMode>.Instance.changeScene(MapTypeConst.MajorCity, true, 5, 1.8f);
			Singleton<CopyMode>.Instance.ApplyQuitCopy ();
			Singleton<ArenaFightView>.Instance.CloseView();
		}

	}
}