using System;
using System.Collections.Generic;
using System.Collections;
using com.game.Public.Confirm;
using com.game.Public.Message;
using com.u3d.bases.loader;
using UnityEngine;
using com.u3d.bases.debug;
using System.IO;
using com.utils;
using com.game.module.loading;
using com.game.consts;
using com.game.data;
using com.game.preloader;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.game.vo;
using com.game.module.map;
namespace com.game.manager
{
    /// <summary>
    /// 加载资源结束
    /// </summary>
    public delegate void LoadAssetFinish<T>(T obj) where T:UnityEngine.Object;


    /// <summary>
    /// 加载资源结束
    /// </summary>
    public delegate void LoadSceneCallBack(string param) ;

    /// <summary>
    /// 加载多资源结束
    /// </summary>
    public delegate void LoadAssetsFinish<T>(List<T> objList) where T : UnityEngine.Object;


    /// <summary>
    /// 加载资源包结束
    /// </summary>
    public delegate void LoadAssetBundleFinish( AssetBundleLoader obj);

    

    class AssetManager:MonoBehaviour
    {
        public delegate void SetUpFinish();

        private bool loadAssetbundle = true;
        ///static properties

        /// <summary>
        /// 平台对应的资源包打包路径
        /// </summary>
        public static string dataPathURL
        {
            get
            {

                if (Application.platform == RuntimePlatform.Android)
                {
                    return Application.streamingAssetsPath + "/";
                }
                else {

                    return "file:///" + Application.streamingAssetsPath + "/";
                }
            }
        }

        /// <summary>
        /// 平台对应的资源包本地存储路径
        /// </summary>
        public static string persistentDataPath = "file:///"+Application.persistentDataPath+"/";

        /// <summary>
        /// 资源包服务端地址
        /// </summary>
        public static  String serverURL {get{return "http://localhost/";}}

        private volatile static AssetManager instance = null;

        public static AssetManager Instance
        {
            get
            {

                if (instance == null)
                {
                    GameObject target = new GameObject("AssetManagerObj");
                    GameObject.DontDestroyOnLoad(target);
                    AssetManager assetManager = target.AddComponent<AssetManager>();
                    instance = assetManager;
                    instance.MaxLoadingNum = 1;
                }
                return instance;

            }

        }

        private  LoadAssetFinish<UnityEngine.Object> initCall;

        private bool isAssetInit = false;
        private volatile int callCount = 0;

               /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="callback">回调信息</param>
        /// <returns>加载列表</returns>
        public void init(LoadAssetFinish<UnityEngine.Object> callback)
        {
            initCall = callback;
            initFont();
        }

        //加载字体
        public void initFont(){
            Instance.LoadAndCacheSharedAssets("UI/Font/SimHei.assetbundle", InitSimHeiCallBack);
            
        }

        private void InitSimHeiCallBack(UnityEngine.Object obj)
        {
            Instance.LoadAndCacheSharedAssets("UI/Font/ShuiMo.assetbundle", InitShuiMoFontCallBack);
        }

        private void InitShuiMoFontCallBack(UnityEngine.Object obj)
        {
            Instance.LoadAndCacheSharedAssets("UI/Font/BitmapFont/ShuiMoBitmapFont.assetbundle", InitFontCallBack);
        }

        private void InitFontCallBack(UnityEngine.Object obj){
            initCommon();
        }

        private void initCommon()
        {
            Instance.LoadAndCacheSharedAssets("UI/Common/common.assetbundle", initCommonCallBack);
        }

        private void initCommonCallBack(UnityEngine.Object obj)
        {
            MessageManager.Init();
            ConfirmMgr.Instance.Init();

            Singleton<StartLoadingView>.Instance.OpenView();
            Singleton<WaitingView>.Instance.OpenView();
        }

        void Update() {
            if (!isAssetInit)
            {
                if (Singleton<StartLoadingView>.Instance.gameObject != null) {
                    Singleton<StartLoadingView>.Instance.loaderList.AddRange(initOther());
                    isAssetInit = true;
                }
            }

            lock (WaitingAssetsBundles)
            {
                lock (loadingAssetsBundles)
                {
                    if (WaitingAssetsBundles.Count > 0 && loadingAssetsBundles.Count < MaxLoadingNum)
                    {
                        AssetBundleLoader assetBundleLoader = WaitingAssetsBundles[0];
                        if (!loadingAssetsBundles.ContainsKey(assetBundleLoader.fileName))
                        {
                            loadingAssetsBundles.Add(assetBundleLoader.fileName, assetBundleLoader);
                            StartCoroutine(LoadAssetInAssetBundle(assetBundleLoader));
                        }
                        WaitingAssetsBundles.RemoveAt(0);
                    }
                }
            } 
        }

        private List<AssetBundleLoader> initOther() // 主要是读取 BinData.xml 信息
        {
            callCount = 2;
            List<AssetBundleLoader> loadList = new List<AssetBundleLoader>();

            if (Application.platform == RuntimePlatform.WindowsPlayer && File.Exists(Application.persistentDataPath + "/Xml/BinData.xml"))
            {

                    Log.info(this, "从xml中加载数据");
                    byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/Xml/BinData.xml");
                    object dataVo = SerializerUtils.binaryDerialize(bytes);
                    // dataVo.keys = SysBuffVo,SysConfigVo,SysDaemonislandVo,SysDailyBuffVo,SysDeedVo,SysMapVo.......

                    BaseDataMgr.instance.init(dataVo);
                    ConfigConst.Instance.SetConfigData(BaseDataMgr.instance.GetDicByType<SysConfigVo>());
                    initCountCallBack(null);
            }
            else
            {
                loadList.Add(Instance.LoadAsset<TextAsset>("Xml/BinData.assetbundle", initBindataCallBack));
            }
            loadList.Add(Instance.LoadAsset<TextAsset>(LanguageManager.URL, InitLoadingLanguageCallBack));
            return loadList;
       }

        private void initCountCallBack(UnityEngine.Object obj)
        {

            lock (instance)
            {
                callCount--;
                if (callCount == 0)
                {
                    initCall(null);
                }
            }     
        }

        private void initBindataCallBack(TextAsset text) {
            if (text != null)
            {
                Log.info(this, "-Start() 4、基础数据长度:" + (text.bytes.Length / 1024) + "KB");
                object dataVo = SerializerUtils.binaryDerialize(text.bytes); //.jsonDerialize(dataAsset.bytes);
                BaseDataMgr.instance.init(dataVo);
                Resources.UnloadAsset(text);
                ConfigConst.Instance.SetConfigData(BaseDataMgr.instance.GetDicByType<SysConfigVo>());
                Log.info(this, "-Start() 5、基础数据初始化【OK】");
            }
            initCountCallBack(text);  
        }


        private void InitLoadingLanguageCallBack(TextAsset text)
        {
            string data = System.Text.Encoding.GetEncoding("UTF_8").GetString(text.bytes);
            StartCoroutine(LanguageManager.SetUp(data, initCountCallBack));


            //initCountCallBack(text); 
        }




        /// <summary>
        /// 共享资源缓存，以避免重复加载
        /// key-资源包名
        /// value-资源
        /// </summary>
        private Dictionary<String, AssetBundleLoader> sharedAssets = new Dictionary<String, AssetBundleLoader>();

        private int MaxLoadingNum { get; set; }

        /// <summary>
        /// 加载的资源包
        /// key-资源包名
        /// value-资源
        /// </summary>
        private Dictionary<String, AssetBundleLoader> loadingAssetsBundles = new Dictionary<String, AssetBundleLoader>();


        /// <summary>
        /// 等待加载的资源包
        /// key-资源包名
        /// value-资源
        /// </summary>
        private List<AssetBundleLoader> WaitingAssetsBundles = new List<AssetBundleLoader>();



        /// <summary>
        /// 缓存的Asset，key为资源包名称+资源名称+资源类型
        /// </summary>
        private Dictionary<String, UnityEngine.Object> cachedAssets = new Dictionary<String, UnityEngine.Object>();

        /// <summary>
        /// 移除缓存的Asset
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="fileName">资源包名</param>
        /// <param name="assetName">资源名</param>
        /// <returns></returns>
        public bool RemoveCachedAsset<T>(string fileName,string assetName) where T:UnityEngine.Object {
            
            string assetKey = getAssetKey(fileName, assetName,typeof(T));
            lock (cachedAssets)
            {
                return cachedAssets.Remove(assetKey);
            }
        }

        /// <summary>
        /// 缓存的Asset调用回调，key为资源包名称+资源名称+资源类型
        /// </summary>
        private Dictionary<String, Delegate> cachedAssetsCallBack = new Dictionary<String, Delegate>();

        private void addCallBack<T>(string assetKey, LoadAssetFinish<T> callBack) where T : UnityEngine.Object
        {
            lock (cachedAssetsCallBack)
            {
                if (!cachedAssetsCallBack.ContainsKey(assetKey))
                {
                    cachedAssetsCallBack[assetKey] = callBack;
                }
                else
                {
                    LoadAssetFinish<T> call = cachedAssetsCallBack[assetKey] as LoadAssetFinish<T>;
                    call += callBack;
                    cachedAssetsCallBack[assetKey] = call;

                }
            }
        }

        private void callBack(string assetKey, UnityEngine.Object param ) 
        {
            Log.info("this", "call back" + assetKey + cachedAssetsCallBack.ContainsKey(assetKey));
            Delegate call = null;
            lock (cachedAssetsCallBack)
            {
                if (cachedAssetsCallBack.ContainsKey(assetKey))
                {
                    //Log.info("this", "call back"+assetKey);
                    call = cachedAssetsCallBack[assetKey];
                    cachedAssetsCallBack.Remove(assetKey);
                }
            }

            if (call!=null)
            {   
                foreach(Delegate callBack in call.GetInvocationList()){
                    callBack.DynamicInvoke(param);
                }
            }
        }

    
        ///methods

        /// <summary>
        /// 下载并存储资源包
        /// </summary>
        /// <param name="assetBundleLoader">资源包下载类</param>
        /// <param name="path">网络路径</param>
        /// <param name="fileName">资源包名</param>
        /// <returns></returns>
        private IEnumerator DownloadAndStoreAssetBundle(AssetBundleLoader assetBundleLoader,LoadAssetBundleFinish callBack)
        {
            yield return StartCoroutine(assetBundleLoader.LoadAssetBundle( true,true));
            //下载存储完成后的回调
            if(callBack!=null)
            {
                callBack(assetBundleLoader);

            }
        }


        /// <summary>
        /// 下载并存储资源包
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="fileName">资源包名次</param>
        /// <param name="callBack">下载存储结束回调</param>
        /// <returns>资源包加载类</returns>
        public AssetBundleLoader DownLoadAssetBundle(string url, string fileName,LoadAssetBundleFinish callBack=null)
        {
            AssetBundleLoader assetBundleLoader = new AssetBundleLoader(url, fileName,null);

            StartCoroutine(DownloadAndStoreAssetBundle(assetBundleLoader, callBack));
            
            return assetBundleLoader;
        }

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="assetBundleLoader">资源包下载类</param>
        /// <param name="path">网络路径</param>
        /// <param name="fileName">资源包名</param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundle(AssetBundleLoader assetBundleLoader,LoadAssetBundleFinish callBack)
        {
            yield return StartCoroutine(assetBundleLoader.LoadAssetBundle( false,false));
            //下载存储完成后的回调
            if(callBack!=null)
            {
                callBack(assetBundleLoader);
                
            }
        }


        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名次</param>
        /// <param name="callBack">加载结束回调</param>
        /// <returns>资源包加载类</returns>
        public AssetBundleLoader LoadAssetBundle(string path, string fileName,LoadAssetBundleFinish callBack=null)
        {
            AssetBundleLoader assetBundleLoader = new AssetBundleLoader(path,fileName,null);
            
            StartCoroutine(LoadAssetBundle(assetBundleLoader,callBack));
            
            return assetBundleLoader;   
        }


        /// <summary>
        /// 加载资源里的资源
        /// </summary>
        /// <param name="assetBundleLoader">资源包下载类</param>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="callBack">加载结束回调</param>
        /// <returns></returns>
        private IEnumerator LoadAssetInAssetBundle(AssetBundleLoader assetBundleLoader) 
        {
            string assetName = assetBundleLoader.assetName;
            bool cache = assetBundleLoader.cache;
            bool scene = assetBundleLoader.isScene;

            string[] depAssetBundles = null;
            //加载依赖资源
            List<AssetBundleLoader> dependAssetBundles = new List<AssetBundleLoader>();
            if (scene)
            {
                //获取依赖描述文件对象
                string depFileAssetName = getDepAssetFileName(assetBundleLoader.fileName);
                string path = getAssetBundleLoaderPath(depFileAssetName);
                AssetBundleLoader dependFileLoader = new AssetBundleLoader(path, depFileAssetName,null);
                yield return StartCoroutine(dependFileLoader.LoadAssetBundle(false, false));
              
                if (dependFileLoader.state == DownLoadState.Loaded)
                {// 先找出依赖项 名称；
                    TextAsset depTxt = dependFileLoader.assetBundle.mainAsset as TextAsset;
                    depAssetBundles = getDependeAssetFilesName(depTxt); // MapResource/10000/10000qj.assetbundle\r\nMapResource/10000/10000wj.assetbundle
                    dependFileLoader.UnloadAssetBundle();
                }


                if (depAssetBundles != null)
                {
                    // MapResource/10000/10000qj.assetbundle\r\nMapResource/10000/10000wj.assetbundle
                    // 是一些地图 图集图片吧
                    foreach (string depAssetFileName in depAssetBundles)
                    {
                        Log.info(this, "加载" + assetBundleLoader.fileName + "的依赖" + depAssetFileName);
                        string depPath = getAssetBundleLoaderPath(depAssetFileName);
                        dependAssetBundles.Add(new AssetBundleLoader(depPath, depAssetFileName,null));
                    }
                }

                if (dependAssetBundles != null)
                {// 加载 依赖项; 
                    foreach (AssetBundleLoader loader in dependAssetBundles)
                    {
                        yield return StartCoroutine(loader.LoadAssetBundle(false, false));
                        if (loader.state == DownLoadState.Loaded)
                        {
                            Log.info(this, "成功加载" + loader.path + loader.fileName);
                        }
                    }
                }
            }

            yield return StartCoroutine(assetBundleLoader.LoadAssetBundle( false, false));
            string assetKey = getAssetKey(assetBundleLoader.fileName, assetName, assetBundleLoader.assetType);
            //assetKey = "Scene/10001.assetbundle10001GameObject"

            //加载场景
            if (scene)
            {   
                //加载场景
                yield return StartCoroutine(LoadSceneLevel(assetBundleLoader, assetName));
                //完成
                assetBundleLoader.UnloadAssetBundle();
                foreach (AssetBundleLoader loader in dependAssetBundles)
                {
                    loader.UnloadAssetBundle();
                }
                //加载场景预加载资源
                LoadScenePreAsset(uint.Parse(assetName), assetKey);
                lock (loadingAssetsBundles)
                {
                    loadingAssetsBundles.Remove(assetBundleLoader.fileName);
                }
                System.GC.Collect();
            }
            else // 资源，非场景
            {
                UnityEngine.Object asset = null;
                if (assetBundleLoader.asynLoad)
                {
                    AssetBundleRequest request = LoadAssetAsync(assetBundleLoader, assetName,
                        assetBundleLoader.assetType);
                    if (request != null)
                    {
                        yield return request;
                        asset = request.asset;
                    }
                }
                else
                {
                    asset = LoadAsset(assetBundleLoader, assetName, assetBundleLoader.assetType);
                }
                if (asset==null)
                {
                    if (!assetBundleLoader.fileName.Contains("bust"))
                    {
                        Log.error(this, "asset==null" + assetBundleLoader.fileName);
                    }  
                }
                if (cache)
                {
                    cachedAssets[assetKey] = asset;
                }
                if (assetBundleLoader.state == DownLoadState.Loaded&&assetBundleLoader.delayUnload!=true)
                {
                    assetBundleLoader.UnloadAssetBundle();

                }
                lock (loadingAssetsBundles)
                {
                    loadingAssetsBundles.Remove(assetBundleLoader.fileName);
                }
                callBack(assetKey, asset);
            }
           
            //加载资源结束后回调
        }


        private IEnumerator LoadSceneLevel(AssetBundleLoader assetBundleLoader, string sceneName) {
            print("****异步切换场景,sceneName = " + sceneName);
            AsyncOperation loadLevel = Application.LoadLevelAsync(sceneName);
            assetBundleLoader.loadSceneOperation = loadLevel;
            yield return loadLevel;
        }

        /// <summary>
        /// 加载资源包中的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名称</param>
        /// <param name="callBack">加载结束回调</param>
        /// <param name="assetName">资源名，默认为null则加载主资源</param>
        /// <returns>资源包加载类</returns>
        public AssetBundleLoader LoadAsset<T>(string path, string fileName, LoadAssetFinish<T> callBack, string assetName=null,bool cache = false,bool scene = false,bool asynload = false) where T : UnityEngine.Object
        {
            //共享中存在，直接返回缓存数据
            if (sharedAssets.ContainsKey(fileName)) {

                UnityEngine.Object asset = LoadAsset(sharedAssets[fileName], assetName,typeof(T));
                if (callBack != null)
                {
                    T result = asset as T;
                    callBack(result);
                }
                return sharedAssets[fileName];
            }
            //缓存中存在
            string assetKey = getAssetKey(fileName, assetName,typeof(T));
            if (cachedAssets.ContainsKey(assetKey)) {
                if (callBack != null)
                {
                    T asset = cachedAssets[assetKey] as T;
                    callBack(asset);
                }
                return new AssetBundleLoader(path, fileName, typeof(T), assetName, cache, DownLoadState.Cached, scene, asynload); 
            }
            //添加回调
            if (callBack != null)
            {
                addCallBack<T>(assetKey, callBack);
            }

            lock (loadingAssetsBundles){
                //检查是否正在加载
                if (loadingAssetsBundles.ContainsKey(fileName))
                {
                    return loadingAssetsBundles[fileName];
                }
            }
            AssetBundleLoader assetBundleLoader = new AssetBundleLoader(path, fileName, typeof(T), assetName, cache, DownLoadState.Init, scene, asynload);
            addWaitAssetBundleLoader(assetBundleLoader);
            return assetBundleLoader;
        }


        private void  addWaitAssetBundleLoader(AssetBundleLoader assetBundleLoader){
            lock (WaitingAssetsBundles)
            {
                for(int i=0;i<WaitingAssetsBundles.Count;i++)
                {
                    if (WaitingAssetsBundles[i].fileName.Equals(assetBundleLoader.fileName))
                    {
                        //重复等待
                        return;
                    }
                }
                WaitingAssetsBundles.Add(assetBundleLoader);
            }
        }
        /// <summary>
        /// 加载资源包中的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名称</param>
        /// <param name="callBack">加载结束回调</param>
        /// <param name="assetName">资源名，默认为null则加载主资源</param>
        ///<param name="cache">是否缓存</param>
        /// <param name="asynLoad">是否异步加载asset</param>
        /// <returns>资源包加载类</returns>
        public AssetBundleLoader LoadAsset<T>(string fileName, LoadAssetFinish<T> callBack, string assetName = null,bool cache = false,bool asynLoad = true) where T : UnityEngine.Object
        {
            string path = getAssetBundleLoaderPath(fileName);
            return LoadAsset<T>(path, fileName, callBack, assetName, cache, false, asynLoad); 
        }

        public AssetBundleLoader LoadAssetFromResources<T>(string fileName, LoadAssetFinish<T> callBack, string assetName = null, bool cache = false) where T : UnityEngine.Object
        {
             fileName = fileName.Replace(".assetbundle", "");
             callBack(ResMgr.instance.load(fileName, typeof(T)) as T);
             return new AssetBundleLoader(fileName, assetName, typeof(T));
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="mapResourceId">场景ID</param>
        /// <param name="callBack">场景加载完成回调</param>
        /// <returns>场景加载对象</returns>
        public AssetBundleLoader LoadSceneLevel(uint mapResourceId,LoadAssetFinish<GameObject> callBack) {

            string sceneFileName = "Scene/" + mapResourceId + ".assetbundle";
            string sceneName = mapResourceId.ToString();

            if (!loadAssetbundle)
            {   AssetBundleLoader s = new  AssetBundleLoader("","",typeof(GameObject));
                StartCoroutine(LoadSceneInResource(mapResourceId, callBack));
                return s;
            }

            int[] subTypeList = { PRTypeConst.ST_SKILL, PRTypeConst.ST_SOUND };
            for (int i = 0; i < subTypeList.Length; i++)
            {
                IList<SysReadyLoadVo> preloadList = BaseDataMgr.instance.GetScenePreLoadList(mapResourceId, subTypeList[i]);
                Singleton<StartLoadingView>.Instance.PreLoadNum += preloadList.Count;
            }
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(MeVo.instance.mapId);
            //获取要加载的怪物队列，并计算预加载数
            if (mapVo.type == MapTypeConst.COPY_MAP)
            {
                MonsterMgr.Instance.PreMonloadList = BaseDataMgr.instance.GetMonPreLoadList(MeVo.instance.mapId);
                if (MonsterMgr.Instance.PreMonloadList.Count > 0)
                    Singleton<StartLoadingView>.Instance.PreLoadNum += MonsterMgr.Instance.PreMonloadList.Count;

            }
            return LoadSceneLevel( sceneFileName,  sceneName,callBack );
        }

        private IEnumerator LoadSceneInResource(uint mapResourceId, LoadAssetFinish<GameObject> callBack)
        {
            AssetBundleLoader s = new AssetBundleLoader("", "", typeof(GameObject));
            yield return StartCoroutine(LoadSceneLevel(s, mapResourceId.ToString()));
            if (callBack != null)
            {
                addCallBack<GameObject>(mapResourceId.ToString(), callBack);
            }
            LoadScenePreAsset(mapResourceId, mapResourceId.ToString());
        }

        public AssetBundleLoader LoadSceneLevel(string sceneFileName,  string sceneName,LoadAssetFinish<GameObject> callBack) 
        {
            string path = getAssetBundleLoaderPath(sceneFileName);
            return LoadAsset<GameObject>(path, sceneFileName, callBack, sceneName,false,true); 
        }


        private string getAssetKey(string fileName, string assetName,Type assetType)
        {

            string assetKey;
            if (assetName != null)
            {
                assetKey = fileName + assetName;
            }
            else
            {
                assetKey = fileName + "main";
            }
            assetKey += assetType.Name;
            return assetKey;

        }

        
        private string getAssetBundleLoaderPath(string fileName)
        {
            if (ClientUpdate.updateFinish&&File.Exists(Application.persistentDataPath + "/" + fileName)) //如果客户端更新，则先去持久化存储路径去找
            {
                return persistentDataPath;
            }
            else
            {
                return dataPathURL;
               
            }
        
        }

        /// <summary>
        /// 加载并缓存资源包里的所有资源
        /// </summary>
        /// <param name="assetBundleLoader">资源包下载类</param>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="callBack">加载结束回调</param>
        /// <returns></returns>
        private IEnumerator LoadAndCachedAllAssetInAssetBundle(AssetBundleLoader assetBundleLoader, LoadAssetFinish<UnityEngine.Object> callBack)
        {
            yield return StartCoroutine(assetBundleLoader.LoadAssetBundle( false, false));
            //加载完成
            if (assetBundleLoader.state == DownLoadState.Loaded)
            {
                //assetBundleLoader.LoadAllAssets();
                lock (sharedAssets)
                {
                    if (!sharedAssets.ContainsKey(assetBundleLoader.fileName))
                    {
                        sharedAssets.Add(assetBundleLoader.fileName, assetBundleLoader);
                    }
                }
               
            }
            if (callBack != null)
            {
                callBack(null);
            }
        }


        /// <summary>
        /// 加载并缓存共享资源包里的资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">资源包名称</param>
        /// <param name="callBack">加载结束回调</param>
        /// <param name="assetName">资源名，默认为null则加载主资源</param>
        /// <returns>资源包加载类</returns>
        public AssetBundleLoader LoadAndCacheSharedAssets(string fileName, LoadAssetFinish<UnityEngine.Object> callBack)
        {
            if (!loadAssetbundle)
            {
                callBack(null);
                return new AssetBundleLoader(fileName, "", typeof (GameObject));
            }
            string path = getAssetBundleLoaderPath(fileName);
            AssetBundleLoader assetBundleLoader = new AssetBundleLoader(path,fileName,null);
            if (!sharedAssets.ContainsKey(fileName))
            {   //未缓存则缓存
                Log.info(this, "加载" + path + fileName);
                StartCoroutine(LoadAndCachedAllAssetInAssetBundle(assetBundleLoader, callBack));
            }
            else
            {
                if (callBack != null)
                {
                    callBack(null);
                }
            }

            return assetBundleLoader;
        }



        
        /// <summary>
        /// 移除共享资源缓存，如果未使用则卸载
        /// </summary>
        /// <param name="assetName">资源包名</param>
        public void RemoveSharedAssetBundle(String assetBundleName,bool isAll = true){
            lock (sharedAssets)
            {
                if (sharedAssets.ContainsKey(assetBundleName)){
                    sharedAssets[assetBundleName].UnloadAssetBundle(isAll);
                    sharedAssets.Remove(assetBundleName);
                    Resources.UnloadUnusedAssets();
                }
            }
        }

        /// <summary>
        /// 移除所有的共享资源缓存，未使用的则卸载
        /// </summary>
        /// <param name="assetName">资源名</param>
        public void RemoveAllSharedAssetBundle(bool isAll = true)
        {
            lock (sharedAssets)
            {
                foreach (AssetBundleLoader assetbundle in sharedAssets.Values)
                {
                    assetbundle.UnloadAssetBundle(isAll);
                }
                sharedAssets.Clear();
                Resources.UnloadUnusedAssets();
            }
        }

        /// <summary>
        /// 获取资源包的依赖描述文件资源包名
        /// </summary>
        /// <param name="assetFileName">资源名</param>
        /// <returns>依赖的加载描述包名</returns>
        private String getDepAssetFileName(string assetFileName)
        {
            char[] end = new char[] { '.' };
            string[] paths = assetFileName.Split(end, StringSplitOptions.RemoveEmptyEntries);
            string depAssetFileName = paths[0] + "_dep." + paths[1];
            return depAssetFileName;
        }

        /// <summary>
        /// 从资源包中加载目标资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetBundleLoader">资源包</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>目标资源</returns>
        private UnityEngine.Object LoadAsset(AssetBundleLoader assetBundleLoader,string assetName,Type assettype)
        {

            UnityEngine.Object asset = null;
 
            //加载完成
            if (assetBundleLoader.state == DownLoadState.Loaded )
            {
                if (assetName != null && !assetName.Equals(String.Empty))
                {
                    //通过资源名称加载资源
                    UnityEngine.Object loadedAsset = assetBundleLoader.assetBundle.Load(assetName, assettype);
                    if (loadedAsset == null)
                    {
                        Log.error(this, string.Format("资源包{0}{1}中类型为{2}的资源 {3} 不存在", assetBundleLoader.path, assetBundleLoader.fileName, assettype.Name, assetName));
                    }
                    else
                    {
                        Log.info(this, string.Format("成功在资源包{0}{1}中加载类型为{2}的资源 {3}", assetBundleLoader.path, assetBundleLoader.fileName, assettype.Name, assetName));
                        asset = loadedAsset;
                    }
                }
                else
                {
                    Log.info(this,assetBundleLoader.fileName + " " + assetBundleLoader.path);
                    //加载主资源
                    UnityEngine.Object mainAsset = assetBundleLoader.assetBundle.mainAsset;
                    if (mainAsset == null)
                    {
                        Log.error(this, string.Format("资源包{0}{1}中主资源不存在", assetBundleLoader.path, assetBundleLoader.fileName));
                    }
                    else if (assettype == typeof(UnityEngine.Object))
                    {
                           return asset = mainAsset;
                    }
                    else if (mainAsset.GetType() != assettype)
                    {
                        Log.error(this, string.Format("资源包{0}{1}中主资源不是{2}类型", assetBundleLoader.path, assetBundleLoader.fileName, assettype.Name));
                    }
                    else
                    {
                        Log.info(this, string.Format("成功在资源包{0}{1}中加载类型为{2}的主资源", assetBundleLoader.path, assetBundleLoader.fileName, assettype.Name));
                        asset = mainAsset;
                    }
                }
            }
            return asset;
        }

        /// <summary>
        /// 从资源包中加载目标资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetBundleLoader">资源包</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>目标资源</returns>
        private AssetBundleRequest LoadAssetAsync(AssetBundleLoader assetBundleLoader, string assetName, Type assettype)
        {
            AssetBundleRequest request = null;

            //加载完成
            if (assetBundleLoader.state == DownLoadState.Loaded)
            {
                if (assetName == null  || assetName.Equals(String.Empty))
                {
                    //通过资源名称加载资源
                    int last1 = assetBundleLoader.fileName.LastIndexOf('/');
                    int last2 = assetBundleLoader.fileName.LastIndexOf('.');

                    assetName = assetBundleLoader.fileName.Substring(last1 + 1, last2 - last1 - 1);
                }
                request = assetBundleLoader.assetBundle.LoadAsync(assetName, assettype);
            }

            return request;
        }


        /// <summary>
        /// 获取文本中的资源包依赖文件名
        /// </summary>
        /// <param name="text">文本资源包</param>
        /// <returns>资源包文件名</returns>
        private string[] getDependeAssetFilesName(TextAsset text) { 
       
            String allDep = text.text;
            char[] end = {'\r','\n'};
            string[] all = allDep.Split(end,StringSplitOptions.RemoveEmptyEntries);
 
            return all;
        }


        /// <summary>
        /// 场景资源预加载处理
        /// </summary>

        private List<uint> scenePreloadHistory = new List<uint>();
        
        /// <summary>
        /// 预加载场景资源
        /// </summary>
        /// <param name="mapId">场景MapId</param>
        /// <param name="key">场景Key</param>
        public void LoadScenePreAsset(uint mapId,string key)
        {
            StartCoroutine(LoadSceneAsset(mapId, key));
        }

        private IEnumerator LoadSceneAsset(uint mapId,string key)
        {
            //场景资源预只会预加载一次
            if (!scenePreloadHistory.Contains(mapId))
            {
                Log.warin(this, "-startLoadScene() 异步加载场景特效资源" + mapId);
                scenePreloadHistory.Add(mapId);

                int[] subTypeList = {PRTypeConst.ST_SKILL, PRTypeConst.ST_SOUND};
                for (int i = 0; i < subTypeList.Length; i++)
                {
                    // 存储 预加载场景的资源 列表	
                    IList<SysReadyLoadVo> preloadList = BaseDataMgr.instance.GetScenePreLoadList(mapId, subTypeList[i]);
                    if (preloadList.Count > 0)
                    {
                        IPreloader loader = PreloaderFactory.Instance.GetPreLoader(preloadList[0]);// 返回特效或者音效 实例
                        foreach (SysReadyLoadVo preload in preloadList)
                        {
                            IList<SysReadyLoadVo> list = new List<SysReadyLoadVo>();
                            list.Add(preload);
                            yield return StartCoroutine(loader.PreloadResourceList(list)); // 主要是加载 特效和音效
                            Singleton<StartLoadingView>.Instance.PreLoadedNum += list.Count;
                        }
                    }
                }
            }
            else
            {
                Singleton<StartLoadingView>.Instance.PreLoadedNum += Singleton<StartLoadingView>.Instance.PreLoadNum;
            }

            //预加载怪物,前面已经获取到加载列表了    
            if (MonsterMgr.Instance.PreMonloadList != null
                && MonsterMgr.Instance.PreMonloadList.Count > 0)
            {
                yield return StartCoroutine(MonsterMgr.Instance.PreloadMonsterList());
            }

            callBack(key,null);
        }

    }

}
