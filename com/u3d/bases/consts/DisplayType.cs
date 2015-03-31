﻿﻿﻿﻿﻿
/**游戏对象类型常量
 * @author 陈开谦
 * @data   2013-06-09
 * **/
namespace com.u3d.bases.consts
{
    public class DisplayType
    {
        /**场景角色**/
        public const int ROLE=100;
        //UI角色
        public const int ROLE_MODE = 101;
        /**场景NPC**/
		public const int NPC=200;
        /**场景Boss**/
		public const int BOSS=300;
        /**普通怪物**/
        public const int MONSTER = 400;
		/**掉落物**/
		public const int DROP=401;
		/**宠物**/
		public const int PET = 402;
        /**场景传送点**/
        public const int MAP_POINT = 500;

        /**副本陷阱*/
        public const int Trap = 501;

        /**副本传送点**/
        public const int COPY_POINT = 600;

		/**场景随机装饰物，如来往的人,add by zhuwei**/
		public const int DECORATE_CREATOR = 700;

        /**通用动画类**/
		public const int ANIMATION=10000;
    }
}
