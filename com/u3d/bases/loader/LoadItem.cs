﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**加载回调类**/
namespace com.u3d.bases.loader
{
    public delegate void LoadFinishBack(string url,object[] args=null);    //加载结束回调
    public delegate void LoadProgressBack(string url,int progress);        //加载进度回调

    public class LoadItem
	{
        public string url;                      //资源地址
        public int priority;                    //加载优先级
        public int resType;                     //资源类型
        internal object[] args;                 //附带参数
        internal LoadFinishBack callback;       //加载成功回调
        internal LoadFinishBack errorback;      //加载失败回调

        private StringBuilder str;              //打印串
        internal WWW www;                       //加载句柄
        internal Type types;                    //指定加载资源类型(Resources.Load用)
        internal int loadBeginTime;             //开始加载时间

        internal void call()
        {
            if (callback != null) callback(url, args);
        }

        internal void callError()
        {
            if (errorback != null) errorback(url, args);
        }

        internal void dispose()
		{
            str = null;
            url = null;
            args = null;
            www = null;
            types = null;
            callback = null;
            errorback = null;
		}
		
		public string toString(){
            if (str == null)
            {
                str = new StringBuilder();
                str.Append("[resType:" + resType + ",");
                str.Append("[url:" + url + ",");
                str.Append("[priority:" + priority + "]");
            }
            return str.ToString();
		}
	}
}
