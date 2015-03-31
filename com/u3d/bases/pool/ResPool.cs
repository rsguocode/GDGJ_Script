﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using com.bases.utils;
using com.u3d.bases.debug;

/**资源缓存管理器**/
namespace com.u3d.bases.loader
{
	internal class ResPool
	{
        private IDictionary<string, UnityEngine.Object> usedPool;     //内存对象池
        private IDictionary<string, UnityEngine.Object> loadedPool;   //内存镜像池
        internal static ResPool instance = new ResPool();

        public ResPool()
        {
            usedPool = new Dictionary<string, UnityEngine.Object>();
            loadedPool = new Dictionary<string, UnityEngine.Object>();  
        }

        /**加入内存对象池
        * @param url  资源地址
        * @param data 资源
        * **/
        internal void addRes(string url, UnityEngine.Object data)
        {
            if (data == null) return;
            if (StringUtils.isEmpty(url)) return;
            if (!usedPool.ContainsKey(url)) usedPool.Add(url, data);
        }

        /**从内存对象池移除并返回移除结果
         * @param url 地址
         * @param true:移除成功,false:移除失败或资源不存在
         * **/
        internal bool delRes(string url)
        {
            if (!hasRes(url)) return false;
            return usedPool.Remove(url);
        }

        /**从内存对象池中取得资源
         * @param url 地址
         * @return
         * **/
        internal UnityEngine.Object getRes(string url)
        {
            return StringUtils.isEmpty(url) ? null : usedPool[url];
        }

        /**取得材质贴图**/
        internal Texture2D getTexture2D(string url)
        {
            return (Texture2D)getRes(url);
        }

        internal AnimationClip getAnimationClip(string url) {
            return (AnimationClip)getRes(url);
        }

        /**资源是否已在内存对象池
         * @param url 地址
         * @return true:已存在,false:未存在
         * **/
        internal bool hasRes(string url)
        {
            if (StringUtils.isEmpty(url)) return false;
            return usedPool.ContainsKey(url) ? true : false;
        }

        /**销毁资源**/
        internal void dispose(string url)
        {
            delRes(url);
            delLoadedRes(url);
        }



        //============== 内存镜像资源 ===============//
        /**缓存已加载资源**/
        internal void addLoadedRes(string url, UnityEngine.Object data)
        {
            if (data == null || StringUtils.isEmpty(url)) return;
            if (!loadedPool.ContainsKey(url)) loadedPool.Add(url, data);
            Log.info(this, "-addLoadedRes() url:"+url+" Add [OK]");
        }

        /**从内存镜像移除资源**/
        internal void delLoadedRes(string url)
        {
            if (!hasLoadedRes(url)) return;
            loadedPool.Remove(url);
        }

        /**从镜像池取资源**/
        internal UnityEngine.Object getLoaedRes(string url)
        {
            if (StringUtils.isEmpty(url)) return null;
            return loadedPool.ContainsKey(url) ? loadedPool[url] : null;
        }

        /**资源是否已加载
        * @param url 地址
        * @return true:已存在,false:未存在
        * **/
        internal bool hasLoadedRes(string url)
        {
            if (StringUtils.isEmpty(url)) return false;
            return loadedPool.ContainsKey(url) ? true : false;
        }

	}
}
