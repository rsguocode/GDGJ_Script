using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;

namespace Com.Game.Module.Waiting
{
    public class WaitingView : BaseView<WaitingView>
    {

        public override string url { get { return "UI/Waiting/WaitingView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }
		public override bool playClosedSound { get { return false; } }
        //public virtual bool isAssetbundle { get { return false; } }
		//public override string firstOpen{ get{return false;}}

		private UISprite fillSprite;


		public AssetBundleLoader loadProcess;

		private TweenPlay playTween;
		private TweenAlphas playAlphas;
        //private int refCount = 0;
        protected override void Init()
        {
			base.firstOpen = false;
			playTween = FindInChild<TweenPlay>("node/waiting");
			playAlphas  = FindInChild<TweenAlphas>("node");
			base.closeTween = playAlphas;
			playAlphas.duration = 0.2f;
			playTween.style = UITweener.Style.PingPong;
			fillSprite = FindInChild<UISprite>("node/waiting/foreground");
        }
		protected override void HandleAfterOpenView()
		{
			playAlphas.ResetToBeginning();
			playTween.PlayForward();
		}
		protected override void HandleBeforeCloseView()
		{
			playTween.ResetToBeginning();
			loadProcess = null;
		}
	    public override void Update()
		{
			if(loadProcess != null)
			{
				float current = loadProcess.progress;
				float last = fillSprite.fillAmount;
				
				if (last != current)
				{
					fillSprite.fillAmount = last + 0.01f;
				}
				if (current > 0.999f)
				{
					fillSprite.fillAmount = 1;
				}
			}
			else 
				fillSprite.fillAmount = 1f;
		}
		
		


    }
}

