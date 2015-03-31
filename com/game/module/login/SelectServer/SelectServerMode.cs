
using System.Collections.Generic;
using com.game.module.test;

namespace com.game.module.login
{
    public class SelectServerMode : BaseMode<SelectServerMode>
    {
        public Dictionary<int,ServerInfo> serverList  = new Dictionary<int, ServerInfo>();
        public int maxServerId;

        public const int ServerList = 1;//服务端

        //设置服务器信息
        public void SetServerListInfo(string[] list)
        {   
            if (list!=null&&list.Length > 0)
            {
                serverList.Clear();
                for (int i = 0; i < list.Length; i++)
                {
                    string[] info = list[i].Split(',');
                    if (info.Length == 5)
                    {
                        ServerInfo server = new ServerInfo(info);
                        if (maxServerId < server.Id)
                        {
                            maxServerId = server.Id;
                        }
                        serverList.Add(server.Id,server);
                    }
                }
            }
            DataUpdate(ServerList);
        }

        //通过服务器id获取服务器信息
        public ServerInfo GetServerInfoByServerId(int serverid)
        {
            if (serverList.ContainsKey(serverid))
                return serverList[serverid];
            return null;
        }

        //获取一个默认的服务器
        public ServerInfo GetDefaultServer()
        {
            for (int i = maxServerId; i>=0; i--)
            {
                if (serverList[i] != null && serverList[i].State == 1 || serverList[i].State == 2) //新服 或者推荐
                {
                    return serverList[i];
                }
            }

            for (int i = maxServerId; i >= 0; i--)
            {
                if (serverList[i] != null && serverList[i].State == 3) //繁忙
                {
                    return serverList[i];
                }
            }
            for (int i = maxServerId; i >= 0; i--)
            {
                if (serverList[i] != null && serverList[i].State == 4) //不可用
                {
                    return serverList[i];
                }
            }
            return null;//维护中，无服务器可用
        }


    }
}
