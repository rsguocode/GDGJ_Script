using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.u3d.bases.debug;
using com.game.vo;
using Com.Game.Module.Waiting;
using com.game.manager;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统--排行榜视图类
 * *******************************************************/
namespace Com.Game.Module.Arena
{
	public class ArenaRankView : BaseView<ArenaRankView>
	{
		public override ViewLayer layerType {
			get {
				return ViewLayer.HighLayer;
			}
		}

		private Button btn_close;
		private UILabel myRank;
		private UILabel myFight;
		private UILabel myLevel;
		private UILabel myName;
		private UILabel myAward;

		private Transform rankItems;

		private List<RankVo> top10Heroes = new List<RankVo>();
		private uint myRankValue;

		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("topright/btn_close");
			rankItems = FindInChild<Transform> ("Center/RankListPanel/Grid/");

			myRank = FindInChild<UILabel>("MyRank/pm");
			myFight = FindInChild<UILabel>("MyRank/zl");
			myAward = FindInChild<UILabel>("MyRank/jl");
			myName = FindInChild<UILabel>("MyRank/mz");
			myLevel = FindInChild<UILabel>("MyRank/dj");

			btn_close.onClick = BackOnClick;
//			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("Viewport"));
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("Title/label").text = LanguageManager.GetWord("ArenaRankView.rankList");
			FindInChild<UILabel>("Background/ts").text = LanguageManager.GetWord("ArenaRankView.remark");
			FindInChild<UILabel>("Center/Title/pm").text = LanguageManager.GetWord("ArenaRankView.rank");
			FindInChild<UILabel>("Center/Title/mz").text = LanguageManager.GetWord("ArenaRankView.name");
			FindInChild<UILabel>("Center/Title/dj").text = LanguageManager.GetWord("ArenaRankView.level");
			FindInChild<UILabel>("Center/Title/zl").text = LanguageManager.GetWord("ArenaRankView.fight");
			FindInChild<UILabel>("Center/Title/jbjl").text = LanguageManager.GetWord("ArenaRankView.dailyAward");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<ArenaMode>.Instance.dataUpdated += UpdateArenaRankView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI"));

//			Singleton<WaitingView>.Instance.OpenView ();
			Singleton<ArenaMode>.Instance.ApplyRankInfo ();  //请求排行榜前十玩家信息

			myRankValue = Singleton<ArenaMode>.Instance.myRank;
			myRank.text = myRankValue.ToString ();
			myFight.text = MeVo.instance.fightPoint.ToString ();
			myName.text = MeVo.instance.Name;
			myLevel.text = MeVo.instance.Level.ToString();
			myAward.text = Singleton<ArenaMode>.Instance.myArenaVo.awardDiamBind.ToString ();


		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateArenaRankView;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		//竞技场排名信息更新回调
		private void UpdateArenaRankView(object sender,int code)
		{
			if (code == Singleton<ArenaMode>.Instance.UPDATE_ARENA_RANK)
			{
//				Singleton<WaitingView>.Instance.CloseView ();
				Log.info (this, "19-1返回的竞技场排名前十玩家信息更新");
				top10Heroes = Singleton<ArenaMode>.Instance.rankList;

				RankVo heroInfo;
				Transform hero;
				for (int i = 0; i < top10Heroes.Count; ++i)
				{
					heroInfo = top10Heroes[i];
					hero = rankItems.FindChild("Item_" + (int)heroInfo.rank / 10 +  (int)heroInfo.rank % 10);
					hero.FindChild("mz").GetComponent<UILabel>().text = heroInfo.name;
					hero.FindChild("dj").GetComponent<UILabel>().text = heroInfo.level.ToString();
					hero.FindChild("zl").GetComponent<UILabel>().text = heroInfo.fightNum.ToString();
					hero.FindChild("jl").GetComponent<UILabel>().text = Singleton<ArenaMode>.Instance.GetDiamAwardBelong10(i + 1).ToString();
				}
				rankItems.GetComponent<UIGrid>().Reposition();
			}
		}

		//返回键被点击
		private void BackOnClick(GameObject go)
		{
			this.CloseView ();
		}

	}
}