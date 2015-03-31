using UnityEngine;
using com.game.manager;
using com.game.module.test;
using Com.Game.Module.Manager;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Proto;
using com.game.module.test;
using com.game.sound;
using com.game.module.effect;
using com.game.utils;
using com.u3d.bases.debug;
using Com.Game.Module.Tips;
using com.game.data;
using com.game.consts;
using com.game.Public.UICommon;


/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li --2014.4.22  
 * function:  副本系统结算视图
 * *******************************************************/
using com.game.vo;
using Com.Game.Module.Role;
using com.u3d.bases.consts;


namespace Com.Game.Module.Copy
{

	//gz1680(吴骏华) 2014-01-21 11:29:22
	//1星显示内容：时间达成
	//2星显示内容：时间达成+（连斩达成或生命达成）
	//gz1680(吴骏华) 2014-01-21 11:29:59
	//3星显示内容：时间+生命+连斩
	public class CopyAwardView : BaseView<CopyAwardView>
    {
		private static int REMAIN_TIME = 15;   //结算界面维持时间
		public override string url { get { return "UI/Copy/CopyAwardView.assetbundle"; } }
		public override ViewLayer layerType
		{
			get { return ViewLayer.MiddleLayer; }
		}

        public override bool waiting { get { return false; } }

        private Button btn_close;
		private List<Transform> goods = new List<Transform>();
		private UISprite lifeAchievement;
		private UISprite timeAchievement;
		private UISprite killAchievement;
		private Transform role;
		
		private UILabel recExpValue;  //获得经验值
		private UILabel recMoneyValue;  // 获得的金钱 
		private UILabel recSoulValue;  // 获得的灵魂点

		private GameObject star1, star2, star3, Exp, Money, Goods, Soul;
		private TweenPosition expTween, moneyTween, goodsTween, soulTween;
		private TweenAlpha star1Tween, star2Tween, star3Tween; 
		private TweenShake tweenShake;
		private TweenPlay playstar1, playstar2, playstar3, playExp, playMoney, playSoul, playGoods;
		private PlaytList playList = new PlaytList ();

		private UILabel remainTimeValue;

		private GameObject model;
		private AwardVo awards;
		private List<Vector3> achievementPos = new List<Vector3>();
		private float delTime = 0.0f;
		private int remainTime = REMAIN_TIME;
		protected override void Init()
        {
			InitLabel ();

			tweenShake = FindInChild<TweenShake>();
			btn_close = FindInChild<Button>("btn_close");
			goods.Add(FindInChild<Transform>("Awards/Goods/1"));
			goods.Add(FindInChild<Transform>("Awards/Goods/2"));
			goods.Add(FindInChild<Transform>("Awards/Goods/3"));
			goods.Add(FindInChild<Transform>("Awards/Goods/4"));
			goods.Add(FindInChild<Transform>("Awards/Goods/5"));
			lifeAchievement = FindInChild<UISprite>("Achievement/life");
			timeAchievement = FindInChild<UISprite>("Achievement/time");
			killAchievement = FindInChild<UISprite>("Achievement/kill");
			recExpValue = FindInChild<UILabel>("Awards/Exp/value");
			recMoneyValue = FindInChild<UILabel>("Awards/Money/value");
			recSoulValue = FindInChild<UILabel>("Awards/Soul/value");
			role =  FindInChild<Transform>("Background/role");

			star1 = FindChild("Achievement/Stars/star1");
			star2 = FindChild("Achievement/Stars/star2");
			star3 = FindChild("Achievement/Stars/star3");
			Exp = FindChild("Awards/Exp");
			Soul  = FindChild("Awards/Soul");
			Goods = FindChild("Awards/Goods");
			Money = FindChild("Awards/Money");
			expTween = FindInChild<TweenPosition>("Awards/Exp");
			soulTween = FindInChild<TweenPosition>("Awards/Soul");
			moneyTween = FindInChild<TweenPosition>("Awards/Money");
			goodsTween = FindInChild<TweenPosition>("Awards/Goods");
			star1Tween = FindInChild<TweenAlpha>("Achievement/Stars/star1");
			star2Tween = FindInChild<TweenAlpha>("Achievement/Stars/star2");
			star3Tween = FindInChild<TweenAlpha>("Achievement/Stars/star3");
			playstar1 = FindInChild<TweenPlay>("Achievement/Stars/star1");
			playstar2 = FindInChild<TweenPlay>("Achievement/Stars/star2");
			playstar3 = FindInChild<TweenPlay>("Achievement/Stars/star3");
			playExp = FindInChild<TweenPlay>("Awards/Exp");
			playSoul = FindInChild<TweenPlay>("Awards/Soul");
			playGoods = FindInChild<TweenPlay>("Awards/Goods");
			playMoney = FindInChild<TweenPlay>("Awards/Money");

			remainTimeValue = FindInChild<UILabel>("QuitCopy/value");

			achievementPos.Add(timeAchievement.transform.localPosition);
			achievementPos.Add(lifeAchievement.transform.localPosition);
           	achievementPos.Add(killAchievement.transform.localPosition);
			btn_close.onClick += CloseOnClick;
			for (int i = 0; i < goods.Count; ++i)
			{
				goods[i].GetComponent<Button>().onClick = GoodsOnClick;
			}
        }

		//静态语言处理
		private void InitLabel()
		{
		}

		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			delTime = 0.0f;
			remainTime = REMAIN_TIME;
			remainTimeValue.text = remainTime.ToString ();

			gameObject.SetActive (false);
			new RoleDisplay().CreateRole(MeVo.instance.job,LoadModelCallBack);

		}
				
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			if (model != null)
				GameObject.Destroy (model);
		}

		public override void Update ()
		{
			base.Update ();
			delTime += Time.deltaTime;
			if (delTime > 1.0f)
			{
				delTime--;
				remainTime--;
				remainTimeValue.text = remainTime.ToString();
				if (remainTime <= 0)
				{
					Singleton<AwardControl>.Instance.AwardEnd ();
				}
			}
		}

		//角色模型创建成功回调(自己)
		private void LoadModelCallBack(GameObject go)
		{
			gameObject.SetActive (true);
			//			this.model = go;
			//this.model.transform.localScale  =comTest.transform.localScale*0.63f;
			go.transform.parent = role;
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(150,150,150);
			go.transform.localPosition = new Vector3(0,-190,-350);
			model = go;

			EffectMgr.Instance.CreateUIEffect(EffectId.UI_Win, go.transform.position, null, true, WinEffectCreatedCallback);
			InitAwardUI ();
			ShowAwardAnim ();

			vp_Timer.In (0.5f, StartPlayModelAnim);
		}

		private void WinEffectCreatedCallback(GameObject effectObj)
		{
			Vector3 localPos = effectObj.transform.localPosition;
			localPos.y = 0f;
			effectObj.transform.localPosition = localPos;
		}

		//开始播放模型动画
		private void StartPlayModelAnim()
		{
			if (model != null)
			{
				model.GetComponentInChildren<Animator> ().SetInteger (Status.STATU, Status.Win);
				vp_Timer.In (0.2f, SetIdle);
			}
		}

		private void SetIdle(){
			if (model != null)
				model.GetComponentInChildren<Animator> ().SetInteger (Status.STATU, Status.IDLE);
		}

		//初始化奖励界面
		private void InitAwardUI()
		{
			awards = Singleton<AwardMode>.Instance.award;
			lifeAchievement.spriteName = "shengmin1";
			timeAchievement.spriteName = "shijian1";
			killAchievement.spriteName = "lianzhanhui";
			UpdateAchievementPos ();

			Soul.transform.localPosition = soulTween.from;
			Goods.transform.localPosition = goodsTween.from;
			Money.transform.localPosition = moneyTween.from;
			Exp.transform.localPosition = expTween.from;
			star1.transform.GetComponent<UISprite>().alpha = star1Tween.from;
			star2.transform.GetComponent<UISprite>().alpha = star1Tween.from;
			star3.transform.GetComponent<UISprite>().alpha = star1Tween.from;

			recExpValue.text = "+" + awards.expReward.ToString ();
			recMoneyValue.text = "+" + awards.siliverReward.ToString ();
			recSoulValue.text = "+" + BaseDataMgr.instance.GetSysDungeonReward(MeVo.instance.mapId).fix_soul;
			ShowAwardGoods (awards.goodsRewardList);
		}

		//根据达成条件更新达成显示顺序
		private void UpdateAchievementPos()
		{
			int start = 0, end = achievementPos.Count - 1;
			timeAchievement.transform.localPosition = awards.timeAchievement ? achievementPos [start++] : achievementPos [end--];
			lifeAchievement.transform.localPosition = awards.hpAchievement ? achievementPos [start++] : achievementPos [end--];
			killAchievement.transform.localPosition = awards.attackAchievement ? achievementPos [start++] : achievementPos [end--];
		}

		//展示奖励界面表现效果
		private void ShowAwardAnim()
		{
			playList.ClearPlay();
			AddStarAnim ();
			AddAwardsAnim ();
			playList.Start();
		}

		//添加星星动画效果
		private void AddStarAnim()
		{
			if(Singleton<AwardMode>.Instance.award.timeAchievement)
			{
//				star1.SetActive(true);
				star1.transform.localScale = Vector3.zero;
				playList.AddPlay(playstar1);
				playList.AddPlay(tweenShake);
				playList.AddDelegate(TimeAchivement);
			}
			if(Singleton<AwardMode>.Instance.award.hpAchievement ||(!Singleton<AwardMode>.Instance.award.hpAchievement
			                                                        && Singleton<AwardMode>.Instance.award.attackAchievement))
			{
				star2.SetActive(true);
				star2.transform.localScale = Vector3.zero;
				playList.AddPlay(playstar2);
				playList.AddPlay(tweenShake);
				//playList.AddTimeInterval(0.1f);
				if(Singleton<AwardMode>.Instance.award.hpAchievement)
					playList.AddDelegate(LifeAchivement);
				else
					playList.AddDelegate(KillAchivement);
			}
			if(Singleton<AwardMode>.Instance.award.hpAchievement
			   &&Singleton<AwardMode>.Instance.award.attackAchievement)
			{
				star3.SetActive(true);
				star3.transform.localScale = Vector3.zero;
				playList.AddPlay(playstar3);
				playList.AddPlay(tweenShake);
				//playList.AddTimeInterval(0.1f);
				playList.AddDelegate(KillAchivement);
			}
		}

		//添加奖励动画效果
		private void AddAwardsAnim()
		{
			playList.AddPlay (playExp);
			playList.AddPlay (playMoney);
			if (awards.isFirstPass)
			{
				playList.AddPlay (playSoul);
			}
			playList.AddPlay (playGoods);
		}

		//时间达成
		private void TimeAchivement()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_RewardClick,timeAchievement.transform.position,TimeAchivementCallBack);
		}
		private void TimeAchivementCallBack()
		{	
			timeAchievement.spriteName = awards.timeAchievement? "shijian": "shijian1";
			playList.Continue();
		}
		//生命达成
		private void LifeAchivement()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_RewardClick,lifeAchievement.transform.position,LifeAchivementCallBack);
		}
		private void LifeAchivementCallBack()
		{
			lifeAchievement.spriteName = awards.hpAchievement? "shengmin" : "shengmin1";
			playList.Continue();
		}
		//连斩达成
		private void KillAchivement()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_RewardClick,killAchievement.transform.position,KillAchivementCallBack);
			Log.info(this,"kill dacheng ");
		}
		private void KillAchivementCallBack()
		{			
			Log.info(this,"kill dacheng call back");
			killAchievement.spriteName = awards.attackAchievement? "lianzhan": "lianzhanhui";
			playList.Continue();
		}

		//显示副本获取物品
		private void ShowAwardGoods(List<uint> goodsIdList)
		{
			for (int i  = 0; i < goods.Count; ++i)
			{
				if (i < goodsIdList.Count)
				{
					if (awards.awardIdType == (byte)IdType.GOODSID)
					{
						Singleton<ItemManager>.Instance.InitItem(goods[i].gameObject, 
					                                         goodsIdList[i], ItemType.BaseGoods);
					}
					else if (awards.awardIdType == (byte)IdType.UNIQUEID)
					{
						Singleton<ItemManager>.Instance.InitItemByUId(goods[i].gameObject, 
						                                         goodsIdList[i], ItemType.BaseGoods);
					}
					goods[i].gameObject.SetActive(true);
				}
				else
				{
					goods[i].gameObject.SetActive(false);
				}
			}
		}

        
		//关闭奖励界面
        private void CloseOnClick(GameObject go)
        {
			Singleton<AwardControl>.Instance.AwardEnd ();
        }

		//物品被点击
		private void GoodsOnClick(GameObject go)
		{
			int sub = int.Parse (go.name) - 1;
			uint goodsId;
			if (sub < awards.goodsRewardList.Count)
			{
				goodsId = awards.goodsRewardList[sub];
				if (awards.awardIdType == (byte)IdType.GOODSID)
				{
					TipsManager.Instance.OpenTipsByGoodsId (goodsId, null, null, "", "");
				}
				else if (awards.awardIdType == (byte)IdType.UNIQUEID)
				{
					TipsManager.Instance.OpenTipsById (goodsId, null, null, "", "", TipsType.DEFAULT_TYPE);
				}		 
			}
		}
	}
}

