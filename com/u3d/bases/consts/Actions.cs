﻿﻿﻿﻿﻿using System;

/**角色动作常量类
 * @author 陈开谦
 * @data   2013-06-09
 * **/
namespace com.u3d.bases.consts
{
    public class Actions
    {
        //========================================//
		//           角色模型动作常量             //
		//========================================//
		//场景动作
		public const String SIT="sit";                //场景打坐
		public const String RUN="run";                //场景跑动
		public const String WAIT="wait";              //场景站立
		public const String HAND="hand";              //悠闲待机
		public const String RIDE_RUN="riderun";       //场景坐骑跑动
		public const String RIDE_WAIT="ridewait";     //场景坐骑站立
		public const String DOUBLE="double";          //双修
		public const String WALK="walk";              //场景走动		

        public const String STORE = "store";          //攻击1动作
        public const String DEFENSE = "defense";      //攻击1动作

		//战斗动作
		public const String WINS ="wins";             //战斗胜利动作 
		public const String STAND="stand";            //战斗站立
		public const String FIGHT="fight";            //释放技能待机动作
		public const String DEATH="death";            //战斗死亡
		public const String DEATHWAIT ="deathwait";   //死亡待机
		public const String PARRY="parry";            //战斗格挡
		public const String INJURED="injured";        //战斗受击
		public const String SPECIAL="special";        //战斗特殊动作
		public const String MAGIC = "magic";          //施法动作
		
		//技能动作
		public const String CONJURE="conjure";          //技能施法
		public const String HIT="hit";                  //技能击中
		public const String FLY="fly";                  //技能飞行
		public const String BUFF="buff";                //技能buff
		public const String EFFECT="effect";            //动画特效

        public const String ATTACK = "attack";          //攻击1动作
		public const String ATTACK1="attack1";          //攻击1动作
        public const String ATTACK2 = "attack2";        //攻击1动作
        public const String ATTACK3 = "attack3";        //攻击1动作
        public const String ATTACK4 = "attack4";        //攻击1动作
        public const String SKILL1 = "skill1";          //技能1;
        public const String SKILL2 = "skill2";          //技能2;
        public const String SKILL3 = "skill3";          //技能3;
        public const String SKILL4 = "skill4";          //技能3;
        public const String SUPERSKILL = "superskill";  //技能大招
		public const String SUPERSKILL1="superskill1";  //技能大招

        //攻击信息索引
        public const int VELOCITY_ORIGIN = 0;             //攻击产生的推力
        public const int ANGLE = 1;             //推力角度
        public const int PROTECTVALUE = 2;      //保护值
        public const int FORCEFEEDBACK = 3;     //力反馈
        public const int HITRECOVER = 4;        //硬直
        public const int HURTANIMATION = 5;     //受击动作

        //技能突进信息索引
        public const int RUSH_VELOCITY = 0;
        public const int RUSH_ACCELERATION = 1;
        public const int RUSH_DISTANCE = 2;
        public const int RUSH_DIRECTION = 3;

        //技能移动信息索引
        public const int MOVE_DISTANCE = 0;
        public const int MOVE_TIME = 1;
        public const int MOVE_DIRECTION = 2;

        //技能中可控制移动信息索引
        public const int CTRL_MOVE_SPEED = 0;

        //受击表现
        public const int HurtNormal = 1001;
        public const int HurtFly = 1003;

        //受击动作
        public const int Hurt1 = 1;
        public const int Hurt2 = 2;
    }
}
