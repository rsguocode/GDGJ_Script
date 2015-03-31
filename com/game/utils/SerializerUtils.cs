﻿﻿﻿using System;
using com.u3d.bases.debug;
using com.game.manager;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/**序列发序列化工具类**/
namespace com.utils
{
    public class SerializerUtils
    {
        /**基于二进制反序列化**/
        public static object binaryDerialize(byte[] bytes)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes); 
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Binder = new UBinder();
                object data = formatter.Deserialize(ms);
                ms.Close();
                ms.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("****-binaryDeserialize() 基于二进制反序列化失败:" + ex.Message);
                Log.error("SerializerUtils", "-binaryDeserialize() 基于二进制反序列化失败:" + ex.Message);
            }
            return null;
        }

        /**Json反序列化**/
        public static object jsonDerialize(byte[] bytes)
        {
            /*Type typeObj = null;
            Dictionary<string, string> jsonTable = null;
            Dictionary<string, object> objectTable = null;
            
            string output = Encoding.UTF8.GetString(bytes);
            object objData = JsonConvert.DeserializeObject(output, typeof(Dictionary<string, Dictionary<string, string>>));
            Dictionary<string, Dictionary<string, string>> data = (Dictionary<string, Dictionary<string, string>>)objData;
            Dictionary<String, Dictionary<String, object>> releaseData = new Dictionary<string, Dictionary<string, object>>();
            //int time = Environment.TickCount;

            foreach (string key in data.Keys)
            {
                jsonTable = data[key];
                typeObj = BaseDataMgr.instance.getClzType(key);
                objectTable = new Dictionary<string, object>();

                foreach (string keyId in jsonTable.Keys)
                {
                    objectTable.Add(keyId, JsonConvert.DeserializeObject(jsonTable[keyId], typeObj));
                    //Log.info("SerializerUtils", jsonTable[keyId]);
                }
                releaseData.Add(key, objectTable);
                Log.info("SerializerUtils", "key:" + key + ",typeObj:" + typeObj.ToString());
                //Log.info("", "\n");
            }
            //Log.info("SerializerUtils", "-jsonDeserialize() 基础数据初始化耗时:" + (Environment.TickCount - time) + " ms");
            return releaseData;*/
            return null;
        }

    }
}
