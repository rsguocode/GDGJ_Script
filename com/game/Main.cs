/**应用层--主入口**/
using System;
using System.Collections.Generic;
using System.Reflection;
using com.game.cmd;
using com.game.consts;
using com.game.Helper;
using com.game.i18n;
using com.game.manager;
using com.game.module;
using Com.Game.Module.ContinueCut;
using Com.Game.Module.Copy;
using com.game.module.effect;
using Com.Game.Module.GoGo;
using com.game.module.hud;
using com.game.module.loading;
using com.game.module.login;
using Com.Game.Module.Manager;
using com.game.module.map;
using com.game.module.network;
using Com.Game.Module.Story;
using com.game.module.SystemData;
using com.game.module.test;
using Com.Game.Module.Tips;
using Com.Game.Module.Waiting;
using com.game.Public.Confirm;
using Com.Game.Public.EffectCameraManage;
using com.game.Public.InitManager;
using com.game.Public.Message;
using com.game.SDK;
using com.game.start;
using com.game.utils;
using com.net;
using com.net.interfaces;
using com.net.p8583;
using com.u3d.bases.debug;
using com.u3d.bases.loader;
using com.game.sound;
using Com.Game.Module.UpdateAnnounce;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
using Tools = com.game.utils.Tools;

namespace com.game
{
    public class Main : MonoBehaviour
    {
        private bool assetInit;
        private float connectTime; //连接时间点
        private bool connected;
        private int frameGap = 1; //帧处理间隔
        private int grameCMDNum = 30; //每帧业务处理数
        private Dictionary<String, NetMsgCallback> handlerList; //业务注册字典
        private bool isInit;
        private MapMode mapMode = null;
        private IList<INetData> netDataList; //接收数据列表 
        private int targetFrame = 30; //目标帧
        public  int TestSceneId;

        public bool NeedUpdated
        {
            get
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    //Iphone下需要Main触发更新
                    return true;
                }
                //其他环境不需要Main触发更新，由AppStart触发更新后加载Main
                return false;
            }
        }

        private void Start()
        {  
			Time.timeScale = 1;
            AppNet.main = this;

            if (AppStart.RunMode == 0)
            {
                App_Start();
            }
            else if (AppStart.RunMode == 1)
            {
				SDKManager.SDKInit();
				SDKManager.SDKActivityOpenLog();
				App_Start();
                
            }
			else if (AppStart.RunMode == 2)
			{
				SDKManager.SDKInit();
			}

//			MessageManager.Init();
//			ConfirmMgr.Instance.Init ();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (!isInit)
            {
                return;
            }
            Log.info(this,"Application Pause:==========================" + pauseStatus);
            if (pauseStatus)
            {
                Singleton<CopyMode>.Instance.PauseCopy();
				SDKManager.SDKPauseGame();
            }
            else
            {
                //Singleton<LoginControl>.Instance.SendHeartMsgDirect();    //先发送心跳，同步服务器时间，然后再做其他事情

				Singleton<CopyMode>.Instance.ResumeCopy();
            }
        }

        /**主应用初始化**/

        public void App_Start()
        {
            //设置帧数为45帧

            Application.targetFrameRate = targetFrame;
            //Application.runInBackground = false;
            AppNet.main = this;
            netDataList = new List<INetData>();
            handlerList = new Dictionary<String, NetMsgCallback>();
            //加载配置文件(一定要去掉后缀名)
            SetLogLevel();
            if (Application.platform != RuntimePlatform.WindowsEditor &&
                Application.platform != RuntimePlatform.OSXEditor)
            {
                Log.ClearLog(); //非Editor环境不执行Log
            }
            //Log.ClearLog(); 
            Log.info(this, "-Start() 1、bitLen:" + AppNet.bitLen);
            Log.info(this,
                "-Start() 2、language:" + ResMgr.instance.versionMgr.language + ", basePath:" +
                ResMgr.instance.versionMgr.basePath);
            Log.info(this, "-Start() 3、初始化配置【OK】");

            if (!ClientUpdate.updateInit)
            {
                //加载登录UI(必须先加载MainUI空预设)
                DontDestroyOnLoad(gameObject);
                var uiRoot = (GameObject) ResMgr.instance.load(UrlConst.UI_ROOT);
                Tools.addChild(gameObject, uiRoot, -30f);
                var logo = Tools.find(gameObject, "uiroot(Clone)/viewtree/Logo");
                DestroyImmediate(logo);
            }

            ViewTree.go = Tools.find(gameObject, "uiroot(Clone)/viewtree");
            Viewport.go = Tools.find(gameObject, "uiroot(Clone)/viewport");

            GameObject battle = new GameObject();
            battle.name = "battle";
            NGUITools.AddChild(ViewTree.go, battle);

            GameObject city = new GameObject();
            city.name = "city";
            NGUITools.AddChild(ViewTree.go, city);
            ViewTree.SetSubObj();

            //添加hudview
            var hv = Tools.find(gameObject, "uiroot(Clone)/viewtree/hudview").AddComponent<HudView>();
            hv.MountPoint = Tools.find(gameObject, "uiroot(Clone)/viewtree/hudview/mountpoint");
            HUDText.Parent = hv.MountPoint;


            if (NeedUpdated && !ClientUpdate.updateFinish)
            {
                StartCoroutine(ClientUpdate.ClientStart(gameObject, AfterUpdate, true));
            }
            else
            {
                AfterUpdate();
            }

            if (AppStart.MainAssembly == null)
            {
                AppStart.MainAssembly = Assembly.GetExecutingAssembly();
            }
            PhoneUtil.DonotSleep();
            gameObject.AddComponent<InitManager>();
        }

        private void AfterUpdate()
        {
            AssetManager.Instance.init(startLoadingFinish);
        }

        /// <summary>
        ///     初始加载回调
        /// </summary>
        /// <param name="obj"></param>
        private void startLoadingFinish(Object obj)
        {
            EffectMgr.Instance.PreloadUIEffect(EffectId.UI_SelectRoleFeedback);
            EffectMgr.Instance.PreloadUIEffect(EffectId.UI_SelectRoleStage2);
            EffectMgr.Instance.PreloadUIEffect(EffectId.UI_SelectRoleStage1);
            EffectMgr.Instance.PreloadUIEffect(EffectId.UI_SelectRoleBurst);

            UpdateView.Instance.closeView();
            Singleton<StartLoadingView>.Instance.CloseView();
            if (AppStart.RunMode == 0)
            {
                PlatformLoginFinish();
            }
			else if (AppStart.RunMode == 1)
			{
				Singleton<WaitingView>.Instance.OpenView();
				SDKManager.SDKAutoUpdate(); //先进行自动更新----modify by lixi
            }
			else if (AppStart.RunMode == 2)
			{
				AutoUpdateEnd();
			}
        }

		//平台登录完成回调
		private void PlatformLoginFinish()
        {
            //Security.PrefetchSocketPolicy(AppNet.ip, AppNet.port);
            AppNet.gameNet = NetFactory.newSocket();
            AppNet.gameNet.msgListenner(receiveNetMsg);
            AppNet.gameNet.statusListener(receiveNetStatus);

			OpenLoginView();
            
            //初始化加载工作
            HudView.Instance.StartLoadAsset();
//            MessageManager.Init();
//            ConfirmMgr.Instance.Init();
            //初始化特效相机
            Singleton<EffectCameraMgr>.Instance.Init();
            //剧情数据初始化
            Singleton<StoryMode>.Instance.Init();

            //绑定脚底灰尘特效
            EffectMgr.Instance.PreloadMainEffect(EffectId.Main_FootSmoke);
            EffectMgr.Instance.PreloadMainEffect(EffectId.Main_AutoSerachRoad);
            EffectMgr.Instance.PreloadMainEffect(EffectId.Main_BossFootBuff); //怪物脚底的特效
            EffectMgr.Instance.PreloadMainEffect(EffectId.Main_BossScene); //怪物脚底的特效  

			PreloadStartMovieResource();

			assetInit = true;
        }

		private void PreloadStartMovieResource()
		{
			//开场音效预加载
			SoundMgr.Instance.PreloadSceneAudio(SoundId.Music_StartMovie);
			//开场动画预加载
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_MovieStart1);
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_MovieStart2);
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_MovieStart3);
		}


		private void OpenLoginView()
		{
            Singleton<LoginView>.Instance.OpenView();
        }

        /**脚本销毁时关闭Net连接**/

        private void OnDestroy()
        {
            if (AppNet.gameNet != null)
            {
                AppNet.gameNet.close();
                Log.info(this, "-OnDestroy() Socket销毁成功！");
            }
        }

        /**初始化应用**/

        private void initApp()
        {
            Log.info(this, "-initApp() 6、开始初始化模块应用！");
            if (isInit) return; //初始化状态
            isInit = true;
            AppNet.main = this;
            AppFacde.instance.Init();
//            var binLoadMgr = gameObject.AddComponent<BinLoadMgr>();
//            ResMgr.instance.initBinLoadMgr(binLoadMgr); //注册加载器
            //ResMgr.instance.setTrace(true); //打印日志开关
//            ResMgr.instance.setNumTry(3); //重试次数
//            ResMgr.instance.setNumThread(3); //加载并发数
            Log.info(this, "-initApp() 7、系统模块初始化【OK】");

            Singleton<LoginControl>.Instance.maxdelay = 10;
            Singleton<LoginControl>.Instance.heartGap = 5;

            //mapMode = (AppFacde.instance.getControl(MapControl.NAME).mode as MapMode);
            //LoginControl loginControl = (LoginControl)AppFacde.instance.getControl(LoginControl.NAME);
            //loginControl.openView();
            //Singleton<LoginView>.Instance.OpenView();

            //图集的初始化
            Singleton<AtlasManager>.Instance.Init();

            CommonModelAssetManager.Instance.Init();
            //UI预先加载
            //Singleton<GoodsAnim>.Instance.OpenView();
            //Singleton<BossView>.Instance.OpenView();   //暂作测试
            Singleton<GoGoView>.Instance.OpenView();
            //Singleton<NetworkInfoView>.Instance.OpenView();
            Singleton<ContinueCutView>.Instance.OpenView();
            //新的MVC测试

            //Singleton<LoginView>.Instance.OpenView();
            Log.info(this, "-initApp() 8、登陆UI打开【OK】");
            DevelopHelper.Init();
        }


        private void SetLogLevel()
        {
            Log.addLevel("5");
            NetParams.addLevel("5");
        }


        //==========  网络监听 ==========//
        /**Net数据包处理**/

        private void Update()
        {
            if (connected && !isInit)
            {
                connectTime = Time.time;
                initApp();
            }
            else
            {
                //网络处理
                if (AppNet.gameNet != null)
                {
                    AppNet.gameNet.Update();
                }

                //网络读写
                if (connected)
                {
                    if (Time.frameCount%targetFrame == 0)
                    {
                        //Singleton<NetworkInfoView>.Instance.UpdateDelayInfo(Singleton<LoginControl>.Instance.getLastDelayTime());

                        bool timeOut = Singleton<LoginControl>.Instance.CheckTimeOut();
                        if (timeOut)
                        {
                            Log.info(this, "检测到心跳网络超时");
                            AppNet.gameNet.close();
                            Singleton<NetworkInfoView>.Instance.SetDisConnect();
                        }
                        else
                        {
                            //暂时被我注释的。
                            //  Log.info(this, "Singleton<LoginControl>.Instance"+Singleton<LoginControl>.Instance.getLastDelayTime());
                            Singleton<LoginControl>.Instance.SendHeartMsg();
                        }
                    }
                    doHandler();
                }

                //业务处理
                if (Time.frameCount%frameGap == 0)
                {
                    PlayerMgr.instance.execute();
                    MonsterMgr.Instance.execute();
                    ViewManager.Update();
                    if (MapMode.CanGoToNextPhase && !MapMode.StartAutoMove)
                    {
                        Singleton<MapControl>.Instance.CheckChangePhase();
                    }
                }
            }

            if (assetInit)
            {
                checkQuitClick();
            }

//			if(Input.GetKeyDown(KeyCode.V))
//			{
//				Singleton<DaemonIslandView>.Instance.OpenView();
//			}

//			Debug.Log (MeVo.instance.Hp);
//			Debug.Log (MeVo.instance.CurHp);
        }

        /***接收数据**/

        public void receiveNetMsg(INetData receiveData)  // NetSocket.cs Update()每帧调用
        {
            string cmd = receiveData.GetCMD();
            Log.info(this,"Received Data CMD :" +cmd + "  ServerTimestamp:" + ServerTime.Instance.Timestamp);
            if (cmd == CMD.CMD_1_0)
            {
                handlerList[cmd](receiveData);
            }
            else
            {
                netDataList.Add(receiveData);
            }
        }

		private void Login()
		{

            Singleton<LoginControl>.Instance.SendHeartMsgDirect();
            Singleton<LoginMode>.Instance.SendClientInfo();
            Singleton<WaitingView>.Instance.CloseView();
            Singleton<LoginMode>.Instance.getRoleList();
            SDKManager.SDKLoginServerLog(Singleton<LoginMode>.Instance.serverId.ToString());
			Time.timeScale = 1;
		}

        /**网络状态监听**/

        public void receiveNetStatus(String status)
        {
            switch (status)
            {
                case NetParams.LINK_OK:
                    Log.info(this,
                        "-receiveNetStatus() " + NetParams.LINK_OK + " 连接服务器[" + AppNet.ip + ":" + AppNet.port + "][" +
                        I18n.instance.getSocketTip(NetConst.RET_A0) + "]");
					connected = true;
					Login();
                    break;
                case NetParams.LINK_FAIL:
                    Log.info(this,
                        "-receiveNetStatus() " + NetParams.LINK_FAIL + " 连接服务器[" + AppNet.ip + ":" + AppNet.port + "][" +
                        I18n.instance.getSocketTip(NetConst.RET_A0, 2) + "]");
                    connected = false;
                    Singleton<NetworkInfoView>.Instance.SetDisConnect();
					//发送强制关闭剧情消息
					Singleton<StoryMode>.Instance.DataUpdate(Singleton<StoryMode>.Instance.FORCE_STOP_STORY);

                    if (Application.platform == RuntimePlatform.Android)
                    {
                        ConfirmMgr.Instance.ShowSelectOneAlert(LanguageManager.GetWord("ConnetAlertView.Message"),
                            ConfirmCommands.CONNECT_SERVER_ERROR, ReConnectClick,
                            LanguageManager.GetWord("ConnetAlertView.Reconnect"), QuitClick,
                            LanguageManager.GetWord("ConnetAlertView.Quit"));
                    }
                    else
                    {
                        ConfirmMgr.Instance.ShowOkAlert(LanguageManager.GetWord("ConnetAlertView.Message"),
                            ConfirmCommands.CONNECT_SERVER_ERROR, ReConnectClick,
                            LanguageManager.GetWord("ConnetAlertView.Reconnect"));
                    }
                    Time.timeScale = 0;
                    Singleton<WaitingView>.Instance.CloseView();
                    break;
                case NetParams.LINKING:
                    Log.info(this,
                        "-receiveNetStatus() " + NetParams.LINKING + " 连接服务器[" + AppNet.ip + ":" + AppNet.port + "][" +
                        I18n.instance.getSocketTip(NetConst.RET_A0, 3) + "]");
                    connected = false;
                    Singleton<WaitingView>.Instance.OpenView();
                    break;
            }
        }


        //重连点击处理
        public void ReConnectClick()
        {
            Log.info(this, "开始重连");
            netDataList.Clear();
            StopAllCoroutines();
            Singleton<LoginControl>.Instance.ResetHeartBeatState();
            AppNet.gameNet.connect(AppNet.gameNet.ip, AppNet.gameNet.port);
        }

        //重连点击处理
        public void QuitClick()
        {
            Log.info(this, "退出应用");
            QuitGame();
        }


        public void checkQuitClick()
        {
            if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            {
				if (AppStart.RunMode == 2)
				{
					SDKManager.SDKBeforeQuitGame ();
				}
				else
				{
	                ConfirmMgr.Instance.ShowSelectOneAlert(LanguageManager.GetWord("QuitAlertView.Message"),
	                    ConfirmCommands.SELECT_ONE, QuitClick, LanguageManager.GetWord("QuitAlertView.Quit"), null,
	                    LanguageManager.GetWord("QuitAlertView.Continue"));
				}
            }
        }

        /**注册回调**/

        public void addCMD(String cmd, NetMsgCallback callback)
        {
            if (callback == null) return;
            if (StringUtils.isEmpty(cmd)) return;
            handlerList[cmd] = callback;
        }

        /**移除回调**/

        public void removeCMD(String cmd)
        {
            if (StringUtils.isEmpty(cmd)) return;
            handlerList.Remove(cmd);
        }

        /**业务处理**/

        private void doHandler()
        {
            //try
            //{
            if (netDataList.Count < 1) return;

            int handlerNum = grameCMDNum; //每帧业务处理个数

            while (handlerNum > 0 && netDataList.Count > 0)
            {
                INetData data = netDataList[0];
                netDataList.Remove(data);
                string cmd = data.GetCMD();
                if (handlerList.ContainsKey(cmd))
                {
                    handlerList[cmd](data);
                    /*int cmdInt = int.Parse(cmd);
		            Log.info(this, "-doHandler() 注册CMD:" + (int)(cmdInt/256) + "  " + cmdInt% 256);*/
                }
                else
                {
                    Log.info(this, "-doHandler() 未注册CMD:" + data.GetCMD());
                }
                handlerNum--;
            }
            //}
            //catch (Exception ex)
            //{
            //    Log.error(this, "-doHandler() 业务处理错误！" + ex.StackTrace);
            //}
        }

		//平台版本自动更新结束后调用
		private void AutoUpdateEnd()
		{
			SDKManager.SDKActivityBeforeLoginLog();
			SDKManager.SDKLogin();
		}
        //-----------------------------  4399SDK回调函数  ---------------------------------------//
		/// <summary>
		///     没有检测到更新版本时回调
		/// </summary>
		/// <param name="msg">Message.</param>
		public void NotNewVersion(string msg)
		{
            Singleton<WaitingView>.Instance.CloseView();
            Log.info(this, "已经是最新版本");
            AutoUpdateEnd();
		}

		public void NotSDCard(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "没有SD卡");
			ConfirmMgr.Instance.ShowOkAlert("没有SD卡", ConfirmCommands.OK_CANCEL, 
			                                    QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}

		public void CancelForceUpdate(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "用户取消强制更新,退出游戏");
			QuitGame ();
		}
		
		public void CancelNormalUpdate(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "用户取消普通更新");
			AutoUpdateEnd ();
		}
		
		public void CheckVersionFailure(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "新版本检测失败");
			MessageManager.Show("版本检测失败");
			AutoUpdateEnd ();
		}
		
		public void NetWorkError(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "网络链接错误");
			ConfirmMgr.Instance.ShowOkAlert("网络链接错误", ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}
		
		public void SsjjsyException(string msg)
		{
			Singleton<WaitingView>.Instance.CloseView();
			Log.info(this, "其它异常");
			ConfirmMgr.Instance.ShowOkAlert("更新发生异常", ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}
		
        /// <summary>
        ///     平台登录成功返回
        /// </summary>
        /// <param name="msg">username,timestamp,signStr,suid,targetServerId,verifyToken.</param>
        public void LoginComplete(string msg)
        {
			string[] loginInfo = msg.Split(',');
			Singleton<LoginMode>.Instance.UpdateLoginInfo(loginInfo [3], loginInfo [5], loginInfo [0], loginInfo [1],
			                                              loginInfo [2], loginInfo [4]);

			PlatformLoginFinish();
        }

		public void LoginError(string errmsg)
		{
			ConfirmMgr.Instance.ShowOkAlert("登陆错误：" + errmsg, ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}

		public void LoginCancel(string msg)
		{
			if (!Singleton<LoginMode>.Instance.IsOpenLoginView)
			{
				QuitGame();
			}
		}

		public void LoginException(string msg)
		{
			ConfirmMgr.Instance.ShowOkAlert("登陆发生异常：" + msg, ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}

		//退出游戏
		public void QuitGame()
		{
			Log.info(this, "Application quit called");
			Application.Quit ();

		}



		//-----------------------------  91SDK回调函数  ---------------------------------------//
		private void SDK91InitSucceed(string msg)
		{
			if (msg == "normal")
			{
				SDKManager.SDKActivityOpenLog();
				App_Start();
			}
			else if (msg == "force_close")
			{
				QuitGame();
			}
		}

		private void Login91Success(string msg)
		{
//			ConfirmMgr.Instance.ShowOkAlert("登陆chenggong：" + msg, ConfirmCommands.OK_CANCEL, 
//			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
			string[] loginInfo = msg.Split(',');
			Singleton<LoginMode>.Instance.UpdateLoginInfo(loginInfo[0], loginInfo[1], loginInfo[2]);

			PlatformLoginFinish();
		}

		private void Login91Fail(string msg)
		{
			ConfirmMgr.Instance.ShowOkAlert("登陆发生错误：" + msg, ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}

		private void Login91Cancel(string msg)
		{
			if (!Singleton<LoginMode>.Instance.IsOpenLoginView)
			{
				SDKManager.SDK91GuestLogin ();
			}
		}



		
		private void Login91GuestSuccess(string msg)
		{
			string[] loginInfo = msg.Split(',');
			Singleton<LoginMode>.Instance.UpdateLoginInfo(loginInfo[0], loginInfo[1], loginInfo[1]);
			PlatformLoginFinish();
		}

		private void Login91GuestFail(string msg)
		{
			ConfirmMgr.Instance.ShowOkAlert("登陆发生错误：" + msg, ConfirmCommands.OK_CANCEL, 
			                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
		}

		private void Login91GuestCancel(string msg)
		{
			if (!Singleton<LoginMode>.Instance.IsOpenLoginView)
			{
				QuitGame();
			}
		}

		private void Pay91Success(string msg)
		{
			MessageManager.Show ("支付成功：" + msg);
		}

		private void Pay91Fail(string msg)
		{
			MessageManager.Show ("支付失败：" + msg);
		}

		private void Pay91Cancel(string msg)
		{
		}

		private void Close91PauseTips(string msg)
		{

		}

		private void BeforeQuitGameCallBack(string msg)
		{
			Application.Quit ();
		}

		private void LoginStateCallback_91(string msg)
		{
			switch (msg)
			{
				case "accountLogin":
					break;
				case "guestLogin":
					break;
				case "notLogin":
					break;
				default:
					break;
			}
		}

		/////////////////////////////////更新公告/////////////////////////
		public void GetAnnounce(string url)
		{
			StartCoroutine(RequestServerInfo(url));
		}

		private IEnumerator RequestServerInfo(string url)
		{
			WWW request = new WWW(url);
			yield return request;

			if (null == request.error)
			{
                string txt = request.text;
                Singleton<UpdateAnnounceMode>.Instance.SaveAnnounce(txt);
                request.Dispose();
			}
		}


//		private void OnApplicationQuit()
//		{
////			SDKManager.SDKBeforeQuitGame ();
//		}
    }
}