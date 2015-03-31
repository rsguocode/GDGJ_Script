using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using System.Collections.Generic;
using Com.Game.Module.Role;
using Com.Game.Module.Waiting;
using com.game.Public.Message;
using com.game.manager;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/24 09:39:52 
 * function:  竞技场系统--主界面视图类
 * *******************************************************/
using com.game.Public.Confirm;
using com.game.data;
using com.game.utils;
using com.game.vo;
using com.game.module.effect;
using com.u3d.bases.consts;


namespace Com.Game.Module.Arena
{
	public class ArenaMainView : BaseView<ArenaMainView>
	{
		private readonly float UP_RANK_TIME = 2.0f;  //排名上升播放时间
		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		private readonly uint ADD_ARENA_TIMES = 1901;         //Price表中增加竞技场挑战次数对应的ID号
		private readonly uint CLEAR_ARENA_CD = 1902;         //Price表中清除竞技场CD对应的ID号

		private Button btn_fanhui;
		private Button btn_rank;
		private Button btn_record;
		private Button btn_add_times;
		private Button btn_clear_cd;
		private Button btn_refresh;


		private UILabel myRank;
		private UILabel diamBindAward;
		private UILabel challengeTimes;
		private UILabel cd;

		//历史最高纪录
		private GameObject NewRankTips;
		private UILabel curRank;
		private UILabel upRank;
		private UILabel award;
		private Button btn_tips_ensure;
		private GameObject newRecord;


		private Transform _heroes;
	    public List<Transform> HeroParents;
		private int roleNum = 0;
		private Transform myHero;
		
		private ArenaVo arenaViewInfo = new ArenaVo();
		private List<ChallengerVo> challengers = new List<ChallengerVo> ();

		//显示CD时间用
		private int second;
		private int min;
		private bool timeCountFlag = false;
		private float preRemainTime;   //剩余时间
		private float curRemainTime;
		private GameObject myModel;

		private bool rankUpFlag = false;
		private uint newRank;
		private uint curMyRank;
		private float delteaTime;
		private UISprite UpJianTou;
	    public Button Hero1;  //指引用的
	    public EventDelegate.Callback ChallengeUpdateCallback;

		private bool refreshFlag = false;
		public void Init()
		{
			InitLabelLanguage ();
			btn_fanhui = FindInChild<Button>("btn_fanhui");
			btn_rank = FindInChild<Button>("btn_rank");
			btn_record = FindInChild<Button>("btn_record");
			btn_add_times = FindInChild<Button>("Challenge/Times/btn_add_times");
			btn_clear_cd = FindInChild<Button>("Challenge/CD/btn_clear_cd");
			btn_refresh = FindInChild<Button>("btn_refresh");

			UpJianTou = FindInChild<UISprite>("Title/MyArena/jiantou");
			myRank = FindInChild<UILabel>("Title/MyArena/MyRank/Value");
			diamBindAward = FindInChild<UILabel>("Title/MyArena/Award/DiamBindAward/Value");
			challengeTimes = FindInChild<UILabel>("Challenge/Times/Value");
			cd = FindInChild<UILabel>("Challenge/CD/Value");

			_heroes = FindInChild<Transform>("Heroes");
			myHero = FindInChild<Transform>("MyHero");

			btn_fanhui.onClick = this.BackOnClick;
			btn_rank.onClick = this.RankOnClick;
			btn_record.onClick = this.RecordOnClick;
			btn_add_times.onClick = this.AddTimesOnClick;
			btn_clear_cd.onClick = this.ClearCDOnClick;
			btn_refresh.onClick = this.RefreshOnClick;

            HeroParents = new List<Transform>();
			foreach(Transform child in _heroes)
			{
                child.GetComponent<Button>().onClick = ChallengerOnClick;
                HeroParents.Add(child);
				
			}
            Hero1 = HeroParents[0].GetComponent<Button>();

			NewRankTips = FindInChild<Transform>("NewRankTips").gameObject;
//			hisBest = FindInChild<UILabel>("NewRankTips/HisBestRank/Value");
			curRank = FindInChild<UILabel>("NewRankTips/NewRank");
//			jiantou = FindInChild<UISprite>("NewRankTips/CurRank/Value/Sprite");
			upRank = FindInChild<UILabel>("NewRankTips/UpRank/Value");
			award = FindInChild<UILabel>("NewRankTips/NewAward/Value");
			btn_tips_ensure = FindInChild<Button>("NewRankTips/btn_ensure");
			newRecord = FindInChild<Transform>("NewRankTips/NewRecord").gameObject;
			
			btn_tips_ensure.onClick = TipsCloseOnClick;

		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("btn_rank/sc").text = LanguageManager.GetWord("ArenaMainView.rank");
			FindInChild<UILabel>("btn_record/sc").text = LanguageManager.GetWord("ArenaMainView.record");
			FindInChild<UILabel>("Challenge/CD/Label").text = LanguageManager.GetWord("ArenaMainView.cd");
			FindInChild<UILabel>("Challenge/Times/Label").text = LanguageManager.GetWord("ArenaMainView.times");
			FindInChild<UILabel>("Title/MyArena/MyRank/Label").text = LanguageManager.GetWord("ArenaMainView.myRank");
			FindInChild<UILabel>("Title/MyArena/Award/DiamBindAward/Label").text = LanguageManager.GetWord("ArenaMainView.award");
			FindInChild<UILabel>("Heroes/Hero_1/FightNum/Label").text = LanguageManager.GetWord("ArenaMainView.fight");
			FindInChild<UILabel>("Heroes/Hero_2/FightNum/Label").text = LanguageManager.GetWord("ArenaMainView.fight");
			FindInChild<UILabel>("Heroes/Hero_3/FightNum/Label").text = LanguageManager.GetWord("ArenaMainView.fight");
			FindInChild<UILabel>("btn_refresh/Label").text = LanguageManager.GetWord("ArenaMainView.refresh");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateArenaMainView;
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateChallengers;
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateChallengeTimes;
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateChallengeCD;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			Singleton<WaitingView>.Instance.OpenView ();
			Singleton<ArenaMode>.Instance.ApplyArenaInfo ();  //请求竞技场信息
			base.HandleAfterOpenView ();
			timeCountFlag = false;
			refreshFlag = false;
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateArenaMainView;
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateChallengers;
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateChallengeTimes;
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateChallengeCD;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
			timeCountFlag = false;
		}

		private void UpRank()
		{
			curMyRank = curMyRank > arenaViewInfo.rank ? curMyRank - 1 : arenaViewInfo.rank;
			myRank.text = curMyRank.ToString ();
			if (curMyRank > newRank)
				vp_Timer.In (delteaTime, UpRank);
			else
				UpJianTou.SetActive(false);
		}

		//竞技场信息更新回调
		private void UpdateArenaMainView(object sender,int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_ARENA)
			{
				Log.info (this, "19-0返回的竞技场信息已更新，更新竞技场信息");
				arenaViewInfo = Singleton<ArenaMode>.Instance.myArenaVo;

				if (curMyRank > 0)
				{
					rankUpFlag = curMyRank > arenaViewInfo.rank? true: false;
					if (rankUpFlag)
					{
						newRank = arenaViewInfo.rank;
						delteaTime = UP_RANK_TIME / (curMyRank - newRank);
						UpJianTou.SetActive(true);
						UpRank();
					}
					else
					{
						curMyRank = arenaViewInfo.rank;
						myRank.text = curMyRank.ToString();
					}

				}
				else
				{
					curMyRank = arenaViewInfo.rank;
					myRank.text = curMyRank.ToString();

				}

				diamBindAward.text = arenaViewInfo.awardDiamBind.ToString();
				challengeTimes.text = arenaViewInfo.challengeRemainTimes.ToString();

				curRemainTime = arenaViewInfo.cd;
				preRemainTime = curRemainTime;
				this.ShowRemainTime();
				timeCountFlag = true;
			}
			else if (code == Singleton<ArenaMode>.Instance.UPDATE_MY_RANK)
			{
				UpdateMyRank();
			}
		}

		//更新玩家排名
		private void UpdateMyRank()
		{
			if (Singleton<ArenaMode>.Instance.IsGetNewBestRank)
			{
				OpenNewRankTips();
			}
			else if (Singleton<ArenaMode>.Instance.IsGetNewRank)
			{
				OpenNewRankTips();
			}
			else
			{
				//排名未上升
//				Singleton<ArenaMode>.Instance.ApplyArenaInfo ();
			}
		}

		//可挑战者信息更新回调
		private void UpdateChallengers(object sender,int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_CHALLENGERLIST)
			{
				Singleton<WaitingView>.Instance.CloseView ();
				Log.info (this, "19-2返回的可挑战者信息已更新，更新可挑战者玩家");
				challengers = Singleton<ArenaMode>.Instance.challengerList;

				Transform child;
				for (int i = 0; i < 3; ++i)
				{
					//child = heroes.FindChild("Hero_" + (i+1));
				    child = HeroParents[i];
					if (i < challengers.Count)
					{
						child.FindChild("Rank").GetComponent<UILabel>().text = challengers[i].rank.ToString();
						child.FindChild("FightNum/Value").GetComponent<UILabel>().text = challengers[i].fightNum.ToString();
						child.FindChild("Name").GetComponent<UILabel>().text = challengers[i].name;
						child.FindChild("Level").GetComponent<UILabel>().text = challengers[i].level.ToString();
						child.gameObject.SetActive(true);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
                if (ChallengeUpdateCallback != null)
                {
                    ChallengeUpdateCallback();
                    ChallengeUpdateCallback = null;
                }
//				heroes.localPosition = new Vector3 (372 -(challengers.Count - 1) * heroes.GetComponent<UIGrid>().cellWidth, 
//				                                    heroes.localPosition.y , heroes.localPosition.z);
//				heroes.GetComponent<UIGrid> ().Reposition ();

				if (myModel == null)
					new RoleDisplay().CreateRole(MeVo.instance.job,LoadMyModelCallBack);
				else
					CreateRoles();

			}
		    
		}

		//角色模型创建成功回调(自己)
		private void LoadMyModelCallBack(GameObject go)
		{
			go.transform.parent = myHero;
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(100,100,100);
			go.transform.localPosition = new Vector3(-365,-126,0);

			myModel = go;
			CreateRoles();
		}

		//可挑战次数更新回调
		private void UpdateChallengeTimes(object sender,int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_CHALLENG_TIMES)
			{
				challengeTimes.text = Singleton<ArenaMode>.Instance.myArenaVo.challengeRemainTimes.ToString();
			}
		}

		//挑战cd更新回调
		private void UpdateChallengeCD(object sender,int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_CD)
			{
//				cd.text = Singleton<ArenaMode>.Instance.myArenaVo.cd.ToString();
				curRemainTime = Singleton<ArenaMode>.Instance.myArenaVo.cd;
				preRemainTime = curRemainTime;
				this.ShowRemainTime();
			}
		}

		//创建人物模型
		private void CreateRoles()
		{
			//清除原有模型
			Transform obj;
			Log.info(this, "开始清除模型：" + roleNum);
			foreach(Transform child in HeroParents)
			{
				obj = child.FindChild("RoleDisplay");
				if (obj != null)
				{
					Log.info(this, "清除模型：" + child.name);
					GameObject.Destroy(obj.gameObject);
					--roleNum;
				}
			}
//			roleNum = 0;
			//创建新模型对象
			if (roleNum < challengers.Count)
			{
				Log.info(this, "创建人物模型，模型职业： " + challengers[roleNum].job);
				new RoleDisplay().CreateRole(challengers[roleNum].job,LoadModelCallBack);  //创建人物模型
			}
		}

		//角色模型创建成功回调
		private void LoadModelCallBack(GameObject go)
		{
			Log.info (this, "已创建模型数量:" + roleNum);
		    go.transform.parent = HeroParents[roleNum];
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(100,100,100);
			go.transform.localPosition = new Vector3(0,100,0);

			++roleNum;
			if (roleNum < challengers.Count)
			{
				new RoleDisplay().CreateRole(challengers[roleNum].job,LoadModelCallBack);
			}

			if (roleNum == 3)
			{
				refreshFlag = true;
				Log.info(this, "创建模型完成：" + roleNum);
			}
			//播放英雄刷新特效
//			EffectMgr.Instance.CreateUIEffect(EffectId.Main_RoleToTeleport, go.transform.position);
		}

		//返回键被点击
		private void BackOnClick(GameObject go)
		{
			Singleton<ArenaControl>.Instance.CloseArenaView ();
		}

		//排行榜按钮被点击
		private void RankOnClick(GameObject go)
		{
			Singleton<ArenaView>.Instance.OpenArenaRankView ();
		}

		//点击被挑战对象
		private void ChallengerOnClick(GameObject go)
		{
			Singleton<ArenaMode>.Instance.selectHeroSub = int.Parse (go.name.Split (new char[]{'_'}) [1]) - 1;

			ConfirmMgr.Instance.ShowCommonAlert(LanguageManager.GetWord("ArenaMainView.challengeEnsure"), ConfirmCommands.OK_CANCEL, 
			                                    StarteBattle, LanguageManager.GetWord("ConfirmView.Ok"), 
			                                    null, LanguageManager.GetWord("ConfirmView.Cancel"));
//			Singleton<ArenaMainView>.Instance.CloseView ();
//			Singleton<ArenaView>.Instance.OpenArenaVsView ();
		}

		//开始挑战
		private void StarteBattle()
		{
			Singleton<ArenaMode>.Instance.ApplyStartChallenge (challengers[Singleton<ArenaMode>.Instance.selectHeroSub].roleId, 
			                                                   challengers[Singleton<ArenaMode>.Instance.selectHeroSub].rank);
		}


		//刷新按钮被点击
		private void RefreshOnClick(GameObject go)
		{
			if (refreshFlag)
			{
				refreshFlag = false;
				Singleton<ArenaMode>.Instance.ApplyArenaInfo ();
			}
		}

		//战斗记录按钮被点击
		private void RecordOnClick(GameObject go)
		{
            Singleton<ArenaView>.Instance.OpenArenaRecordView();
		}

		//增加战斗次数按钮被点击
		private void AddTimesOnClick(GameObject go)
		{
			SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos (ADD_ARENA_TIMES);
			string[] price = StringUtils.GetValueListFromString (priceVo.diam);
			uint needDiam = uint.Parse(price[0]) + uint.Parse(price[1]) * ((uint)Singleton<ArenaMode>.Instance.myArenaVo.buyTimes);
			needDiam = needDiam > uint.Parse (price [2]) ? uint.Parse (price [2]) : needDiam;

			ConfirmMgr.Instance.ShowCommonAlert(LanguageManager.GetWord("ArenaMainView.payEnsure") + needDiam 
			                                    + LanguageManager.GetWord("ArenaMainView.addTimesEnsure"), 
			                                    ConfirmCommands.OK_CANCEL, 
			                                    AddTimes, LanguageManager.GetWord("ConfirmView.Ok"), 
			                                    null, LanguageManager.GetWord("ConfirmView.Cancel"));

		}
		private void AddTimes()
		{
			Singleton<ArenaMode>.Instance.ApplyAddTimes ();
		}


		//清除时间按钮被点击
		private void ClearCDOnClick(GameObject go)
		{
			SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos (CLEAR_ARENA_CD);
			string[] price = StringUtils.GetValueListFromString (priceVo.diam);
			uint needDiam = uint.Parse(price[1]) * ( ((uint)curRemainTime / (60 * uint.Parse(price[0])) + 1));

			if (needDiam > 0)
			{
				ConfirmMgr.Instance.ShowCommonAlert(LanguageManager.GetWord("ArenaMainView.payEnsure") + needDiam 
				                                    + LanguageManager.GetWord("ArenaMainView.clearCDEnsure"),
				                                    ConfirmCommands.OK_CANCEL, 
			    	                                ClearCD, LanguageManager.GetWord("ConfirmView.Ok"), 
			        	                            null, LanguageManager.GetWord("ConfirmView.Cancel"));
			}
			else
			{
				MessageManager.Show(LanguageManager.GetWord("ArenaMainView.CDCleared"));
			}
		}
		private void ClearCD()
		{
			Singleton<ArenaMode>.Instance.ApplyClearCD ();
		}

		//------------------------------------- CD时间 ------------------------//
		public override void Update ()
		{
			base.Update ();
			if (timeCountFlag)
			{
				curRemainTime -= Time.deltaTime;
				if (curRemainTime < 0)
				{
					curRemainTime = 0;
				}
				if (preRemainTime - curRemainTime> 1.0f)
				{
					preRemainTime = curRemainTime;
					this.ShowRemainTime();
				}
			}
		}

		private void ShowRemainTime()
		{
			min = ((int)curRemainTime) / 60;
			second = (int)curRemainTime - min * 60;

			cd.text = (min < 10? "0": "") + min + ":" + (second < 10? "0": "") + second;
		}

		//历史最高tips的关闭按钮被点击
		private void TipsCloseOnClick(GameObject go)
		{
			NewRankTips.SetActive (false);
			if (myModel != null)
			{
				myModel.GetComponentInChildren<Animator> ().SetInteger (Status.STATU, Status.Win);
				vp_Timer.In (0.2f, SetIdle);
			}
		}

		private void SetIdle(){
			if (myModel != null)
				myModel.GetComponentInChildren<Animator> ().SetInteger (Status.STATU, Status.IDLE);
		}
		


		//打开排名上升tips
		public void OpenNewRankTips()
		{
			NGUITools.SetLayer(NewRankTips, LayerMask.NameToLayer("TopUI"));
//			EffectMgr.Instance.CreateUIEffect(EffectId.UI_PetLight, NewBestRankTips.transform.position);

			ArenaVo arenaVo = Singleton<ArenaMode>.Instance.myArenaVo;
			curRank.text = arenaVo.rank.ToString();
			upRank.text = (curMyRank - arenaVo.rank).ToString();
			award.text = arenaVo.awardDiamBind.ToString();
			
			Singleton<ArenaMode>.Instance.UpdateMyBestRank(Singleton<ArenaMode>.Instance.myArenaVo.rank);
			newRecord.SetActive( Singleton<ArenaMode>.Instance.IsGetNewBestRank? true: false);
			NewRankTips.SetActive (true);
		}

	}
}
