
/**场景类型常量**/
namespace com.game.consts
{
    public class MapTypeConst
    {
        //场景类型常量
        public const int CITY_MAP = 1;             //城镇场景
        public const int COPY_MAP = 2;             //副本场景
		public const int SPECIAL_MAP = 3;             //特殊场景

		//场景子类型
		public const int MAIN_COPY = 1; 			//主线副本
		public const int DAEMONISLAND_COPY = 7;		//恶魔岛副本
		public const int FIRST_BATTLE_COPY = 8;		//首战副本

        //场景中跳转点类型常量
        public const int WORLDMAP_POINT = 1;      //世界传送点
        public const int COPY_POINT = 2;          //副本传送点
        public const int MAP_POINT = 3;           //场景传送点(即到达该传送点时直接切入另一个场景)

        //场景ID常量
        public const uint FirstFightMap = 10000; //第一场战斗
        public const uint MajorCity = 10001;    //主城ID
        public const uint FirstCopy = 21001; //第一个副本

		public const uint WORLD_BOSS = 40001 ;   //世界Boss场景Id
		public const uint ARENA_MAP = 60001;   // 英雄本场景Id
        public const uint GoldHit_MAP = 40002;  //击石成金的场景Id
		public const uint GoldSilverIsland_MAP = 60002;   //金银岛场景Id

		// 巨人副本id
		public const uint MAPID_DUNGEON_GIANT = 200001;

		// 场景中角色属性变更
		public const byte ROLE_HP_CHANGE_KEY = 1; //血变化
		public const byte ROLE_HP_FULL_CHANGE_KEY = 2; //最大血变化
		public const byte ROLE_MP_CHANGE_KEY = 3; //蓝变化
		public const byte ROLE_MP_FULL_CHANGE_KEY = 4; //最大变化
		public const byte ROLE_LV_CHANGE_KEY = 5;  // 等级变化
		public const byte Role_BUFF_LIST_CHANGE_KEY = 6; //buff列表变化
        public const byte RolePetChange = 7;  //宠物Id变化

		// 角色复活方式
		public const byte ROLE_REVIVE_USE_DIAM = 1; //使用钻石复活
		public const byte ROLE_REVIVE_USE_ITEM = 2; //使用道具复活
		public const byte ROLE_REVIVE_RETURN_CITY = 3; //回城复活
        public const byte ROLE_REVIVE_NO_MONEY = 4; //不用钻石原地复活
    }
}
