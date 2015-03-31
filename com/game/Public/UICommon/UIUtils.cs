using UnityEngine;
using System.Collections;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/02/19 10:39:52 
 * function:  常用UI实现API
 * *******************************************************/
using Com.Game.Module.Role;


namespace com.game.Public.UICommon
{
	public class UIUtils{

		/// <summary>
		/// 用途描述：用于多页滑动显示时根据配置表创建足够多的页数，对象数和页点数
		/// 限制：需与固定的结构配合使用
		/// 详细步骤：
		/// 创建足够多的物品组和对应的页点对象.
		/// 显示所需的物品组和页点对象
		/// 隐藏显示物品组内多余的物品对象(只隐藏，不显示。显示在具体的更新商品显示后再控制显示)
		/// 对物品组合页点对象进行重新排列位置
		/// </summary>
		/// <param name="itemNum">需要的物品数量.</param>
		/// <param name="itemGroups">物品组的父对象.</param>
		/// <param name="fanye">对应的页点的父对象.</param>
		/// <param name="itemNumPerGroup">每个物品组内包含的物品对象个数.</param>
		public static void UpdateItemAndPoint(int itemNum, Transform itemGroups, Transform fanye, int itemNumPerGroup)
		{
			int needItemGroupNum = itemNum / itemNumPerGroup + (itemNum % itemNumPerGroup > 0 ? 1 : 0);
			int curItemGroupNum = itemGroups.childCount;
			Transform itemGroupModle = itemGroups.FindChild ("1");
			Transform yeDianModel = fanye.FindChild ("1");
			
			// 创建足够多的物品组和对应的页点对象.
			while(curItemGroupNum < needItemGroupNum)
			{
				//根据物品个数增加页数
				curItemGroupNum++;
				GameObject clone = UIUtils.CloneObj(itemGroupModle);
				clone.name = curItemGroupNum.ToString();
				
				//根据物品个数增加页点数
				GameObject yeDianClone = UIUtils.CloneObj(yeDianModel);
				yeDianClone.name = curItemGroupNum.ToString();
			}
			
			//显示所需的物品组和页点对象
			for (int i = 1; i <= curItemGroupNum; ++i)
			{
				if (i <= needItemGroupNum)
				{
					itemGroups.FindChild(i.ToString()).gameObject.SetActive(true);
					fanye.FindChild(i.ToString()).gameObject.SetActive(true);
				}
				else
				{
					itemGroups.FindChild(i.ToString()).gameObject.SetActive(false);
					fanye.FindChild(i.ToString()).gameObject.SetActive(false);
				}
			}
			
			//隐藏显示物品组内多余的物品对象(只隐藏，不显示。显示在具体的更新商品显示后再控制显示)
			Transform lastYe = itemGroups.FindChild (needItemGroupNum.ToString ());
			int lastYeGoodsNum = itemNum % itemNumPerGroup > 0 ? itemNum % itemNumPerGroup : itemNumPerGroup;
			for (int i = lastYeGoodsNum + 1; i <= itemNumPerGroup;++i)
			{
				lastYe.FindChild("Items/" + i).gameObject.SetActive(false);
			}
			
			//调用grid的reposition重新进行排版,并设置显示第一页。新版NGUI默认只排版一次，排版完后就关闭了这个组件
			itemGroups.GetComponent<UIGrid>().Reposition();
			itemGroups.GetComponent<UICenterOnChild>().CenterOn(itemGroups.FindChild("1"));
			
			//重置页点位置，并置第一个点为亮，其他为暗
			fanye.localPosition = new Vector3 (fanye.localPosition.x -(curItemGroupNum - 1) * fanye.GetComponent<UIGrid>().cellWidth / 2,
			                                   fanye.localPosition.y, 0);
			fanye.GetComponent<UIGrid> ().Reposition ();
			fanye.FindChild ("1").GetComponent<UIToggle> ().value = true;
		}
		
		/// <summary>
		/// 克隆一个与被克隆对象具有相同父节点和transform组件的对象
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="obj">被克隆对象.</param>
		public static GameObject CloneObj(Transform obj)
		{
			GameObject clone = GameObject.Instantiate(obj.gameObject, obj.localPosition, obj.localRotation) as GameObject;
			clone.transform.parent = obj.parent;
			clone.transform.localPosition = obj.localPosition;
			clone.transform.localRotation = obj.localRotation;
			clone.transform.localScale = obj.localScale;
			
			return clone;
		}

		/// <summary>
		/// 将剩余时间（秒数）转换为hh:mm:ss格式
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="cd">Cd.</param>
		public static void ShowTimeToHMS(int time, UILabel cd)
		{
			int min, second, hour;
			hour = (int)time / 3600;
			time = (int)time - hour * 3600;
			min = (int)time  / 60;
			second = time - min * 60;
			
			cd.text =   (hour < 10? "0": "") + hour + ":"
						+(min < 10? "0": "") + min + ":" 
						+ (second < 10? "0": "") + second;
			//			secondLabel.text = (second < 10? "0": "") + second;
		}

		/// <summary>
		/// 将剩余时间（秒数）转换为mm:ss格式
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="cd">Cd.</param>
		public static void ShowTimeToMS(int time, UILabel cd)
		{
			int min, second, hour;
			min = (int)time / 60;
			second = (int)time - min * 60;
			
			cd.text = (min < 10? "0": "") + min + ":" + (second < 10? "0": "") + second;
			//			secondLabel.text = (second < 10? "0": "") + second;
		}

		//切换sprite为灰化shader的图集
		public static void ChangeGrayShader(UISprite spr, int depth = 0)
		{
			if (spr.atlas == spr.normalAtlas)
			{
				spr.enableGrayAtlas = true;
				spr.atlas = spr.grayAtlas;
				
				spr.depth = depth==0? spr.depth + 500: depth;
			}
			else
			{
				spr.enableGrayAtlas = true;
			}

//			if (!spr.enableGrayAtlas)
//			{
//				spr.enableGrayAtlas = true;
//				spr.atlas = spr.grayAtlas;
//
//				spr.depth = depth==0? spr.depth + 500: depth;
//			}
		}

		//切换sprite为正常shader的图集
		public static void ChangeNormalShader(UISprite spr, int depth = 0)
		{
			if (spr.atlas == spr.normalAtlas)
			{
				spr.enableGrayAtlas = false;
			}
			else
			{
				spr.enableGrayAtlas = false;
				spr.atlas = spr.normalAtlas;
				spr.depth = depth==0? spr.depth - 500: depth;
			}

//			if (spr.enableGrayAtlas)
//			{
//				spr.enableGrayAtlas = false;
//				spr.atlas = spr.normalAtlas;
//				spr.depth = depth==0? spr.depth - 500: depth;
//			}
		}

		//将属性类型转化成对应的string
		public static string ChangeAttrTypeToString(byte type)
		{
			string result = "";
			switch (type)
			{
				//属性加成常用的12个类型//
				case (byte)AttrType.ATTR_ID_HP:
					result = "生命";
					break;
				case (int)AttrType.ATTR_ID_MP:
					result = "魔法";
					break;
				case (int)AttrType.ATTR_ID_HIT:
					result = "命中";
					break;
				case (int)AttrType.ATTR_ID_CRIT:
					result = "暴击";
					break;
				case (int)AttrType.ATTR_ID_CRIT_RATIO:
					result = "暴伤";
					break;
				case (int)AttrType.ATTR_ID_ATT_MAX:
					result = "攻击";
					break;
				case (int)AttrType.ATTR_ID_ATT_DEF_M:
					result = "魔防";
					break;
				case (int)AttrType.ATTR_ID_DODGE:
					result = "闪避";
					break;
				case (int)AttrType.ATTR_ID_FLEX:
					result = "韧性";
					break;
				case (int)AttrType.ATTR_ID_LUCK:
					result = "幸运";
					break;
				case (int)AttrType.ATTR_ID_HURT_RE:
					result = "格挡";
					break;
				case (int)AttrType.ATTR_ID_ATT_DEF_P:
					result = "物防";
					break;
				//-------------------------------------//

				case (int)AttrType.ATTR_ID_ATT_MIN:
					result = "最小攻击";
					break;
				case (int)AttrType.ATTR_ID_SPEED:
					result = "速度";
					break;
				case (int)AttrType.ATTR_ID_ATT_P_MIN:
					result = "最小物攻";
					break;
				case (int)AttrType.ATTR_ID_ATT_P_MAX:
					result = "最大物攻";
					break;
				case (int)AttrType.ATTR_ID_ATT_M_MIN:
					result = "最小魔攻";
					break;
				case (int)AttrType.ATTR_ID_ATT_M_MAX:
					result = "最大魔攻";
					break;
				case (int)AttrType.ATTR_ID_STR:
					result = "力量";
					break;
				case (int)AttrType.ATTR_ID_AGI:
					result = "敏捷";
					break;
				case (int)AttrType.ATTR_ID_PHY:
					result = "体力";
					break;
				case (int)AttrType.ATTR_ID_WIT:
					result = "智力";
					break;
				default:
					result = "";
					break;
			}
			return result;
		}


        //按钮的通用响应
        public static void TogglePress(GameObject go, bool isPress)
        {
            UISprite highLight = NGUITools.FindInChild<UISprite>(go,"highlight");
            if (highLight != null)
            {
                TweenAlpha.Begin(highLight.gameObject, 0.15f, isPress ? 1f : 0f);
            }
        } 

		/// <summary>
		/// 设置button按钮状态，false时会设置button变灰且不可点击
		/// </summary>
		/// <param name="button">Button需挂载BoxCollider组件，子对象中包含background，background对象是一个UISprite对象且已复制灰化和正常图集</param>
		/// <param name="state">If set to <c>true</c> state.</param>
		public static void SetButtonState(Transform btn, bool state, int depth)
		{
			if (state)
			{
				UIUtils.ChangeNormalShader(btn.FindChild("background").GetComponent<UISprite>(), depth);
				btn.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				UIUtils.ChangeGrayShader(btn.FindChild("background").GetComponent<UISprite>(), depth);
				btn.GetComponent<BoxCollider>().enabled = false;
			}
		}
    
    }
}
