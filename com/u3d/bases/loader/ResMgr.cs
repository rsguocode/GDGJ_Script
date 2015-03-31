﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using com.u3d.bases.loader.consts;
using com.u3d.bases.debug;
using com.bases.utils;
using UnityEngine;
using System.Threading;

/**资源管理器**/
namespace com.u3d.bases.loader
{
    public class ResMgr 
	{
        public static string PathURL
		{
			get{
				if (Application.platform == RuntimePlatform.WindowsEditor||Application.platform == RuntimePlatform.WindowsPlayer||Application.platform == RuntimePlatform.WindowsWebPlayer)
				{
					return "file:///" + Application.dataPath + "/StreamingAssets/";
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					return "jar:file://" + Application.dataPath + "!/assets/";
				}
				else if(Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return "file:///"+Application.dataPath + "/Raw/";
				}else{
					return string.Empty;
				}
			}
		}

        private BinLoadMgr binLoadMgr;                                             //远程加载管理器
        private VerMgr _verMgr;                                            //版本管理器
        internal ResLoadMgr resLoadMgr;                                            //Resource资源加载管理器 
        private Dictionary<int, int> typeConfigList;                               //默认优先级配置
        private Dictionary<string, IList<LoadItem>> loadingList;               //加载列表
        private Dictionary<string, IList<LoadProgressBack>> progressList;      //加载进度回调列表
        public static ResMgr instance = new ResMgr();

        public GameObject asset;


        public ResMgr() {
            _verMgr = new VerMgr();
            typeConfigList = new Dictionary<int, int>();
            loadingList = new Dictionary<string, IList<LoadItem>>();
            progressList = new Dictionary<string, IList<LoadProgressBack>>();

            //注册 每类资源的默认优先级
            regTypeConfig(ResType.XML, ResPriority.XML);
            regTypeConfig(ResType.TXT, ResPriority.XML);
            regTypeConfig(ResType.ZIP, ResPriority.XML);
            regTypeConfig(ResType.IMGAE, ResPriority.IMGAE);
            regTypeConfig(ResType.SOUND, ResPriority.SOUND);

            regTypeConfig(ResType.DATAVO, ResPriority.BINARY);
            regTypeConfig(ResType.BINARY, ResPriority.BINARY);
            regTypeConfig(ResType.ANIMATION, ResPriority.EFFECT);
        }

        public VerMgr versionMgr { get { return _verMgr; } }

        /**初始化化远程加载器**/
        public void initBinLoadMgr(BinLoadMgr binLoadMgr)
        {
            if (this.binLoadMgr == null)
            {
                this.binLoadMgr = binLoadMgr;
                this.binLoadMgr.loadback = loadCompleteHandler;
                this.binLoadMgr.errorback = loadErrorHandler;
                this.binLoadMgr.progressback = loadProgressHandler;
                resLoadMgr = new ResLoadMgr(binLoadMgr, versionMgr);
            } 
        }

        /**根据资源类型--注册加载优先级
         * @param resType  资源类型
         * @param priority 加载优先级
         * **/
        public void regTypeConfig(int resType, int priority)
        {
            if (resType < 1 || priority < 1) return;
            removeTypeConfig(resType);
            typeConfigList.Add(resType, priority);
        }

        /**根据资源类型--移除加载优先级**/
        public void removeTypeConfig(int resType) {
            if (typeConfigList.ContainsKey(resType)) typeConfigList.Remove(resType);
        }

        /**加载资源
		 * @param url       加载地址
         * @param resType   资源类型
		 * @param callback  加载成功回调 callback(url,args)
         * @param priority  加载优先级，数字越大越早加载
         * @param errorback 加载失败回调 errorback(url,args)
		 * @param args      其它参数 
		 */
        public bool loadRes(string url, int resType, LoadFinishBack callback = null, int priority = 0, LoadFinishBack errorback = null, object[] args = null)
        {
            if (StringUtils.isEmpty(url)) return false;
			//A-1 内存镜像已有
            if (ResPool.instance.hasLoadedRes(url)) 
            {
                if (binLoadMgr.isTrace) Log.info(this, "-loadRes() url:" + url + " 缓存已有！");
                if (callback != null) callback(url, args);
                return true;
            }
			
            //A-2 不指定优先级，从默认配置项取
            if (priority<1)
            {
                priority = typeConfigList.ContainsKey(resType) ? typeConfigList[resType] : ResPriority.LOW;
            }
            LoadItem item=new LoadItem();
            item.url = url;
            item.args = args;
            item.resType = resType;
            item.priority = priority;
            item.callback = callback;
            item.errorback = errorback;
            if(loadingList.ContainsKey(url))
            {
                loadingList[url].Add(item);   
            }
            else
            {
                loadingList.Add(url, new List<LoadItem>());
                loadingList[url].Add(item);
                binLoadMgr.addTask(item); // 这个很重要，将要下载的item 放进 BinLoadMgr 去排队下载;
            }
            if (binLoadMgr.isTrace) Log.info(this, "-loadRes() url:" + url + " 已放入加载队列！");
			return true;
		}

        /**取消加载**/
        public void unloadRes(String url,LoadFinishBack callback) {
            if (StringUtils.isEmpty(url)) return;
            if (!loadingList.ContainsKey(url)) return;

            IList<LoadItem> list = loadingList[url];
            foreach (LoadItem item in list) 
            {
                if (item.url.Equals(url) && item.callback == callback)
                {
                    binLoadMgr.unload(item);
                    list.Remove(item);
                    item.dispose();
                    break;
                }
            }

            if (list.Count < 1)
            {
                removeProgress(url);
                loadingList.Remove(url);
            } 
        }

        public UnityEngine.Object load(string url,Type type=null)
        {
            Log.info(this,url);
            if (StringUtils.isEmpty(url)) return null;
            if (ResPool.instance.hasLoadedRes(url)) return ResPool.instance.getLoaedRes(url);
            //string loadUrl = versionMgr.getLoadUrl(url);
            string loadUrl = url;
            return type != null ? Resources.Load(loadUrl, type) : Resources.Load(loadUrl);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="url">资源包路径：本地或网络路径</param>
        /// <param name="name">资源名称，null则返回主资源</param>
        /// <param name="unload">是否卸载资源包镜像</param>
        /// <returns>具体的资源</returns>
        public T loadAsset<T>(string url, string name = null,bool unload = true) where T : UnityEngine.Object
        {
            Log.info(this,"start loading asset" +  PathURL + url + " " + typeof(T).Name );
            WWW bundle = LoadAssetBundle(PathURL + url);
            T result;
            if (name == null) {
                 result = (T)(bundle.assetBundle.mainAsset);
                 Log.info(this, "loading mainAsset in " + PathURL + url + " " + typeof(T).Name + " " + result.name);
            }else{
                result  = (T)bundle.assetBundle.Load(name, typeof(T));
                Log.info(this, "loading  in " + PathURL + url + " "+typeof(T).Name +" "+ result.name);
            }
            if (unload)
            {
                bundle.assetBundle.Unload(false);
            }
            bundle.Dispose();
            bundle = null;
            return result;
            
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="url">资源包路径：本地或网络路径</param>
        /// <param name="name">资源名称，null则返回主资源</param>
        /// <param name="unload">是否卸载资源包镜像</param>
        /// <returns>具体的资源</returns>
        public UnityEngine.Object[] loadAllAsset(string url, bool unload = true) 
        {
            WWW bundle = LoadAssetBundle(PathURL+ url);
            UnityEngine.Object[] result =null;
            result = bundle.assetBundle.LoadAll();
            if (unload)
            {
                bundle.assetBundle.Unload(false);
            }
            bundle.Dispose();
            bundle = null;
            return result;

        }

        /// <summary>
        /// 加载资源包，此方法未释放镜像
        /// </summary>
        /// <param name="url">资源路径：本地或网络路径</param>
        /// <param name="url">version：资源版本</param>
        /// <returns>资源加载器WWW</returns>
        private WWW LoadAssetBundle(string path)
        {
	         WWW bundle =  new WWW(path);
             return bundle;
        }


        public UnityEngine.Object[] loadAll(string url, Type type=null)
        {
            if (StringUtils.isEmpty(url)) return null;
            string loadUrl = versionMgr.getLoadUrl(url);
            return type != null ? Resources.LoadAll(loadUrl, type) : Resources.LoadAll(loadUrl);
        }

		internal void loadScene(uint mapId, LoadItem item, LoadProgressBack progresFun = null)
        {
            UnityEngine.Debug.Log("****loadScene, mapId = " + mapId);

            binLoadMgr.loadScene(mapId, item, progresFun);
        }

        /**设置加载并发数**/
        public void setNumThread(int numThread) {
            if (numThread < 1) return;
            if (binLoadMgr != null) binLoadMgr.maxNumThread = numThread;
            
        }

        /**设置加载最大重试数**/
        public void setNumTry(int numTry) {
            if (numTry < 1) return;
            if (binLoadMgr != null) binLoadMgr.maxNumTry = numTry;
        }

        /**返回加载重试数**/
        public int numTry { get { return binLoadMgr != null ? binLoadMgr.maxNumTry : 0; ; } }
        /**返回加载并发数**/
        public int numThread { get { return binLoadMgr != null ? binLoadMgr.maxNumThread : 0; } }

        /**设置日志打印**/
        public void setTrace(bool trace) {
            if (binLoadMgr != null) binLoadMgr.isTrace = trace;
        }

        /**添加进度回调**/
        public void addProgress(string url,LoadProgressBack callback) {
            if (callback == null) return;
            if (StringUtils.isEmpty(url)) return;
            if (!progressList.ContainsKey(url)) progressList.Add(url, new List<LoadProgressBack>());
            progressList[url].Add(callback);
        }

        /**移除进度回调**/
        public void removeProgress(string url, LoadProgressBack callback) {
            if (callback == null) return;
            if (StringUtils.isEmpty(url)) return;
            IList<LoadProgressBack> list = progressList.ContainsKey(url) ? progressList[url] : null;
            if (list != null && list.IndexOf(callback) != -1) list.Remove(callback);
        }

        /**移除进度回调**/
        private void removeProgress(string url) {
            if (StringUtils.isEmpty(url)) return;
            if (progressList.ContainsKey(url)) progressList.Remove(url);
        }


        /**加载失败处理**/
        private void loadErrorHandler(string url, object[] args = null)
        {
            if (!loadingList.ContainsKey(url)) return;
            IList<LoadItem> list = loadingList[url];
            if (binLoadMgr.isTrace) Log.error(this, "-loadErrorHandler() url:" + url + " 加载失败！即将回调:" + (list != null ? list.Count : 0) + " 个函数！");
           
            removeProgress(url);
            loadingList.Remove(url);
            LoadItem item = null;
            while (list.Count>0) 
            {
                item = list[0];
                list.Remove(item);
                item.callError();
                item.dispose();
                item = null;
            }
        }

        /**加载进度处理**/
        private void loadProgressHandler(string url,int progress)
        {
            if (!progressList.ContainsKey(url)) return;
            IList<LoadProgressBack> list = progressList[url];
            foreach (LoadProgressBack callback in list)
            {
                callback(url, progress);
            }
        }

        /**加载完成处理**/
        private void loadCompleteHandler(string url,object[] args=null)
        {
            if (!loadingList.ContainsKey(url)) return;
            IList<LoadItem> list = loadingList[url];
            if (binLoadMgr.isTrace) Log.info(this, "-loadCompleteHandler() url:" + url + " 加载成功,即将回调:" + (list != null ? list.Count : 0) + " 个函数！");

            removeProgress(url);
            loadingList.Remove(url);
            LoadItem item = null;
            while (list.Count > 0)
            {
                item = list[0];
                list.Remove(item);
                item.call();
                item.dispose();
                item = null;
            }
        }

	}
}
