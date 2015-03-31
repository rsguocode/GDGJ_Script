using UnityEngine;
using System.Collections;
using com.game.module.test;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/02/25 09:39:52 
 * function:  地宫寻宝系统视图类
 * *******************************************************/
using System.Collections.Generic;
using com.game.manager;
using com.game.Public.UICommon;
using com.game.utils;
using Com.Game.Module.Manager;
using com.game.consts;
using com.game.data;
using Com.Game.Module.Waiting;
using Com.Game.Module.Role;
using com.game.module.effect;
using Com.Game.Module.Tips;


namespace Com.Game.Module.Treasure
{
	public class TreasureView : BaseView<TreasureView> {
		private readonly uint XUN_BAO_GOODS_ID = 120002;     //寻宝道具ID

		public override string url { get { return "UI/Treasure/TreasureView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; } }

		private Button btn_close;
		private Button btn_refresh;
		private UISprite Treasure1;
		private UISprite Treasure2;
		private UISprite Treasure3;
		private UILabel Treasure1Name;
		private UILabel Treasure2Name;
		private UILabel Treasure3Name;
		private UISprite TakedTreasure1;
		private UISprite TakedTreasure2;
		private UISprite TakedTreasure3;
		private UILabel TakedTreasure1Name;
		private UILabel TakedTreasure2Name;
		private UILabel TakedTreasure3Name;
		private GameObject AdditionalAward;
		private UISprite AdditionalAwardTubiao;
		private UILabel RemainTimes;
		private UILabel RemainGoodsNum;
		private GameObject RefresureEnsurePannel;
		private UIToggle AutoRefresh;
		private Button refresh_btn_ensure;
		private Button refresh_btn_quxiao;

		private Transform awardCombineItems;
		private Vector3 selectedTreasurePos;
		private GameObject selectedTreasure;
//		private bool applyTreasureInfoWait = false;
		protected override void Init()
		{
			btn_close = FindInChild<Button>("topright/btn_close");
			btn_refresh = FindInChild<Button>("right/btn_fresh");
			awardCombineItems = FindInChild<Transform>("left/SpecialAwardPanel/Items");
			Treasure1 = FindInChild<UISprite>("right/Treasure1");
			Treasure2 = FindInChild<UISprite>("right/Treasure2");
			Treasure3 = FindInChild<UISprite>("right/Treasure3");
			Treasure1Name = FindInChild<UILabel>("right/Treasure1/label");
			Treasure2Name = FindInChild<UILabel>("right/Treasure2/label");
			Treasure3Name = FindInChild<UILabel>("right/Treasure3/label");
			TakedTreasure1 = FindInChild<UISprite>("right/GetBao1");
			TakedTreasure2 = FindInChild<UISprite>("right/GetBao2");
			TakedTreasure3 = FindInChild<UISprite>("right/GetBao3");
			TakedTreasure1Name = FindInChild<UILabel>("right/GetBao1/label");
			TakedTreasure2Name = FindInChild<UILabel>("right/GetBao2/label");
			TakedTreasure3Name = FindInChild<UILabel>("right/GetBao3/label");
			RemainTimes = FindInChild<UILabel>("right/cishu/value");
			RemainGoodsNum = FindInChild<UILabel>("right/daoju/value");
			AdditionalAward = FindInChild<Transform>("right/AdditionalAward-0").gameObject;
			AdditionalAwardTubiao = FindInChild<UISprite>("right/AdditionalAward-0/tubiao");
			RefresureEnsurePannel = FindInChild<Transform>("RefresureEnsurePannel").gameObject;
			AutoRefresh = FindInChild<UIToggle>("RefresureEnsurePannel/AutoFresh");
			refresh_btn_ensure = FindInChild<Button>("RefresureEnsurePannel/btn_ensure");
			refresh_btn_quxiao = FindInChild<Button>("RefresureEnsurePannel/btn_quxiao");

			InitLabelLanguage ();

			btn_close.onClick = CloseTreasureView;
			btn_refresh.onClick = RefreshOnClick;
			refresh_btn_ensure.onClick = RefreshEnsureOnClick;
			refresh_btn_quxiao.onClick = RefreshCancleOnClick;
			Treasure1.GetComponent<Button>().onClick = TakeTreasure;
			Treasure2.GetComponent<Button>().onClick = TakeTreasure;
			Treasure3.GetComponent<Button>().onClick = TakeTreasure;

			AdditionalAward.GetComponent<UIToggle>().onSelect = GoodsOnClick;
			InitViewByConfig ();
		}

		//静态多语言处理
		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("top/label").text = LanguageManager.GetWord("TreasureView.diGongXunBao");
			FindInChild<UILabel>("left/jianli").text = LanguageManager.GetWord("TreasureView.specialAward");
			FindInChild<UILabel>("left/remark").text = LanguageManager.GetWord("TreasureView.remark");
			FindInChild<UILabel>("right/cishu/label").text = LanguageManager.GetWord("TreasureView.remainTimes");
			FindInChild<UILabel>("right/daoju/label").text = LanguageManager.GetWord("TreasureView.remainGoods");
			FindInChild<UILabel>("right/btn_fresh/label").text = LanguageManager.GetWord("TreasureView.refresh");
			FindInChild<UILabel>("RefresureEnsurePannel/btn_ensure/label").text = LanguageManager.GetWord("TreasureView.ensure");
			FindInChild<UILabel>("RefresureEnsurePannel/btn_quxiao/label").text = LanguageManager.GetWord("TreasureView.cancle");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<TreasureMode>.Instance.dataUpdated += UpdateTreasureView;
			Singleton<TreasureMode>.Instance.dataUpdated += GetTreasureCallBack;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
//			Singleton<WaitingView>.Instance.OpenView ();
			AutoRefresh.value = false;
			Singleton<TreasureMode>.Instance.ApplyTreasureInfo ();
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<TreasureMode>.Instance.dataUpdated -= UpdateTreasureView;
			Singleton<TreasureMode>.Instance.dataUpdated -= GetTreasureCallBack;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		//从配置表读取信息，初始化TreasureView
		private void InitViewByConfig()
		{
			Dictionary<uint, object> treasureConfigDic = BaseDataMgr.instance.GetSysTreasureDic ();
			int treasureNum = treasureConfigDic.Count;

			//克隆足够数量的对象
			Transform mode = awardCombineItems.FindChild ("1");
			GameObject clone;
			for (int i  = awardCombineItems.childCount + 1; i <= treasureNum; ++i)
			{
				clone = UIUtils.CloneObj(mode);
				clone.name = i.ToString();
			}
			awardCombineItems.GetComponent<UIGrid>().Reposition();

			//初始化
			int j = 1;
			Transform orderItem;
			string[] treasureList;
			uint awardGoodId;
			GameObject Award;
			foreach (uint key in treasureConfigDic.Keys)
			{
				orderItem = awardCombineItems.FindChild(j.ToString());
				//显示额外奖励
				awardGoodId = uint.Parse(StringUtils.GetValueListFromString
				                           (BaseDataMgr.instance.GetSysTreasure(key).other_award )[0]);
				//增加额外奖励点击处理
				Award = orderItem.FindChild("Award-0").gameObject;
				Singleton<ItemManager>.Instance.InitItem(Award, awardGoodId, ItemType.BaseGoods);
				Award.name = "Award-" + awardGoodId;
				Award.GetComponent<UIToggle>().onSelect = GoodsOnClick;

				//显示组合宝藏点
				treasureList = StringUtils.GetValueListFromString( BaseDataMgr.instance.GetSysTreasure(key).order );
				orderItem.FindChild("Place_1").GetComponent<UISprite>().spriteName = treasureList[0];
				orderItem.FindChild("Place_2").GetComponent<UISprite>().spriteName = treasureList[1];
				orderItem.FindChild("Place_3").GetComponent<UISprite>().spriteName = treasureList[2];
				++j;
			}

		}

		//更新宝藏信息
		private void UpdateTreasureView(object send, int code)
		{
			if (code == Singleton<TreasureMode>.Instance.UPDATE_TREASURE_INFO)
			{
				//随机宝藏点位置
				Treasure1.transform.localPosition = new Vector3(Random.Range(-200f, -120f),
				                                                     Random.Range(115f, -33f),0);
				Treasure2.transform.localPosition = new Vector3(Random.Range(-40f, 40f),
				                                                     Random.Range(115f, -33f),0);
				Treasure3.transform.localPosition = new Vector3(Random.Range(120f, 200f),
				                                                     Random.Range(115f, -33f),0);

				TreasureInfo treasInfo = Singleton<TreasureMode>.Instance.TreasureInfo;
				AdditionalAwardTubiao.color =  ColorConst.GRAY;
				RemainTimes.text = treasInfo.remainFindTimes.ToString();
				RemainGoodsNum.text = Singleton<GoodsMode>.Instance.GetCountByGoodsId(XUN_BAO_GOODS_ID).ToString();
				Treasure1.spriteName = treasInfo.treasure1;
				Treasure2.spriteName = treasInfo.treasure2;
				Treasure3.spriteName = treasInfo.treasure3;
				Treasure1Name.text = treasInfo.treasure1Name;
				Treasure2Name.text = treasInfo.treasure2Name;
				Treasure3Name.text = treasInfo.treasure3Name;

				Treasure1.name = treasInfo.treasure1;
				Treasure2.name = treasInfo.treasure2;
				Treasure3.name = treasInfo.treasure3;
				Treasure1.GetComponent<BoxCollider>().enabled = true;
				Treasure2.GetComponent<BoxCollider>().enabled = true;
				Treasure3.GetComponent<BoxCollider>().enabled = true;

//				GameObject AdditionaAward;
				if (treasInfo.takedTreasure1Id == 0)
				{
					TakedTreasure1.spriteName = "wenhao";
					TakedTreasure2.spriteName = "wenhao";
					TakedTreasure3.spriteName = "wenhao";
					TakedTreasure1.color = ColorConst.GRAY;
					TakedTreasure2.color = ColorConst.GRAY;
					TakedTreasure3.color = ColorConst.GRAY;
					TakedTreasure1Name.text = "";
					TakedTreasure2Name.text = "";
					TakedTreasure3Name.text = "";
					Singleton<ItemManager>.Instance.InitItem(AdditionalAward, 0, ItemType.BaseGoods);

//					AdditionaAward = orderItem.FindChild("Award").gameObject;
//					Singleton<ItemManager>.Instance.InitItem(Award, awardGoodId, ItemType.BaseGoods);
					AdditionalAward.name = "AdditionalAward-0";

				}
				else
				{
					SysTreasure info = BaseDataMgr.instance.GetSysTreasure( (uint)treasInfo.takedTreasure1Id );
					string[] order = StringUtils.GetValueListFromString(info.order );

					TakedTreasure1.spriteName = order[0];
					TakedTreasure2.spriteName = order[1];
					TakedTreasure3.spriteName = order[2];
					TakedTreasure1.color = new Color(1,1, 1, 1);
					TakedTreasure2.color = treasInfo.takedTreasure2Id == 0? ColorConst.GRAY: new Color(1, 1, 1, 1);
					TakedTreasure3.color = treasInfo.takedTreasure3Id == 0? ColorConst.GRAY: new Color(1, 1, 1, 1);
					TakedTreasure1Name.text = BaseDataMgr.instance.GetSysTreasure (uint.Parse(order[0])).place;
					TakedTreasure2Name.text = BaseDataMgr.instance.GetSysTreasure (uint.Parse(order[1])).place;
					TakedTreasure3Name.text = BaseDataMgr.instance.GetSysTreasure (uint.Parse(order[2])).place;

					uint awardId = uint.Parse(StringUtils.GetValueListFromString(info.other_award )[0]);
					Singleton<ItemManager>.Instance.InitItem(AdditionalAward, awardId, ItemType.BaseGoods);
					AdditionalAward.name = "AdditionalAward-" + awardId;
				}
			}
		}

		private void GetTreasureCallBack(object send, int code)
		{
			if (code == Singleton<TreasureMode>.Instance.UPDATE_GETTED_TREASURE)
			{
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_SkillIcon, selectedTreasurePos);
				//Singleton<GoodsAnim>.Instance.ShowAnimation(Singleton<TreasureMode>.Instance.GetTreasureList, selectedTreasurePos);
				if (Singleton<TreasureMode>.Instance.AdditionalAwardId != 0)
				{
					EffectMgr.Instance.CreateUIEffect(EffectId.UI_SkillIcon, AdditionalAward.transform.position);
					TakedTreasure3.color = new Color(1, 1, 1, 1);
					AdditionalAwardTubiao.color =  new Color(1, 1, 1, 1);
					vp_Timer.In(1.5f, UpdateTreasureInfo);
				}
				else
				{
					UpdateTreasureInfo();
				}

//				uint goodId = Singleton<TreasureMode>.Instance.GetTreasureList[0];
//				Singleton<ItemManager>.Instance.InitItem(gameObject.transform.FindChild("right/TreasureGood").gameObject, goodId, ItemType.BaseGoods);
//
//				selectedTreasure.SetActive(false);
//				gameObject.transform.FindChild("right/TreasureGood").gameObject.SetActive(true);
//				gameObject.transform.FindChild("right/TreasureGood").transform.position = selectedTreasurePos;
//				vp_Timer.In(1f, xxx);
			}
		}

		//更新宝藏信息
		private void UpdateTreasureInfo()
		{
			Singleton<TreasureMode>.Instance.ApplyTreasureInfo ();
		}

//		private void xxx()
//		{
//			gameObject.transform.FindChild("right/TreasureGood").gameObject.SetActive(false);
//			Treasure1.gameObject.SetActive(true);
//			Treasure2.gameObject.SetActive(true);
//			Treasure3.gameObject.SetActive(true);
//		}

		//关闭地宫寻宝UI
		private void CloseTreasureView(GameObject go)
		{
			base.CloseView ();
		}

		//刷新按钮被点击
		private void RefreshOnClick(GameObject go)
		{
			if (!AutoRefresh.value)
			{
				RefresureEnsurePannel.SetActive (true);
			}
			else
			{
				Singleton<TreasureMode>.Instance.ApplyFreshTreasureInfo ();
			}
		}

		//刷新确定界面的确定按钮被点击
		private void RefreshEnsureOnClick(GameObject go)
		{
			Singleton<TreasureMode>.Instance.ApplyFreshTreasureInfo ();
			RefresureEnsurePannel.SetActive (false);
		}

		//刷新确定界面的关闭按钮被点击
		private void RefreshCancleOnClick(GameObject go)
		{
			RefresureEnsurePannel.SetActive (false);
		}

		//点击宝藏地点寻宝
		private void TakeTreasure(GameObject go)
		{
			Singleton<TreasureMode>.Instance.ApplyGetTreasure (byte.Parse(go.name));
			selectedTreasurePos = go.transform.position;
			selectedTreasure = go;
//			go.GetComponent<BoxCollider> ().enabled = false;
		}

		private void GoodsOnClick(GameObject go, bool state)
		{
			uint goodId = uint.Parse (go.name.Split('-')[1]);
			if (goodId != 0)
			{
				Singleton<TipsManager>.Instance.OpenTipsByGoodsId (goodId, null, null, "", "", TipsType.DEFAULT_TYPE);
			}
		}
	}
}
