
﻿﻿﻿using System.Collections;
﻿﻿﻿using com.game.module.test;
﻿﻿﻿using com.game.Public.Message;
﻿﻿﻿using com.u3d.bases.debug;
﻿﻿﻿using UnityEngine;
namespace com.game.module.login
{
	/// <summary>
	/// 服务器列表信息处理类
	/// </summary>
	public class ServerListInfo :MonoBehaviour
	{
        //172.16.10.140/api/set_my_server.php 参数：accountName  serverId

        //172.16.10.140/api/get_server_list.php  参数：accountName

        //myServer,1|1,皓月迷城,服务器状态,IP,端口|2,皓月迷城2,服务器状态2,IP2,端口2
        //	1 => '新服',2 => '推荐',3 => '繁忙',4 => '维护'

	    private float setTime = 0;
	    private float timeGap = 10;

	    public string ServerHost
	    {
	        get
	        {
	            if (Application.platform == RuntimePlatform.WindowsEditor ||
	                Application.platform == RuntimePlatform.WindowsPlayer)
	            {
                    return "http://172.16.10.166/phone_list/"; //内网
	            }
	            else
	            {
                    //return "http://gateway.mxqy.4399sy.com/"; //外网
                    return "http://115.236.76.13/phone_list/";
	            }
	        }
	    }

        public string GetURL = "phone_list";//"api/get_server_list.php";  //请求服务器列表
        public string GetStateURL = "api/get_server_list2.php";  //请求服务器列表-状态
        public string SetURL = "api/set_my_server.php"; //设置服务器

        public void GetServerStateInfo()
        {
            if (setTime == 0 || (RealTime.time - setTime) > timeGap)
                //StartCoroutine(RequestServerInfo(ServerHost+GetURL));
                StartCoroutine(RequestServerInfo(ServerHost));
        }

        public void GetServerInfo()
        {
            if (setTime==0||(RealTime.time - setTime) > timeGap)
                //StartCoroutine(RequestServerInfo(ServerHost+GetURL));
                StartCoroutine(RequestServerInfo(ServerHost));
            setTime = RealTime.time;
        }

	    private IEnumerator RequestServerInfo(string url)
	    {
            WWW request = new WWW(url);
	        yield return request;
	        if (request.error == null)
	        {
	            string txt = request.text;
                Log.info(this,txt);
                Singleton<SelectServerMode>.Instance.SetServerListInfo(txt.Split('|'));
                request.Dispose();
	        }
	        else
            {
                MessageManager.Show(request.error);
                MessageManager.Show("获取服务器列表失败!");
                LoginView.Instance.SetServerTips("获取失败，点击重试");
            }
	    }


        public void SetServerInfo(string url, string accoutName,string serverId)
        {

            //StartCoroutine(SetAccoutServer(url, accoutName, serverId));
        }



	}
}
