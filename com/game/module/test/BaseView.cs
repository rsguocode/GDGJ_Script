using System;
using System.Collections.Generic;
using com.game.module.Guide;
using com.game.module.Guide.GuideLogic;
using com.game.module.map;
using com.game.vo;
using UnityEngine;
using com.u3d.bases.loader;
using com.game.manager;
using Com.Game.Module.Waiting; 
using com.game.sound;

/**模块视图--基础类
 * 1、只定义了初始化、显示、关闭、销毁视图空实现的接口
 * **/
using com.u3d.bases.debug;


namespace com.game.module.test
{
    /// <summary>
    /// HighLayer，TopLayer是由ViewPort的照相机渲染
	/// LowLayer,MiddleLyaer是由UI的照相机渲染
    /// </summary>
    public enum  ViewLayer
    {
        BaseLayer = 0,  //要比战斗UI，主UI的情况
        LowLayer=1,//  主UI ，战斗UI 等
        MiddleLayer=2,  //   UI 层
        HighLayer=3,  //tips层  
        TopLayer=4,  //消息框层
        TopUILabyer = 5,  //TopUILayer
		NoneLayer //由“父”统一调整,自身不同调整panel depth
    }

    public enum ViewType
    {
        NormalView =0, //UI类型-普通
        BattleView = 1,//战斗UI
        CityView = 3,//城镇UI-非战斗场景
    }

    public delegate void OpenViewGuideDelegate();  //no params 方便调用
   

    public class BaseView<T>: Singleton<T>,IView  where T : new()
    {

		private GameObject mGameObject;
        public bool IsOpened;

		public GameObject gameObject 
		{ 
			get{
				return mGameObject;
			}
			set{
                //加强限制，因为我们的View 和 预设是具有强关联的，
                //预设的结构对应Init的不同实现，
                //所以没有必要考虑其他情况
                if (mGameObject == null && value != null)
                {
					mGameObject = value;
				    transform = mGameObject.transform;
				    if (!isInit) //不是动态加载的
				    {
				        Init();
				        isInit = true;
				    }
				}
			}
		}
        /// <summary>
        /// transform 指和gameObject 绑定就不需要提供外景set接口了
        /// </summary>
		public Transform transform { get; private set; }

        public virtual ViewLayer layerType { get { return ViewLayer.BaseLayer; } }
		public virtual bool  IsFullUI { get { return false; } }  //是否为全屏UI
        /// <summary>
        /// 关闭时是否销毁
        /// </summary>
        public virtual bool isDestroy { get { return false; } }
		public virtual bool isUnloadDelay { get { return false; } }
        /// <summary>
        /// 是否异步AssetBundle.LoadAsync
        /// </summary>
        public virtual bool isAsyncLoad {get{return true;}}

		//关闭时是否播放关闭音效
		public virtual bool playClosedSound { get { return true; } }
        /// <summary>
        /// 是否已经开始加载，防止多次加载
        /// </summary>
        private bool isInit = false;
		/// <summary>
		/// 可以利用这个属性实现提前加载预设
		/// </summary>
        protected  bool firstOpen = true;    //第一次调用OpenView是否打开View

		private bool isSecondOpen = false;
        public virtual string url { get{return null;} } //资源包路径路径
        public virtual string viewName { get { return null; } }//资源名称
        public virtual bool isAssetbundle { get { return true; } }//是否是资源包加载

		//private void 
		public virtual bool waiting { get { return true; }set{} } //是否打开等待加载页面

        public virtual ViewType viewType { get { return ViewType.NormalView; } } //UI类型默认为普通

		protected TweenPlay showTween;
		protected IPlay closeTween;//
        public OpenViewGuideDelegate AfterOpenGuideDelegate;  //打开面板时执行指引功能的代理

		AssetBundleLoader assetLoader;

        //初始化 控件  子类重写,只在加载完AssetBundle之后执行一次
        protected virtual void Init()
        {

        }

        //加载 AssetBundle 回调函数
        protected void LoadViewCallBack(GameObject prefab)
        {
            if (viewType == ViewType.CityView)
            {
                gameObject = NGUITools.AddChild(ViewTree.city, prefab); //Instantiate prefab
            }
            else if (viewType == ViewType.BattleView)
            {
                gameObject = NGUITools.AddChild(ViewTree.battle, prefab); //Instantiate prefab
            }
            else
            {
                gameObject = NGUITools.AddChild(ViewTree.go, prefab); //Instantiate prefab
            }

            //transform = gameObject.transform;
			if(layerType == ViewLayer.HighLayer || layerType == ViewLayer.TopLayer)
				NGUITools.SetLayer(gameObject,LayerMask.NameToLayer("Viewport"));
            Init();

			if(isSecondOpen)
				firstOpen = true;
			gameObject.SetActive(firstOpen);
			if (waiting && !this.GetType().Equals(Singleton<WaitingView>.Instance.GetType()))
			{
				Singleton<WaitingView>.Instance.CloseView();
			}
			if(firstOpen)
			{
				OpenViewHelp();
			}
        }
        // 加载 AssetBundle 基类实现
        private void LoadView()
        {
            UnityEngine.Debug.Log("LoadView: url = " + url);

            if (isInit)
			{
				isSecondOpen = true;
                return;
			}
            isInit = true;
			if (waiting && !this.GetType().Equals(Singleton<WaitingView>.Instance.GetType()))
            {
                Singleton<WaitingView>.Instance.OpenView();
            }
            if (isAssetbundle)
			{
                Singleton<WaitingView>.Instance.loadProcess = AssetManager.Instance.LoadAsset<GameObject>(url, LoadViewCallBack, viewName, false, isAsyncLoad);
                if (isUnloadDelay == true)
                {
                    assetLoader = Singleton<WaitingView>.Instance.loadProcess;
                    assetLoader.delayUnload = true;
                }
			}
            else
            {
                LoadViewCallBack(ResMgr.instance.load(url) as GameObject);
            }
        }

        //打开UI后的处理 ，打开 View 的特效，动画 ,协议请求等处理 ，子类重写，基类调用
		protected virtual void HandleAfterOpenView()
		{
		
		}
        //关闭UI前的处理，关闭 View 的特效 ，动画 等处理 ，子类重写，基类调用,每次CloseView都会被调用
        protected virtual void HandleBeforeCloseView()
        {

        }
        //View 是否打开状态 防止动画交叉的情况
        private bool openState = false;
        //打开 View 
        public virtual void OpenView()
        {
            IsOpened = true;
            if (gameObject == null)
            {
                LoadView();
            }
            else 
            {
                
				if(!gameObject.activeInHierarchy)
                	gameObject.SetActive(true);
				OpenViewHelp();
            }
            
        }

        private void OpenViewHelp()
        {
            openState = true;
			ViewManager.Register(this);
			RegisterUpdateHandler();
			HandleAfterOpenView();
			if(showTween != null)  //播放打开动画
				showTween.PlayForward();
            if (AfterOpenGuideDelegate != null)
            {
                AfterOpenGuideDelegate();
                AfterOpenGuideDelegate = null;
            }
        }

        public virtual void CloseView()
        {
            if (gameObject != null)
            {
                openState = false;
				HandleBeforeCloseView();
				CancelUpdateHandler();
				ViewManager.UnRegister(this);

				if(closeTween != null)  //播放关闭动画
				{
					EventDelegate.Add(closeTween.OnEnd,CloseViewHelp);
					closeTween.Begin();
				}
				else if(showTween != null)
				{
					EventDelegate.Add(showTween.onFinished,CloseViewHelp);
					showTween.PlayReverse();
				}
				else 
					CloseViewHelp();

				if (playClosedSound)
				{
					SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmClose);
				}
			}
            IsOpened = false;
        }

		private void CloseViewHelp()
		{
		    if (openState)  //在关闭动画播放过程中又 打开了 就不关闭
		        return;
            if(gameObject)  //防止已经被父节点销毁了的情况
			    gameObject.SetActive(false);
		    if (isDestroy)
		    {
		        Destroy();
                DestoryHelp();
		        isInit = false;
		    }
		    if (isUnloadDelay && assetLoader != null) 
			{		
				assetLoader.UnloadAssetBundle (true);
				assetLoader = null;
					
			}
			if(showTween != null)
			{
				EventDelegate.Remove(showTween.onFinished,CloseViewHelp);
			}

		}
		//Update函数
        public virtual void Update()
        {
        }
        //销毁相应的处理
        public virtual void Destroy()
        {
            
        }

        private void DestoryHelp()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
                isInit = false;
                Resources.UnloadUnusedAssets();
            }
        }

        //注册数据更新处理函数
        public virtual void RegisterUpdateHandler()
        {
        }

        //数据更新响应函数
        public virtual void DataUpdated(object sender, int code)
        {
        }

        //取消数据更新处理函数
        public virtual void CancelUpdateHandler()
        {
        }

        //查找路径下的组件 T
        public T FindInChild<T>(string path=null) where T : Component
        {
            if (string.IsNullOrEmpty(path))
            {
                return gameObject.GetComponent<T>();
            }
            else
                return NGUITools.FindInChild<T>(gameObject, path);
        }


        public GameObject FindChild(string path)
        {
            if (transform.Find(path))
                return transform.Find(path).gameObject;
            else
                return null;
        }

    }

    public interface IView
    {
		GameObject gameObject { get; set; }
		Transform transform { get;  }
        ViewLayer layerType { get;  }
		bool IsFullUI { get; }

        void CloseView();
        void OpenView();
        void Update();				

        //注册数据更新器
        void RegisterUpdateHandler();
        //数据更新响应
        void DataUpdated(object sender, int code);
        //取消数据更新器
        void CancelUpdateHandler();
    }
    public static class ViewManager
    {
        public static List<IView> openViewList = new List<IView>();

		public static void Register(IView obj)
        {
			lock(openViewList)
			{
				if (obj.IsFullUI)
				{
					HidePrevFullUI(obj);
				    EnableMainCamera(false);
				}

	            AddView(obj);
			}
        }

        private static void EnableMainCamera(bool enable)
        {
            if (!ReferenceEquals(MapControl.Instance.MyCamera, null))
                MapControl.Instance.MyCamera.gameObject.GetComponent<Camera>().enabled = enable;
        }

        private static void HidePrevFullUI(IView topObj)
		{
			for (int i=openViewList.Count-1; i>=0; i--)
			{
				IView view = openViewList[i];
			    GameObject parent = view.gameObject.transform.parent.gameObject;
                if (view != topObj && (parent == ViewTree.go || parent == ViewTree.city || parent == ViewTree.battle) && !(view is GuideView))
                {
					view.gameObject.SetActive(false);
				}
			}
		}

		private static void ShowPrevFullUI()
		{
			for (int i=openViewList.Count-1; i>=0; i--)
			{
				IView view = openViewList[i];
                GameObject parent = view.gameObject.transform.parent.gameObject;
                if ( (parent == ViewTree.go || parent == ViewTree.city || parent == ViewTree.battle) && !(view is GuideView))
                {
                    view.gameObject.SetActive(true);
                }
             
                if (view.IsFullUI && view.gameObject.transform.parent.gameObject.activeSelf)
			    {
			        return;
			    }
			}
            EnableMainCamera(true);
		}

        public static void UnRegister(IView obj)
        {
			lock(openViewList)
			{
	            openViewList.Remove(obj);

	            if(openViewList.Contains(obj))
	            {

	            }

				if (obj.IsFullUI)
				{
					ShowPrevFullUI();
				}
			}
        }


        public static void CloseAll()
        {
			lock(openViewList)
			{
	            foreach (IView temp in openViewList)
	            {
	                temp.CloseView();
	                
	            }
				openViewList.Clear();
			}
        }

        public static void Update()
        {
			lock(openViewList)
			{ 	
				int length = openViewList.Count;
				for (int i =0; i<length;i++)
				{   
					int curLength = openViewList.Count;

					openViewList[i].Update();

					length = openViewList.Count;

					if(curLength-length>0){
						i -= curLength-length;
						if(i<0)
						{
							i=0;
						}
					}
	            }
			}
        }


		private static void AddView(IView view)
        {
			if(view.layerType != ViewLayer.NoneLayer)
			{
		    	int depth = GetMaxDepth(view.layerType);
		    	depth ++;
		        UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel>(true);
		        Array.Sort(panels, DepthCompareFunc);
					int lastDepth = -9999;
		        foreach (UIPanel panel in panels)
		        {	
					if(panel.depth==lastDepth)
					{
		            	panel.depth = depth-3;
					}
					else
					{
							lastDepth = panel.depth;
							panel.depth = depth;
							depth += 3;
					}
				} 
			}
            if(!openViewList.Contains(view))
                openViewList.Add(view);

            
        }

        private static int DepthCompareFunc(UIPanel left, UIPanel right)
        {
            if (left.depth == right.depth)
                return 0;
            else if (left.depth > right.depth)
                return 1;
            else
                return -1;
        }
        private static int GetMaxDepth(ViewLayer layer)
        {
            int depth = 100*(int)layer;
			int minDepth = 1000;
			int maxDepth = depth;
            foreach (IView view in openViewList)
            {
                if (view.layerType == layer)
                {
                    UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel>(true);
                    foreach (UIPanel panel in panels)
                    {
                        if (maxDepth < panel.depth)
                            maxDepth = panel.depth;
						if (minDepth > panel.depth)
							minDepth = panel.depth;
                    }
                }
            }
			//统一调整基准点
			foreach (IView view in openViewList)
			{
				if (view.layerType == layer)
				{
					UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel>(true);
					foreach (UIPanel panel in panels)
					{
						panel.depth = panel.depth - minDepth + depth;
					}
				}
			}
			if(maxDepth-minDepth + depth< depth)
				return depth;
			else
				return maxDepth-minDepth + depth;
        }
    }

   
    
}
