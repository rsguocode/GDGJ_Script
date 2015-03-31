using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.vo;
using Com.Game.Module.Role;
using com.game.module.map;
using com.game.manager;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统--vs展示视图类
 * *******************************************************/
namespace Com.Game.Module.Arena
{
	public class ArenaVsView : BaseView<ArenaVsView>
	{
		public override ViewLayer layerType {
			get {
				return ViewLayer.HighLayer;
			}
		}

		private Button btn_close;
		private UILabel myName;
		private UILabel myFight;
		private UILabel myRank;
		private UILabel myLevel;
		private UILabel vsName;
		private UILabel vsFight;
		private UILabel vsRank;
		private UILabel vsLevel;
		private Transform left;
		private Transform right;
		private Button Start;


		private GameObject myModel;
		private GameObject vserModel;
		private ChallengerVo challengerInfo;
		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("btn_fanhui");
			myName = FindInChild<UILabel>("Heroes/Left/Name");
			myFight = FindInChild<UILabel>("Heroes/Left/Fight/Value");
			myRank = FindInChild<UILabel>("Heroes/Left/Rank/Value");
			myLevel = FindInChild<UILabel>("Heroes/Left/Level/Value");
			vsName = FindInChild<UILabel>("Heroes/Right/Name");
			vsFight = FindInChild<UILabel>("Heroes/Right/Fight/Value");
			vsRank = FindInChild<UILabel>("Heroes/Right/Rank/Value");
			vsLevel = FindInChild<UILabel>("Heroes/Right/Level/Value");
			left = FindInChild<Transform>("Heroes/Left");
			right = FindInChild<Transform>("Heroes/Right");
			Start = FindInChild<Button>("btn_tz");
			
			btn_close.onClick = BackOnClick;
			Start.onClick = StarteBattleOnClick;
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("Heroes/Left/Fight/Label").text = LanguageManager.GetWord("ArenaVsView.fight");
			FindInChild<UILabel>("Heroes/Left/Level/Label").text = LanguageManager.GetWord("ArenaVsView.level");
			FindInChild<UILabel>("Heroes/Left/Rank/Label").text = LanguageManager.GetWord("ArenaVsView.rank");
			FindInChild<UILabel>("Heroes/Right/Fight/Label").text = LanguageManager.GetWord("ArenaVsView.fight");
			FindInChild<UILabel>("Heroes/Right/Level/Label").text = LanguageManager.GetWord("ArenaVsView.level");
			FindInChild<UILabel>("Heroes/Right/Rank/Label").text = LanguageManager.GetWord("ArenaVsView.rank");
			FindInChild<UILabel>("btn_tz/label").text = LanguageManager.GetWord("ArenaVsView.challenge");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
//			Singleton<ArenaMode>.Instance.dataUpdated += UpdateArenaVsView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
//			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI"));
			ShowArenaVsView ();
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
//			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateArenaVsView;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		private void ShowArenaVsView()
		{
			if (myModel != null)
				GameObject.Destroy (myModel);
			if (vserModel != null)
				GameObject.Destroy (vserModel);

			challengerInfo = Singleton<ArenaMode>.Instance.challengerList [Singleton<ArenaMode>.Instance.selectHeroSub];
			myName.text = MeVo.instance.Name;
			myLevel.text = MeVo.instance.Level.ToString();
			myRank.text = Singleton<ArenaMode>.Instance.myRank.ToString();
			myFight.text = MeVo.instance.fightPoint.ToString();
			new RoleDisplay().CreateRole(MeVo.instance.job,LoadMyModelCallBack);

			vsName.text = challengerInfo.name;
			vsLevel.text = challengerInfo.level.ToString ();
			vsRank.text = challengerInfo.rank.ToString ();
			vsFight.text = challengerInfo.fightNum.ToString ();
		}

		//角色模型创建成功回调(自己)
		private void LoadMyModelCallBack(GameObject go)
		{
			//			this.model = go;
			//this.model.transform.localScale  =comTest.transform.localScale*0.63f;
			go.transform.parent = left;
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(100,100,100);
			go.transform.localPosition = new Vector3(-300,10,0);
			myModel = go;
//			NGUITools.SetLayer(myModel, LayerMask.NameToLayer("TopUI"));

			new RoleDisplay().CreateRole(challengerInfo.job,LoadVserModelCallBack);
		}
		//角色模型创建成功回调(对手)
		private void LoadVserModelCallBack(GameObject go)
		{
			//			this.model = go;
			//this.model.transform.localScale  =comTest.transform.localScale*0.63f;
			go.transform.parent = right;
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(-100,100,100);
			go.transform.localPosition = new Vector3(320,10,0);
			vserModel = go;
//			NGUITools.SetLayer(vserModel, LayerMask.NameToLayer("TopUI"));
		}
		
		//返回键被点击
		private void BackOnClick(GameObject go)
		{
			this.CloseView ();
			Singleton<ArenaView>.Instance.OpenArenaMainView ();
		}

		//挑战按钮被点击
		private void StarteBattleOnClick(GameObject go)
		{
			Singleton<ArenaMode>.Instance.ApplyStartChallenge (challengerInfo.roleId, challengerInfo.rank);
//			Singleton<MapMode>.Instance.changeScene(60001, false, 5, 1.8f);
//			this.CloseView ();
		}
	}
}