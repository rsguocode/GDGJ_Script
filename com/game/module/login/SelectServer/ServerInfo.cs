

namespace com.game.module.login
{
    public class ServerInfo
    {        //myServer,1|1,皓月迷城,服务器状态,IP,端口|2,皓月迷城2,服务器状态2,IP2,端口2
        //	1 => '新服',2 => '推荐',3 => '繁忙',4 => '维护'

        public int Id {  get; private set; }
        public string Name { get; private set; }
        public string IP { get; private set; }
        public int Port { get; private set; }
        public int State { get; private set; }

        public ServerInfo(string[] serverInfo)
        {
            Id = int.Parse(serverInfo[0]);
            Name = serverInfo[1];
           State = int.Parse(serverInfo[2]);
           IP = serverInfo[3]; //"172.16.232.41";
           Port = int.Parse(serverInfo[4]); //9101; 
        }

        /// <summary>
        /// 获取状态的文字信息
        /// </summary>
        /// <returns></returns>
        public string ServerStateStr()
        {
            if (State == 1)
            {
                return "新服";
            }
            else if (State == 2)
            {
                return "推荐";
            }
            else if (State == 3)
            {
                return "繁忙";

            }
            else
            {
                return "维护";
            }
        }
    }
}
