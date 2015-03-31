﻿
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/02 02:56:49 
 * function: 手机特有的一些效果的控制，如手机震动等，统一管理
 * *******************************************************/

namespace com.game.utils
{
    public class PhoneUtil
    {
        /// <summary>
        /// 手机震动效果
        /// </summary>
        public static void Vibrate()
        {
            Handheld.Vibrate();
        }

        /// <summary>
        /// 禁止屏幕休眠
        /// </summary>
        public static void DonotSleep()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}