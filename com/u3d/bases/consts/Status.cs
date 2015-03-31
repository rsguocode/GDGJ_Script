using UnityEngine;

/**角色状态控制相关常量类
 * @author 骆琦
 * @data   2013-10-31
 * **/

namespace com.u3d.bases.consts
{
    public class Status
    {
        /**基本状态*/
        public const int IDLE = 0;               //待机状态		
        public const int RUN =  1;               //移动状态
        public const int ROLL = 2;               //滚动状态
        public const int ATTACK1 = 3;            //普通攻击1
        public const int ATTACK2 = 4;            //普通攻击2
        public const int ATTACK3 = 5;            //普通攻击3
        public const int ATTACK4 = 6;            //普通攻击4
        public const int SKILL1 = 7;             //技能1攻击
        public const int SKILL2 = 8;             //技能2攻击
        public const int SKILL3 = 9;             //技能3攻击
        public const int HURT1 = 10;             //受击状态1
        public const int DEATH = 11;             //死亡状态
        public const int HURT2 = 12;             //受击状态2
        public const int HURT3 = 13;             //受击状态3
        public const int HURT4 = 14;             //受击状态4,击飞
        public const int HURTDOWN = 15;          //击飞后从落地到弹起再落地的状态
        public const int STANDUP = 16;           //从躺在地上的状态爬起来到站立状态的过程
        public const int SKILL4 = 17;            //技能4
        public const int Win = 18;               //胜利状态
        public const int SKILL5 = 19;            //技能5
        public const int SKILL6 = 20;            //技能6
        public const int SKILL7 = 21;            //技能7
        public const int SKILL8 = 22;            //技能8
        public const int ATTACK5 = 23;            //普通攻击5:普攻冲刺终结;
        public const int ATTACK6 = 24;            //普通攻击6:普通抓投终结;

        //属性状态;
        /// <summary>死亡</summary>
        public const int ATTR_STATU_DEATH = 0;
        /// <summary>待机</summary>
        public const int ATTR_STATU_IDLE = 1;
        /// <summary>移动</summary>
        public const int ATTR_STATU_MOVE = 2;
        /// <summary>攻击</summary>
        public const int ATTR_STATU_ATTACK = 3;
        /// <summary>技能</summary>
        public const int ATTR_STATU_SKILL = 4;
        /// <summary>受击僵直</summary>
        public const int ATTR_STATU_ATTACKED_STOP = 5;
        /// <summary>击倒</summary>
        public const int ATTR_STATU_ATTACK_DOWN = 6;
        /// <summary>抓取</summary>
        public const int ATTR_STATU_GRASP = 7;
        /// <summary>被抓取</summary>
        public const int ATTR_STATU_GRASPED = 8;
        /// <summary>击飞</summary>
        public const int ATTR_STATU_ATTACK_FLY = 9;
        /// <summary>闪避</summary>
        public const int ATTR_STATU_DODGE = 10;
        /// <summary>霸体</summary>
        public const int ATTR_STATU_BATI = 11;
        /// <summary>无敌</summary>
        public const int ATTR_STATU_WUDI = 12;
        /// <summary>昏迷</summary>
        public const int ATTR_STATU_HUNMI = 13;
        /// <summary>定身</summary>
        public const int ATTR_STATU_DINGSHEN = 14;
        /// <summary>缴械</summary>
        public const int ATTR_STATU_JIAOXIE = 15;
        /// <summary>恐惧</summary>
        public const int ATTR_STATU_KONGJU = 16;
        /// <summary>变形</summary>
        public const int ATTR_STATU_BIANXING = 17;
        /// <summary>致盲</summary>
        public const int ATTR_STATU_ZHIMANG = 18;


        /**连击状态*/
        public const int COMB_0 = 0;  //0连击状态
        public const int COMB_1 = 1;  //1连击状态
        public const int COMB_2 = 2;  //2连击状态
        public const int COMB_3 = 3;  //3连击状态
        public const int COMB_4 = 4;  //4连击状态
        public const int COMB_5 = 5;  //5连击状态:普攻冲刺终结;
        public const int COMB_6 = 6;  //6连击状态:普通抓投终结;

        /**animator中对应的动画状态*/
        public static readonly int NAME_HASH_IDLE = Animator.StringToHash("Base Layer.Idle");  //Name Hash for Idle Statu
        public static readonly int NAME_HASH_RUN = Animator.StringToHash("Base Layer.Run");  //Name Hash for Run Statu
        public static readonly int NAME_HASH_ROLL = Animator.StringToHash("Base Layer.Roll");  //Name Hash for Roll Statu
        public static readonly int NAME_HASH_ATTACK1 = Animator.StringToHash("Base Layer.Attack1");  //Name Hash for Attack1 Statu
        public static readonly int NAME_HASH_ATTACK2 = Animator.StringToHash("Base Layer.Attack2");  //Name Hash for Attack2 Statu
        public static readonly int NAME_HASH_ATTACK3 = Animator.StringToHash("Base Layer.Attack3");  //Name Hash for Attack3 Statu
        public static readonly int NAME_HASH_ATTACK4 = Animator.StringToHash("Base Layer.Attack4");  //Name Hash for Attack4 Statu
        public static readonly int NAME_HASH_ATTACK5 = Animator.StringToHash("Base Layer.Attack5");  //Name Hash for Attack5 Statu
        public static readonly int NAME_HASH_ATTACK6 = Animator.StringToHash("Base Layer.Attack6");  //Name Hash for Attack6 Statu
        public static readonly int NAME_HASH_SKILL1 = Animator.StringToHash("Base Layer.Skill1");  //Name Hash for Skill1 Statu
        public static readonly int NAME_HASH_SKILL2 = Animator.StringToHash("Base Layer.Skill2");  //Name Hash for Skill2 Statu
        public static readonly int NAME_HASH_SKILL3 = Animator.StringToHash("Base Layer.Skill3");  //Name Hash for Skill3 Statu
        public static readonly int NAME_HASH_SKILL4 = Animator.StringToHash("Base Layer.Skill4");  //Name Hash for Skill4 Statu
        public static readonly int NAME_HASH_SKILL5 = Animator.StringToHash("Base Layer.Skill5");  //Name Hash for Skill5 Statu
        public static readonly int NAME_HASH_SKILL6 = Animator.StringToHash("Base Layer.Skill6");  //Name Hash for Skill6 Statu
        public static readonly int NAME_HASH_SKILL7 = Animator.StringToHash("Base Layer.Skill7");  //Name Hash for Skill7 Statu
        public static readonly int NAME_HASH_SKILL8 = Animator.StringToHash("Base Layer.Skill8");  //Name Hash for Skill8 Statu
        public static readonly int NAME_HASH_DEATH = Animator.StringToHash("Base Layer.Death");  //Name Hash for Death Statu
        public static readonly int NAME_HASH_HURT1 = Animator.StringToHash("Base Layer.Hurt1");  //Name Hash for Be Attacked Statu
        public static readonly int NAME_HASH_HURT2 = Animator.StringToHash("Base Layer.Hurt2");  //Name Hash for Be Attacked Statu
        public static readonly int NAME_HASH_HURT3 = Animator.StringToHash("Base Layer.Hurt3");  //Name Hash for Be Attacked Statu
        public static readonly int NAME_HASH_HURT4 = Animator.StringToHash("Base Layer.Hurt4");  //Name Hash for Be Attacked Statu
        public static readonly int NAME_HASH_HURTDOWN = Animator.StringToHash("Base Layer.HurtDown");  //Name Hash for HurtDown Statu
        public static readonly int NAME_HASH_STANDUP = Animator.StringToHash("Base Layer.StandUp"); //Name Hash for StandUp Statu
        public static readonly int NAME_HASH_DEATH_END = Animator.StringToHash("Base Layer.DeathEnd"); //Name Hash for DeathEnd Statu
        public static readonly int NAME_HASH_Win = Animator.StringToHash("Base Layer.Win"); //Name Hash for DeathEnd Statu
        public static readonly int NAME_HASH_Special = Animator.StringToHash("Base Layer.Special"); //Name Hash for DeathEnd Statu


        /**animator中用于控制状态的参数名*/
        public const string STATU = "State";
        public const string IDLE_TYPE = "IdleType";

        /**待机状态*/
        public const float NORMAL_IDLE = 0f; //正常待机状态
        public const float BATTLE_IDLE = 1f; //战斗待机状态，副本中的待机状态
        public const float FrontIdle = 10f;  //正面待机状态
        public const float CreateRoleIdleOne = 110f;  //创角第一次表现
        public const float CreateRoleIdleTwo = 120f;  //创角第二次表现
        public const float CreateRoleIdleThree = 130f; //创建第三次表现

    }
}
