/**登陆模块--控制器
 * 1、它是 Mode 和 View 数据传输的直接者
 * 2、它是控制和调度 Mode 和 View
 * 3、Mode随着Control实例而实例化,View到使用时，才实例化一次
 * **/

using System.IO;
using com.game.cmd;
using com.game.consts;
using Com.Game.Module.GoldSilverIsland;
using Com.Game.Module.Lottery;
using com.game.module.map;
using Com.Game.Module.Role;
using com.game.module.rolecreate;
using com.game.module.SystemData;
using Com.Game.Module.SystemSetting;
using com.game.module.test;
using com.game.Public.Message;
using com.game.SDK;
using com.game.start;
using com.game.vo;
using com.net.interfaces;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;
using com.game.module.effect;
using com.game.sound;
using UnityEngine;
using com.game.Public.Confirm;
using com.game.data;
using com.game.manager;
using Com.Game.Module.UpdateAnnounce;
using System.Globalization;

namespace com.game.module.login
{
    public class LoginControl : BaseControl<LoginControl>
    {
        public static float fPosY = 1.3f;
        public static uint myID = 0;
        private readonly byte[] heart = new byte[4] {0, 2, 1, 0};
        private bool _isCreateRole;
        private float lastDelayTime;
        private float lastReceiveTime; //最后一次收到心跳的时间
        private float lastSendTime; //最后一次心跳发送时间
        public string platformName;
        private uint reciveCount; //接收计数
        public string[] roleList;
        private uint sendCount; //发送计数

        public short heartGap { get; set; } //心跳发送间隔时间(秒)
        public short maxdelay { get; set; } //最大延迟时间

		private bool startMoviePlayed = false;


        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_1_0, Fun_1_0); //心跳
            AppNet.main.addCMD(CMD.CMD_1_1, Fun_1_1); //登陆
            AppNet.main.addCMD(CMD.CMD_1_2, Fun_1_2); //选择角色
            AppNet.main.addCMD(CMD.CMD_1_3, Fun_1_3); //创建角色
            AppNet.main.addCMD(CMD.CMD_1_6, Fun_1_6); //发送客户端信息返回
        }

        /**发送心跳**/

        public void SendHeartMsg()
        {
            float time = RealTime.time;
            if (heartGap == 0)
                heartGap = 5;

            if (sendCount == reciveCount && time - lastSendTime >= heartGap)
            {
                Log.info(this, "Send HeartMsg");
                var msdata = new MemoryStream();
                Module_1.write_1_0(msdata);
                AppNet.gameNet.send(msdata, 1, 0);
                lastSendTime = time;
                sendCount++;
            }
            else if (lastSendTime > time)
            {
                lastSendTime = time;
            }
        }

        //直接发送心跳，有的地方有这种需求
        public void SendHeartMsgDirect()
        {
            //Log.info(this, "Send HeartMsg");
            var msdata = new MemoryStream();
            Module_1.write_1_0(msdata);
            AppNet.gameNet.send(msdata, 1, 0);
            lastSendTime = RealTime.time;
            sendCount++;
        }

        //接收心跳
        private void Fun_1_0(INetData data)
        {
            Log.info(this, "Receive HeartMsg");
            var heartMsg = new LoginHeartMsg_1_0();
            heartMsg.read(data.GetMemoryStream());
            ServerTime.Instance.Timestamp = (int) heartMsg.serverTime;
            lastReceiveTime = RealTime.time;
            if (reciveCount < sendCount)
            {
                lastDelayTime = lastReceiveTime - lastSendTime;
                reciveCount++;
            }
        }

        //检查是否超时
        public bool CheckTimeOut()
        {
            if (maxdelay == 0)
            {
                maxdelay = 10;
            }
            return getLastDelayTime() > maxdelay;
        }

        //获取上一次的延时信息
        public float getLastDelayTime()
        {
            if (sendCount == reciveCount)
            {
                return lastDelayTime;
            }
            float curdelaytime = RealTime.time - lastSendTime;
            if (curdelaytime < lastDelayTime)
            {
                return lastDelayTime;
            }
            return curdelaytime;
        }


        /// <summary>
        ///     重置心跳连接
        /// </summary>
        public void ResetHeartBeatState()
        {
            lastDelayTime = 0;
            lastSendTime = 0;
            lastReceiveTime = 0;
            sendCount = 0;
            reciveCount = 0;
        }

        /// <summary>
        ///     获取角色列表返回
        /// </summary>
        private void Fun_1_1(INetData data)
        {
            var loginMsg = new LoginLoginMsg_1_1();
            loginMsg.read(data.GetMemoryStream());

            PLoginInfo[] loginInfoArrary = loginMsg.loginInfo.ToArray();

            if (loginMsg.code != 0)
            {
                Log.info(this, "-Fun_1_1 登陆失败！code:" + loginMsg.code);
                if (loginMsg.code == 12)
                {
                    Log.debug(this, "没有创建角色，进入角色创建界面");
                    Singleton<LoginView>.Instance.CloseView();

					if (!startMoviePlayed)
					{
						startMoviePlayed = true;
						//如果没有创建角色，播放开场动画、开场音乐
						MovieStart1PlayStart();
					}
                }
				else
				{
//					ErrorCodeManager.ShowError(loginMsg.code);
					SysErrorCodeVo errorCodeVo = BaseDataMgr.instance.GetErrorCodeVo(uint.Parse(loginMsg.code.ToString(CultureInfo.InvariantCulture)));
					string result = "";
					if (errorCodeVo == null)
					{
						result = string.Format("Errorcode {0} is not defined", loginMsg.code);
					}
					else
					{
						result = errorCodeVo.desc;
					}
					ConfirmMgr.Instance.ShowOkAlert(result + ",退出游戏再试一次吧，亲~", ConfirmCommands.OK_CANCEL, 
					                                QuitGame, LanguageManager.GetWord("ConfirmView.Ok"));
				}
            }
            else
            {
                Log.debug(this, "-Fun_1_1 登录成功，获取登陆角色信息:");
                for (int i = 0; i < loginInfoArrary.Length; ++i)
                {
                    Log.debug(this, "id: " + loginInfoArrary[i].id);
                    Log.debug(this, "name: " + loginInfoArrary[i].name);
                    Log.debug(this, "job: " + loginInfoArrary[i].job);
                    Log.debug(this, "level: " + loginInfoArrary[i].level);
                    Log.debug(this, "sex id: " + loginInfoArrary[i].sex);
                    Log.debug(this, "lastLoginTime: " + loginInfoArrary[i].lastLoginTime);
                    Log.debug(this, "serverId: " + loginInfoArrary[i].serverId);
                }

                Log.debug(this, "-Fun_1_1 默认选择第一个游戏角色进入游戏");
                MeVo.instance.Id = loginInfoArrary[0].id;
                myID = MeVo.instance.Id; //保存我自己的ID
                MeVo.instance.Name = loginInfoArrary[0].name;
                MeVo.instance.job = loginInfoArrary[0].job;
                MeVo.instance.Level = loginInfoArrary[0].level;
                MeVo.instance.sex = loginInfoArrary[0].sex;
                MeVo.instance.lastLoginTime = loginInfoArrary[0].lastLoginTime;
                MeVo.instance.serverId = loginInfoArrary[0].serverId;

                var msdata = new MemoryStream();
                Module_1.write_1_2(msdata, MeVo.instance.Id);
                AppNet.gameNet.send(msdata, 1, 2);

				SoundMgr.Instance.PlaySceneAudio(SoundId.Music_EnterGame);
            }
        }

		private void QuitGame()
		{
			Application.Quit ();
		}

		private void PlayStartMovieMusic(GameObject effectObj)
		{
			SoundMgr.Instance.PlaySceneAudio(SoundId.Music_StartMovie);
		}

		private void MovieStart1PlayStart()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_MovieStart1, Vector3.zero, MovieStart1PlayFinish, true, PlayStartMovieMusic);
		}
		
		private void MovieStart1PlayFinish()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_MovieStart2, Vector3.zero, MovieStart2PlayFinish);
		}
		
		private void MovieStart2PlayFinish()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_MovieStart3, Vector3.zero, OpenRoleCreateView);
		}

		private void OpenRoleCreateView()
		{
			SoundMgr.Instance.StopAll();
			SoundMgr.Instance.PlaySceneAudio(SoundId.Music_EnterGame);
			Singleton<RoleCreateView>.Instance.OpenView(); //新角色创建UI
		}

        /**创建角色失败返回**/

        private void Fun_1_3(INetData data)
        {
            //MemoryStream msdata = NetData.Decode (data);
            var createRolMsg = new LoginCreateRoleMsg_1_3();
            createRolMsg.read(data.GetMemoryStream());
            if (createRolMsg.code != 0)
            {
                ErrorCodeManager.ShowError(createRolMsg.code);
                return;
            }
            _isCreateRole = true;
            Log.info(this, "-Fun_1_3() 创建角色成功！角色ID：" + createRolMsg.id);
            MeVo.instance.Id = createRolMsg.id;
			SDKManager.SDKCreateRoleLog (Singleton<RoleMode>.Instance.roleName,
			                            Singleton<LoginMode>.Instance.serverId.ToString ());
        }

        public void ChangeToFirstScene()
        {
            Singleton<MapMode>.Instance.changeScene(MapTypeConst.FirstFightMap, false, 5, 1.8f);
        }

        /**创建角色|登陆成功，返回本角色信息
         * **/

        private void Fun_1_2(INetData data)
        {
            var selectRoleMsg = new LoginSelectRoleMsg_1_2();
            selectRoleMsg.read(data.GetMemoryStream());
            if (selectRoleMsg.code != 0)
            {
                ErrorCodeManager.ShowError(selectRoleMsg.code);
                if (selectRoleMsg.code == 108)
                {
                    Log.debug(this, "-Fun_1_2账号被锁定，锁定时间：" + selectRoleMsg.time);
                }
                return;
            }

            Log.debug(this, "-Fun_1_2 创建角色成功，获取角色信息(默认选择第一个角色作为本玩家选择的角色)");
//			Singleton<LoginMode>.Instance.SendClientInfo ();
            PRole role = selectRoleMsg.role.ToArray()[0];
            MeVo.instance.Id = role.id;
            MeVo.instance.Name = role.name;
            MeVo.instance.sex = role.sex;
            MeVo.instance.Level = role.level;
            MeVo.instance.job = role.job;
            MeVo.instance.mapId = role.mapId;
            MeVo.instance.X = role.x*GameConst.PositionUnit;
            MeVo.instance.Y = role.y*GameConst.PositionUnit;
            MeVo.instance.toX = MeVo.instance.X;
            MeVo.instance.toY = MeVo.instance.Y;

            Log.debug(this, "-Fun_1_2选择角色进入场景：");
            Log.debug(this, "选择人物mapId: " + MeVo.instance.mapId);
            Log.debug(this, "选择人物X坐标: " + MeVo.instance.X);
            Log.debug(this, "选择人物Y坐标: " + MeVo.instance.Y);
            LoginSuccess();
        }


        /**创建角色|登陆成功**/

        public void LoginSuccess()
        {
            Log.info(this, "serverId:" + Singleton<LoginMode>.Instance.serverId);
            Log.info(this, "serverInfo:" + Singleton<LoginView>.Instance.Info.Id);
            if (AppStart.RunMode != 0)
            {
				SDKManager.SDKCreateAssistant(Singleton<LoginMode>.Instance.serverId.ToString());
                SDKManager.SDKShowAssistant();
            }
            //获取背包和物品信息
            GoodsControl.Instance.RequestWrapInfo(1);
            GoodsControl.Instance.RequestWrapInfo(2);
            //获取系统配置
            Singleton<SystemSettingMode>.Instance.GetAllSystemSetting();
            //获取抽奖信息
            Singleton<LotteryMode>.Instance.QueryLotteryInfo(0);
            //获取金银岛协助次数
            Singleton<GoldSilverIslandMode>.Instance.GetAssistRemainCount();

            if (!_isCreateRole)
            {
                Singleton<MapMode>.Instance.ApplySceneInfo(); // 登陆直接到请求4-2，副本阶段功能依赖于此
            }
            else
            {
                _isCreateRole = false;
                Singleton<RoleCreateView>.Instance.CloseView();
            }
            //A-4 发起进入场景请求   执行放前面，进度条早点显示
//			Singleton<MapMode>.Instance.changeScene (MeVo.instance.mapId, false, MeVo.instance.toX, MeVo.instance.toY);
//          Singleton<MapMode>.Instance.EnterScene();
            Singleton<LoginMode>.Instance.loginSuc = true;

            //A-1 销毁登陆|创建角色框
            Singleton<LoginView>.Instance.CloseView();
            Singleton<SelectServerView>.Instance.CloseView();

			//获取更新公告信息
			Singleton<UpdateAnnounceControl>.Instance.GetAnnounce();
        }

        //发送客户端信息返回
        private void Fun_1_6(INetData data)
        {
            var msg = new LoginClientInfoMsg_1_6();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
        }
    }
}