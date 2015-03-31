//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：EquipIdentificationView
//文件描述：装备图鉴工具类
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.utils;
using com.game.vo;

namespace com.game.module.EquipIdentification
{
	public class EquipIndentificationUtil
	{
		//过滤所有数据
		public static List<SysEquipVo> getFilterAllEquipData()
		{
			List<SysEquipVo> allEquipList = new List<SysEquipVo> ();
			Dictionary<uint,object> allEquipmentDic = BaseDataMgr.instance.GetDicByType<SysEquipVo>();
			foreach (SysEquipVo allEquipVo in allEquipmentDic.Values)
			{
				if (allEquipVo.type == GoodsConst.TYPE_COMMON && allEquipVo.subtype == GoodsConst.SUBTYPE_COMMON && allEquipVo.color != GoodsConst.RED)
				{
					if(allEquipVo.job == 0 || allEquipVo.job == MeVo.instance.job)
					{
						allEquipList.Add(allEquipVo);
					}
				}
			}
			return allEquipList;
		}
		
		//过滤推荐数据
		public static List<SysEquipVo> getRecommendEquipData(List<SysEquipVo> allEquipList)
		{
			List<SysEquipVo> recommenEquipList = new List<SysEquipVo> ();
			foreach (SysEquipVo equipListVo in allEquipList)
			{
				string [] lvlStr = StringUtils.GetValueListFromString (equipListVo.lvl);
				int equipLevel = int.Parse (lvlStr [0]);
				int fugyreLevel = 0;
				if(MeVo.instance.Level <= 10)
				{
					fugyreLevel = 10;
				}else
				{
					fugyreLevel = (int)MeVo.instance.Level / 10 * 10;
				}
				if (equipLevel == fugyreLevel &&
				    equipListVo.color != GoodsConst.ORANGE &&
				    equipListVo.color != GoodsConst.RED)
				{
					recommenEquipList.Add (equipListVo);
				}
			}
			return recommenEquipList;
		}
		
		//设置分类数据
		public static List<SysEquipVo> getEquipListData(int type , List<SysEquipVo> allEquipList)
		{
			List<SysEquipVo> filterEquipList = new List<SysEquipVo>();
			foreach (SysEquipVo equipListVo in allEquipList)
			{
				if (equipListVo.pos == type)
				{
					if(equipListVo.job == 0 || equipListVo.job == MeVo.instance.job)
					{
						filterEquipList.Add(equipListVo);
					}
				}
			}
			return filterEquipList;
		}
		
		//截取数组之间的数据
		public static List<SysEquipVo> getBetweenData(List<SysEquipVo> dataList , int start , int end)
		{
			List<SysEquipVo> data = new List<SysEquipVo> ();
			for (int i = start ; i < end ; i ++)
			{
				data.Add(dataList[i]);
			}
			return data;
		}

		//获取当前装备是否大于10级
		public static bool getCurrentEquipIsGreaterTen(int lvl)
		{
			bool isTen = lvl > MeVo.instance.Level + EquipIdentificationConst.POOR_GRADES_10 ? true : false;
			return isTen;
		}

		//根据sort字段排序
		public static int setSortCompareList(SysEquipVo x, SysEquipVo y)
		{
			return x.sort - y.sort;
		}
	}
}