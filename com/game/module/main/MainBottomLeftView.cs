using com.game.consts;
using Com.Game.Module.Chat;
using com.game.module.test;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display.controler;
using System.Collections.Generic;
using Com.Game.Module.Manager;
using UnityEngine;

namespace com.game.module.main
{
    public class MainBottomLeftView : BaseView<MainBottomLeftView>
    {
        private float _leftClickTime; //上一次按左键的时间
        private float _rightClickTime; //上一次按右键的时间
        private Button btn_liaotian;
        private bool hitMapPoint;
        private UISprite joystickHighLight;
        public UISprite liaotianBg; //聊天按钮的背景图标
        private UILabel mainChatContent1;
        private NGUIJoystick nguiJoystick;
        private UILabel welMsg; //欢迎标签

        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        public bool HitMapPoint
        {
            get { return hitMapPoint; }

            set { hitMapPoint = value; }
        }

        public override bool isDestroy
        {
            get { return false; }
        }

        public bool RoleLocked = false;

        protected override void Init()
        {
            //聊天
            btn_liaotian = FindInChild<Button>("btn_liaotian");
            liaotianBg = FindInChild<UISprite>("btn_liaotian/background");
            btn_liaotian.onClick += ChatOnClick;
            mainChatContent1 = FindInChild<UILabel>("MainChat/wenzi1");
            nguiJoystick = FindChild("MoveControl/Joystick/Button").AddComponent<NGUIJoystick>();
            joystickHighLight = FindChild("MoveControl/Joystick/Background").GetComponent<UISprite>();
            nguiJoystick.radius = 80;
            welMsg = FindInChild<UILabel>("msg");
            vp_Timer.In(2f, CloseMsg, 1, 1f);
            base.showTween = FindInChild<TweenPlay>();
        }

        public void CloseMsg()
        {
            welMsg.gameObject.SetActive(false);
        }

        public override void RegisterUpdateHandler()
        {
            Singleton<ChatMode>.Instance.dataUpdated += ShowChatContent;
        }

        public override void CancelUpdateHandler()
        {
            Singleton<ChatMode>.Instance.dataUpdated -= ShowChatContent;
        }


        /// <summary>
        ///     新消息到达，闪动聊天图标背景给出提示
        /// </summary>
        /// <param name="alarm">是否给出提示，true：提示</param>
        public void NewMsgAlarm(UISprite liaotianBg1, bool alarm)
        {
            if (alarm)
            {
                Log.debug(this, "should highlight");
                liaotianBg1.spriteName = "btn_lt_highlight";
                liaotianBg1.GetComponent<Animator>().enabled = true;
            }
            else
            {
                Log.debug(this, "should not highlight");
                liaotianBg1.spriteName = "btn_lt_background";
                liaotianBg1.GetComponent<Animator>().enabled = false;
                liaotianBg1.transform.localScale = Vector3.one;
            }
        }

        //动态展示聊天内容
        private void ShowChatContent(object sender, int code)
        {
            if (code == Singleton<ChatMode>.Instance.UPDATE_MAIN_CHAT_CONTENT)
            {
                mainChatContent1.text = Singleton<ChatMode>.Instance.mainChatContent[0];
				ShowEmotion(mainChatContent1,  mainChatContent1.text);

//				ShowEmotion(mainChatContent2, mainChatContent2.text);
            }
        }

        private void ShowEmotion(UILabel content, string cont)
        {
			List<EmotionVo> emotionVoList = new List<EmotionVo>();
			Font contentFont = content.trueTypeFont;
			
			float emotionPosX = 0;
			float emotionPosY;
			string result = "";
			string emotionId = "";
			string newLine = "";
			
			char[] contentChat = cont.ToCharArray ();
			for (int i = 0; i < contentChat.Length; ++i)
			{
				if (contentChat[i] == '/')
				{
					emotionId = "";
					int len = (contentChat.Length - 1 - i)>2?3:(contentChat.Length - 1 - i);
					int j = 1;
					for (j = 1; j <= len; ++j)
					{
						emotionId += contentChat[i + j];
					}
					
					i = i + j - 1;
					
					if (Singleton<ChatMode>.Instance.emoDic.ContainsKey(emotionId))
					{
						result += "：";
						newLine += "：";
						
						//						emo.gameObject.SetActive(true);
						emotionPosX = NGUIText.CalculatePrintedSize (newLine).x % content.width;
						emotionPosY = content.height - NGUIText.CalculatePrintedSize (newLine).y-20.9f;
						//						emo.localPosition = new Vector3(emotionPosX - ChatMode.FONT_SIZE, emotionPosY, 0);
						if (NGUIText.CalculatePrintedSize (newLine).x <= content.width)
						{
							emotionVoList.Add(new EmotionVo(new Vector3(emotionPosX - ChatConfig.FONT_SIZE, emotionPosY, 0), "bq_" + emotionId));
						}
						else
						{

						}
						
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

        //点击聊天按钮，打开不聊天界面
        private void ChatOnClick(GameObject go)
        {
            Singleton<ChatControl>.Instance.OpenChatUI();
            NewMsgAlarm(liaotianBg, false); //点击了聊天，就取消高亮的提示
        }

        public override void Update()
        {
            if (AppMap.Instance.me == null)
            {
                return;
            }

            //主角是否移动到传送点
            if (AppMap.Instance.mapParser.MapVo.type == MapTypeConst.CITY_MAP)
            {
                if (null != AppMap.Instance.me && null != AppMap.Instance.me.Controller)
                {
					//玩家进入传送点检测范围中，只有走出后才能继续进入
					if (AppMap.Instance.InHitPointPos(AppMap.Instance.me.Controller.transform.position))					
					{
	                    if (!hitMapPoint)
	                    {
	                        hitMapPoint = AppMap.Instance.HitMapPoint();
							RoleLocked = hitMapPoint;
	                    }
					}
					else if (AppMap.Instance.InWorldMapPointPos(AppMap.Instance.me.Controller.transform.position))					
					{
						if (!hitMapPoint)
						{
							hitMapPoint = AppMap.Instance.HitWorldMapPoint();
							RoleLocked = hitMapPoint;
						}
					}
					else
					{
						hitMapPoint = false;
					}
                }
            }
            else
            {
                hitMapPoint = false;
            }

            Vector2 position = nguiJoystick.position;
            //Debug.Log("摇杆位置:" + position);
            if (position.x > 0 || position.y > 0 || position.x < 0 || position.y < 0)
            {
                var playerControler = AppMap.Instance.me.Controller as PlayerControler;
                if (playerControler != null)
                {
                    int dir = Directions.GetDirByVector2(position);
					if (!hitMapPoint || !RoleLocked)
					{
                    	playerControler.MoveByDir(dir);
					}
                    joystickHighLight.spriteName = "Joystick2";
                }
            }
            else
            {
                joystickHighLight.spriteName = "Joystick1";
            }
        }

        protected override void HandleAfterOpenView()
        {
            joystickHighLight.spriteName = "Joystick1";
            nguiJoystick.Reset();
        }


        private void AfterHitMapPoint()
        {
            //hitMapPoint = false;
        }
    }
}