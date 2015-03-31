﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**任务常量类**/
namespace com.u3d.bases.task
{
    public class TaskConst
    {
        public const String ZERO = "0";   //0字符串

        //======================================//
		//              任务类型常量             //
		//======================================//
		public const int MAIN_TASK=1;     //主线任务
		public const int SUB_TASK=2;      //支线任务
		public const int DAY_TASK=3;      //日常任务
		public const int FOR_TASK=4;      //环跑任务
		public const int NEW_TAKS=5;      //新手任务
		
		
		//======================================//
		//              任务状态常量             //
        //   0:可接  1:已接  2:完成  3:不可接    //
		//======================================//
		public const int STATE_ACCEPT   =0;      
		public const int STATE_DOING    =1;        
		public const int STATE_FINISH   =2;      
        public const int STATE_UNACCEPT =3;   

        //======================================//
        //              NPC任务状态常量         //
        //   0:没任务,1:可接,2:已接,3:可完成    //
        //======================================//
        public const int NPC_TASK_NO     = 0;      
        public const int NPC_TASK_ACCPET = 1;     
        public const int NPC_TASK_DOING  = 2;     
        public const int NPC_TASK_FINISH = 3;    


        //======================================//
		//            任务条件分类常量          //
		//======================================//
		public const String C_3="3";       //杀怪    [在场景]
		public const String C_4="4";       //采集物品[在场景]
		public const String C_5="5";       //收集物品[在场景]
		public const String C_6="6";       //检测背包或装备栏是否有某物品
		public const String C_7="7";       //主角升级
		public const String C_8="8";       //技能升级
		public const String C_9="9";       //通关某个副本
        public const String C_10 = "10";   //参加活动


        /**任务状态**/
		public static String getTaskState(int state){
            String desc = "";
			switch(state)
			{
				case STATE_UNACCEPT: desc="不可接"; break;
				case STATE_ACCEPT:   desc="可接";   break;
				case STATE_DOING:    desc="已接";   break;
				case STATE_FINISH:   desc="完成";   break;
			}
            return desc;
		}
		
		/**任务分类**/
        public static String getTaskType(int type)
        {
            String desc = "";
			switch(type)
			{
				case MAIN_TASK: desc = "主"; break;
                case SUB_TASK:  desc = "支"; break;
                case DAY_TASK:  desc = "日"; break;
                case FOR_TASK:  desc = "军"; break;
			}
			return desc;
		}
		
		/**取得NPC任务状态描述**/
        public static String getNpcTaskState(int state)
        {
            String desc = "没有";
			switch(state)
			{
				case 1: desc = "可接"; break;
                case 2: desc = "已接"; break;
                case 3: desc = "完成"; break;
			}
            return desc;
		}
		
    }
}
