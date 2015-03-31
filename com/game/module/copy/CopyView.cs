using Com.Game.Module.DaemonIsland;
using com.game.module.test;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Copy
{
    public class CopyView : BaseView<CopyView>
    {
        public override string url { get { return "UI/Copy/CopyView.assetbundle"; } }
        public override ViewLayer layerType
        {
            get { return ViewLayer.HighLayer; }
        }
        private OpenViewType mSubViewType = OpenViewType.None;

        public OpenViewType SubViewType
        {
            get { return mSubViewType; }
            private set { mSubViewType = value; }
        }

        public override bool IsFullUI
        {
            get { return true; }
        }

        public enum OpenViewType
        {
            None,
            DameonIslandView,
            CopyPointView
        }
        protected override void Init()
        {
            DaemonIslandView.Instance.gameObject = FindChild("DaemonIslandView");
            CopyPointView.Instance.gameObject = FindChild("CopyPointView");

            Singleton<DaemonCopyDetailView>.Instance.gameObject = FindChild("CopyDetailView");
            Singleton<DaemonCopyFastFightView>.Instance.gameObject = FindChild("FastFightView");

            Singleton<CopyDetailView>.Instance.gameObject = FindChild("CopyDetailView");
            Singleton<FastFightView>.Instance.gameObject = FindChild("FastFightView");
        }

        public override void OpenView()
        {
            if (IsOpened) return;
            base.OpenView();
        }

		public override void CloseView ()
		{
			base.CloseView ();
			Singleton<CopyPointView>.Instance.CloseView ();
			Singleton<DaemonIslandView>.Instance.CloseView ();
			Singleton<CopyDetailView>.Instance.CloseView ();
			Singleton<FastFightView>.Instance.CloseView ();
		}

        protected override void HandleAfterOpenView()
        {
            if (SubViewType == OpenViewType.DameonIslandView)
            {
                CopyPointView.Instance.CloseView();
                DaemonIslandView.Instance.OpenView();
            }
            else if (SubViewType == OpenViewType.CopyPointView)
            {
                DaemonIslandView.Instance.CloseView();
                CopyPointView.Instance.OpenView();
            }
        }

        protected override void HandleBeforeCloseView()
        {
            if (SubViewType == OpenViewType.DameonIslandView)
            {
                DaemonIslandView.Instance.CloseView();
            }
            else if (SubViewType == OpenViewType.CopyPointView)
            {
                CopyPointView.Instance.CloseView();
            }
            SubViewType = OpenViewType.None;
        }
        public void OpenDameonIslandView()
        {
            SubViewType = OpenViewType.DameonIslandView;
            OpenView();
        }


        public void OpenCopyPointView()
        {
            SubViewType = OpenViewType.CopyPointView;
            OpenView();
        }

        



    }

}

