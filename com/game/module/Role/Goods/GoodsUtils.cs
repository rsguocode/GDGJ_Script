using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using PCustomDataType;

public class GoodsUtils : Singleton<GoodsUtils>
{
	private UIWidgetContainer lastWc = null;//上次选中的item
	public PGoods GetSelectedObject(List<UIWidgetContainer> itemList,UIWidgetContainer wc,List<PGoods> goodsList)
	{
		//UIWidgetContainer currentItem = go.GetComponent<UIWidgetContainer>();
		//currentItem = go.GetComponent<UIWidgetContainer>();
		int i = 0, length = itemList.Count;
		for (; i < length; i++)
		{
			if (itemList[i].Equals(wc))
				break;
		} 
		if(goodsList.Count<i)
			return null;
		return goodsList[i] ;
	}
	/// <summary>
	/// 高亮处理
	/// </summary>
	/// <param name="go">Go.</param>
	public void GoodsHightlightOnClick(GameObject go)
	{
		UIWidgetContainer item = go.GetComponent<UIWidgetContainer>();
		if(!item.Equals(lastWc))
		{
			UISprite hightlight = item.FindInChild<UISprite>("gaoguang");
			hightlight.gameObject.SetActive(true);
			if(lastWc!=null)
			{
				lastWc.FindInChild<UISprite>("gaoguang").gameObject.SetActive(false);
			}
		}
	}
}
