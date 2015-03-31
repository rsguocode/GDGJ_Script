﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using com.u3d.bases.debug;
using UnityEngine;

/**采用Resources.Load加载--管理器**/
namespace com.u3d.bases.loader
{
    internal class ResLoadMgr
    {
        public bool isTrace = true;
        private bool isLoading;
        private BinLoadMgr binLoadMgr;
        private VerMgr verMgr;
        private IList<LoadItem> waitList;                              //加载列表
        private Dictionary<string, IList<LoadFinishBack>> okBackList;
        private Dictionary<string, IList<LoadFinishBack>> errorBackList;  

        private const int MAX_OFFET_PRIORITY = 65535;                      //同一类型的资源，加载优先偏移值
        private int offetPriority = MAX_OFFET_PRIORITY;


        public ResLoadMgr(BinLoadMgr binLoadMgr, VerMgr verMgr)
        {
            this.binLoadMgr = binLoadMgr;
            this.verMgr = verMgr;
            waitList = new List<LoadItem>();
            okBackList = new Dictionary<string, IList<LoadFinishBack>>();
            errorBackList = new Dictionary<string, IList<LoadFinishBack>>();
        }

        public void addTask(string url, int resType, LoadFinishBack callback = null, Type types=null, int priority = 0, LoadFinishBack errorback = null)
        {
            if (okBackList.ContainsKey(url)) 
            {
                addFinishBack(url, callback);
                return;
            }

            if (ResPool.instance.hasLoadedRes(url))
            {//已加载
                callFinishBack(url);
                remove(url, okBackList);
                if (callback != null) callback(url);
                return;
            }

            LoadItem item = new LoadItem();
            item.url = url;
            item.types = types;
            item.resType = resType;
            item.priority = priority * MAX_OFFET_PRIORITY + offetPriority;
            offetPriority--;
            waitList.Add(item);
            if (!isLoading) loadNext();
        }


        private void addFinishBack(string url,LoadFinishBack callback) {
            if (callback == null) return;
            IList<LoadFinishBack> list = okBackList.ContainsKey(url) ? okBackList[url] : null;
            if (list == null)
            {
                list = new List<LoadFinishBack>();
                okBackList.Add(url, list);
            }
            list.Add(callback);
        }

        private void addErrorBack(string url, LoadFinishBack errorback)
        {
            if (errorback == null) return;
            IList<LoadFinishBack> list = errorBackList.ContainsKey(url) ? errorBackList[url] : null;
            if (list == null)
            {
                list = new List<LoadFinishBack>();
                errorBackList.Add(url, list);
            }
            list.Add(errorback);
        }

        private void callFinishBack(string url)
        {
            IList<LoadFinishBack> list = okBackList.ContainsKey(url) ? okBackList[url] : null;
            if (list == null) return;
            foreach (LoadFinishBack item in list) item(url);
        }

        private void callErrorBack(string url)
        {
            IList<LoadFinishBack> list = errorBackList.ContainsKey(url) ? errorBackList[url] : null;
            if (list == null) return;
            foreach (LoadFinishBack item in list) item(url);
        }

        private void remove(string url,Dictionary<string, IList<LoadFinishBack>> dict) {
            IList<LoadFinishBack> list = dict.ContainsKey(url) ? dict[url] : null;
            if (list == null) return;
            dict.Remove(url);
            list.Clear();
            list = null;
        }


        //===============加载队列器===============//
        /**加载下一个**/
        private void loadNext() {
            if (waitList.Count < 1)
            {
                offetPriority = MAX_OFFET_PRIORITY;
                isLoading = false;
                if (isTrace) Log.info(this, "-loadNext() 加载器已空闲！");
                return;
            }
            else isLoading = true;

            LoadItem item =waitList[0];
            string loadUrl=verMgr.getLoadUrl(item.url);
            if (isTrace) Log.info(this, "-loadNext() url:" + loadUrl + " 加载开始！");
            UnityEngine.Object data = item.types != null ? Resources.Load(loadUrl, item.types) : Resources.Load(loadUrl);
            ResPool.instance.addLoadedRes(item.url,data);
            callFinishBack(item.url);
            remove(item.url, okBackList);
            loadNext();
        }
    }
}
