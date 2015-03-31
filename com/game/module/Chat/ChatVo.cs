using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using PCustomDataType;
namespace Com.Game.Module.Chat
{
	public class ChatConfig
	{
		public const int FONT_SIZE = 22;    //字体大小
		public const float RAW_DELTA = 9;     //行间距
		public const float LEFT_BELOW = 2.0f * FONT_SIZE;     //左端空白
		public const float RIGHT_BELOW = 2.0f * FONT_SIZE;     //右端空白
		public const float UP_BELOW = 1.0f * FONT_SIZE;     //顶部空白
		public const float DOWN_BELOW = 1.0f * FONT_SIZE;     //底部空白
		public const int MSG_NUM_MAX = 30;     //存储的最大消息数量
		//		public const int MAX_MSG_LENGTH = 80;  //发送单条消息的最大长度
		//public const float MinPosY = 
		//public const int VISIBLE_MSG_NUM_MAX = 10;
		
		public Color GREEN = new Color (74.0f / 255, 216.0f / 255, 59.0f / 255, 1);  //颜色不能为const类型变量
		public Color PURPLE = new Color (59.0f / 255, 143.0f / 255, 237.0f / 255, 1);  //颜色不能为const类型变量
		public Color YELLOW = new Color (1, 228.0f / 255, 1.0f / 255, 1);  //颜色不能为const类型变量
		public Color WHITE = new Color (1, 1, 1, 1);  //颜色不能为const类型变量
	}

	public class ChatVo {
		public byte chatType = 0;   //1：综合， 2：阵营， 7：私聊
		public byte nationId = 0;
		public uint senderId = 0;
		public ushort serverId = 0;
		public string senderName = "";
		public byte senderSex = 0;
		public byte senderJob = 0;
		public byte senderLvl = 0;
		public byte senderVip = 0;
		public string content = "";
		public PChatPushGoods goods = new PChatPushGoods();
	}

//	public class ChatPlayerPrefs
//	{
//		public const string REC_ZH_CHANNEL = "RecieveZongHeChannel";
//		public const string REC_ZY_CHANNEL = "RecieveZhenYingChannel";
//		public const string REC_SL_CHANNEL = "RecieveSiLiaoChannel";
//	}

	public enum ReceiveState
	{
		DEFAULT,  //默认为接收
		RECEIVE,  //接收该频道消息
		REJECT    //不接受该频道消息
	}

	public enum ChatType : byte   //1：综合， 2：阵营， 7：私聊
	{
		ZongHe = 1,          
		ZhenYing = 3,
		SiLiao = 7
	}

	public class PrivateChatObj
	{
		public string Objname;
		public uint roleId;
	}

	public class SendMesVo
	{
		public byte sendChatType;
		public string content;
		public string privateChatName;
		public uint privateChatRoleId;
	}
}