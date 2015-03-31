
using System;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/13 02:48:16 
 * function: 时间工具类
 * *******************************************************/

namespace com.game.utils
{
	public class TimeProp
	{
		public int Days;
		public int Hours;
		public int Minutes;
		public int Seconds;
	}

    public class TimeUtil
    {

        /// <summary>
        /// 根据时间戳返回日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 根据时间戳返回yyyy/MM/dd HH:mm:ss格式的日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string GetTimeYyyymmddHhmmss(uint timeStamp)
        {
            return string.Format("{0:yyyy/MM/dd HH:mm:ss}", GetTime(timeStamp.ToString()));
        }

		public static string GetTimeHhmmss(int leftTime)
		{
			int hour = leftTime/3600;
			string hourstr = hour < 10 ? "0" + hour : hour + "";

			int min = (leftTime-hour*3600)/60;
			string minstr = min < 10 ? "0" + min : min + "";

			int sec = leftTime%60;
			string secstr = sec < 10 ? "0" + sec : sec + "";

			string result = hourstr + ":" + minstr + ":" + secstr;

			return result;
		}

		public static string GetTimeYyyymmdd(uint timeStamp)
		{
			return string.Format("{0:yyyy-MM-dd}", GetTime(timeStamp.ToString()));
		}

		//根据时间戳获得距当前已过时间，如：30分钟前
		public static TimeProp GetElapsedTime(uint timeStamp)
		{
			TimeProp timeProp = new TimeProp();

			if (timeStamp > 0)
			{
				DateTime now = DateTime.Now;
				DateTime pre = GetTime(timeStamp.ToString());
				TimeSpan diff = now - pre;

				timeProp.Days = diff.Days;
				timeProp.Hours = diff.Hours;
				timeProp.Minutes = diff.Minutes;
				timeProp.Seconds = diff.Seconds;
			}
			else
			{
				timeProp.Days = 0;
				timeProp.Hours = 0;
				timeProp.Minutes = 0;
				timeProp.Seconds = 0;
			}

			return timeProp;
		}
    }
}