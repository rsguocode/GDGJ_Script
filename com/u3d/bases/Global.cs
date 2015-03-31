using System;

/**全局变量类
 * @author 陈开谦
 * @data   2013-06-0
 * **/
namespace com.u3d.bases.consts
{
    public class Global
    {
		public const float HERO_WEIGHT = 1;   //人物位置距离相对于屏幕边界的距离
		public const int UNITYTOPIXEL = 80;   //UNITY单位与像素值的比例
		public const float SERVICE_TO_CLIENT = 0.5f;  //服务端坐标和客户端坐标的比值
		public const ushort CLIENT_TO_SERVICE = 2;  //服务端坐标和客户端坐标的比值

		public const float MOVEJUDGE = 5.0f;  //角色移动判定
		public const float MOVEINTERAL = 1.0f;   //角色移动同步间隔时间


        public const int ROLE_RUN_SPEED = 5;       //角色跑动速度 
        public const int MONSTER_RUN_SPEED = 2;    //怪物跑动速度
        public const int HORSE_RUN_SPEED = 5;      //坐骑跑动速度

        //地图在Y方向的移动范围
        public static float MIN_Y = 0.2f;
        public static float MAX_Y = 2f;

        //用于记录按方向次数
        public static float Times_Left = 0;
        public static float Times_Right = 0;
        public static float Times_Top = 0;
        public static float Times_Down = 0;
    }
}
