using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.manager;
using Com.Game.Module.Waiting;
using Com.Game.Module.Farm;
using System.Collections.Generic;
using com.game.Public.UICommon;
using com.game.data;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/10 09:39:52 
 * function:  家园种植系统种子商店视图类
 * *******************************************************/
using com.game.utils;
using com.game.Public.Message;
using Com.Game.Module.Manager;


namespace Com.Game.Module.Farm
{
	public class SeedsStoreView : BaseView<SeedsStoreView> {

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		private readonly int GOODS_PER_GROUP = 8;         //每页的商品个数

		private Button btn_close;
		private Transform seedGroups;
		private Transform fanye;

		//tips 相关
		private GameObject seedTips;
		private UILabel normalOut;
		private UILabel useLevel;
		private UILabel describe;
		private UILabel tipsSeedName;
		private UILabel matureTime;
		private Button btn_buy;
		private Button tips_btn_close;
		private GameObject tipsItem;

		//输入购买个数界面相关
		private Transform shuziUI;
		private Button btn_closeShuziUI;
		private Button btn_shuzi_0;
		private Button btn_shuzi_1;
		private Button btn_shuzi_2;
		private Button btn_shuzi_3;
		private Button btn_shuzi_4;
		private Button btn_shuzi_5;
		private Button btn_shuzi_6;
		private Button btn_shuzi_7;
		private Button btn_shuzi_8;
		private Button btn_shuzi_9;
		private UILabel shuZiValue;
		private Button btn_shuzi_del;
		private Button btn_shuzi_ensure;

		//购买确认界面相关
		private Transform buyUI;
		private Button btn_buy_ensure;
		private Button btn_buy_ensure_num;
		private Button btn_buy_ensure_close;
		private UILabel buy_ensure_name;
		private UILabel buy_ensure_num;
		private UILabel buy_ensure_price;
		private UISprite buy_ensure_moneyType;
		private GameObject buy_ensure_item;

		//
		private List<SysSeedMallVo> mallList;
		private uint selectedSeedId;
		private uint buyNum;
		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("topright/btn_close");
			seedGroups = FindInChild<Transform>("Center/SeedsPanel/ItemGroups");
			fanye = FindInChild<Transform>("Center/fanye");
			btn_close.onClick = CloseSeedsStoreView;

			//tips相关
			seedTips = FindInChild<Transform>("UpPanel/tips").gameObject;
			normalOut = FindInChild<UILabel>("UpPanel/tips/NormalOut");
			useLevel = FindInChild<UILabel>("UpPanel/tips/dj/value");
			describe = FindInChild<UILabel>("UpPanel/tips/describe");
			tipsSeedName = FindInChild<UILabel>("UpPanel/tips/name");
			matureTime = FindInChild<UILabel>("UpPanel/tips/time");
			btn_buy = FindInChild<Button>("UpPanel/tips/btn_buy");
			tips_btn_close = FindInChild<Button>("UpPanel/tips/topright/btn_close");
			btn_buy.onClick = BuySeedOnClick;
			tips_btn_close.onClick = CloseSeedTips;
			tipsItem = FindInChild<Transform>("UpPanel/tips/item").gameObject;

			//输入购买个数界面相关
			shuziUI = FindInChild<Transform>("UpPanel/shuzi");
			btn_closeShuziUI = FindInChild<Button>("UpPanel/shuzi/btn_close");
			btn_shuzi_0 = FindInChild<Button>("UpPanel/shuzi/anniu/0");
			btn_shuzi_1 = FindInChild<Button>("UpPanel/shuzi/anniu/1");
			btn_shuzi_2 = FindInChild<Button>("UpPanel/shuzi/anniu/2");
			btn_shuzi_3 = FindInChild<Button>("UpPanel/shuzi/anniu/3");
			btn_shuzi_4 = FindInChild<Button>("UpPanel/shuzi/anniu/4");
			btn_shuzi_5 = FindInChild<Button>("UpPanel/shuzi/anniu/5");
			btn_shuzi_6 = FindInChild<Button>("UpPanel/shuzi/anniu/6");
			btn_shuzi_7 = FindInChild<Button>("UpPanel/shuzi/anniu/7");
			btn_shuzi_8 = FindInChild<Button>("UpPanel/shuzi/anniu/8");
			btn_shuzi_9 = FindInChild<Button>("UpPanel/shuzi/anniu/9");
			shuZiValue = FindInChild<UILabel>("UpPanel/shuzi/Num");
			btn_shuzi_del = FindInChild<Button>("UpPanel/shuzi/btn_del");
			btn_shuzi_ensure = FindInChild<Button>("UpPanel/shuzi/btn_ensure");
			btn_closeShuziUI.onClick = CloseShuZiUI;
			btn_shuzi_0.onClick = NumOnClick;
			btn_shuzi_1.onClick = NumOnClick;
			btn_shuzi_2.onClick = NumOnClick;
			btn_shuzi_3.onClick = NumOnClick;
			btn_shuzi_4.onClick = NumOnClick;
			btn_shuzi_5.onClick = NumOnClick;
			btn_shuzi_6.onClick = NumOnClick;
			btn_shuzi_7.onClick = NumOnClick;
			btn_shuzi_8.onClick = NumOnClick;
			btn_shuzi_9.onClick = NumOnClick;
			btn_shuzi_del.onClick = NumDelOnClick;
			btn_shuzi_ensure.onClick = NumEnsureOnClick;

			//购买确认相关
			buyUI = FindInChild<Transform>("UpPanel/goumai");
			btn_buy_ensure = FindInChild<Button>("UpPanel/goumai/btn_ensure");
			btn_buy_ensure_num = FindInChild<Button>("UpPanel/goumai/center/buyNum");
			btn_buy_ensure_close = FindInChild<Button>("UpPanel/goumai/btn_close");
			buy_ensure_name = FindInChild<UILabel>("UpPanel/goumai/name");
			buy_ensure_num = FindInChild<UILabel>("UpPanel/goumai/center/buyNum");
			buy_ensure_price = FindInChild<UILabel>("UpPanel/goumai/center/price/value");
			buy_ensure_moneyType = FindInChild<UISprite>("UpPanel/goumai/center/price/sprite");
			buy_ensure_item = FindInChild<Transform> ("UpPanel/goumai/item").gameObject;

			btn_buy_ensure.onClick = BuyEnsureOnClick;
			btn_buy_ensure_num.onClick = BuyEnsureNumOnClick;
			btn_buy_ensure_close.onClick = BuyEnsureUICloseOnClick;
		}

		private void InitLabelLanguage()
		{
	//		FindInChild<UILabel>("btn_rank/label").text = LanguageManager.GetWord("ArenaMainView.rank");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated += UpdateSeedsStoreView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			this.InitSeedStore ();
			Singleton<FarmMode>.Instance.ApplySeedStoreInfo ((byte)SeedStoreType.GOLD);
	//		Singleton<WaitingView>.Instance.OpenView ();
	//		Singleton<FarmMode>.Instance.ApplySeedInfo ();  //请求竞技场信息

		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated -= UpdateSeedsStoreView;
			
		}

		//初始化种子商店
		private void InitSeedStore()
		{

			mallList = this.GetSeedsMallListByType((int)SeedStoreType.GOLD);
			UIUtils.UpdateItemAndPoint(mallList.Count, seedGroups, fanye, GOODS_PER_GROUP);
			
			Transform updateGoods;
			int updateYeshuSub;
			int updateItemSub;
			SysSeedMallVo mallVo;
			for (int item = 1; item <= mallList.Count; ++item)
			{
				//获取更新商品对象
				mallVo = mallList[item - 1];
				updateYeshuSub = item / GOODS_PER_GROUP + (item % GOODS_PER_GROUP > 0 ? 1 : 0);
				updateItemSub = item % GOODS_PER_GROUP > 0 ? item % GOODS_PER_GROUP : GOODS_PER_GROUP;
				updateGoods = seedGroups.transform.FindChild(updateYeshuSub + "/Items/" + updateItemSub);
				
				ShowSeedGoods(updateGoods, mallVo);
			}
		}

		//更具类别获取商品列表
		private List<SysSeedMallVo> GetSeedsMallListByType(int type)
		{
			List<SysSeedMallVo> seedMallList = BaseDataMgr.instance.GetSysSeedMallVoListByType (type);
			
			//对GoodsList根据queue值进行排序
			seedMallList.Sort (new ICSeedMallVo ());
			return seedMallList;
		}

		//根据配表，对商品的各个属性进行填充
		private void ShowSeedGoods(Transform seedGoods, SysSeedMallVo seedMallVo)
		{
			seedGoods.FindChild ("nr/money/Value").GetComponent<UILabel> ().text = seedMallVo.price.ToString();
			seedGoods.FindChild ("nr/Name").GetComponent<UILabel> ().text = seedMallVo.name;
			//		switch (seedMallVo.money)
			//		{
			//			case 1:
			//				seedGoods.FindChild ("nr/money").GetComponent<UISprite> ().spriteName = "yb1";
			//				break;
			//			default:
			//				break;
			//		}
			seedGoods.GetComponent<Button> ().onClick = SeedGoodsOnClick;

			//设置种子图标
			ItemManager.Instance.InitItem (seedGoods.FindChild("item").gameObject, (uint)seedMallVo.id, ItemType.BaseGoods);
		}

		//数据更新回调
		private void UpdateSeedsStoreView(object sender, int code)
		{
			if (code == Singleton<FarmMode>.Instance.UPDATE_SEED_GOODS_NUM)
			{
				this.UpdateSeedsNum();

			}
		}

		//更新种子商品可购买数量
		private void UpdateSeedsNum()
		{
			Transform updateGoods;
			int updateYeshuSub;
			int updateItemSub;
			uint seedId;
			SysSeedMallVo mallVo;
			for (int item = 1; item <= mallList.Count; ++item)
			{
				//获取更新商品对象
				mallVo = mallList[item - 1];
				updateYeshuSub = item / GOODS_PER_GROUP + (item % GOODS_PER_GROUP > 0 ? 1 : 0);
				updateItemSub = item % GOODS_PER_GROUP > 0 ? item % GOODS_PER_GROUP : GOODS_PER_GROUP;
				updateGoods = seedGroups.transform.FindChild(updateYeshuSub + "/Items/" + updateItemSub);


				updateGoods.FindChild("nr/Num").GetComponent<UILabel>().text = Singleton<FarmMode>.Instance.seedGoodsDic[(uint)mallVo.id].ToString();
			}
		}

		//关闭种子商店按钮被点击
		private void CloseSeedsStoreView(GameObject go)
		{
			this.CloseView ();
		}

		//种子商品被点击
		private void SeedGoodsOnClick(GameObject go)
		{
			int itemId = int.Parse (go.name);
			int groupId = int.Parse (seedGroups.GetComponent<UICenterOnChild> ().centeredObject.name);
			SysSeedMallVo seedMallVo = mallList [(groupId - 1) * GOODS_PER_GROUP + itemId - 1];
			selectedSeedId = (uint)seedMallVo.id;

			OpenSeedTips ();
		}

// -------------------------------------------------------tips相关-------------------------------------------------------
		//打开种子tips
		private void OpenSeedTips()
		{
			SysSeedVo seedVo = BaseDataMgr.instance.GetSysSeedVo (selectedSeedId);
			string outGoods = StringUtils.GetValueCost (seedVo.normal_gains) [0];
			uint outId = uint.Parse (outGoods.Split(',')[0]);
			uint outNum = uint.Parse (outGoods.Split(',')[1]);
			string outName;
			switch (outId)
			{
			case 1:
				outName = "经验";
				break;
			case 2:
				outName = "金币";
				break;
			default:
				outName = BaseDataMgr.instance.GetDataById<SysItemVo>(outId).name;
				break;
			}
			normalOut.text = "常规产出：" + outNum + outName;
			useLevel.text = seedVo.lvl.ToString ();
			describe.text = seedVo.desc;
			tipsSeedName.text = seedVo.name;
			matureTime.text = "成熟时间：" + (int)seedVo.mature_time / 3600 + "小时";

			//设置种子图标
			ItemManager.Instance.InitItem (tipsItem, selectedSeedId, ItemType.BaseGoods);

			seedTips.SetActive (true);
		}

		//种子tips上的关闭按钮被点击
		private void CloseSeedTips(GameObject go)
		{
			seedTips.SetActive (false);
		}

		//购买种子按钮被点击
		private void BuySeedOnClick(GameObject go)
		{
			seedTips.SetActive (false);
			shuziUI.gameObject.SetActive (true);
		}

		//----------------------------------------------输入购买个数界面相关-------------------------------------------
		//关闭数字窗口界面
		private void CloseShuZiUI(GameObject go)
		{
//			Log.info(this, "关闭数字输入窗口");
			shuziUI.gameObject.SetActive (false);
		}
		
		//数字键点击
		private void NumOnClick(GameObject go)
		{
			int num = int.Parse (shuZiValue.text + go.name);
			int maxNum = (int)Singleton<FarmMode>.Instance.seedGoodsDic [selectedSeedId];
			num = num > maxNum? maxNum: num;
			shuZiValue.text = num.ToString ();
		}
		
		//数字键盘的删除按钮被点击
		private void NumDelOnClick(GameObject go)
		{
			if (shuZiValue.text != "")
			{
				shuZiValue.text = shuZiValue.text.Substring (0, shuZiValue.text.Length - 1);
			}
		}
		
		//数字键盘的确定按钮被点击
		private void NumEnsureOnClick(GameObject go)
		{
			if (shuZiValue.text != "")
			{
				this.buyNum = uint.Parse(shuZiValue.text);
				shuziUI.gameObject.SetActive(false);
				this.OpenBuyEnsureUI();
			}
			else
			{
//				string showMessage = isBuy?"请输入购买数量":"请输入赠送数量";
				MessageManager.Show("请输入购买数量");
			}
		}

//--------------------------------------------------------------购买确认界面---------------------------------
		//打开购买确认界面
		private void OpenBuyEnsureUI()
		{
//			Log.info(this, "打开购买确认界面");
			buyUI.gameObject.SetActive(true);
			buy_ensure_num.text = buyNum.ToString();
			SysSeedMallVo mallVo = BaseDataMgr.instance.GetSysSeedMallVo (selectedSeedId, (uint)SeedStoreType.GOLD);
			buy_ensure_price.text = (buyNum * mallVo.price).ToString ();
			buy_ensure_name.text = mallVo.name;

//		switch (seedMallVo.money)
//		{
//			case 1:
//				btn_ensure_moneyType.spriteName = "yb2";
//				break;
//			default:
//				break;
//		}
			//设置种子图标
			ItemManager.Instance.InitItem (buy_ensure_item, selectedSeedId, ItemType.BaseGoods);
		}
		
		//确认购买按钮被点击
		private void BuyEnsureOnClick(GameObject go)
		{
			this.CloseBuyEnsureUI();
			Singleton<FarmMode>.Instance.ApplyBuySeed ((byte)SeedStoreType.GOLD, selectedSeedId, buyNum);  //购买种子
		}
		
		//购买确认界面数字按钮被点击
		private void BuyEnsureNumOnClick(GameObject go)
		{
			this.CloseBuyEnsureUI();
			shuziUI.gameObject.SetActive (true);
		}
		
		//确认购买界面关闭按钮被点击
		private void BuyEnsureUICloseOnClick(GameObject go)
		{
			this.CloseBuyEnsureUI();
		}
		
		//关闭购买确认界面
		public void CloseBuyEnsureUI()
		{
			buyUI.gameObject.SetActive (false);
		}
	}
}
