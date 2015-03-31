﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.consts;
using com.game.preloader;
using com.game.data;
using com.game.module.test;

//using com.game.vo;
/**基于异步的2进制加载管理器**/
namespace com.u3d.bases.loader
{
    public class BinLoadMgr:MonoBehaviour
	{
        internal bool isTrace=false;                     //是否打印日志[true:是,false:否]
        internal int maxNumTry = 3;                      //加载重试次数
        internal int maxNumThread = 1;                   //加载并发数  
        internal int overtime = 5 * 1000;                //加载超时(单位：秒)
        internal LoadFinishBack loadback;                //加载成功回调
        internal LoadFinishBack errorback;               //加载失败回调
        internal LoadProgressBack progressback;          //加载进度回调

        private bool isLoading;                          //加载状态[true:忙碌中,false:空闲]
        private IList<LoadItem> waitList;            //等待队列
        private IList<LoadItem> loadingList;         //正在加载队列
        private const int MAX_OFFET_PRIORITY=65535;      //同一类型的资源，加载优先偏移值
		private int offetPriority=MAX_OFFET_PRIORITY;
        private Dictionary<string, int> loadFailList;    //记录加载失败列表
		private IList<uint> scenePreloadHistory;    //记录场景资源预加载历史

        //加载的场景背景资源项
        private bool isLoadingScene = false;
        private LoadItem loadSceneItem;              
        private AsyncOperation loadSceneOpt;
        private LoadProgressBack progresFun;

        public BinLoadMgr() { }

        void Start() {
            waitList = new List<LoadItem>();
            loadingList = new List<LoadItem>();
            loadFailList = new Dictionary<string, int>();
            scenePreloadHistory = new List<uint>();
        }

        void Update() 
		{ 
			try
			{
	            //更改加载进度
	            //checkOvertimes();
	            if (isLoadingScene)
	            {
					//场景资源加载进度
	                //float value = loadSceneOpt.progress;
	                //Log.info(this, "-Update() progress:" + loadSceneOpt.progress);
					if ((null != progresFun) && (null != loadSceneOpt)) 
					{
						progresFun(null, (int)(loadSceneOpt.progress * 100));
					}
	            }
			}
			catch(Exception e)
			{
				Log.warin(this, e.Message);
			}
        }

        /**加载场景文件**/
		internal void loadScene(uint mapId, LoadItem item, LoadProgressBack progresFun = null)
        {
            loadSceneItem = item;
            this.progresFun = progresFun;
            StartCoroutine(startLoadScene(mapId, loadSceneItem.url));
        }

		private IEnumerator startLoadScene(uint mapId, string url)
        {
			try
            {
                print("****异步加载场景特效资源, mapId,url = " + mapId + ", " + url);

                isLoadingScene = true;
                Log.info(this, "-startLoadScene() 异步加载场景，场景uri： " + url);
                loadSceneOpt = Application.LoadLevelAsync(url);
                yield return loadSceneOpt;

				//场景资源预只会预加载一次
				if (!scenePreloadHistory.Contains(mapId))
				{
                    Log.warin(this, "-startLoadScene() 异步加载场景特效资源" + mapId);                    

                    scenePreloadHistory.Add(mapId);

					int[] subTypeList = {PRTypeConst.ST_SKILL, PRTypeConst.ST_SOUND};
					for (int i=0; i<subTypeList.Length; i++)
					{
						IList<SysReadyLoadVo> preloadList = BaseDataMgr.instance.GetScenePreLoadList(mapId, subTypeList[i]);
						if (preloadList.Count > 0)
						{
							IPreloader loader = PreloaderFactory.Instance.GetPreLoader(preloadList[0]);
							Task t =CoroutineManager.StartCoroutine(loader.PreloadResourceList(preloadList)); 
							yield return t.Routine;
						}
					}
				}

				//Log.info (this, "-startLoadScene() 加载完成，调用加载成功回调函数" + loadSceneOpt.progress);
	            if (loadSceneItem != null) loadSceneItem.call();
            
			}
			finally
			{
				isLoadingScene = false;
			}

//			Vector3 cameraPos = GameObject.Find ("main_camera").transform.position;
//			GameObject.Find ("main_camera").transform.position = new Vector3 (MeVo.instance.x, cameraPos.y, cameraPos.z);
        }


        /**添加加载项**/
        internal void addTask(LoadItem task) {
            if (task == null) return;
            if (waitList.IndexOf(task)!=-1) return;
            if (loadingList.IndexOf(task) != -1) return;
            if (isOverTrynum(task.url))
            {//超过重试次数,直接回调
                if (isTrace) Log.error(this, "-loadRes() url:" + task.url + " 已超过加载重试:" + maxNumTry + "次！");
                errorback(task.url);
                return;
            }

            task.priority = task.priority * MAX_OFFET_PRIORITY + offetPriority;
            offetPriority--;
            waitList.Add(task);
            if (!isLoading) loadNext();
        }

        /**取消加载**/
        internal void unload(LoadItem task)
        {
            if (task == null) return;
            if (waitList.IndexOf(task) != -1) waitList.Remove(task);
        }

        /**加载下一个**/
        private void loadNext()
        {
            //A-1 队列已满
            if (loadingList.Count >= maxNumThread) return;
            //A-2 检查队列是否已空闲
            if (waitList.Count < 1)
            {
                if (loadingList.Count < 1) 
                {
                    isLoading = false;
                    offetPriority = MAX_OFFET_PRIORITY;
                    if (isTrace) Log.info(this, "-loadNext() 加载器已空闲！");
                }
                return;
            }

            //A-3 远程加载
            LoadItem item = waitList[0];
            waitList.RemoveAt(0);
            loadingList.Add(item);
            StartCoroutine(loadFromHttp(item));
            if (isTrace) Log.info(this, "-loadNext() url:" + item.url + " 远程加载开始！");
            item = null;
        }

        /**执行异步加载*/
        private IEnumerator loadFromHttp(LoadItem item)
        {
            item.www = WWW.LoadFromCacheOrDownload(item.url, 1);
            item.loadBeginTime = Environment.TickCount;
            yield return item.www;

            if (isTrace) Log.info(this, "-loadFromHttp() url:" + item.url + " 远程加载完成！");
            if (loadingList.IndexOf(item) != -1)    loadingList.Remove(item);
            if (loadFailList.ContainsKey(item.url)) loadFailList.Remove(item.url);
            ResPool.instance.addLoadedRes(item.url,item.www.assetBundle.mainAsset);
            loadback(item.url);
            item = null;
            loadNext();
        }

        /**加载超时检查**/
        private void checkOvertimes() {
            foreach (LoadItem item in loadingList)
            {
                if (isOvertimes(item)) addTrynum(item.url);
            }
        }

        /**等待时间检查
         * @return [true:超时,false:未超时]
         * **/
        private bool isOvertimes(LoadItem task){
            int time = Environment.TickCount;
            return time - task.loadBeginTime >= overtime ? true : false;
        }

        /**重试次数检查
         * @param  url 资源地址
         * @return [true:超过,false:未超过]
         * **/
        private bool isOverTrynum(String url)
        {
            if (!loadFailList.ContainsKey(url)) return false;
            return loadFailList[url] >= maxNumTry ? true : false;
        }

        /**追加重试次数**/
        private void addTrynum(String url) {
            if (loadFailList.ContainsKey(url))
            {
                loadFailList[url]++;
            }
            else 
            {
                loadFailList.Add(url, 1);
            }
        }

	}
}
