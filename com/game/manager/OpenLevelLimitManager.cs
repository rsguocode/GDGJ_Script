//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：OpenLevelLimitManager
//文件描述：界面打开等级限制管理类
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.data;
using com.game.manager;
using com.game.Public.Message;

public class OpenLevelLimitManager : MonoBehaviour {
	
	/// <summary>
	/// 根据guide_limit判断
	/// </summary>
	public static bool checkLeveByGuideLimitID(int id , int playerLevel)
	{
		bool isLimit = false;
		SysGuideLimitVo vo = BaseDataMgr.instance.GetDataById<SysGuideLimitVo>((uint)id);
		if(vo.ID == id && playerLevel < vo.lvl)
		{
			isLimit = true;
			string warngingStr = LanguageManager.GetWord("OpenLevelLimitManager.levelLimit", string.Empty + vo.lvl);
			MessageManager.Show(warngingStr);
		}
		return isLimit;
	}
}
