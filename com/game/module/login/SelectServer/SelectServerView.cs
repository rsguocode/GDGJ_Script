using com.game.module.test;
﻿﻿using com.u3d.bases.debug;
﻿﻿using UnityEngine;


namespace com.game.module.login
{
    public class SelectServerView : BaseView<SelectServerView>
    {
        public override string url { get { return "UI/SelectServer/SelectServerView.assetbundle"; } }
		public override bool playClosedSound { get { return false; } }
        public override bool isDestroy
        {
            get
            {
                return true;
            }
        }

		public override bool isUnloadDelay { get { return true; } }
        private Button btn_area;
        //private Button btn_area;
        //private Button btn_area;
        //private Button btn_area;
        private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        //private Button btn_server;
        private Button btn_recommendserver;
        private Button btn_lastserver;
        private Button btn_fanhui;
        private UIGrid grid_area;
        private UIGrid grid_server;
        protected override void Init()
        {
            //btn_area = FindInChild<Button>("clip_area/grid_area/btn_area");
            //grid_area = FindInChild<UIGrid>("clip_area/grid_area");
            //btn_area = FindInChild<Button>("grid_area/btn_area");
            //btn_area = FindInChild<Button>("grid_area/btn_area");
            //btn_area = FindInChild<Button>("grid_area/btn_area");
            //btn_server = FindInChild<Button>("clip_server/grid_server/btn_server");
            //grid_server = FindInChild<UIGrid>("clip_server/grid_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_server = FindInChild<Button>("grid_server/btn_server");
            //btn_recommendserver = FindInChild<Button>("yourserver/btn_recommendserver");
            btn_lastserver = FindInChild<Button>("center/xia/shangci/btn_lastserver");
            btn_fanhui = FindInChild<Button>("btn_fanhui");

            //模拟 初始化 区 和服务器 
            //AddArea(3);
            //AddServer(7);
            //grid_area.Reposition();
            //grid_server.Reposition();
            btn_lastserver.onClick += RecommendSeverOnClick;
            btn_fanhui.onClick += FanHuiOnClick;

        }

        //初始化 区 信息
        private void AddArea(int num)
        {
            for (int i = 0; i < num; i++)
            {
                NGUITools.AddChild(grid_area.gameObject, btn_area.gameObject);
            }
        }
        private void AddServer(int num)
        {
            for (int i = 0; i < num; i++)
            {
                NGUITools.AddChild(grid_server.gameObject, btn_server.gameObject);
            }

        }
        private void FanHuiOnClick(GameObject go)
        {
            CloseView();
//            Singleton<LoginView>.Instance.OpenView();

        }

        private void RecommendSeverOnClick(GameObject go)
        {
            Log.info(this, "this is RecommendSeverOnClick");
            CloseView();
			Singleton<LoginMode>.Instance.serverId = 999;
//			SDKManager.SDKLoginServerLog (Singleton<LoginMode>.Instance.targetServer);
//            Singleton<RoleCreateView>.Instance.OpenView();
			this.CloseView ();
            
        }

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();		
//			Singleton<ContinueCutView>.Instance.CloseView();	
		}
    }
}

