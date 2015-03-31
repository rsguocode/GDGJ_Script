using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using com.game;
using com.game.Public.Message;
using com.game.module.test;
using Proto;
using com.u3d.bases.debug;
using PCustomDataType;

namespace Com.Game.Module.Chat
{
	public class ChatMode : BaseMode<ChatMode> {
		public Color GREEN = new Color (74.0f / 255, 216.0f / 255, 59.0f / 255, 1);  //颜色不能为const类型变量
		public Color PURPLE = new Color (59.0f / 255, 143.0f / 255, 237.0f / 255, 1);  //颜色不能为const类型变量
		public Color YELLOW = new Color (1, 228.0f / 255, 1.0f / 255, 1);  //颜色不能为const类型变量
		public Color WHITE = new Color (1, 1, 1, 1);  //颜色不能为const类型变量
        public string [] FilterString;   //需要过滤的敏感词汇

		public List<PChatGoods> goods = new List<PChatGoods>(); 
		public Dictionary<string, uint> nameToIdDic = new Dictionary<string, uint> ();
		public Dictionary<string, bool> emoDic = new Dictionary<string, bool> ();

		//-------------------------------------------------------------
		public readonly int UPDATE_MAIN_CHAT_CONTENT = 1;
		public readonly int UPDATE_CHAT_MSG = 2;
		public readonly int UPDATE_NOTICE = 3;
		public readonly int UPDATE_RUMOR = 4;

		public readonly int UPDATE_UPARROW = 5;
		public readonly int UPDATE_DOWNARROW = 6;

		private string[] _mainChatContent = new string[2];
		private List<ChatVo> _recMsgList = new List<ChatVo>();

		public string[] mainChatContent{get{return _mainChatContent;}}
		public List<ChatVo> recChatVoList{get{return _recMsgList;}}

		private string notice;
		public string Notice
		{
			get {return notice;}

			set
			{
				notice = value;

				if (string.Empty != value)
				{
					DataUpdate (UPDATE_NOTICE);
				}
			}
		}

		private string rumor;
		public string Rumor
		{
			get {return rumor;}
			
			set
			{
				rumor = value;
				
				if (string.Empty != value)
				{
					DataUpdate (UPDATE_RUMOR);
				}
			}
		}

		//更新主界面的聊天内容
		public void UpdateMainChatContent(string name, byte chatType, string content)
		{
			_mainChatContent [0] = _mainChatContent [0];
			string newContent = "";
			switch(chatType)
			{
				case (byte)ChatType.ZongHe:
					newContent += "[ffffff]";
					break;
				case (byte)ChatType.ZhenYing:
					newContent += "[4ad836]";
					break;
				case (byte)ChatType.SiLiao:
					newContent += "[3b8fed]";
					break;
				default:
					newContent += "[ffffff]";
					break;
			}
			newContent += name + "[-]：";
			newContent += content;
			_mainChatContent[0] = newContent;
			DataUpdate (UPDATE_MAIN_CHAT_CONTENT);
		}

		//添加消息到缓存
		public void AddChatMsg(ChatVo addMsg)
		{
			Log.info(this, "将收到的消息添加到消息列表中，最多保留三十条");
			if (_recMsgList.Count < 30)
			{
				_recMsgList.Add(addMsg);
			}
			else
			{
				_recMsgList.Remove(_recMsgList[0]);
				_recMsgList.Add(addMsg);
			}
			DataUpdate (UPDATE_CHAT_MSG);
		}


		//发送10-1协议（综合，阵营聊天请求）给服务器
		public void SendPublicMsg(SendMesVo sendMes)
		{
			Log.info(this, "发送10-1（综合，阵营聊天请求）给服务器");

			MemoryStream msdata = new MemoryStream ();
			Module_10.write_10_1 (msdata, sendMes.sendChatType, sendMes.content, goods);
			AppNet.gameNet.send(msdata, 10 , 1);
		}

		//发送10-2协议（私聊请求给服务器）
		public void SendPrivateMsg(SendMesVo sendMes)
		{
			Log.info(this, "发送10-2私聊请求给服务器");
			MemoryStream msdata = new MemoryStream ();
			if (sendMes.privateChatName != null)
			{
//				roleId = nameToIdDic [ReciveNameSL];
				Module_10.write_10_2 (msdata, sendMes.privateChatRoleId, sendMes.content, goods);
				AppNet.gameNet.send(msdata, 10 , 2);
			}
			else
			{
				MessageManager.Show("未指定私聊对象");
			}

			
		}
	
	}
}
