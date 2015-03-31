//////////////////////////////////////////////////////////////////////////////////////////////
//文件描述：VIP VO
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Com.Game.Module.VIP
{
	public class VIPVo
	{

	}

	//奖励的元素
	public class AwardItem
	{
		public GameObject obj; //物品对象
		public Button ItemBtn;
		public UILabel name;  // 物品名称
		public UILabel count; //物品个数
		public UISprite icon; //图片图标
		public int itemID;    //物品ID
	}

	public class ITEM
	{
		public GameObject pannel;
		public Button btnAward;   //领取奖励的按钮
		public UISprite btnBg;    //领取按钮的背景图
		public UILabel btnWord;   //按钮上的提示文字
		public GameObject desItem;//描述的第一条
		public GameObject desObj;  
		public UILabel vipTitle; //显示VIP等级的标题
		public UILabel vipTequan;//显示VIP特权的标题
		public List<AwardItem> AwardList = new List<AwardItem>(); //礼包的物品列表
		public List<UILabel> DesList = new List<UILabel>();       //VIP特权描述的列表           
	}
}