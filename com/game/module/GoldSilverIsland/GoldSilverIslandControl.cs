//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoldSilverIslandControl
//文件描述：
//创建者：黄江军
//创建日期：2014-02-19
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.cmd;
using com.net.interfaces;
using com.game;
using Proto;
using com.u3d.bases.debug;
using com.game.Public.Message;
using com.game.module.map;
using com.game.consts;
using Com.Game.Module.Arena;
using com.game.module.effect;
using Com.Game.Module.Copy;
using com.game.vo;

namespace Com.Game.Module.GoldSilverIsland
{
	public class GoldSilverIslandControl : BaseControl<GoldSilverIslandControl> 
	{
		//注册Socket数据返回监听
		override protected void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_26_0, Fun_26_0);      //每个冒险玩家的信息
			AppNet.main.addCMD(CMD.CMD_26_1, Fun_26_1);      //剩余打劫次数
			AppNet.main.addCMD(CMD.CMD_26_2, Fun_26_2);      //剩余协助次数
			AppNet.main.addCMD(CMD.CMD_26_3, Fun_26_3);      //剩余冒险次数
			AppNet.main.addCMD(CMD.CMD_26_4, Fun_26_4);      //打劫cd
			AppNet.main.addCMD(CMD.CMD_26_5, Fun_26_5);      //清除抢劫cd时间
			AppNet.main.addCMD(CMD.CMD_26_6, Fun_26_6);      //当前品质和刷新次数
			AppNet.main.addCMD(CMD.CMD_26_7, Fun_26_7);      //当前的协助玩家
			AppNet.main.addCMD(CMD.CMD_26_8, Fun_26_8);      //删除冒险玩家信息
			AppNet.main.addCMD(CMD.CMD_26_9, Fun_26_9);      //增加冒险玩家
			AppNet.main.addCMD(CMD.CMD_26_10, Fun_26_10);    //更新玩家被打劫的次数
			AppNet.main.addCMD(CMD.CMD_26_11, Fun_26_11);    //邀请协助者
			AppNet.main.addCMD(CMD.CMD_26_12, Fun_26_12);    //推送邀请应答
			AppNet.main.addCMD(CMD.CMD_26_13, Fun_26_13);    //随机品质
			AppNet.main.addCMD(CMD.CMD_26_14, Fun_26_14);    //开始冒险
			AppNet.main.addCMD(CMD.CMD_26_15, Fun_26_15);    //抢劫
			AppNet.main.addCMD(CMD.CMD_26_16, Fun_26_16);    //疾风术
			AppNet.main.addCMD(CMD.CMD_26_18, Fun_26_18);    //好友剩余协助次数列表
			AppNet.main.addCMD(CMD.CMD_26_19, Fun_26_19);    //剩余协助次数（给好友广播）
			AppNet.main.addCMD(CMD.CMD_26_20, Fun_26_20);    //打劫结果
			AppNet.main.addCMD(CMD.CMD_26_21, Fun_26_21);    //完成冒险
			AppNet.main.addCMD(CMD.CMD_26_22, Fun_26_22);    //推送邀请应答
			AppNet.main.addCMD(CMD.CMD_26_23, Fun_26_23);    //推送邀请请求
			AppNet.main.addCMD(CMD.CMD_26_25, Fun_26_25);    //返回应答协议
			AppNet.main.addCMD(CMD.CMD_26_26, Fun_26_26);    //打劫预览
		}

		//每个冒险玩家的信息
		private void Fun_26_0(INetData data)
		{
			WoodsPanalInfoMsg_26_0 panelInfoMsg = new WoodsPanalInfoMsg_26_0();
			panelInfoMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.PlayerList = panelInfoMsg.players;
		}

		//剩余打劫次数
		private void Fun_26_1(INetData data)
		{
			WoodsRemainRobTiemsMsg_26_1 remainRobTimsMsg = new WoodsRemainRobTiemsMsg_26_1();
			remainRobTimsMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.RobRemainTimes = remainRobTimsMsg.times;
		}

		//剩余协助次数
		private void Fun_26_2(INetData data)
		{
			WoodsRemainAssistTimesMsg_26_2 remainAssistTimesMsg = new WoodsRemainAssistTimesMsg_26_2();
			remainAssistTimesMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.AssistRemainTimes = remainAssistTimesMsg.times;
		}

		//剩余冒险次数
		private void Fun_26_3(INetData data)
		{
			WoodsRemainAdvenTimesMsg_26_3 remainAdvenTimesMsg = new WoodsRemainAdvenTimesMsg_26_3();
			remainAdvenTimesMsg.read(data.GetMemoryStream());
			Singleton<GoldSilverIslandMode>.Instance.AdventRemainTimes = remainAdvenTimesMsg.times;
		}

		//打劫cd
		private void Fun_26_4(INetData data)
		{
			WoodsRobCdMsg_26_4 robCdMsg = new WoodsRobCdMsg_26_4();
			robCdMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.RobCDTime = robCdMsg.time;
		}

		//清除抢劫cd时间
		private void Fun_26_5(INetData data)
		{
			WoodsClearRobCdMsg_26_5 clearRobCdMsg = new WoodsClearRobCdMsg_26_5();
			clearRobCdMsg.read(data.GetMemoryStream());		

			if (0 == clearRobCdMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.RobCDTime = 0;
			}
			else
			{
				ErrorCodeManager.ShowError(clearRobCdMsg.code);
			}
		}

		//当前品质和刷新次数
		private void Fun_26_6(INetData data)
		{
			WoodsGradeInfoMsg_26_6 gradeInfoMsg = new WoodsGradeInfoMsg_26_6();
			gradeInfoMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.SetGradeAndRefreshTimes(gradeInfoMsg.grade, gradeInfoMsg.refreshTimes);
		}

		//当前的协助玩家
		private void Fun_26_7(INetData data)
		{
			WoodsAssistInfoMsg_26_7 assistInfoMsg = new WoodsAssistInfoMsg_26_7();
			assistInfoMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.AssistId = assistInfoMsg.assistId;
		}

		//删除冒险玩家信息
		private void Fun_26_8(INetData data)
		{
			WoodsClearPlayersMsg_26_8 clearPlayersMsg = new WoodsClearPlayersMsg_26_8();
			clearPlayersMsg.read(data.GetMemoryStream());
			Singleton<GoldSilverIslandMode>.Instance.DelPlayer(clearPlayersMsg.playerId);
		}

		//增加冒险玩家
		private void Fun_26_9(INetData data)
		{
			WoodsInsertPlayerMsg_26_9 insertPlayerMsg = new WoodsInsertPlayerMsg_26_9();
			insertPlayerMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.AddPlayers(insertPlayerMsg.playerList);
		}

		//更新玩家被打劫的次数
		private void Fun_26_10(INetData data)
		{
			WoodsUpdateRobTimesMsg_26_10 updateRobTimesMsg = new WoodsUpdateRobTimesMsg_26_10();
			updateRobTimesMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.SetPlayerRobbedTimes(updateRobTimesMsg.playerId, updateRobTimesMsg.robTimes);
		}

		//邀请协助者
		private void Fun_26_11(INetData data)
		{
			WoodsInviteAssistMsg_26_11 inviteAssistMsg = new WoodsInviteAssistMsg_26_11();
			inviteAssistMsg.read(data.GetMemoryStream());	

			if (0 != inviteAssistMsg.code)
			{
				ErrorCodeManager.ShowError(inviteAssistMsg.code);
			}
		}

		//推送邀请应答
		private void Fun_26_12(INetData data)
		{
			WoodsReplyInviteMsg_26_12 replyInviteMsg = new WoodsReplyInviteMsg_26_12();
			replyInviteMsg.read(data.GetMemoryStream());	

			if (0 != replyInviteMsg.code)
			{
				ErrorCodeManager.ShowError(replyInviteMsg.code);
			}
		}

		//随机品质
		private void Fun_26_13(INetData data)
		{
			WoodsRandGradeMsg_26_13 randGradeMsg = new WoodsRandGradeMsg_26_13();
			randGradeMsg.read(data.GetMemoryStream());	

			if (0 == randGradeMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.SetGradeAndRefreshTimes(randGradeMsg.grade, randGradeMsg.refreshTimes);
			}
			else
			{
				Singleton<GoldSilverIslandMode>.Instance.SetGradeRefreshError();
				ErrorCodeManager.ShowError(randGradeMsg.code);
			}
		}

		//开始冒险
		private void Fun_26_14(INetData data)
		{
			WoodsStartAdvenMsg_26_14 startAdvenMsg = new WoodsStartAdvenMsg_26_14();
			startAdvenMsg.read(data.GetMemoryStream());		

			if (0 != startAdvenMsg.code)
			{
				ErrorCodeManager.ShowError(startAdvenMsg.code);
			}
		}

		//抢劫
		private void Fun_26_15(INetData data)
		{
			WoodsRobPlayerMsg_26_15 robPlayerMsg = new WoodsRobPlayerMsg_26_15();
			robPlayerMsg.read(data.GetMemoryStream());	

			if (0 == robPlayerMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.SetRobbedPlayerInfo(robPlayerMsg.id, robPlayerMsg.name, 
				                                                             robPlayerMsg.job, robPlayerMsg.lvl, 
				                                                             robPlayerMsg.attr[0], robPlayerMsg.skills);
			}
			else
			{
				ErrorCodeManager.ShowError(robPlayerMsg.code);
			}
		}

		//疾风术
		private void Fun_26_16(INetData data)
		{
			WoodsFinishRightNowMsg_26_16 finishRightNowMsg = new WoodsFinishRightNowMsg_26_16();
			finishRightNowMsg.read(data.GetMemoryStream());		

			if (0 == finishRightNowMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.SetBlast();
			}
			else
			{
				ErrorCodeManager.ShowError(finishRightNowMsg.code);
			}
		}

		//好友剩余协助次数列表
		private void Fun_26_18(INetData data)
		{
			WoodsFriendsInfoMsg_26_18 friendsInfoMsg = new WoodsFriendsInfoMsg_26_18();
			friendsInfoMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.FriendList = friendsInfoMsg.friendList;
		}

		//剩余协助次数（给好友广播）
		private void Fun_26_19(INetData data)
		{
			WoodsFriendRemainInfoMsg_26_19 friendRemainInfoMsg = new WoodsFriendRemainInfoMsg_26_19();
			friendRemainInfoMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.UpdateFriendAssistRemain(friendRemainInfoMsg.remainInfo);
		}

		//打劫结果
		private void Fun_26_20(INetData data)
		{
			WoodsRobResultMsg_26_20 robResultMsg = new WoodsRobResultMsg_26_20();
			robResultMsg.read(data.GetMemoryStream());
			Singleton<GoldSilverIslandMode>.Instance.SetRobReward(robResultMsg.gold, robResultMsg.repu);
			ShowRobSuccess();
		}

		//完成冒险
		private void Fun_26_21(INetData data)
		{
			WoodsFinishAdvenMsg_26_21 finishAdvenMsg = new WoodsFinishAdvenMsg_26_21();
			finishAdvenMsg.read(data.GetMemoryStream());
			Singleton<GoldSilverIslandMode>.Instance.SetAdventFinished(finishAdvenMsg.repu, finishAdvenMsg.gold);
		}

		//推送邀请应答
		private void Fun_26_22(INetData data)
		{
			WoodsPushReplyInviteMsg_26_22 pushReplyInviteMsg = new WoodsPushReplyInviteMsg_26_22();
			pushReplyInviteMsg.read(data.GetMemoryStream());
			Singleton<GoldSilverIslandMode>.Instance.FriendReply = pushReplyInviteMsg.result;
		}

		//推送邀请请求
		private void Fun_26_23(INetData data)
		{
			WoodsPushInviteMsg_26_23 pushInviteMsg = new WoodsPushInviteMsg_26_23();
			pushInviteMsg.read(data.GetMemoryStream());	
			Singleton<GoldSilverIslandMode>.Instance.SetInviteInfo(pushInviteMsg.id, pushInviteMsg.name, pushInviteMsg.grade);
		}

		//返回应答协议
		private void Fun_26_25(INetData data)
		{
			WoodsReplyAnswerMsg_26_25 woodsReplyAnswerMsg = new WoodsReplyAnswerMsg_26_25();
			woodsReplyAnswerMsg.read(data.GetMemoryStream());	

			if (0 == woodsReplyAnswerMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.FriendReply = 0;
			}
			else
			{
				Singleton<GoldSilverIslandMode>.Instance.FriendReply = 1;
			}
		}

		//打劫预览
		private void Fun_26_26(INetData data)
		{
			WoodsRobRewardPreviewMsg_26_26 woodsRobRewardPreviewMsg = new WoodsRobRewardPreviewMsg_26_26();
			woodsRobRewardPreviewMsg.read(data.GetMemoryStream());	
			
			if (0 == woodsRobRewardPreviewMsg.code)
			{
				Singleton<GoldSilverIslandMode>.Instance.SetRobbedPlayerPreReward(woodsRobRewardPreviewMsg.gold, woodsRobRewardPreviewMsg.repu);
			}
			else
			{
				ErrorCodeManager.ShowError(woodsRobRewardPreviewMsg.code);
			}
		}

		//显示打劫成功界面
		public void ShowRobSuccess()
		{
			Singleton<RobSuccessView>.Instance.OpenView();
		}

		public void ShowRobFailed()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_ChallengeFail,Vector3.zero, FailidCallback);
		}

		private void FailidCallback()
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
			Singleton<MapMode>.Instance.changeScene(MapTypeConst.MajorCity, true, 5, 1.8f);
			Singleton<ArenaFightView>.Instance.CloseView();
		}
	
	}
}
