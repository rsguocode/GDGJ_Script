using UnityEngine;
using System.Collections;
using com.game.vo;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2013/12/12 09:39:52 
 * function:  本地变量管理类
 * *******************************************************/
namespace com.game.Public.LocalVar
{
	public class LocalVarManager{
//		public const string CUR_WORLD_ID = "CopyMode_CurWorldId";
//		public const string CUR_SUBWORLD_ID = "CopyMode_CurSubWorldId";
//		public const string CUR_COPYGROUP_ID = "CopyMode_CurCopyGroupId";
		public const string COPY_WORLD_ID = "Copy_WorldId";  //用于保存进入副本对应的城市ID
		public const string CHAT_REC_ZH_CHANNEL = "Chat_RecieveZongHeChannel";
		public const string CHAT_REC_ZY_CHANNEL = "Chat_RecieveZhenYingChannel";
		public const string CHAT_REC_SL_CHANNEL = "Chat_RecieveSiLiaoChannel";

		public const string LOGIN_SERVER = "Login_Server";

		public static void SetInt(string key, int value)
		{
			key = MeVo.instance.Id + key;
			PlayerPrefs.SetInt (key, value);
		}

		public static int GetInt(string key, int defaultValue = 0)
		{
			key = MeVo.instance.Id + key;
			return (PlayerPrefs.GetInt (key, defaultValue));
		}
		
		public static void SetString(string key, string value)
		{
			key = MeVo.instance.Id + key;
			PlayerPrefs.SetString (key, value);
		}

		public static string GetString(string key, string defaultValue)
		{
			key = MeVo.instance.Id + key;
			return (PlayerPrefs.GetString (key, defaultValue));
		}

		public static void SetFloat(string key, float value)
		{
			key = MeVo.instance.Id + key;
			PlayerPrefs.SetFloat (key, value);
		}
		
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			key = MeVo.instance.Id + key;
			return (PlayerPrefs.GetFloat (key, defaultValue));
		}

	}
}
