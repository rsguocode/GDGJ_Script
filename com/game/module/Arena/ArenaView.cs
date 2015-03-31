using UnityEngine;
using System.Collections;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.u3d.bases.debug;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统视图管理类
 * *******************************************************/
namespace Com.Game.Module.Arena
{
	public class ArenaView : BaseView<ArenaView> {

		public override string url { get { return "UI/Arena/ArenaView.assetbundle"; } }

		public override ViewLayer layerType
		{
			get { return ViewLayer.MiddleLayer; }
		}
		public override ViewType viewType
		{
			get { return ViewType.CityView; }
		}

		public override bool waiting {
			get {
				return false;
			}
		}

		private int _subView = 0;



		protected override void Init()
		{
			Singleton<ArenaMainView>.Instance.gameObject = FindChild("ArenaMainView");
			Singleton<ArenaMainView>.Instance.Init();

			Singleton<ArenaRankView>.Instance.gameObject = FindChild("ArenaRankView");
			Singleton<ArenaRankView>.Instance.Init();

			Singleton<ArenaRecordView>.Instance.gameObject = FindChild("ArenaRecordView");
			Singleton<ArenaRecordView>.Instance.Init();
            //预设删除了
			//Singleton<ArenaVsView>.Instance.gameObject = FindChild("ArenaVsView");
			//Singleton<ArenaVsView>.Instance.Init();
        }

		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			//			Singleton<WaitingView>.Instance.OpenView ();
			//			Singleton<CopyMode>.Instance.isInitCopyView = true;
			switch (this._subView)
			{
			case 0:
				break;
			case 1:
				Singleton<WaitingView>.Instance.CloseView ();
				Singleton<ArenaMainView>.Instance.OpenView ();
				break;
			case 2:
				Singleton<WaitingView>.Instance.CloseView ();
				Singleton<ArenaRankView>.Instance.OpenView ();
				break;
			case 3:
				Singleton<WaitingView>.Instance.CloseView ();
				Singleton<ArenaRecordView>.Instance.OpenView ();
				break;
			case 4:
				Singleton<WaitingView>.Instance.CloseView ();
                //预设删除了
				//Singleton<ArenaVsView>.Instance.OpenView ();
				break;
			}
			
		}

		//判定是否已打开竞技场UI
		private void OpenArenaViewJudge()
		{
			if (gameObject != null && gameObject.activeInHierarchy)
				HandleAfterOpenView ();
			else
				Singleton<ArenaView>.Instance.OpenView ();
		}

		//初始化ArenaView(预加载用，不显示waiting)
		public void PreLoadArenaView()
		{
			this._subView = 0;
			OpenArenaViewJudge ();
		}

		//打开竞技场主UI
		public void OpenArenaMainView()
		{
			this._subView = 1;
			Singleton<WaitingView>.Instance.OpenView ();
			OpenArenaViewJudge ();
		}

		//打开竞技场排行榜UI
		public void OpenArenaRankView()
		{
			this._subView = 2;
			Singleton<WaitingView>.Instance.OpenView ();
			OpenArenaViewJudge ();
		}

		//打开竞技场排行榜UI
		public void OpenArenaRecordView()
		{
			this._subView = 3;
			Singleton<WaitingView>.Instance.OpenView ();
			OpenArenaViewJudge ();
		}

		//打开竞技场VS UI
		public void OpenArenaVsView()
		{
			this._subView = 4;
			Singleton<WaitingView>.Instance.OpenView ();
			OpenArenaViewJudge ();
		}
	}
}