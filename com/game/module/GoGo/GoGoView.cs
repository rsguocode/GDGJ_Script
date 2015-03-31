using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.module.map;

namespace Com.Game.Module.GoGo
{
	public class GoGoView : BaseView<GoGoView> {

		public override string url {get {return "UI/GoGo/GoGoView.assetbundle";}}
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; } }
		public override bool waiting{ get{return false;}}
		public override bool playClosedSound { get { return false; } }

		private TweenPlay play;
		private float x;
		private float y;

		protected override void Init()
		{
			//transform.localScale = new Vector3(0.4f,0.4f,0.4f);
			play = FindInChild<TweenPlay>();
			play.style = UITweener.Style.Loop;
			base.firstOpen = false;
		}

		public void OpenView(float x,float y)
		{
			this.x = x;
			this.y = y;
			base.OpenView();
		}

		protected override void HandleAfterOpenView()
		{
			transform.localPosition = new Vector3(x,y,0);
			play.PlayForward();
		}
		protected override void HandleBeforeCloseView()
		{
			play.ResetToBeginning();
			PlaytList pl = new PlaytList();
			pl.AddTimeInterval(0.1f,Singleton<MapControl>.Instance.ShowStageEffect);

			pl.Start();
		}
	}
}
