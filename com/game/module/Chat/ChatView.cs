using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.vo;
using com.game.module.main;
using com.game.Public.LocalVar;
using Com.Game.Module.Friend;
using com.game.module.Mail;
using Com.Game.Module.Role;
using Com.Game.Module.Tips;
using com.game.manager;
using com.game.consts;
using Com.Game.Module.Manager;

namespace Com.Game.Module.Chat
{
	public class ChatView : BaseView<ChatView> {
		public override string url { get { return "UI/Chat/ChatView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; } }

		private UIToggle ckb_zonghe;
		private UIToggle ckb_zhenying;
		private UIToggle ckb_siliao;
		private UIToggle ckb_rec_channel_zhenying_set;
		private UIToggle ckb_rec_channel_zonghe_set;
		private UIToggle ckb_rec_channel_siliao_set;

		private Button btn_close_chat;
		private Button btn_fs;

		private Button btn_fsChannel_0;
		private Button btn_fsChannel_1;
		private Button btn_fsChannel_2;
		private Button ckb_jsChannel_zh;
		private Button ckb_jsChannel_zy;
		private Button ckb_jsChannel_sl;
		private Button btn_biaoqing;
		private GameObject gnView;
		private GameObject jsChanelView;
		private GameObject emotionPkgView;
		private Button btn_jsChannel_set;
		private Button btn_gn_sl;
//		private Button btn_gn_fsyj;
		private Button btn_gn_jwhy;
		private Button btn_gn_jhmd;
		private Button btn_gn_xxzl;
		private Transform biaoqing_packge;
        private UIScrollView chat_scroll_view;

		public UIInput msgInput;
		public UIPanel MsgPannel;

		public GameObject msg;
		public Transform msgs;
		public UISprite msgBackground;
		public List<GameObject> msgList = new List<GameObject> ();
		public UILabel chatContent;

		private bool lockSendChannel = false;   //发送频道锁定标识
		public SendMesVo sendMessage = new SendMesVo();
		private string clickName;
		private byte openChatType = 1;        //聊天窗口类型，1为默认打开方式，7为私聊窗口
		private Dictionary<byte, bool> recChatTypeDic = new Dictionary<byte, bool> ();

		private float nextPosX = 0;
		private float nextPosY = 0;
		private float startPosY = -30;
		private Font contentFont;

		public List<string> sendHistoryList = new List<string>();
		private int curHistoryIndex = 0;

		protected override void Init()
		{
			InitLabelLanguage ();
//			transform.localPosition = new Vector3 (0, -94, 0);
			msgBackground = FindInChild<UISprite>("center/Msgbackground");
			MsgPannel = FindInChild<UIPanel>("center/MsgPannel");
			msgs = FindInChild<Transform>("center/MsgPannel/Msgs");
            chat_scroll_view = MsgPannel.GetComponent<UIScrollView>();
			//上下，左右的间隔要分别相等
			MsgPannel.clipRange = new Vector4 (0, 0, msgBackground.width - ChatConfig.LEFT_BELOW - ChatConfig.RIGHT_BELOW + 1,
			                                   msgBackground.height - ChatConfig.UP_BELOW - ChatConfig.DOWN_BELOW + 1);
			msgs.transform.localPosition = new Vector3 (-MsgPannel.clipRange.z / 2.0f, MsgPannel.clipRange.w / 2.0f - ChatConfig.FONT_SIZE, 0);

			ckb_zonghe = FindInChild<UIToggle>("top/ckb_zonghe");
			ckb_zhenying = FindInChild<UIToggle>("top/ckb_zhenying");
			ckb_siliao = FindInChild<UIToggle>("top/ckb_siliao");
			ckb_zonghe.onStateChange = this.ZongHeChannelOnStateChange;
			ckb_zhenying.onStateChange = this.ZhenYingChannelZongHeOnStateChange;
			ckb_siliao.onStateChange = this.SiLiaoChannelZongHeOnStateChange;

			ckb_rec_channel_zhenying_set = FindInChild<UIToggle>("UpPannel/jsChannel/zy");
			ckb_rec_channel_zonghe_set = FindInChild<UIToggle>("UpPannel/jsChannel/zh");
			ckb_rec_channel_siliao_set = FindInChild<UIToggle>("UpPannel/jsChannel/sl");
			ckb_rec_channel_zhenying_set.onStateChange = this.ZY_RecChannelSetOnStateChange;
			ckb_rec_channel_zonghe_set.onStateChange = this.ZH_RecChannelSetOnStateChange;
			ckb_rec_channel_siliao_set.onStateChange = this.SL_RecChannelSetOnStateChange;

			btn_jsChannel_set = FindInChild<Button>("center/btn_shezhi");
			btn_jsChannel_set.onClick = SetRecChannelOnClick;

			btn_close_chat = FindInChild<Button>("topright/btn_close_chat");
			chatContent = FindInChild<UILabel>("center/Input/content");
			btn_fsChannel_0 = FindInChild<Button>("UpPannel/fsChannel/btn_0");
			btn_fsChannel_0.onClick = SetSendChannelOnClick;
			btn_fsChannel_1 = FindInChild<Button>("UpPannel/fsChannel/btn_1");
			btn_fsChannel_1.onClick = SetSendChannelOnClick;
			btn_fsChannel_2 = FindInChild<Button>("UpPannel/fsChannel/btn_2");
			btn_fsChannel_2.onClick = SetSendChannelOnClick;

			btn_biaoqing = FindInChild<Button>("center/btn_biaoqing");
			btn_biaoqing.onClick = EmotionFuncOnClick;
			msgInput = FindInChild<UIInput>("center/Input");
			msgInput.characterLimit = 48;


			ckb_jsChannel_zh = FindInChild<Button>("top/ckb_zonghe");
			ckb_jsChannel_zy = FindInChild<Button>("top/ckb_zhenying");
			ckb_jsChannel_sl = FindInChild<Button>("top/ckb_siliao");
			btn_fs = FindInChild<Button>("center/btn_fs");
			gnView = FindInChild<Transform>("UpPannel/gn").gameObject;

			msgBackground.GetComponent<UIWidgetContainer> ().onClick = MsgBackgroundOnClick;
			btn_gn_sl = FindInChild<Button> ("UpPannel/gn/background/btn_sl");
			btn_gn_sl.onClick = this.SWSLOnClick;
//			btn_gn_fsyj = FindInChild<Button> ("UpPannel/gn/background/btn_fsyj");
//			btn_gn_fsyj.onClick = FSYJOnClick;
			btn_gn_jwhy = FindInChild<Button> ("UpPannel/gn/background/btn_jwhy");
			btn_gn_jwhy.onClick = JWHYOnClick;
			btn_gn_jhmd = FindInChild<Button> ("UpPannel/gn/background/btn_jhmd");
			btn_gn_jhmd.onClick = JHMDOnClick;
			btn_gn_xxzl = FindInChild<Button> ("UpPannel/gn/background/btn_xxzl");
			btn_gn_xxzl.onClick = XXZLOnClick;

			jsChanelView = FindInChild<Transform>("UpPannel/jsChannel").gameObject;
			emotionPkgView = FindInChild<Transform>("UpPannel/bq").gameObject;

			//直接克隆出最大条数的msg对象缓存着使用
			msg = FindInChild<Transform>("center/MsgPannel/Msgs/msg").gameObject;
			msg.GetComponent<UIWidgetContainer> ().onClick = MsgOnClick;
			msg.transform.FindChild ("zy/tc1/name").GetComponent<Button> ().onClick = NameOnClick;
			msgList.Add (msg);
			for (int i = 0; i < ChatConfig.MSG_NUM_MAX - 1; ++i)
			{
				GameObject msgClone = GameObject.Instantiate(Singleton<ChatView>.Instance.msg, Singleton<ChatView>.Instance.msg.transform.position, 
				                                        Singleton<ChatView>.Instance.msg.transform.rotation) as GameObject;
				msgClone.transform.parent = msg.transform.parent;
				msgClone.GetComponent<Button> ().onClick = MsgOnClick;
				msgClone.transform.FindChild ("zy/tc1/name").GetComponent<Button> ().onClick = NameOnClick;


				msgList.Add(msgClone);
			}

			btn_close_chat.onClick = CloseChatOnClick;
			btn_fs.onClick = SendMsgOnClick;

			//表情
			biaoqing_packge = FindInChild<Transform>("UpPannel/bq/bq_packge");
			foreach(Transform child in biaoqing_packge)
			{
//			Button biaoqing = biaoqing_packge.FindChild ("bq_001").GetComponent<Button> ();
				child.GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Chat);
				child.GetComponent<Button>().onClick = EmotionOnClick;
				Singleton<ChatMode>.Instance.emoDic.Add(child.GetComponent<UISprite>().spriteName.Split(new char[] {'_'})[1],true);
			}

			recChatTypeDic.Add ((byte)ChatType.ZongHe, true);
			recChatTypeDic.Add ((byte)ChatType.ZhenYing, true);
			recChatTypeDic.Add ((byte)ChatType.SiLiao, true);
//			Log.info(this, "接收频道字典已初始化");

//			msgInput.onSubmit.Add( msgEnterOnClick);
			EventDelegate.Add (msgInput.onSubmit, msgEnterOnClick);
        }

		private void msgEnterOnClick()
		{
			Log.info(this, "Enter");
			//SendMsgOnClick(btn_fs.gameObject);
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("top/ckb_zonghe/label").text = LanguageManager.GetWord("ChatView.zonghe");
			FindInChild<UILabel>("top/ckb_zhenying/label").text = LanguageManager.GetWord("ChatView.zhenying");
			FindInChild<UILabel>("top/ckb_siliao/label").text = LanguageManager.GetWord("ChatView.siliao");
			FindInChild<UILabel>("center/btn_fs/label").text = LanguageManager.GetWord("ChatView.send");
			FindInChild<UILabel>("UpPannel/gn/background/btn_xxzl").text = LanguageManager.GetWord("ChatView.roleData");
			FindInChild<UILabel>("UpPannel/gn/background/btn_jwhy").text = LanguageManager.GetWord("ChatView.addFriend");
			FindInChild<UILabel>("UpPannel/gn/background/btn_jhmd").text = LanguageManager.GetWord("ChatView.addBlack");
//			FindInChild<UILabel>("UpPannel/gn/background/btn_fsyj").text = LanguageManager.GetWord("ChatView.sendMail");
			FindInChild<UILabel>("UpPannel/gn/background/btn_sl").text = LanguageManager.GetWord("ChatView.siliao2");
			FindInChild<UILabel>("UpPannel/gn/background/btn_ckwp").text = LanguageManager.GetWord("ChatView.checkGoods");
			FindInChild<UILabel>("UpPannel/jsChannel/wenzi").text = LanguageManager.GetWord("ChatView.setReceiveChannel");
			FindInChild<UILabel>("UpPannel/jsChannel/zh/wenzi").text = LanguageManager.GetWord("ChatView.zongheChannel");
			FindInChild<UILabel>("UpPannel/jsChannel/zy/wenzi").text = LanguageManager.GetWord("ChatView.zhenyingChannel");
			FindInChild<UILabel>("UpPannel/jsChannel/sl/wenzi").text = LanguageManager.GetWord("ChatView.siliaoChannel");
		}

		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
//			Singleton<ChatMode>.Instance.chatViewOpenFlag = true;
			//设置发送频道标识为未手动设置,默认发送频道为综合
			InitSendChannel ();
			//读取玩家配置的接收频道设置信息
			InitReceiveChannelSet ();
			//切换面板
			vp_Timer.In(0.1f, AutoSetChannelPanel, 1, 0);

			switch(openChatType)
			{
				case ((byte)ChatType.SiLiao):
					SetSendChannel(openChatType);
					break;
				default:
					break;
			}

			this.ShowMsgs();
		}

		private void AutoSetChannelPanel()
		{
			if (this.openChatType == (byte)ChatType.SiLiao)
			{
				ckb_siliao.value = true;
			}
			else
			{
				ckb_zonghe.value = true;
			}
		}

		//初始化发送频道为未锁定
		private void InitSendChannel()
		{
			lockSendChannel = false;  
//			sendMessage.sendChatType = (byte)ChatType.ZongHe;
//			this.btn_fsChannel_0.FindInChild<UILabel>("name").text = "综合";
//			this.msgInput.defaultText = "点此输入你想说的话：";
//			if (this.msgInput.value == "")
//				chatContent.text = this.msgInput.defaultText;
		}

		//初始化接收频道设置信息
		private void InitReceiveChannelSet()
		{
//			//刚打开时默认为选择综合接收频道
//			recChatTypeDic [(byte)ChatType.ZongHe] = true;
//			recChatTypeDic [(byte)ChatType.ZhenYing] = true;
//			recChatTypeDic [(byte)ChatType.SiLiao] = true;

			//从本地读取接收频道的设置信息
			ckb_rec_channel_zhenying_set.startsActive = 
				(LocalVarManager.GetInt (LocalVarManager.CHAT_REC_ZH_CHANNEL, 0) == (int)ReceiveState.REJECT ? false : true);
			ckb_rec_channel_zonghe_set.startsActive =
				(LocalVarManager.GetInt (LocalVarManager.CHAT_REC_ZY_CHANNEL, 0) == (int)ReceiveState.REJECT ? false : true);
			ckb_rec_channel_siliao_set.startsActive =
				(LocalVarManager.GetInt (LocalVarManager.CHAT_REC_SL_CHANNEL, 0) == (int)ReceiveState.REJECT ? false : true);
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<ChatMode>.Instance.dataUpdated += UpdateMsg;
			Singleton<ChatMode>.Instance.dataUpdated += UpdateUpArrowHandle;
			Singleton<ChatMode>.Instance.dataUpdated += UpdateDownArrowHandle;
		}

		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<ChatMode>.Instance.dataUpdated -= UpdateMsg;
			Singleton<ChatMode>.Instance.dataUpdated -= UpdateUpArrowHandle;
			Singleton<ChatMode>.Instance.dataUpdated -= UpdateDownArrowHandle;
		}

		private void UpdateUpArrowHandle(object sender, int type)
		{
			if (Singleton<ChatMode>.Instance.UPDATE_UPARROW == type)
			{
				SetInputText(true);
			}
		}

		private void UpdateDownArrowHandle(object sender, int type)
		{
			if (Singleton<ChatMode>.Instance.UPDATE_DOWNARROW == type)
			{
				SetInputText(false);
			}
		}

		private void SetInputText(bool upArrow)
		{
			if (!msgInput.isSelected || sendHistoryList.Count<1)
			{
				return;
			}

			if (upArrow)
			{
				curHistoryIndex--;
			}
			else
			{
				curHistoryIndex++;
			}

			if (curHistoryIndex < 0)
			{
				curHistoryIndex = 0;
			}
			else if (curHistoryIndex > sendHistoryList.Count-1)
			{
				curHistoryIndex = sendHistoryList.Count-1;
			}

			msgInput.value = sendHistoryList[curHistoryIndex]; 
		}

		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
//			Singleton<ChatMode>.Instance.chatViewOpenFlag = false;
		}


		//聊天窗口关闭按钮
		private void CloseChatOnClick(GameObject go)
		{
			this.CloseView ();
		}

		//发送按钮
		private void SendMsgOnClick(GameObject go)
		{
			if (chatContent.text != this.msgInput.defaultText)
			{
                string temp = this.msgInput.value;  //只要还有敏感词就要一直过滤下去
                //先过滤敏感词
//                while (ContainWord(temp) != "*")   //没有处理完屏蔽词汇
//                {                    
//                    temp = ContainWord(temp);
//                }
				temp = FilterContent(temp);
				temp = temp.Trim();

				if (string.Empty == temp)
				{
					msgInput.value = string.Empty;
					return;
				}

                //Log.debug(this,"处理结果     "+ temp);
                sendMessage.content = temp; // this.msgInput.value;
				switch (sendMessage.sendChatType)
				{
					case (byte)ChatType.ZongHe:
					case (byte)ChatType.ZhenYing:
						Singleton<ChatMode>.Instance.SendPublicMsg(sendMessage);
						break;
					case (byte)ChatType.SiLiao:
						Singleton<ChatMode>.Instance.SendPrivateMsg(sendMessage);
						break;
					default:
						Log.error(this, "聊天类型异常:" + sendMessage.sendChatType);
						break;
				}
				this.emotionPkgView.SetActive(false);

				if (0 == sendHistoryList.Count
				    || sendHistoryList[sendHistoryList.Count-1] != sendMessage.content)
				{
					sendHistoryList.Add(sendMessage.content);
					curHistoryIndex = sendHistoryList.Count;
				}
				else
				{
					curHistoryIndex = sendHistoryList.Count;
				}
			}
		}

        

        /// <summary>
        /// 是否包含敏感词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ContainWord(string str)
        {
            for (int i = 0; i < Singleton<ChatMode>.Instance.FilterString.Length; i++)
            {
                if (str.Contains(Singleton<ChatMode>.Instance.FilterString[i]))
                {
                    str = str.Replace(Singleton<ChatMode>.Instance.FilterString[i], "**");
                    return str;
                }
            }
            return "*";  //表示处理结束
        }

		private string FilterContent(string str)
		{
			for (int i=0; i<Singleton<ChatMode>.Instance.FilterString.Length; i++)
			{
				if (str.Contains(Singleton<ChatMode>.Instance.FilterString[i]))
				{
					str = str.Replace(Singleton<ChatMode>.Instance.FilterString[i], "");
				}
			}

			return str;
		}


        public bool ContainsFilter(string str)
        {
            for (int i = 0; i < Singleton<ChatMode>.Instance.FilterString.Length; i++)
            {
                if (str.Contains(Singleton<ChatMode>.Instance.FilterString[i]))
                {
                    return true;
                }
            }
            return false;
        }
		//人物名字被点击
		private void NameOnClick(GameObject go)
		{
			if (go.GetComponent<UILabel>().text != MeVo.instance.Name
			    && go.GetComponent<UILabel>().text != LanguageManager.GetWord("ChatView.SysNotice"))
			{
				gnView.transform.position = go.transform.position;
				string name = go.GetComponent<UILabel>().text;
				name = name.Replace("[u]", "");
				name = name.Replace("[/u]", "");
				this.clickName = name;
				gnView.gameObject.SetActive (true);
			}
		}

		//消息背景框被点击
		private void MsgBackgroundOnClick(GameObject go)
		{
			gnView.gameObject.SetActive (false);
		}

		//消息被点击
		private void MsgOnClick(GameObject go)
		{
			gnView.gameObject.SetActive (false);
		}

		//接收频道，综合频道勾选/去掉勾选
		private void ZH_RecChannelSetOnStateChange(bool state)
		{
			Log.info (this, "勾选综合频道接收？" + state);
			LocalVarManager.SetInt (LocalVarManager.CHAT_REC_ZH_CHANNEL, (int)(state ? ReceiveState.RECEIVE : ReceiveState.REJECT));
		}
		//接收频道，阵营频道勾选/去掉勾选
		private void ZY_RecChannelSetOnStateChange(bool state)
		{
			Log.info (this, "勾选阵营频道接收？" + state);
			LocalVarManager.SetInt (LocalVarManager.CHAT_REC_ZY_CHANNEL, (int)(state ? ReceiveState.RECEIVE : ReceiveState.REJECT));
		}
		//接收频道，私聊频道勾选/去掉勾选
		private void SL_RecChannelSetOnStateChange(bool state)
		{
			Log.info (this, "勾选私聊频道接收？" + state);
			LocalVarManager.SetInt (LocalVarManager.CHAT_REC_SL_CHANNEL, (int)(state ? ReceiveState.RECEIVE : ReceiveState.REJECT));
		}

		//选择/离开综合频道
		private void ZongHeChannelOnStateChange(bool state)
		{
			if (state)
			{
				Log.info(this, "选择综合频道");
				recChatTypeDic [(byte)ChatType.ZongHe] = true;
				recChatTypeDic [(byte)ChatType.ZhenYing] = true;
				recChatTypeDic [(byte)ChatType.SiLiao] = true;
				msgInput.value = "";
				this.ShowMsgs ();


				if (!lockSendChannel)
				{
					//设置发送频道
					SetSendChannel((byte)ChatType.ZongHe);
				}
			}
			else
			{
//				Log.info(this, "离开综合频道");
			}
		}

		//选择/离开阵营频道
		private void ZhenYingChannelZongHeOnStateChange(bool state)
		{
			if (state)
			{
				Log.info(this, "选择阵营频道");
				recChatTypeDic [(byte)ChatType.ZongHe] = false;
				recChatTypeDic [(byte)ChatType.ZhenYing] = true;
				recChatTypeDic [(byte)ChatType.SiLiao] = false;
				this.ShowMsgs ();

				if (!lockSendChannel)
				{
					SetSendChannel((byte)ChatType.ZhenYing);
				}
			}
			else
			{
//				Log.info(this, "离开阵营频道");
			}
		}

		//选择/离开私聊频道
		private void SiLiaoChannelZongHeOnStateChange(bool state)
		{
			if (state)
			{
				Log.info(this, "选择私聊频道");
				recChatTypeDic [(byte)ChatType.ZongHe] = false;
				recChatTypeDic [(byte)ChatType.ZhenYing] = false;
				recChatTypeDic [(byte)ChatType.SiLiao] = true;
				msgInput.value = "";
				this.ShowMsgs ();
				if (!lockSendChannel)
				{
					SetSendChannel((byte)ChatType.SiLiao);
				}

			}
			else
			{
//				Log.info(this, "离开私聊频道");
			}
		}

		//设置发送频道
		private void SetSendChannel(byte sendChannel)
		{
			Log.info (this, "发送频道被修改为：" + sendChannel);
			this.sendMessage.sendChatType = sendChannel;
			switch (sendChannel)
			{
				case (byte)ChatType.ZongHe:
					this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.zonghe");
					this.msgInput.defaultText = LanguageManager.GetWord("ChatView.defaultText");
					break;
				case (byte)ChatType.ZhenYing:
					this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.zhenying");
					this.msgInput.defaultText = LanguageManager.GetWord("ChatView.defaultText");
					break;
				case (byte)ChatType.SiLiao:
					this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.siliao");
					if (sendMessage.privateChatName != null)
						this.msgInput.defaultText = LanguageManager.GetWord("ChatView.youSay") + sendMessage.privateChatName + LanguageManager.GetWord("ChatView.speak");
					else
						this.msgInput.defaultText = LanguageManager.GetWord("ChatView.selectTalkRole");
					break;
				default:
					Log.error(this, "发送频道异常:" + sendChannel);
					break;
			}
			if (this.msgInput.value == "")
				chatContent.text = this.msgInput.defaultText;
		}


		//点击设置接收频道按钮
		private void SetRecChannelOnClick(GameObject go)
		{
			jsChanelView.SetActive (!jsChanelView.activeSelf);
			gnView.gameObject.SetActive (false);
		}

		//点击发送频道设置按钮
		private void SetSendChannelOnClick(GameObject go)
		{
			gnView.gameObject.SetActive (false);
			this.lockSendChannel = true;
			this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.zonghe");
			switch (go.name)
			{
				case "btn_0":
					this.SetSendChannel((byte)ChatType.ZongHe);
					break;
				case "btn_1":
					this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.siliao");
					this.SetSendChannel((byte)ChatType.SiLiao);
					break;
				case "btn_2":
					this.btn_fsChannel_0.FindInChild<UILabel>("name").text = LanguageManager.GetWord("ChatView.zhenying");
					this.SetSendChannel((byte)ChatType.ZhenYing);
					break;
				default:
					Log.error(this, "发送频道按钮名字异常:" + go.name);
					break;
			}
			btn_fsChannel_1.gameObject.SetActive (!btn_fsChannel_1.gameObject.activeSelf);
			btn_fsChannel_2.gameObject.SetActive (!btn_fsChannel_2.gameObject.activeSelf);
		}

		//点击表情按钮
		private void EmotionFuncOnClick(GameObject go)
		{
			gnView.gameObject.SetActive (false);
			this.emotionPkgView.SetActive (!this.emotionPkgView.activeSelf);
		}

		//选中某个具体的表情
		private void EmotionOnClick(GameObject go)
		{
			this.emotionPkgView.SetActive (false);
			string bqId = "/" + go.GetComponent<UISprite>().spriteName.Split(new char[] {'_'})[1];
			this.msgInput.value += bqId;
			//ChatMode.MAX_MSG_LENGTH += 3;
		}

		//设为私聊对象按钮
		private void SWSLOnClick(GameObject go)
		{
			//关闭点击名字弹出的功能框
			gnView.gameObject.SetActive (false);
			ckb_siliao.value = true;
			//更新私聊对象信息
			this.SetPrivateChatObject (this.clickName, Singleton<ChatMode>.Instance.nameToIdDic [this.clickName]);
		}

		//根据玩家ID和玩家姓名设置私聊对象
		public void SetPrivateChatObject(string name, uint Id)
		{

			sendMessage.privateChatName = name;
			sendMessage.privateChatRoleId = Id;
			this.SetSendChannel ((byte)ChatType.SiLiao);
		}

		//加为好友按钮点击事件
		private void JWHYOnClick(GameObject go)
		{
			Log.info(this, "加为好友:" + this.clickName);
			Singleton<FriendControl>.Instance.SendFriendAskByName (this.clickName);
		}

		//加黑名单按钮点击事件
		private void JHMDOnClick(GameObject go)
		{
			Log.info(this, "加黑名单:" + this.clickName);
			Singleton<FriendControl>.Instance.MoveToBlackList (Singleton<ChatMode>.Instance.nameToIdDic[this.clickName]);
		}

//		//发送邮件按钮点击事件
//		private void FSYJOnClick(GameObject go)
//		{
//			Log.info(this, "发送邮件给" + this.clickName);
//			Singleton<MailView>.Instance.OpenOutboxView (this.clickName);
//		}

		//详细资料按钮点击事件
		private void XXZLOnClick(GameObject go)
		{
			Singleton<PlayerDetailView>.Instance.ShowWindow(Singleton<ChatMode>.Instance.nameToIdDic[this.clickName]);
			//Singleton<GoodsView>.Instance.ShowEquipView(Singleton<ChatMode>.Instance.nameToIdDic[this.clickName],OpenRoleInfoHandle,CloseRoleInfoHandle);
			Log.info(this, "详细资料按钮被点击");
		}
		//打开玩家信息界面
		private void OpenRoleInfoHandle(GameObject go)
		{
		}
		//关闭玩家信息界面
		private void CloseRoleInfoHandle(GameObject go )
		{
		}

		//更新消息显示框的内容
		private void UpdateMsg(object sender, int code)
		{
			if (code == Singleton<ChatMode>.Instance.UPDATE_CHAT_MSG)
			{
				//显示消息对象对象更新显示
				this.ShowMsgs ();
			}
		}
		
		//显示消息内容
		private void ShowMsgs()
		{
			Log.info (this, "更新聊天框信息");
			//先将所有消息对象置为false
			foreach (GameObject msgObj in Singleton<ChatView>.Instance.msgList)
			{
				if (!msgObj.activeSelf)
					break;
			}

			nextPosY = startPosY;
			GameObject msg;
			int msgIndex = 0;
			float bottomPosY = 0;
            int nCount = 0;
			float magTotalHeight = 0f;
			foreach (ChatVo recChat in Singleton<ChatMode>.Instance.recChatVoList)
			{
                nCount++;
				if (recChatTypeDic[recChat.chatType])
				{
					msg = Singleton<ChatView>.Instance.msgList[msgIndex++];

					//设置完成，显示
					msg.gameObject.SetActive(true);  //---modify by lixi先显示，再更新，因为NGUI做的改动会导致在未激活状态下，更新文本框内容时不会刷新文本框的长宽属性

					msg.transform.localPosition = new Vector3(nextPosX, nextPosY, 0);
					msg.transform.localRotation = new Quaternion(0, 0, 0, 0);
					msg.transform.localScale = new Vector3(1, 1, 1);
					
					//设置阵营相关信息
					UILabel zhenying = msg.transform.FindChild("zy").GetComponent<UILabel>();
					zhenying.color = Singleton<ChatMode>.Instance.GREEN;
					zhenying.fontSize = ChatConfig.FONT_SIZE;
					zhenying.text = recChat.nationId != 0? "[" + LanguageManager.GetWord("ChatView.zhenying") + recChat.nationId + "]": "";
					
					//设置填充1相关内容
					UILabel tc1 = zhenying.transform.FindChild("tc1").GetComponent<UILabel>();
					tc1.transform.localPosition = new Vector3(zhenying.width, 0, 0);
					tc1.color = Singleton<ChatMode>.Instance.PURPLE;
					tc1.fontSize = ChatConfig.FONT_SIZE;
					tc1.text = (recChat.chatType == (byte)ChatType.SiLiao)? (recChat.senderId == MeVo.instance.Id? LanguageManager.GetWord("ChatView.youSay"): ""): "";
					
					//设置名字
					UILabel name = tc1.transform.FindChild("name").GetComponent<UILabel>();
					name.transform.localPosition = new Vector3(tc1.width, 0, 0);
					name.color = (recChat.chatType == (byte)ChatType.SiLiao)?Singleton<ChatMode>.Instance.PURPLE:
						(recChat.chatType == (byte)ChatType.ZongHe?Singleton<ChatMode>.Instance.YELLOW:Singleton<ChatMode>.Instance.GREEN);
					//系统公告需要设置为红色
					if (GameConst.SystemNoticeId == recChat.senderId)
					{
						name.color = Color.red;
					}
					name.fontSize = ChatConfig.FONT_SIZE;
					//					name.text = ((recChat.chatType == 7) && recChat.senderId == MeVo.instance.Id)? Singleton<ChatMode>.Instance.ReciveNameSL: recChat.senderName;
					name.text = ((recChat.chatType == (byte)ChatType.SiLiao) && recChat.senderId == MeVo.instance.Id)? sendMessage.privateChatName: recChat.senderName;

					//世界中别的玩家名字加下划线
					if (recChat.chatType == (byte)ChatType.ZongHe)
					{
						if (MeVo.instance.Id != recChat.senderId
						    && GameConst.SystemNoticeId != recChat.senderId)
						{
							name.text = "[u]" + name.text + "[/u]";
						}
					}

					BoxCollider boxCollider = name.gameObject.GetComponent<BoxCollider>();
					if (null == boxCollider)
					{
						name.gameObject.AddComponent<BoxCollider>();
					}
					boxCollider.size = new Vector3(name.width, name.height, 1);
					boxCollider.center = new Vector3(name.width / 2.0f, name.height / 2.0f, 0);
					
					
					//设置填充2
					UILabel tc2 = name.transform.FindChild("tc2").GetComponent<UILabel>();
					tc2.transform.localPosition = new Vector3(name.width, 0, 0);
					tc2.color = (recChat.chatType == (byte)ChatType.SiLiao)?Singleton<ChatMode>.Instance.PURPLE: 
						(recChat.chatType == (byte)ChatType.ZongHe?Singleton<ChatMode>.Instance.WHITE:Singleton<ChatMode>.Instance.GREEN);
					tc2.fontSize = ChatConfig.FONT_SIZE;
					tc2.text = (recChat.chatType == (byte)ChatType.SiLiao)? ((recChat.senderId == MeVo.instance.Id)? LanguageManager.GetWord("ChatView.speak")
					                                   											: LanguageManager.GetWord("ChatView.sayToYou"))
													  : ": ";
					
					//设置content
					UILabel content = tc2.transform.FindChild("content").GetComponent<UILabel>();
					content.fontSize = ChatConfig.FONT_SIZE;
					content.width = (int)(Singleton<ChatView>.Instance.MsgPannel.clipRange.z - 
					                      zhenying.width - tc1.width - name.width - tc2.width);
					//					content.width = content.width - content.width % ChatMode.FONT_SIZE;  
					//content.color = (recChat.chatType == (byte)ChatType.SiLiao)?Singleton<ChatMode>.Instance.PURPLE: 
						//(recChat.chatType == (byte)ChatType.ZongHe?Singleton<ChatMode>.Instance.WHITE:Singleton<ChatMode>.Instance.GREEN);
					content.color = Singleton<ChatMode>.Instance.WHITE;
					content.text = recChat.content;
					ShowEmotion(content, content.processedText);
					content.transform.localPosition = new Vector3(tc2.width, -content.height + content.fontSize, 0);
					
					//设置消息框的碰撞器范围
					float msgWidth = zhenying.width + tc1.width + name.width + tc2.width + content.width;
					msg.GetComponent<BoxCollider>().size = new Vector3 (msgWidth, content.height + ChatConfig.RAW_DELTA, 1);
					msg.GetComponent<BoxCollider>().center = new Vector3 (msgWidth / 2.0f, -content.height / 2.0f + ChatConfig.FONT_SIZE, 0);
					magTotalHeight += msg.GetComponent<BoxCollider>().size.y;
					
					nextPosY = nextPosY - content.height - ChatConfig.RAW_DELTA;

					
					//计算是否需要自动翻滚
					bottomPosY = msg.transform.localPosition.y - msg.GetComponent<BoxCollider>().size.y;
					if (bottomPosY < -Singleton<ChatView>.Instance.MsgPannel.clipRange.w)
					{
						//自动上移
						startPosY += (-Singleton<ChatView>.Instance.MsgPannel.clipRange.w - bottomPosY);
						//						Log.info(this, "自动上移，startPOsY为：" + Singleton<ChatMode>.Instance.startPosY);
						this.ShowMsgs();
						break;
					}
				}
			}
            
			if (magTotalHeight > Singleton<ChatView>.Instance.MsgPannel.clipRange.w-32)
			{
                chat_scroll_view.enabled = true;
            }
            else 
			{
                chat_scroll_view.enabled = false;
            }

			//隐藏不需要显示的消息
			for (int i=msgIndex; i<Singleton<ChatView>.Instance.msgList.Count; i++)
			{
				msg = Singleton<ChatView>.Instance.msgList[i];
				msg.gameObject.SetActive(false);
			}
		}
		
		//替换表情
		private void ShowEmotion(UILabel content, string cont)
		{
			//			Transform emo = content.transform.FindChild("emo").GetComponent<Transform>();
			//			emo.gameObject.SetActive (false);
			
			List<EmotionVo> emotionVoList = new List<EmotionVo>();
			if (contentFont == null)
				contentFont = content.trueTypeFont;
			
			float emotionPosX = 0;
			float emotionPosY;
			string result = "";
			string emotionId = "";
			string newLine = "";
			
			char[] contentChat = cont.ToCharArray ();
			for (int i = 0; i < contentChat.Length; ++i)
			{
				if (contentChat[i] == '\n')
				{
					newLine = "\n";
					result += contentChat[i];
					continue;
				}
				
				if (contentChat[i] == '/')
				{
					Log.info(this, "calX:" + NGUIText.CalculatePrintedSize (newLine).x);
					Log.info(this, "calY:" + NGUIText.CalculatePrintedSize (newLine).y);
					emotionId = "";
					int len = (contentChat.Length - 1 - i)>2?3:(contentChat.Length - 1 - i);
					int j = 1;
					for (j = 1; j <= len; ++j)
					{
						if (contentChat[i + j] == '\n')
						{
							++len;
							result += '\n';
							newLine = "\n";
							continue;
						}
						emotionId += contentChat[i + j];
					}
					
					i = i + j - 1;
					
					if (Singleton<ChatMode>.Instance.emoDic.ContainsKey(emotionId))
					{
						result += "：";
						newLine += "：";
						
						//						emo.gameObject.SetActive(true);
						emotionPosX = NGUIText.CalculatePrintedSize ( newLine).x % content.width;
						emotionPosY = content.height - NGUIText.CalculatePrintedSize (newLine).y;
						//						emo.localPosition = new Vector3(emotionPosX - ChatMode.FONT_SIZE, emotionPosY, 0);
						emotionVoList.Add(new EmotionVo(new Vector3(emotionPosX - ChatConfig.FONT_SIZE, emotionPosY, 0), "bq_" + emotionId));
						
					}
					else
					{
						result += "/" + emotionId;
						newLine += "/" + emotionId;
					}
				}
				else
				{
					result += contentChat[i];
					newLine += contentChat[i];
				}
			}
			content.text = result;
			
			int emoNum = content.transform.childCount;
			Transform emo = content.transform.FindChild("emo").GetComponent<Transform>();
			emo.GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Chat);
			
			while (content.transform.childCount < emotionVoList.Count)
			{
				Transform emoClone = (GameObject.Instantiate(emo.gameObject, emo.position, emo.rotation) as GameObject).transform;
				emoClone.parent = emo.parent;
			}
			
			int sub = 0;
			foreach (Transform child in content.transform)
			{
				if (sub < emotionVoList.Count)
				{
					child.localPosition = emotionVoList[sub].loacalPos;
					child.localRotation = new Quaternion(0, 0, 0, 0);
					child.localScale = new Vector3(1, 1, 1);
					child.gameObject.SetActive(true);
					child.GetComponent<UISprite>().spriteName = emotionVoList[sub].emoName;
					sub++;
				}
				else
				{
					child.gameObject.SetActive(false);
				}
			}
		}

		//打开私聊聊天view
		public void OpenPrivateChatView(string name, uint roleId)
		{
			this.openChatType = (byte)ChatType.SiLiao;
			sendMessage.privateChatName = name;
			sendMessage.privateChatRoleId = roleId;
			this.OpenView ();
		}
	}
}
