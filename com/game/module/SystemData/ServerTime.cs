﻿﻿
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/13 03:31:36 
 * function: 系统时间
 * *******************************************************/

namespace com.game.module.SystemData
{
    public class ServerTime
    {
        private static ServerTime _instance; 
        private int _timestamp; //服务器本地时间的时间戳,单位到秒


        public int Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                vp_Timer.CancelAll("UpdateServerTime");
                vp_Timer.In(1f, UpdateServerTime, 10000000, 1f);
            }
        }

        private void UpdateServerTime()
        {
            _timestamp += 1;
        }

        public static ServerTime Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServerTime();
                }
                return _instance;
            }
            set { _instance = value; }
        }
    }
}