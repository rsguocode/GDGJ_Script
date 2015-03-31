using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.module.map;
using com.game.vo;
using com.game.manager;
using com.game.Public.Confirm;
using Com.Game.Module.Role;
using com.game.consts;
using com.game.Public.LocalVar;
using System.Collections.Generic;
using com.game.utils;
using com.game.Public.UICommon;
using PCustomDataType;
using Com.Game.Module.Copy;
using Com.Game.Module.Manager;
using Com.Game.Module.Tips;

namespace Com.Game.Module.DaemonIsland
{
	public class DaemonCopyFastFightView : BaseView<DaemonCopyFastFightView>
	{
		private readonly int NORMAL_ATLAS_DEPTH = 1026;
		private readonly int GRAY_ATLAS_DEPTH = 1526;
		private readonly int MAX_AWARD_GOOD_NUM = 5;  //副本奖励物品最多五个，多了会出错，摆不下啊
		private readonly float FIGHT_DELTA_TIME = 1;  //每次扫荡间隔时间
		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		//副本详细信息UI相关
		private Button btn_close;
		private UISprite closeBackground;
		private List<GameObject> fightResult = new List<GameObject> ();
		private GameObject fightEnd;
		private UICenterOnChild resultCenter;
		private UIGrid resultGrid;
		private UIScrollView resultScrollView;
		private BoxCollider background;

		private int fastFightTimes;
		private bool startFightFlag = false;
		private float durTime = 1;
		private int curFightTimes = 0;
		private List<PDungeonReward> awards = new List<PDungeonReward> ();
		protected override void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button> ("btn_guanbi");
			closeBackground = FindInChild<UISprite> ("btn_guanbi/background");
			fightEnd = FindInChild<Transform> ("FastFightResult/Center/Grid/End").gameObject;
			resultGrid = FindInChild<UIGrid> ("FastFightResult/Center/Grid");
			resultCenter = FindInChild<UICenterOnChild> ("FastFightResult/Center");
			resultScrollView = FindInChild<UIScrollView> ("FastFightResult");
			background = FindInChild<BoxCollider> ("Background/tipsk");
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/01").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/02").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/03").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/04").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/05").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/06").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/07").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/08").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/09").gameObject);
			fightResult.Add(FindInChild<Transform> ("FastFightResult/Center/Grid/10").gameObject);

			btn_close.onClick = CloseFastFightView;
			for (int i = 0; i < fightResult.Count; ++i)
			{
				foreach(Transform goods in fightResult[i].transform.FindChild("Goods"))
				{
					goods.GetComponent<Button>().onClick = GoodsOnClick;
				}
			}
		}

		private void InitLabelLanguage()
		{
			string awardContent = LanguageManager.GetWord("FastFightView.awardContent");
			string goodsGetted = LanguageManager.GetWord("FastFightView.goodsGetted");
			FindInChild<UILabel> ("FastFightResult/Center/Grid/01/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/02/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/03/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/04/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/05/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/06/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/07/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/08/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/09/jlnr").text = awardContent;
			FindInChild<UILabel> ("FastFightResult/Center/Grid/10/jlnr").text = awardContent;
			FindInChild<UILabel>("FastFightResult/Center/Grid/01/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/02/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/03/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/04/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/05/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/06/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/07/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/08/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/09/hdwp").text = goodsGetted;
			FindInChild<UILabel>("FastFightResult/Center/Grid/10/hdwp").text = goodsGetted;
			
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<CopyPointMode>.Instance.dataUpdated += UpdateCopyDetailView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
            //重新指定事件
            btn_close.onClick = CloseFastFightView;
            for (int i = 0; i < fightResult.Count; ++i)
            {
                foreach (Transform goods in fightResult[i].transform.FindChild("Goods"))
                {
                    goods.GetComponent<Button>().onClick = GoodsOnClick;
                }
            }
            //end 德索添加
			InitFastFightView ();
			Singleton<CopyPointMode>.Instance.ApplyFastFight (Singleton<DaemonIslandMode>.Instance.selectedCopyPoint, 
			                                                  (byte)Singleton<DaemonIslandMode>.Instance.fightNum);

		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<CopyPointMode>.Instance.dataUpdated -= UpdateCopyDetailView;
			
		}

		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
			for (int i = 0;i < fightResult.Count; ++i)
			{
				fightResult[i].SetActive(false);
//				fightResult[i].transform.localPosition = new Vector3(0, 0, 0);
			}
			resultGrid.Reposition ();
			resultScrollView.ResetPosition ();
			fightEnd.SetActive (false);
			startFightFlag = false;

		}

		//挑战次数更新回调
		private void UpdateCopyDetailView(object sender,int code)
		{
			if (code == Singleton<CopyPointMode>.Instance.UPDATE_FAST_FIGHT_AWARDS)
			{
				awards = Singleton<CopyPointMode>.Instance.fastFightAwards;
				startFightFlag = true;
			}
		}

		//扫荡前初始化一些数据
		private void InitFastFightView()
		{
			//初始化
			fastFightTimes = Singleton<DaemonIslandMode>.Instance.fightNum;
			durTime = 1;
			curFightTimes = 0;
			btn_close.GetComponent<BoxCollider> ().enabled = false;     //关闭关闭按钮
			UIUtils.ChangeGrayShader (closeBackground, GRAY_ATLAS_DEPTH);
			background.enabled = false;        //关闭滑动功能
			startFightFlag = false;            //关闭刷新扫荡结果开关，等收到数据后再开启
		}


		public override void Update ()
		{
			base.Update ();
			if (startFightFlag)    //开始显示扫荡结果
			{
				durTime += Time.deltaTime;
				if (durTime >= 1)
				{
					durTime = 0;
					curFightTimes++;
					if (curFightTimes <= fastFightTimes)
					{
						UpdateFightAward(curFightTimes);
					}
					else
					{
						ShowFightEnd();
					}

				}
			}
		}

		//刷新扫荡奖励
		private void UpdateFightAward(int num)
		{
			fightResult [num - 1].transform.FindChild ("jlnr/Exp/Value").GetComponent<UILabel> ().text = awards [num - 1].exp.ToString();
			fightResult [num - 1].transform.FindChild ("jlnr/Money/Value").GetComponent<UILabel> ().text = awards [num - 1].gold.ToString();
			Transform goods;
			for (int i = 0; i < MAX_AWARD_GOOD_NUM; ++i)
			{
				goods = fightResult [num - 1].transform.FindChild ("Goods/" + (i + 1));
				if (i < awards [num - 1].goodsId.Count)
				{
					ShowGoods(goods, awards [num - 1].goodsId[i], awards [num - 1].idType);
					goods.gameObject.SetActive(true);
                    goods.gameObject.GetComponent<TweenScale>().from = new Vector3(1.2f, 1.2f, 1.2f);
                    goods.gameObject.GetComponent<TweenScale>().to = Vector3.one;
                    goods.gameObject.GetComponent<TweenScale>().delay = i * 0.2f;
                    goods.gameObject.GetComponent<TweenPlay>().PlayForward();
				}
				else 
				{
					goods.gameObject.SetActive(false);
				}
			}
			fightResult [num - 1].SetActive (true);

			resultGrid.Reposition ();
			if (num > 2)
			{
				resultCenter.enabled = true;
				resultCenter.CenterOn (fightResult [num - 1].transform);
			}
		}

		//根据物品ID显示物品
		private void ShowGoods(Transform goods, uint goodsId, byte idType)
		{
			if (idType == (byte)IdType.GOODSID)
			{
				ItemManager.Instance.InitItem (goods.gameObject, goodsId, ItemType.BaseGoods);
			}
			else if(idType == (byte)IdType.UNIQUEID)
			{
				ItemManager.Instance.InitItemByUId(goods.gameObject, 
				                                   goodsId, ItemType.BaseGoods);
			}
		}

		//扫荡结束
		private void ShowFightEnd()
		{

			fightEnd.SetActive (true);
            fightEnd.GetComponent<TweenPlay>().PlayForward();
			startFightFlag = false;
			resultGrid.Reposition ();
			resultCenter.CenterOn (fightResult[fastFightTimes - 1].transform.FindChild("fgx"));
			resultCenter.enabled = false;  
			background.enabled = true;   //开启滑动检测
			btn_close.GetComponent<BoxCollider> ().enabled = true;  //开启关闭按钮
			UIUtils.ChangeNormalShader (closeBackground, NORMAL_ATLAS_DEPTH);
		}


		//关闭副本扫荡界面
		private void CloseFastFightView(GameObject go)
		{
			this.CloseView ();
		}

		//物品被点击
		private void GoodsOnClick(GameObject go)
		{
			uint goodsId;
			uint idType;
			int awardId = int.Parse (go.name);
			int fightId = int.Parse (go.transform.parent.parent.name);
			goodsId = awards [fightId - 1].goodsId [awardId - 1];
			idType = awards [fightId - 1].idType;
			if (idType == (byte)IdType.GOODSID)
			{
				TipsManager.Instance.OpenTipsByGoodsId (goodsId, null, null, "", "");
			}
			else if (idType == (byte)IdType.UNIQUEID)
			{
				TipsManager.Instance.OpenTipsById (goodsId, null, null, "", "", TipsType.DEFAULT_TYPE);
			}
		}
	}
}
