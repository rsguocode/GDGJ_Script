using System;
using System.Collections;
using System.Collections.Generic;
using com.game.consts;
using com.game.module.test;
using com.game.Public.LocalVar;
using com.game.Public.Message;
using com.game.SDK;
using com.game.sound;
using com.game.start;
using com.game.utils;
using com.u3d.bases.debug;
using UnityEngine;

namespace com.game.module.login
{
    /// <summary>
    ///     登录视图。
    /// </summary>
    public class LoginView : BaseView<LoginView>
    {
        private Button _btnServer;
        private Button _btnUsername;
        private UIGrid _centerGrid;
        private GameObject _chose;
        public ServerInfo Info;
        private Button _loginGo; // 登录按钮
        private UIInput _nameInput;
        private List<GameObject> _serverObjs;
        private GameObject _serverlist;
        private UILabel _usernameLab; // 用户名标签

        private int serverId = -1;

        /// <summary>
        /// 预设路径
        /// </summary>
        public override string url
        {
            get { return "UI/Login/LoginView.assetbundle"; }
        } 

        /// <summary>
        /// 关闭时是否播放音乐
        /// </summary>
        public override bool playClosedSound
        {
            get { return false; }
        }

        /// <summary>
        /// 关闭时是否消耗
        /// </summary>
        public override bool isDestroy
        {
            get { return true; }
        }

        /// <summary>
        /// 是否出现加载等待
        /// </summary>
        public override bool waiting
        {
            get { return false; }
        }

        /// <summary>
        /// 是否延迟卸载
        /// </summary>
        public override bool isUnloadDelay
        {
            get { return true; }
        }


        private ServerListInfo serverList;

        /// <summary>
        /// UI初始化
        /// </summary>
        protected override void Init()
        {
            _usernameLab = Tools.find(gameObject, "username/inp_input/label").GetComponent<UILabel>();
            _nameInput = Tools.find(gameObject, "username/inp_input").GetComponent<UIInput>();
            _btnServer = FindInChild<Button>("currentserver");
            _loginGo = FindInChild<Button>("login");
            _btnUsername = FindInChild<Button>("username/btn_username");
            _loginGo.onClick += LoginOnClick;
            _btnServer.onClick = ServerOnClick;
            _btnUsername.onClick = ChangeUserNameOnClick;
            _serverObjs = new List<GameObject>();
            _serverObjs.Add(FindChild("serverlist/all/container/oncenter/1"));
            _serverObjs[0].SetActive(false);
            _serverObjs[0].name = "99999";
            _serverObjs[0].GetComponent<Button>().onClick = OnServerChoseClick;
            FindInChild<Button>("serverlist/last").onClick = OnServerChoseClick;
            _chose = FindChild("serverlist/all/chose");
            _centerGrid = FindInChild<UIGrid>("serverlist/all/container/oncenter");
            _serverlist = FindChild("serverlist");
            NGUITools.FindInChild<UIWidgetContainer>(_serverlist, "background").onClick = OnCloseServerList;
            serverList = gameObject.AddComponent<ServerListInfo>();

            _serverlist.SetActive(false);
            _chose.SetActive(false);

            
        }

        /// <summary>
        /// 注册数据更新回调函数
        /// </summary>
        public override void RegisterUpdateHandler()
        {
            Singleton<SelectServerMode>.Instance.dataUpdated += UpdateLoginData;
        }

        /// <summary>
        /// 关闭数据更新回调函数
        /// </summary>
        public override void CancelUpdateHandler()
        {
            Singleton<SelectServerMode>.Instance.dataUpdated -= UpdateLoginData;
        }

        protected override void HandleAfterOpenView()
        {
			Singleton<LoginMode>.Instance.IsOpenLoginView = true;
            if (AppStart.RunMode != 0)
            {
                _nameInput.value = Singleton<LoginMode>.Instance.platformName;
                _btnUsername.gameObject.SetActive(true);
            }
            else
            {
                _btnUsername.gameObject.SetActive(false);
            }
            LoadSetting();
            serverList.GetServerInfo();
        }

        /// <summary>
        /// 加载本地设置
        /// </summary>
        private void LoadSetting()
        {
            if (AppStart.RunMode != 0)
            {
                return;
            }
            _nameInput.value = PlayerPrefs.GetString("UserName");
            string server = PlayerPrefs.GetString("UserServer");
            if (server != null && server.Equals(string.Empty))
            {
                serverId = 0;
            }
            else
            {
                serverId = int.Parse(server);
            }

        }

        /// <summary>
        /// 保存设置
        /// </summary>
        private void SaveSetting()
        {
            if (AppStart.RunMode != 0)
            {
                return;
            }
            if(!_nameInput.value.Equals(string.Empty) && Info != null)
            {   
                PlayerPrefs.SetString("UserName", _nameInput.value);
                PlayerPrefs.SetString("UserServer", Info.Id.ToString());
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="send"></param>
        /// <param name="code"></param>
        private void UpdateLoginData(object send, int code)
        {   
            if (send == Singleton<SelectServerMode>.Instance && code == SelectServerMode.ServerList)
            {
                InitServerObjs();
                if (Info != null)
                {
                    Info = Singleton<SelectServerMode>.Instance.GetServerInfoByServerId(Info.Id);
                }

                if (Info == null)
                {
                    Info = Singleton<SelectServerMode>.Instance.GetServerInfoByServerId(serverId);
                }

                if (Info == null)
                {
                    Info = Singleton<SelectServerMode>.Instance.GetDefaultServer();
                    NGUITools.FindInChild<UILabel>(_serverlist, "last/title").text = "推荐服务器";
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(_serverlist, "last/title").text = "最近登陆服务器";
                }

                if (Info != null)
                {
                    SetCurrentServerInfo();
                    SetServerInfo(NGUITools.FindChild(_serverlist, "last"), Info);
                }

                int maxId = Singleton<SelectServerMode>.Instance.maxServerId;
                int j = 0;
                for (int i = 0; i < maxId; i++)
                {
                    ServerInfo server = SelectServerMode.Instance.GetServerInfoByServerId(i);
                    if(server!=null){
                    GameObject obj = GetServerObj(j);
                    SetServerInfo(obj, server);
                    obj.SetActive(true);
                    j++;
}
                }
                _centerGrid.repositionNow = true;
            }
        }

        /// <summary>
        /// 设置当前服务器信息
        /// </summary>
        private void SetCurrentServerInfo()
        {
            if (Info != null)
            {
                Color color = GetStateColor(Info);
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/name").text = Info.Name + "(" +
                                                                                        Info.ServerStateStr() + ")";
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/num").text = Info.Id + "区";
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/name").color = color;
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/num").color = color;
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/tips").text = "点击换区";
            }
            else
            {
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/name").text = "";
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/num").text = "";
                NGUITools.FindInChild<UILabel>(gameObject, "currentserver/tips").text = "无服务器信息";
            }
        }

        private void InitServerObjs()
        {
            foreach (GameObject obj in _serverObjs)
            {
                obj.SetActive(false);
            }

        }

        /// <summary>
        /// 获取服务器对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private GameObject GetServerObj(int index)
        {
            if (index < _serverObjs.Count)
            {
                return _serverObjs[index];
            }
            else
            {
                var obj = GameObject.Instantiate(_serverObjs[0]) as GameObject;

                obj.transform.parent = _serverObjs[0].transform.parent;
                obj.transform.localPosition = _serverObjs[0].transform.localPosition;
                obj.name = (99999 - index).ToString(); //对象名称排序处理
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Button>().onClick = OnServerChoseClick;
                _serverObjs.Add(obj);
                return obj;
            }
        }

        /// <summary>
        /// 设置服务器信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        private void SetServerInfo(GameObject obj, ServerInfo info)
        {
            NGUITools.FindInChild<UILabel>(obj, "name").text = info.Name;
            NGUITools.FindInChild<UILabel>(obj, "num").text = info.Id + "区";
            NGUITools.FindInChild<UILabel>(obj, "tips").text = info.ServerStateStr();
            Color color = GetStateColor(info);
            NGUITools.FindInChild<UILabel>(obj, "name").color = color;
            NGUITools.FindInChild<UILabel>(obj, "num").color = color;
            NGUITools.FindInChild<UILabel>(obj, "tips").color = color;
        }


        private Color GetStateColor(ServerInfo info)
        {
                        Color color = ColorConst.FONT_YELLOW;
            if (info.State == 1)
            {
                color = ColorConst.FONT_GREEN;
            }
            else if (info.State == 2)
            {
                color = ColorConst.FONT_BLUE;
            }
            else if (info.State == 3)
            {
                color = ColorConst.FONT_RED;
            }
            else
            {
                color = ColorConst.FONT_GRAY;
            }
            return color;
        }

        private void OnCheckboxChange()
        {
        }

        /// <summary>
        ///     登录事件
        /// </summary>
        /// <param name="go">登录按钮</param>
        private void LoginOnClick(GameObject go)
        {
            if (Info == null)
            {
                MessageManager.Show("无服务器信息");
                return;
            }
            if (StringUtils.isEmpty(_nameInput.value))
            {
                _usernameLab.text = "请输入账号名";
                MessageManager.Show("请输入账号名");
                return;
            }
            if (Info.State > 3)
            {
                MessageManager.Show("此服务器维护中");
                serverList.GetServerStateInfo();
                return;
            }

            SaveSetting();
            SoundMgr.Instance.PlayUIAudio(SoundId.Sound_Login);
            ConnectServer();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        private void ConnectServer()
        {
            Singleton<LoginMode>.Instance.userName = _nameInput.value;
            AppNet.ip = Info.IP;
            AppNet.port = Info.Port;
            AppNet.gameNet.connect(Info.IP, Info.Port);
            Singleton<LoginMode>.Instance.serverId = Info.Id;
        }

        /// <summary>
        /// 服务器选择框被点击
        /// </summary>
        /// <param name="go"></param>
        private void ServerOnClick(GameObject go)
        {
            if (SelectServerMode.Instance.serverList.Count == 0)
            {
                serverList.GetServerInfo();
                SetServerTips("正在请求...");
            }
            else
            {
                serverList.GetServerInfo();
                _serverlist.SetActive(true);
                _chose.SetActive(false);
            }
        }

        /// <summary>
        /// 服务器信息被点击
        /// </summary>
        /// <param name="obj"></param>
        private void OnServerChoseClick(GameObject obj)
        {
            string name = NGUITools.FindInChild<UILabel>(obj, "num").text;
            int serverId = int.Parse(name.Replace("区", ""));

            ServerInfo info = Singleton<SelectServerMode>.Instance.GetServerInfoByServerId(serverId);
            if (info == null)
            {
                return;
            }
            Info = info;

            SetCurrentServerInfo();
            _chose.transform.parent = obj.transform.parent;
            _chose.transform.localPosition = obj.transform.localPosition;
            _chose.SetActive(true);
            _serverlist.SetActive(false);
            //CloseServerList();
            //CoroutineManager.StartCoroutine(CloseServerList());
        }

        /// <summary>
        /// 关闭服务器列表
        /// </summary>
        /// <returns></returns>
        private IEnumerator CloseServerList()
        {
            yield return new WaitForSeconds(0.1f);
            _serverlist.SetActive(false);
        }

        /// <summary>
        /// 服务器列表关闭回调事件
        /// </summary>
        /// <param name="obj"></param>
        private void OnCloseServerList(GameObject obj)
        {
            _serverlist.SetActive(false);
        }

        /// <summary>
        /// 修改用户名按钮被点击
        /// </summary>
        /// <param name="go"></param>
        private void ChangeUserNameOnClick(GameObject go)
        {
            if (AppStart.RunMode != 0)
            {
                SDKManager.SDKChangeLoginUser();
            }
        }

        public void SetServerTips(string tips)
        {
            NGUITools.FindInChild<UILabel>(gameObject, "currentserver/tips").text = tips;
        }
    }
}