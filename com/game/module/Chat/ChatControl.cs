using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using Proto;
using com.game;
using com.game.Public.Confirm;
using com.game.cmd;
using com.game.dialog;
using com.game.manager;
using com.game.module.main;
using com.game.module.test;
using com.net.interfaces;
using com.u3d.bases.debug;
using com.game.vo;
using com.game.Public.Message;
using com.game.Public.LocalVar;
using com.game.module.battle;
using com.game.data;
using PCustomDataType;
using com.game.consts;
using Com.Game.Module.Role;

namespace Com.Game.Module.Chat
{
	public class ChatControl : BaseControl<ChatControl> {

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_10_1, Fun_10_1);				//非私聊请求聊天返回
			AppNet.main.addCMD(CMD.CMD_10_2, Fun_10_2);				//私聊请求聊天返回
			AppNet.main.addCMD(CMD.CMD_10_3, Fun_10_3);				//聊天内容推送
			AppNet.main.addCMD(CMD.CMD_10_6, Fun_10_6);				//系统公告
			AppNet.main.addCMD(CMD.CMD_10_7, Fun_10_7);				//传闻电视广播
			
		}

		//非私聊请求聊天返回
		private void Fun_10_1(INetData data)
		{
			ChatReqMsg_10_1 chatReqMsg_10_1 = new ChatReqMsg_10_1 ();
			chatReqMsg_10_1.read (data.GetMemoryStream ());
			if (chatReqMsg_10_1.code != 0)
			{

				ErrorCodeManager.ShowError(chatReqMsg_10_1.code);	
				return;
			}
			Singleton<ChatView>.Instance.msgInput.value = "";
		}

		//私聊请求聊天返回
		private void Fun_10_2(INetData data)
		{
			ChatPrivateChatMsg_10_2 privateChatReqMsg_10_2 = new ChatPrivateChatMsg_10_2();
			privateChatReqMsg_10_2.read (data.GetMemoryStream ());

			if (privateChatReqMsg_10_2.code != 0)
			{
				ErrorCodeManager.ShowError(privateChatReqMsg_10_2.code);
				return;
			}
//			Singleton<ChatMode>.Instance.ReciveNameSL = privateChatReqMsg_10_2.roleName;

			ChatVo newMSg = new ChatVo ();
			newMSg.chatType = 7;
			newMSg.senderId = MeVo.instance.Id;
			newMSg.serverId = (ushort)MeVo.instance.serverId;
			newMSg.senderName = MeVo.instance.Name;
			newMSg.senderSex = MeVo.instance.sex;
			newMSg.senderJob = MeVo.instance.job;
			newMSg.senderLvl = (byte)MeVo.instance.Level;
			newMSg.senderVip = MeVo.instance.vip;
			newMSg.content = Singleton<ChatView>.Instance.sendMessage.content;
//			updateChatVO.goods = Singleton<ChatMode>.Instance.goods.Count > 0?Singleton<ChatMode>.Instance.goods[0]:null;
			newMSg.nationId = MeVo.instance.nation;

			Singleton<ChatMode>.Instance.AddChatMsg (newMSg);
			Singleton<ChatMode>.Instance.UpdateMainChatContent (newMSg.senderName, newMSg.chatType, newMSg.content);
			Singleton<ChatView>.Instance.msgInput.value = "";
		}

		private bool IsValidMsg(byte msgType)
		{
			bool isWorldMsg = (msgType == (byte)ChatType.ZongHe);
			bool isGuildMsg = (msgType == (byte)ChatType.ZhenYing);
			bool isPrivateMsg = (msgType == (byte)ChatType.SiLiao);

			return (isWorldMsg || isGuildMsg || isPrivateMsg);
		}

		private bool IsBlackListMan(string name)
		{
			if (null != Singleton<FriendMode>.Instance.blacksList)
			{
				foreach (PRelationInfo item in Singleton<FriendMode>.Instance.blacksList)
				{
					if (name.Equals(item.name))
					{
						return true;
					}
				}
			}

			return false;
		}

		//聊天内容推送
		private void Fun_10_3(INetData data)
		{
			ChatContentPushMsg_10_3 recChatMsg = new ChatContentPushMsg_10_3();
			recChatMsg.read (data.GetMemoryStream ());

			if (!IsValidMsg(recChatMsg.chatType))
			{
				return;
			}

			if (IsBlackListMan(recChatMsg.senderName))
			{
				return;
			}

			Log.info (this, "判断是否接收该频道消息");
			switch (recChatMsg.chatType)
			{
				case (byte)ChatType.SiLiao:
					if (LocalVarManager.GetInt(LocalVarManager.CHAT_REC_SL_CHANNEL, 0) == (int)ReceiveState.REJECT)
						return;
					else
						break;
				case (byte)ChatType.ZongHe:
					if (LocalVarManager.GetInt(LocalVarManager.CHAT_REC_ZH_CHANNEL, 0) == (int)ReceiveState.REJECT)
						return;
					else
						break;
				case (byte)ChatType.ZhenYing:
					if (LocalVarManager.GetInt(LocalVarManager.CHAT_REC_ZY_CHANNEL, 0) == (int)ReceiveState.REJECT)
						return;
					else
						break;
				default:
						break;
			}

			ChatVo recChat = new ChatVo ();
			recChat.chatType = recChatMsg.chatType;
			recChat.senderId = recChatMsg.senderId;
			recChat.serverId = recChatMsg.serverId;
			recChat.senderName = recChatMsg.senderName;
			recChat.senderSex = recChatMsg.senderSex;
			recChat.senderJob = recChatMsg.senderJob;
			recChat.senderLvl = recChatMsg.senderLvl;
			recChat.senderVip = recChatMsg.senderVip;
			recChat.content = recChatMsg.content;
			recChat.goods = recChatMsg.goodsList.Count > 0?recChatMsg.goodsList[0]:null;
			recChat.nationId = recChatMsg.senderNation;

			SendChatMsg(recChat);
		}

		private void SendChatMsg(ChatVo recChat)
		{			
			//Log.error(this, "new message come! , msg" + recChatMsg.senderName);
			//如果不是自己发出的消息，就要给出消息提示
			if (recChat.senderName != MeVo.instance.Name)
			{
				if (Singleton<MainBottomLeftView>.Instance.liaotianBg)
				{
					Singleton<MainBottomLeftView>.Instance.NewMsgAlarm(Singleton<MainBottomLeftView>.Instance.liaotianBg, true);
				}

				if(Singleton<BattleView>.Instance.BtnChatBg)
				{
					Singleton<BattleView>.Instance.NewMsgAlarm(Singleton<BattleView>.Instance.BtnChatBg, true);
				}
			}
			//记录名字对应的ID号
			if (Singleton<ChatMode>.Instance.nameToIdDic.ContainsKey(recChat.senderName))
			{
				Singleton<ChatMode>.Instance.nameToIdDic[recChat.senderName] = recChat.senderId;
			}
			else
			{
				Singleton<ChatMode>.Instance.nameToIdDic.Add(recChat.senderName, recChat.senderId);
			}
			
			Singleton<ChatMode>.Instance.AddChatMsg (recChat);
			Singleton<ChatMode>.Instance.UpdateMainChatContent (recChat.senderName, recChat.chatType, recChat.content);
		}

		//系统公告
		private void Fun_10_6(INetData data)
		{
			ChatSysAnounceMsg_10_6 chatSysAnounceMsg = new ChatSysAnounceMsg_10_6();
			chatSysAnounceMsg.read (data.GetMemoryStream ());

			string content = chatSysAnounceMsg.content;
			Singleton<ChatMode>.Instance.Notice = content;
			SendNoticeMsg(content);
		}

		//传闻电视广播
		private void Fun_10_7(INetData data)
		{
			ChatRumorMsg_10_7 chatRumorMsg = new ChatRumorMsg_10_7();
			chatRumorMsg.read (data.GetMemoryStream ());			

			string content = GetRumorText(chatRumorMsg);
			Singleton<ChatMode>.Instance.Rumor = content;
			SendNoticeMsg(content);
		}

		private void SendNoticeMsg(string content)
		{
			ChatVo recChat = new ChatVo ();
			recChat.chatType = (byte)ChatType.ZongHe;
			recChat.senderName = LanguageManager.GetWord("ChatView.SysNotice");
			recChat.senderId = GameConst.SystemNoticeId;
			recChat.content = content;
			
			SendChatMsg(recChat);
		}

		private string GetRumorText(ChatRumorMsg_10_7 rumorMsg)
		{
			string result = string.Empty;
			string roleTag = "$role$";
			string goodsTag = "$goods$";
			string monsterTag = "$mon$";
			string intTag = "$int$";
			string stringTag = "$string$";
			
			SysRumor rumorVo = BaseDataMgr.instance.GetSysRumor(rumorMsg.typeId);
			if (null == rumorVo)
			{
				return result;
			}

			PRumor rumerContent = new PRumor(); 
            if (rumorMsg.content.Count>0)
		    {
                rumerContent = rumorMsg.content[0];
		    }

		    string formatStr = rumorVo.format;
			int i = 0;
			int roleTagIndex = 0;
			int goodsTagIndex = 0;
			int monsterTagIndex = 0;
			int intTagIndex = 0;
			int stringTagIndex = 0;

			while (i < formatStr.Length)
			{
				if ('$' != formatStr[i])
				{
					result += formatStr[i];
					i++;
				}
				else
				{
					//角色
					if (roleTag == formatStr.Substring(i, roleTag.Length))
					{
						result += "[00ff00]" + rumerContent.role[roleTagIndex].name + "[-]";
						roleTagIndex++;
						i += roleTag.Length;
					}
					//物品
					else if (goodsTag == formatStr.Substring(i, goodsTag.Length))
					{
						uint goodsId = rumerContent.goods[goodsTagIndex].goodsId;
						SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);	
						result += "[0000ff]" + vo.name + "[-]";
						goodsTagIndex++;
						i += goodsTag.Length;
					}
					//怪物
					else if (monsterTag == formatStr.Substring(i, monsterTag.Length))
					{
						uint monsterId = rumerContent.mon[monsterTagIndex].monId;
						SysMonsterVo vo = BaseDataMgr.instance.GetDataById<SysMonsterVo>(monsterId);	
						result += "[ff0000]" + vo.name + "[-]";
						monsterTagIndex++;
						i += monsterTag.Length;
					}
					//数值
					else if (intTag == formatStr.Substring(i, intTag.Length))
					{						
						result += "[ffff00]" + rumerContent.iData[intTagIndex] + "[-]";
						intTagIndex++;
						i += intTag.Length;
					}
					//字符串
					else if (stringTag == formatStr.Substring(i, stringTag.Length))
					{						
						result += "[00ffff]" + rumerContent.sData[stringTagIndex] + "[-]";
						stringTagIndex++;
						i += stringTag.Length;
					}
					//普通字符
					else
					{
						result += formatStr[i];
						i++;
					}
				}
			}

			return result;
		}

		//点击主菜单中的聊天按钮触发
		public void OpenChatUI()
		{
			Singleton<MainBottomRightView>.Instance.ShriftBottomRight (true);
			Singleton<ChatView>.Instance.OpenView ();
		}
	}
}