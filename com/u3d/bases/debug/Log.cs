using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/**日志接口**/
namespace com.u3d.bases.debug
{
    public class Log
    {
        private const int DEBUG = 1;   
        private const int INFO  = 2;
        private const int WARIN = 3;
        private const int ERROR = 4;
        private const int ALL   = 5;
        private const int CLOSE = 6;
        private static IList<int> levelList = new List<int>();


        /**打印方式[true:控制台打印,false:Unity Debug打印]**/
        public static bool printWay { get; set; }
 
        /**添加打印级别**/
        public static void addLevel(int level){
            if(level>0) levelList.Add(level);
        }

        public static void ClearLog()
        {
            levelList.Clear();
        }

        /**添加打印级别
         * @param format 格式化字符串(级别1,级别2)
         * **/
        public static void addLevel(String format) {
            if (format==null || format.Equals(String.Empty)) return;
            String[] array=format.Split(',');
            if (array == null || array.Length < 1) return;
            int level = 0;
            foreach (String str in array) 
            {
                level=int.Parse(str);
                if (levelList.IndexOf(level) == -1) levelList.Add(level);
            }
        }

        public static void debug(String name, String log)
        {
            if (levelList.IndexOf(DEBUG) != -1 || levelList.IndexOf(ALL) != -1)
            {
                format(name, "DEBUG", log);
            }
        }

        public static void debug(object obj, String log)
        {
            debug(obj.GetType().FullName , log);
        }

        public static void info(String name,String log){
            if (levelList.IndexOf(INFO) != -1 || levelList.IndexOf(ALL) != -1)
            {
                format(name, "INFO", log);
            }
        }

        public static void info(object obj, String log)
        {
            info(obj.GetType().FullName , log);
        }

        public static void warin(String name, String log)
        {
            if (levelList.IndexOf(WARIN) != -1 || levelList.IndexOf(ALL) != -1)
            {
                format(name, "WARIN", log);
            }
        }

        public static void warin(object obj, String log)
        {
            warin(obj.GetType().FullName, log);
        }

        public static void error(String name, String log)
        {
            if (levelList.IndexOf(ERROR) != -1 || levelList.IndexOf(ALL) != -1)
            {
                format(name, "ERROR", log);
            }
        }

        public static void error(object obj, String log)
        {
            error(obj.GetType().FullName, log);
        }

        private static void format(String name,String level,String log) {
            StringBuilder str = new StringBuilder();
            str.Append("[" + level + "]");
            str.Append("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+":"+DateTime.Now.Millisecond+"] ");
            str.Append("[" + name+"]");
            str.Append(" "+log);
            /*
            if (printWay) Console.WriteLine(str);
            else
            {
                if (level.Equals("ERROR"))
                {
                    Debug.LogError(str);
                }
                else if (level.Equals("WARIN"))
                {
                    Debug.LogWarning(str);
                }
                else
                {
                    Debug.Log(str);
                }
            }
            */
        }

        internal static void debug(Com.Game.Module.Arena.ArenaControl arenaControl, uint p)
        {
            throw new NotImplementedException();
        }
    }
}
