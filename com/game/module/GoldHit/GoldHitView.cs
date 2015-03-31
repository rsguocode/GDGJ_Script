using UnityEngine;
using System.Collections;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.game.vo;
using com.u3d.bases.display.character;
using com.u3d.bases.debug;
using com.game.consts;

namespace Com.Game.Module.GoldHit
{
    //点击点石成金首先进入的界面
    public class GoldHitView : BaseView<GoldHitView>
    {
        public override string url { get { return "UI/GoldHit/GoldHitView.assetbundle"; } }
        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }
        public override bool waiting
        {
            get
            {
                return false;
            }
        }

        protected  override void Init()
        {
            Singleton<GoldHitLogView>.Instance.gameObject = FindChild("GoldHitLogView");
            Singleton<GoldHitLogView>.Instance.Init();

            Singleton<GoldHitMainView>.Instance.gameObject = FindChild("GoldHitMainView");
            Singleton<GoldHitMainView>.Instance.Init();

            Singleton<GoldHitJieSuan>.Instance.gameObject = FindChild("GoldHitJieSuan");
            Singleton<GoldHitJieSuan>.Instance.Init();
        }

        public void InitMain()
        {
            Init();
        }

        public void Close()
        {
            this.CloseView();
            Singleton<MessageView1>.Instance.CloseView();
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<GoldHitMainView>.Instance.gameObject.SetActive(false);
            if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP)   //如果是击石成金副本的话，显示面板
            {
                Singleton<GoldHitLogView>.Instance.CloseView();
                Singleton<GoldHitMainView>.Instance.OpenView();
            }
            else
            {
                Singleton<MessageView1>.Instance.CloseView();
                Singleton<GoldHitLogView>.Instance.OpenView();
            }

			Singleton<GoldHitMode>.Instance.StopTips();
        }

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			Singleton<GoldHitMode>.Instance.StartTips();
			Singleton<GoldHitJieSuan>.Instance.CloseView();
		}

        //判定是否已打开点石成金的UI
        private void OpenGoldHitViewJudge()
        {
            if (gameObject != null && gameObject.activeInHierarchy)
                HandleAfterOpenView();
            else
                Singleton<GoldHitView>.Instance.OpenView();
        }
    }
}