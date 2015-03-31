using System;

namespace com.game.Public.Confirm
{
    public class ConfirmCommands
    {
        //通用提示框
        public const string OK_CANCEL = "OK_CANCEL";  //确定取消
        public const string OK = "OK";  //确定
		public const string SELECT_ONE = "SELECT_ONE";  //两者必选其一

        //需要单独实例的
        public const string DEAD = "DEAD";  //死亡复活
        public const string BUY_VIGOUR = "BUY_VIGOUR";  //体力购买
		public const string CONNECT_SERVER_ERROR = "CONNECT_SERVER_ERROR";  //连接服务器错误
        public const string DeedCost = "DeedCost";      //购买契约
        public const string WantedTaskRefresh = "WantedTaskRefresh"; //刷新悬赏任务
    }
}

