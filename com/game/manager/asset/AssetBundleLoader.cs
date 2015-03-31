using UnityEngine;
using System.Collections;
using System.IO;
using com.u3d.bases.debug;
using System;
namespace com.game.manager
{   /// <summary>
    /// 资源包下载-加载类
    /// </summary>
    public class AssetBundleLoader
    {
		//properties
        /// <summary>
        /// 资源包
        /// </summary>
        public AssetBundle assetBundle{ get; private set; }
        /// <summary>
        /// 资源下载器
        /// </summary>
        private WWW downloader;

        /// <summary>
        /// 资源包下载状态
        /// </summary>
        public DownLoadState state { get; private set; }

        /// <summary>
        /// 资源包大小
        /// </summary>
        public int size { get; private set; }
		/// <summary>
		/// 路径
		/// </summary>
        public string path { get; private set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; private set; }
        /// <summary>
        /// 是否是场景
        /// </summary>
        public bool isScene { get; private set; }

        public string assetName { get;  set; }
        public bool cache { get;set; }

        public Type assetType { get; set; }

		public bool delayUnload { get; set;}

        public bool asynLoad { get; set; } 

    /// <summary>
        /// 场景加载操作
        /// </summary>
        public AsyncOperation loadSceneOperation { get; set; }

        private UnityEngine.Object[] allAsset;

        public AssetBundleLoader(string path, string fileName, Type type , string assetName = null, bool cache = false, DownLoadState state = DownLoadState.Init, bool isScene = false,bool asynLoad = false) 
        {
            this.path = path;
            this.fileName = fileName;
            this.assetName = assetName;
            this.state = state;
            this.isScene = isScene;
            this.assetType = type;
            this.asynLoad = asynLoad;
            this.cache = cache;
        }

		/// <summary>
		/// 加载进度
		/// </summary>
		public float progress
		{
			get
            {
                return getProgress();
			}
       
		}


        private float getProgress() {
            float downloadProgress = 0f;
            if (state == DownLoadState.Loaded || state == DownLoadState.Stored)
            {
                downloadProgress = 1F;
            }
            else if (state == DownLoadState.Init || state == DownLoadState.LoadFailure || state == DownLoadState.StoreFailure)
            {
                downloadProgress = 0F;
            }
            else if (state == DownLoadState.Loading)
            {
                downloadProgress = downloader.progress; 
            }

            if (isScene)
            {
                if (loadSceneOperation != null)
                {
                    downloadProgress += loadSceneOperation.progress;
                }
                downloadProgress /= 2;
            }
            return downloadProgress;
        }
		///methods


        /// <summary>
        /// 加载资源包，可本地存储资源包到持久化目录
        /// </summary>
        /// <param name="path">路径：网络或者本地路径</param>
        /// <param name="fileName">资源包文件名</param>
        /// <param name="storeLocal">是否需本地存储,默认false</param>
        /// <param name="unload">是否卸载</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle( bool storeLocal = false,bool unload = false)
        {
            state = DownLoadState.Init;
            downloader = new WWW(path + fileName);
            state = DownLoadState.Loading;
            yield return downloader;
            if (downloader.error != null)
                {
                    //异常退出
                    if (!downloader.error.Contains("bust"))
                    {
                        Log.error(this, downloader.error);
                    }                   
                    state = DownLoadState.LoadFailure;
                    downloader.Dispose();
                    yield break;
                }
            size = downloader.bytesDownloaded;

            assetBundle = downloader.assetBundle;
                if (assetBundle != null)
                {
                    state = DownLoadState.Loaded;

                    if (storeLocal)//本地存储
                    {
                        if (CreateAssetBundleFile(fileName, downloader.bytes))
                        {
                            state = DownLoadState.Stored;
                        }
                        else
                        {
                            state = DownLoadState.StoreFailure;
                        }
                        if (unload)
                        {	//存储后直接卸载
                            assetBundle.Unload(true);
                            assetBundle = null;
                        }
                    }
                }
                else {
                    Log.error(this, "加载" + fileName + "异常，可能资源重复加载");
                    state = DownLoadState.LoadFailure;
                }
                downloader.Dispose();
                //downloader = null;
        }

        /// <summary>
        /// 加载资源包并缓存
        /// </summary>
        /// <param name="path">路径：网络或者本地路径</param>
        /// <param name="fileName">资源包文件名</param>
        /// <param name="fileName">是否卸载,默认</param>
        /// <param name="version">版本，默认1</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundleCached(bool unload = false,int version = 1)
        {
            state = DownLoadState.Init;
            if (Caching.enabled)
            {
                while (!Caching.ready)
                    yield return null;
                downloader = WWW.LoadFromCacheOrDownload(path + fileName, version);
            }

            state = DownLoadState.Loading;
            yield return downloader;
            if (downloader.error != null)
			{	//异常退出
                Log.error(this, downloader.error);
                state = DownLoadState.LoadFailure;
                downloader.Dispose();
                yield break;
            }
            
            assetBundle = downloader.assetBundle;
            if (assetBundle != null)
            {
                state = DownLoadState.Loaded;
                if (unload)
                {	//加载后直接卸载
                    assetBundle.Unload(true);
                    assetBundle = null;
                }
            }
            else 
            {
                Log.error(this, "加载" + fileName + "异常，可能资源重复加载");
                state = DownLoadState.LoadFailure;
            }
            downloader.Dispose();
            downloader = null;
        }

        /// <summary>
        /// 创建文件存储数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="bytes">文件数据</param>
		/// <returns>创建存储成功</returns>
        private bool CreateFile(string path, string fileName, byte[] bytes)
        {
            //文件流信息   
            Stream sw = null;
            string fullPath = path + fileName;
            string dir = fullPath.Substring(0, fullPath.LastIndexOf("/"));
			bool suc = false;
			try
			{
	            DirectoryInfo dirInfo = new DirectoryInfo(dir);
	            if (!dirInfo.Exists) //检查文件夹是否存在
				{	Log.info(this,"创建文件夹:"+dir);
	                dirInfo.Create();
	            }
	            Log.info(this, "创建文件:" + fullPath);
	            FileInfo t = new FileInfo(fullPath);
	            sw = new FileStream(t.FullName, FileMode.OpenOrCreate, FileAccess.Write);
	            //写入字节信息 
	            sw.Write(bytes, 0, bytes.Length);
				suc =  true;
			}
			catch(IOException e)
			{
                Log.error(this, e.Message);
			}
			finally
			{
                if (sw != null)
                {
                    //关闭流   
                    sw.Close();
                    //销毁流   
                    sw.Dispose();
                }
			}
			return suc;
        }

        /// <summary>
        /// 加载全部的资源
        /// </summary>
        /// <returns>
        /// 加载的全部资源
        /// </returns>
        public UnityEngine.Object[] LoadAllAssets()
        {
            if(assetBundle!=null&&allAsset==null) 
            {
                allAsset = assetBundle.LoadAll();
            }
            return allAsset;
        }

        /// <summary>
        /// 卸载此资源包
        /// </summary>
        /// <param name="isAll">是否全部卸载（包括已经加载的内容）</param>
        public void UnloadAssetBundle(bool isAll = false) 
        {
            if (assetBundle != null)
            {
                assetBundle.Unload(isAll);
            }
            allAsset = null;
            
        }

        /// <summary>
		/// 创建资源包文件，以Application.persistentDataPath为持久化目录
        /// </summary>
        /// <param name="fileName">资源包文件名</param>
        /// <param name="bytes">资源包数据</param>
		/// <returns>创建存储成功</returns>
        private bool CreateAssetBundleFile(string fileName, byte[] bytes)
        {
            return CreateFile(Application.persistentDataPath+"/", fileName, bytes);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">文件名称</param>
    
        private void DeleteFile(string path, string fileName)
        {
            File.Delete(path + fileName);

        }  

    }
}