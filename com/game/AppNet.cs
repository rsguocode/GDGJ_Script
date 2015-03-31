using com.net.interfaces;

/**应用层--信道管理类
 * 1、除了游戏逻辑信道外，可能还包含聊天信道
 * **/
namespace com.game
{
    public class AppNet
    {
        public static string ip;          //服务器IP
        public static int port;           //服务器端口 
        public static int bitLen;         //使用位元长度

        public static Main main;          //主线程 
        public static ISocket gameNet;    //逻辑信道

		public static int uniq = 99;	// unique
    }
}
