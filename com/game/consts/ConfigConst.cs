
using System.Collections.Generic;
using com.game.data;
using com.game.module.test;
using com.u3d.bases.debug;

namespace com.game.consts
{
	public class ConfigConst : Singleton<ConfigConst>
	{
        private Dictionary<string,string> configDatas = new Dictionary<string, string>(); 

	    public void SetConfigData(Dictionary<uint,object> data)
	    {
            foreach (object obj in data.Values)
            {
                SysConfigVo configdata = (SysConfigVo)obj;
                configDatas.Add(configdata.name, configdata.value);
	        }
	    }

        /// <summary>
        /// 获取int类型配置常量
        /// </summary>
	    public int GetConfigData(string name)
	    {
	        int data = 0;
	        if (configDatas.ContainsKey(name))
	        {
                int.TryParse(configDatas[name], out data);
	        }
	        else
	        {
                Log.error(this,"配置项不存在:"+name);
	        }
	        return data;
	    }
        /// <summary>
        /// 获取List<int>类型配置常量
        /// </summary>
	    public List<int> GetConfigDatas(string name)
	    {
            List<int> result = new List<int>();
	        if (configDatas.ContainsKey(name))
	        {
	            string configdatas = configDatas[name];
                configdatas = configdatas.Substring(1, configdatas.Length - 2);
	            string[] datas = configdatas.Split(',');
                foreach (string data in datas)
	            {
	                result.Add(int.Parse(data));
	            }

            }
            else
            {
                Log.error(this, "配置项不存在:" + name);
            }
            return result;
	    }
	}
}
