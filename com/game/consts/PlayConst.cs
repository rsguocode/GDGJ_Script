//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：PlayConst;
//文件描述 : 放置一些显示对象的常量;
//创建者：潘振峰;
//创建日期：2014/6/6 18:03:25
//////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.game.consts
{
    public class PlayConst
    {
        /// <summary>
        /// 头部;
        /// </summary>
        public const string POS_HEAD = "overhead";
        /// <summary>
        /// 胸口;
        /// </summary>
        public const string POS_CHEST = "chest";
        /// <summary>
        /// 手部;
        /// </summary>
        public const string POS_HAND = "hand";
        /// <summary>
        /// 大腿;
        /// </summary>
        public const string POS_LEG = "leg";
        /// <summary>
        /// 脚部;
        /// </summary>
        public const string POS_FOOT = "origin";
        /// <summary>
        /// 脖子;
        /// </summary>
        public const string POS_NECK = "Neck";

        /// <summary>
        /// BUFF状态
        /// </summary>
        public enum BUFF_STATE
        {
            NO_START, //未开启;
            RUNNING, //运行状态;
            END//结束;
        }

        /// <summary>
        /// 抓取自救左右摇杆次数;
        /// </summary>
        public const int GRASP_RELEASE_MIN_KEY_COUNT = 8;
        /// <summary>
        /// 抓取自动失效时间(s);
        /// </summary>
        public const int GRASP_RELEASE_AUTO_TIME = 3;

        /// <summary>
        /// 普通抓投终结 检测范围X正方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_X_OFFSET = 100;
        /// <summary>
        /// 普通抓投终结 检测范围X负方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_X_2_OFFSET = 25; 
        /// <summary>
        /// 普通抓投终结 检测范围Y正方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_Y_OFFSET = 25; 
        /// <summary>
        /// 普通抓投终结 检测范围Y负方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_Y_2_OFFSET = 25; 
        /// <summary>
        /// 普通抓投终结 检测范围Z正方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_Z_OFFSET = 25; 
        /// <summary>
        /// 普通抓投终结 检测范围Z负方向 unity单位;
        /// </summary>
        public const int NORMAL_GRAB_Z_2_OFFSET = 0; 
    }
}
