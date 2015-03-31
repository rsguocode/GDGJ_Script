//////////////////////////////////////////////////////////////////////////////////////////////
//文件描述：VIP工具类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

using com.game.data;
using com.game.manager;

namespace Com.Game.Module.VIP
{
	public class VIPUtil
	{
		//获取下一级VIP
		public static int GetNextVIPPrice(int presentVIP)
		{
			if ((uint)presentVIP + 1 <= (int)VIPConsts.PAGEMAX_12)
			{
				SysVipInfoVo vipinfo = BaseDataMgr.instance.GetVIPDescribe((uint)presentVIP + 1);
				return vipinfo.diam;
			}else
			{
				SysVipInfoVo vipinfo = BaseDataMgr.instance.GetVIPDescribe((uint)VIPConsts.PAGEMAX_12);
				return vipinfo.diam;
			}
		}

		//根据点击的对象获取物品的ID
		public static int GetItemID(string name , ITEM [] ShowPannel , PageIndex pageIndex)
		{
			foreach (AwardItem item in ShowPannel[pageIndex.current-1].AwardList)
			{
				if (item.obj.name == name)
				{
					return item.itemID;     //返回物品的ID
				}
			}
			return 0;
		}
	}
}