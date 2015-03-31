/**登陆模块--与通讯交互类
 * 1、必须覆盖listListener()方法，进行注册监听
 * 2、必须覆盖netCallback()方法,接收Socket数据
 * **/
using System;
using System.IO;
using com.game.module.test;
using com.game.Public.LocalVar;
using com.game.start;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;


namespace com.game.module.login
{
    public class LoginMode : BaseMode<LoginMode>
    {
        public readonly int UPDATE_SERVER = 1;

		public bool IsOpenLoginView = false;
        //  SDK接入返回变量 //
        private String _platformName;
        private int _serverId;
        private String _signStr;
        private String _suid;
        private String _targetServer;
        private String _timestamp;
        private String _verifyToken;
        public string platform = "4399";

        public String platformName
        {
            get { return _platformName; }
        }


        public String userName { get; set; }
        public bool loginSuc { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public int serverId
        {
            get { return _serverId; }
            set
            {
                _serverId = value;
                LocalVarManager.SetInt(LocalVarManager.LOGIN_SERVER, serverId);
                DataUpdate(UPDATE_SERVER);
            }
        }

        public void UpdateLoginInfo(string suid, string verifyToken, string platformName = "", string timestamp = "0",
            string signStr = "", string targetServer = "")
        {
            _platformName = platformName;
            _timestamp = timestamp;
            _signStr = signStr;
            _suid = suid;
            _targetServer = targetServer;
            _verifyToken = verifyToken;
        }


        /**注册Socket数据返回监听**/


        /**创建角色
         * @param carser       职业[1:斩魂,2:武尊,3:玄天]
         * @param sex          性别[1:女,2:男]
         * @param roleName     角色名称
         * @param platformName 平台账号名
         * **/

        public void createRole(int carser, int sex, String roleName)
        {
            var msdata = new MemoryStream();
            Module_1.write_1_3(msdata, roleName, (byte) carser, (byte) sex, (uint) serverId);
            Log.debug(this,
                "-createRole 发送创建角色消息,carser:" + carser + " sex:" + sex + " roleName:" + roleName + " platforName:" +
                _platformName);
            AppNet.gameNet.send(msdata, 1, 3);
        }

        //发送客户端信息
        public void SendClientInfo()
        {
            string os = SystemInfo.operatingSystem;
            string os_ver = "2.3.4";
            string device = SystemInfo.deviceModel;
            string deviceType = SystemInfo.deviceType.ToString();
            string screen = Screen.width + "*" + Screen.height;
            string mno = "中国移动";
            string nm = "wifi";
            Log.info(this,
                "发送1-6客户端信息给服务端：" + os + "," + os_ver + "," + device + "," + deviceType + "," + screen + "," + mno + "," +
                nm
                + "," + platform + "," + serverId + ",");
            var msdata = new MemoryStream();
            platform = AppStart.RunMode == 2 ? "91" : "4399";
            Module_1.write_1_6(msdata, os, os_ver, device, deviceType, screen, mno, nm, platform, (uint) serverId);
            AppNet.gameNet.send(msdata, 1, 6);
        }

        /************************************************************************/
        /* 获取角色列表                                                                     */
        /************************************************************************/

        public void getRoleList()
        {
            var msdata = new MemoryStream();
            byte fcm = 1;
            if (AppStart.RunMode == 0)
            {
                Log.info(this, "发送1-1协议");
                Module_1.write_1_1(msdata, userName, "", fcm, "ios", "", 0);
            }
            else if (AppStart.RunMode != 0)
            {
                Log.info(this, "发送1-1协议");
                Module_1.write_1_1(msdata, _suid, _signStr, fcm, platform, _verifyToken, uint.Parse(_timestamp));
            }
            AppNet.gameNet.send(msdata, 1, 1);
        }
    }
}