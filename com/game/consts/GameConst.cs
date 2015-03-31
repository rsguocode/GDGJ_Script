/**游戏中常量类**/
namespace com.game.consts
{
    public class GameConst
    {
		public const double PROB_FULL_D = 10000d;	//概率基准
        public const int PROB_FULL_I = 10000;	//概率基准
        public const float PositionUnit = 0.001f;  //坐标转换

        public const char REGX_1 = '|';      //分割符
        public const char REGX_2 = ',';
        public const char REGX_3 = '@';
        public const char REGX_4 = '#';
        public const char REGX_5 = ';';

        public const int PIXEL_TO_UNITY = 80;

        public const int SEX_WUMAN = 1;      //性别女
        public const int SEX_MAN = 2;        //性别男 

		/** 职业 */
		public const int JOB_JIAN = 1;		//魔剑士
		public const int JOB_FASHI = 2;		//元素师
		public const int JOB_CHIKE = 3;		//刺客

		public const int JOB_MON = 4;		//怪物(计算伤害时使用)
		public const int JOB_PET = 5;		//宠物  

		public const int ATTACK = 1;         //普通攻击
        public const int SUPERSKILL = 2;     //圈攻击
        public const int MAGIC = 3;          //搓动攻击
        public const int STORE = 4;          //蓄气
        public const int STOREATTACK = 5;    //挑动

		public const int EFFECT_ANIMATION = 0; //普通动画特效
		public const int EFFECT_FX_MAKER = 1;  //FX Maker特效

		/** 攻击类型 */ 
		public const int ATT_TYPE_PHY = 1;		//物理攻击
		public const int ATT_TYPE_MAG = 2;		//魔法攻击


		/** 关于技能伤害类型 */
		public const uint DAMAGE_TYPE_MISS = 0;		// miss
		public const uint DAMAGE_TYPE_CRIT = 2;		//暴击
		public const uint DAMAGE_TYPE_NORMAL = 3;	//普通
		public const uint DAMAGE_TYPE_RIGOR = 5;	//僵直
		public const uint DAMAGE_TYPE_BACK = 6;		//击退
		public const uint DAMAGE_TYPE_REF = 7;		//反射
		public const uint DAMAGE_TYPE_ABSORB = 8;	//伤害吸收

		/** 关于技能产生的状态常量 */
		public const uint SKILL_DAMAGE_STATE_NONE = 0;		// 无状态
		public const uint SKILL_DAMAGE_STATE_STIFF = 0x1;	//僵直

		//体力购买方式
		public const byte BUY_VIGOUR_BY_MONEY = 0;  //购买
		public const byte BUY_VIGOUR_BY_TOOL = 1;  //道具

		//系统设置相关
        public const uint CritShakeKey = 10001;  //暴击震动
        public const uint HidePlayerKey = 10002;  //隐藏玩家
        public const uint MuteKey = 10003;  //静音
        public const uint SceneVolumnKey = 10004;  //暴击震动
        public const uint EffectVolumnKey = 10005;  //暴击震动
        //技能位置 系统设置
        public const uint SkillPos1 = 13001;  //技能位置1
        public const uint SkillPos2 = 13002;  //技能位置2
        public const uint SkillPos3 = 13003;  //技能位置3
        public const uint SkillPos4 = 13004;  //技能位置4
        public const uint SkillPos5 = 13005;  //技能位置5

		//通用对话框遮罩Depth
		public const int ConfirmMaskDepth = 1000;

		//金银岛参数
		public const ushort MaxRobTimes = 2;  //玩家打劫次数上限
		public const ushort MaxAssistTimes = 5;  //玩家协助次数上限

		//货币类型
		public const int MONEY_DIAMOND = 1;//钻石
		public const int MONEY_BINGING_DIAMOND = 2;//绑定钻石
		public const int MONEY_DIAM = 3;//钻石

		//传送点坐标相关
		public const float HitPointEffectOffH = 0.45f; //传送点特效Y坐标
		public const float HitPointRadius  = 0.5f;      //副本传送点半径



		//系统公告
		public const uint SystemNoticeId  = 1;   //系统公告Id

		//法师半身像位置
		public const float MagicBustX = -448.3f;

		//系统缺省音量
		public const uint DefaultSceneVolumn = 40;
		public const uint DefaultEffectVolumn = 100;

        //普通攻击长按键不间断执行攻击
        public const bool IS_NORMAL_COMBO_BY_PRESS_LONG = true;
    }
}
