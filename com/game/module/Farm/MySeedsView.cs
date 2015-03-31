using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.manager;
using Com.Game.Module.Waiting;
using Com.Game.Module.Farm;
using System.Collections.Generic;
using com.game.Public.UICommon;
using com.game.data;
using com.game.utils;
using com.game.Public.Message;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/10 09:39:52 
 * function:  家园种植系统我的种子背包视图类
 * *******************************************************/
using Com.Game.Module.Manager;


namespace Com.Game.Module.Farm
{
	public class MySeedsView : BaseView<MySeedsView> {

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		private readonly int SEED_NUM_PER_GROUP = 8;         //每页的种子类型数

		private Button btn_close;
		private Transform seedGroups;
		private Transform fanye;

		private GameObject mySeedTips;
		private UILabel normalOut;
		private UILabel useLevel;
		private UILabel describe;
		private UILabel tipsSeedName;
		private UILabel matureTime;
		private Button btn_use;
		private Button tips_btn_close;
		private GameObject tipsItem;

		private List<MySeedVo> allSeeds;
		private uint selectedSeedId;
		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("topright/btn_close");
			seedGroups = FindInChild<Transform>("Center/SeedsPanel/ItemGroups");
			fanye = FindInChild<Transform>("Center/fanye");

			mySeedTips = FindInChild<Transform>("MySeedTips").gameObject;
			normalOut = FindInChild<UILabel>("MySeedTips/NormalOut");
			useLevel = FindInChild<UILabel>("MySeedTips/dj/value");
			describe = FindInChild<UILabel>("MySeedTips/describe");
			tipsSeedName = FindInChild<UILabel>("MySeedTips/name");
			matureTime = FindInChild<UILabel>("MySeedTips/time");
			btn_use = FindInChild<Button>("MySeedTips/btn_sy");
			tips_btn_close = FindInChild<Button>("MySeedTips/topright/btn_close");
			tipsItem = FindInChild<Transform>("MySeedTips/item").gameObject;

			btn_close.onClick = CloseMySeedsViewOnClick;
			btn_use.onClick = SeedUseOnClick;
			tips_btn_close.onClick = CloseMySeedTips;
		}

		private void InitLabelLanguage()
		{
	//		FindInChild<UILabel>("btn_rank/label").text = LanguageManager.GetWord("ArenaMainView.rank");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated += UpdateMySeedsView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			Singleton<WaitingView>.Instance.OpenView ();
			Singleton<FarmMode>.Instance.ApplySeedInfo ();  //请求种子背包信息

		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated -= UpdateMySeedsView;
			
		}

		//更新我的种子背包
		private void UpdateMySeedsView(object sender, int code)
		{
			if (code == Singleton<FarmMode>.Instance.UPDATE_MY_SEEDS)
			{
				Singleton<WaitingView>.Instance.CloseView ();
				allSeeds =  Singleton<FarmMode>.Instance.mySeedsList;

				//根据当前对象个数创建副本点页数和对应的页点数
				UIUtils.UpdateItemAndPoint (allSeeds.Count, seedGroups, fanye, 8);
				ShowSeeds();

			}
		}

		//根据数据显示种子属性
		private void ShowSeeds ()
		{
			Transform updateSeed;
			int updateYeshuSub;
			int updateItemSub;
			SysSeedVo seedVo;
			for (int item = 1; item <= allSeeds.Count; ++item)
			{
				//获取更新商品对象
				seedVo = BaseDataMgr.instance.GetSysSeedVo( allSeeds[item - 1].id );
				updateYeshuSub = item / SEED_NUM_PER_GROUP + (item % SEED_NUM_PER_GROUP > 0 ? 1 : 0);
				updateItemSub = item % SEED_NUM_PER_GROUP > 0 ? item % SEED_NUM_PER_GROUP : SEED_NUM_PER_GROUP;
				updateSeed = seedGroups.transform.FindChild(updateYeshuSub + "/Items/" + updateItemSub);

				//设置种子的各个属性
				updateSeed.FindChild("nr/Num/Value").GetComponent<UILabel>().text = allSeeds[item - 1].num.ToString();
				updateSeed.FindChild("nr/Name").GetComponent<UILabel>().text = seedVo.name;
				updateSeed.GetComponent<Button>().onClick = SeedOnClick;

				//设置种子图标
				ItemManager.Instance.InitItem (updateSeed.FindChild("item").gameObject, allSeeds[item - 1].id, ItemType.BaseGoods);
			}



		}

		//种子被点击
		private void SeedOnClick(GameObject go)
		{
			int itemId = int.Parse (go.name);
			int groupId = int.Parse (seedGroups.GetComponent<UICenterOnChild> ().centeredObject.name);
			selectedSeedId = allSeeds [(groupId - 1) * SEED_NUM_PER_GROUP + itemId - 1].id;
			OpenSeedTips ();
		}

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

			mySeedTips.SetActive (true);
		}

		//使用种子按钮被点击
		private void SeedUseOnClick(GameObject go)
		{
			byte landPos = Singleton<FarmMode>.Instance.selectedLandPos;
			if (landPos == 0)
			{
				MessageManager.Show("请先选择种植土地");
			}
			else
			{
				//播种并关闭我的种子界面
				Singleton<FarmMode>.Instance.ApplyCrop(selectedSeedId, landPos);

			}
		}

		//我的种子tips上的关闭按钮被点击
		private void CloseMySeedTips(GameObject go)
		{
			mySeedTips.SetActive (false);
		}

		//关闭按钮被点击
		private void CloseMySeedsViewOnClick(GameObject go)
		{
			this.CloseMySeedsView ();
		}

		//关闭我的种子view
		public void CloseMySeedsView()
		{
			mySeedTips.SetActive(false);
			this.CloseView();
		}
	}
}
