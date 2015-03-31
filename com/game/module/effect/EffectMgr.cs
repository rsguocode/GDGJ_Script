using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.u3d.bases.debug;
using com.game.utils;
using com.u3d.bases.consts;
using com.game.manager;
using com.game.preloader;
using com.game.data;
using com.game.module.test; 
using Com.Game.Public.EffectCameraManage;
using com.game.module;

namespace com.game.module.effect
{
	public class EffectMgr : IPreloader
    {
        public static EffectMgr Instance = (Instance == null ? new EffectMgr() : Instance);

		private const string MAIN_EFFECT_LAYER = "Default";
		private const string UI_EFFECT_LAYER = "UIEffect";
		private float mainCameraEffectZOff = -31f;

		private IDictionary<string, GameObject> effectDictionary;
		private IDictionary<string, GameObject> resDictionary;       //资源对象池
		private IList<Effect> effectCache;
		private IList<Effect> autoDestroyEffectList;
        private Effect curEffectVo;
        private string curEffectUrl;

		private IList<SysReadyLoadVo> preLoadList;
		private int preloadIndex;

		private GameObject mainEffectRoot;
		private GameObject uiEffectRoot;

		public float GetMainCameraEffectZ(float y)
		{
			return y*5+mainCameraEffectZOff;
		}

		public GameObject MainEffectRoot
		{
			get {return mainEffectRoot;}
		}

		//确保不能外部实例化
        private EffectMgr()
        {
			resDictionary = new Dictionary<string, GameObject>();
			effectDictionary = new Dictionary<string, GameObject>();
			autoDestroyEffectList = new List<Effect>();
			effectCache = new List<Effect>(); 

			//Main相机特效根节点
			mainEffectRoot = new GameObject("MainEffectRoot");
			NGUITools.SetLayer(mainEffectRoot, LayerMask.NameToLayer(MAIN_EFFECT_LAYER));
			GameObject.DontDestroyOnLoad(mainEffectRoot);

			//UI相机特效根节点
			uiEffectRoot = NGUITools.AddChild(ViewTree.go);
			uiEffectRoot.name = "UIEffectRoot";
			NGUITools.SetLayer(uiEffectRoot, LayerMask.NameToLayer(UI_EFFECT_LAYER));
			GameObject.DontDestroyOnLoad(uiEffectRoot);
        }

		public IEnumerator PreloadResourceList(IList<SysReadyLoadVo> preLoadList)
        {
			this.preLoadList = preLoadList;
			this.preloadIndex = 0;

			PreloadResource();

			while (preloadIndex < this.preLoadList.Count)
			{
				yield return 0;
			}
        }

		private void PreloadResource()
		{
			try
			{
				while (preloadIndex < preLoadList.Count)
				{
					curEffectUrl = "Effect/Skill/" + preLoadList[preloadIndex].subid + ".assetbundle";

					if (!resDictionary.ContainsKey(curEffectUrl))
					{
						Log.info(this, "preload effect asset " + curEffectUrl);
						AssetManager.Instance.LoadAsset<GameObject>(curEffectUrl, SkillEffectPreLoaded);
						break;
					}
					else
					{
						preloadIndex++;
					}
				}
			}
			catch (Exception e)
			{
				Log.error(this, "preloadResource error, exception is: " + e.Message);
			}
		}

		//技能特效预加载后处理
		private void SkillEffectPreLoaded(GameObject effectObj)
		{
			try
			{
				if (null == effectObj)
				{
					return;
				}

				//将特效对象缓存
				if (!resDictionary.ContainsKey(curEffectUrl))
				{
					resDictionary.Add(curEffectUrl, effectObj);
				}

				//特效GameObject缓存
				CreatePreloadEffectObj(curEffectUrl, effectObj);		
			}
			catch (Exception e)
			{
				Log.error(this, "SkillEffectPreLoaded error, exception is: " + e.Message);
			}
			finally
			{
				preloadIndex++;
				PreloadResource();
			}
		}

		//主相机特效预加载
		public void PreloadMainEffect(string effectId)
		{
			string effectUrl = UrlUtils.GetMainEffectUrl(effectId);
			
			if (StringUtils.isEmpty(effectUrl))
			{
				return;
			}	
			
			if (!resDictionary.ContainsKey(effectUrl))
			{
                Log.info(this, "preload effect asset " + effectUrl);
                AssetManager.Instance.LoadAsset<GameObject>(effectUrl, MainEffectPreLoaded);
			}
		}

		//主相机特效预加载后处理
		private void MainEffectPreLoaded(GameObject effectObj)
		{
			if (null == effectObj)
			{
				return;
			}

			string effectUrl = UrlUtils.GetMainEffectUrl(effectObj.name);
			
			//将特效对象缓存
			if (!resDictionary.ContainsKey(effectUrl))
			{
				resDictionary.Add(effectUrl, effectObj);
			}
			
			//特效GameObject缓存
			CreatePreloadEffectObj(effectUrl, effectObj);
		}

		//UI相机特效预加载
		public void PreloadUIEffect(string effectId)
		{
			string effectUrl = UrlUtils.GetUIEffectUrl(effectId);
			
			if (StringUtils.isEmpty(effectUrl))
			{
				return;
			}	
			
			if (!resDictionary.ContainsKey(effectUrl))
			{
				Log.info(this, "preload effect asset " + effectUrl);
				AssetManager.Instance.LoadAsset<GameObject>(effectUrl, UIEffectPreLoaded);
			}
		}

		//UI相机特效预加载后处理
		private void UIEffectPreLoaded(GameObject effectObj)
		{
			if (null == effectObj)
			{
				return;
			}
			
			string effectUrl = UrlUtils.GetUIEffectUrl(effectObj.name);
			
			//将特效对象缓存
			if (!resDictionary.ContainsKey(effectUrl))
			{
				resDictionary.Add(effectUrl, effectObj);
			}
		}

		//UI特效内部创建函数
		private void CreateInternalUIEffect(string url, Vector3 pos, Effect.EffectCallback playedCallBack, bool needCache, Effect.EffectCreateCallback createdCallBack)
		{
			if (StringUtils.isEmpty(url))
			{
				return;
			}
			
			if (!Singleton<EffectCameraMgr>.Instance.CameraEnabled)
			{
				Singleton<EffectCameraMgr>.Instance.OpenCamera();
			}
			
			Effect effectVo = new Effect();
			effectVo.URL = url;
			effectVo.BasePosition = pos;
			effectVo.BasePosition.z = Singleton<EffectCameraMgr>.Instance.CameraPosition.z + 40;
			effectVo.Offset = Vector3.zero;
			effectVo.AutoDestroy = true;
			effectVo.UIEffect = true;
			effectVo.PlayedCallback = playedCallBack;
			effectVo.CreatedCallback = createdCallBack;
			effectVo.NeedCache = needCache;
			
			CreateEffect(effectVo);
		}

		//Main特效内部创建函数
		private void CreateInternalMainEffect(string url, Vector3 pos, bool autoDestroy, Effect.EffectCallback playedCallBack, bool needCache, float lastTime, Effect.EffectCreateCallback createdCallBack)
		{			
			if (StringUtils.isEmpty(url))
			{
				return;
			}	
			
			Effect effectVo = new Effect();
			effectVo.URL = url;
			effectVo.BasePosition = pos;
			effectVo.BasePosition.z =  GetMainCameraEffectZ(pos.y);
			effectVo.Offset = Vector3.zero;
			effectVo.AutoDestroy = autoDestroy;
			effectVo.PlayedCallback = playedCallBack;
			effectVo.CreatedCallback = createdCallBack;
			effectVo.NeedCache = needCache;
			effectVo.LastTime = lastTime;
			CreateEffect(effectVo);
		}
		
		//创建剧情场景特效
		public void CreateStorySceneEffect(string effectId, Vector3 pos, Effect.EffectCallback playedCallBack = null, Effect.EffectCreateCallback createdCallBack = null)
		{
			string url = UrlUtils.GetStorySceneEffectUrl(effectId);
			CreateInternalMainEffect(url, pos, true, playedCallBack, true, 0, createdCallBack);
		}

		//创建剧情动画特效
		public void CreateStoryMovieEffect(string effectId, Vector3 pos, Effect.EffectCallback playedCallBack = null)
		{
			string url = UrlUtils.GetStoryMovieEffectUrl(effectId);
			CreateInternalUIEffect(url, pos, playedCallBack, true, null);
		}

		//创建UI特效
		public void CreateUIEffect(string effectId, Vector3 pos, Effect.EffectCallback playedCallBack = null, bool needCache = true, Effect.EffectCreateCallback createdCallBack = null)
		{
			string url = UrlUtils.GetUIEffectUrl(effectId);
			CreateInternalUIEffect(url, pos, playedCallBack, needCache, createdCallBack);
		}

		//创建Main相机特效
		public void CreateMainEffect(string effectId, Vector3 pos, bool autoDestroy = true, Effect.EffectCallback playedCallBack = null, bool needCache = true, float lastTime = 0)
		{
			string url = UrlUtils.GetMainEffectUrl(effectId);
			CreateInternalMainEffect(url, pos, autoDestroy, playedCallBack, needCache, lastTime, null);
		}

		//创建Main相机跟随特效
		public void CreateMainFollowEffect(string effectId, GameObject target, Vector3 offset, bool autoDestroy = true, Effect.EffectCallback playedCallBack = null, Effect.EffectCreateCallback createdCallBack = null, bool needCache = true)
		{
			string url = UrlUtils.GetMainEffectUrl(effectId);
			
			if (StringUtils.isEmpty(url))
			{
				return;
			}	
			
			Effect effectVo = new Effect();
			effectVo.URL = url;
			effectVo.BasePosition = target.transform.position;
			effectVo.BasePosition.z =  GetMainCameraEffectZ(effectVo.BasePosition.y);
			effectVo.Offset = offset;
			effectVo.Target = target;
			effectVo.AutoDestroy = autoDestroy;
			effectVo.CreatedCallback = createdCallBack;
			effectVo.PlayedCallback = playedCallBack;
            effectVo.NeedCache = true;
			CreateEffect(effectVo);
		}

		//创建技能特效
		public void CreateSkillEffect(Effect vo)
		{
			vo.BasePosition.z =  GetMainCameraEffectZ(vo.BasePosition.y);
			vo.Offset.z = 0;

			CreateEffect(vo);
		}

        /// <summary>
        /// 创建buff特效;
        /// </summary>
        /// <param name="vo"></param>
        public void CreateBuffEffect(Effect vo)
        {
            vo.BasePosition.z = GetMainCameraEffectZ(vo.BasePosition.y);
            vo.Offset.z = 0;
            CreateEffect(vo);
        }



        /// <summary>
        /// 创建特效
        /// </summary>
		private void CreateEffect(Effect vo)
		{
			if (!StringUtils.IsValidConfigParam(vo.URL))
			{
				return;
			}

			//如果多个特效同时播放，需要缓存，保证同一时间点只播放一个特效
			//主要原因是AssetBundle模块加载是异步的，回调接口没有提供当前文件路径
			lock(effectCache)
			{
				effectCache.Add(vo);

				if (vo.AutoDestroy)
				{
					autoDestroyEffectList.Add(vo);
				}

				if (1 == effectCache.Count)
				{
					CreateInternalEffect(vo);
				}
			}
		}

        private void CreateInternalEffect(Effect vo)
        {
            try
            {
                curEffectVo = vo;
                curEffectUrl = vo.URL;                

                if (!resDictionary.ContainsKey(curEffectUrl))
                {
                    Log.info(this, "load effect asset " + curEffectUrl);
                    AssetBundleLoader loader = AssetManager.Instance.LoadAsset<GameObject>(curEffectUrl, EffectLoaded);
                }
                else
                {
                    EffectLoaded(resDictionary[curEffectUrl]);
				}
            }
            catch (Exception e)
            {
                Log.error(this, "CreateEffect error, exception is: " + e.Message);
            }
        }

        //删除特效对象
		public void RemoveEffect(string effectUrl)
		{
			if (null == effectUrl)
			{
				return;
			}

			if (effectDictionary.ContainsKey(effectUrl))
			{
 				GameObject.Destroy(effectDictionary[effectUrl]);
			}

			lock(effectDictionary)
			{
				effectDictionary.Remove(effectUrl);
			}

			foreach (Effect item in autoDestroyEffectList)
			{
				if ((null != item.DictKey) && item.DictKey.Equals(effectUrl))
				{
					autoDestroyEffectList.Remove(item);
					break;
				}
			}

			if (uiEffectRemain <= 0)
			{
				if (Singleton<EffectCameraMgr>.Instance.CameraEnabled)
				{
					Singleton<EffectCameraMgr>.Instance.CloseCamera();
				}
			}
        }   

		//UI特效剩余数量
		private int uiEffectRemain
		{
			get
			{
				int remain = 0;

				foreach (Effect item in autoDestroyEffectList)
				{
					if (item.UIEffect)
					{
						remain++;
					}
				}

				return remain;
			}
		}

		//剧情动画特效是否还在播放
		public bool IsStoryMovieEffectStopped(string effectId)
		{
			string effectUrl = UrlUtils.GetStoryMovieEffectUrl(effectId);
			return IsAutoDestroyEffectStopped(effectUrl);
		}

		//剧情场景特效是否还在播放
		public bool IsStorySceneEffectStopped(string effectId)
		{
			string effectUrl = UrlUtils.GetStorySceneEffectUrl(effectId);
			return IsAutoDestroyEffectStopped(effectUrl);
		}

		private bool IsAutoDestroyEffectStopped(string url)
		{
			foreach (Effect item in autoDestroyEffectList)
			{
				if (item.URL.Equals(url))
				{
					return false;
				}
			}

			return true;
		}

		//获得Hierachy特效对象
		private GameObject GetHierEffectObj(string baseEffectUrl, GameObject effectObj)
		{
			GameObject effectBase;
			GameObject effectGO;

			//如果特效实例还没创建或者正在播放中，则需要创建新实例
			if (!effectDictionary.ContainsKey(baseEffectUrl) || effectDictionary[baseEffectUrl].active)
			{
				if (!effectDictionary.ContainsKey(baseEffectUrl))
				{
					curEffectVo.DictKey = baseEffectUrl;
				}
				else
				{
					int index = 1;
					string effectKey = baseEffectUrl + "_" + index.ToString();
					while (effectDictionary.ContainsKey(effectKey) && effectDictionary[effectKey].active)
					{					    
                        index++;
                        if (index > 5)
                        {
                            //限制同一种特效最多同时存在5个
                            return null; 
                        }
					    effectKey = baseEffectUrl + "_" + index.ToString();
					}
					curEffectVo.DictKey = effectKey;
				}

				if (!effectDictionary.ContainsKey(curEffectVo.DictKey))
				{
				    if (curEffectVo.NeedCache)
				    {
				        effectBase = NGUITools.AddChild(curEffectRoot);
				    }
				    else
				    {
                        effectBase = NGUITools.AddChild(null);
				    }

				    effectBase.name = curEffectVo.DictKey;
					effectGO = GameObject.Instantiate(effectObj) as GameObject;
					effectGO.transform.parent = effectBase.transform;
					NGUITools.SetLayer(effectBase, LayerMask.NameToLayer(curLayer));

					if (!curEffectVo.AutoDestroy)
					{
						GameObject.DontDestroyOnLoad(effectBase);
					}

				    if (curEffectVo.NeedCache)
				    {
						lock(effectDictionary)
						{
                        	effectDictionary.Add(curEffectVo.DictKey, effectBase);
						}
				    }
				}
				else
				{
					effectBase = effectDictionary[curEffectVo.DictKey];
				}
			}
			else
			{
				curEffectVo.DictKey = baseEffectUrl;
				effectBase = effectDictionary[curEffectVo.DictKey];
			}

			return effectBase;
		}

        /// <summary>
        /// 判断两个物体是否在同一个格子,格子的宽度1，高度1
        /// </summary>
        /// <param name="pos1">物体1的位置</param>
        /// <param name="pos2">物体2的位置</param>
        /// <returns></returns>
	    private bool IsInTheSameGrid(Vector3 pos1, Vector3 pos2)
        {            
            var pos1GridX = (int)pos1.x;
            var pos1GridY = (int)pos1.y;
            var pos2GridX = (int)pos2.x;
            var pos2GridY = (int)pos2.y;
            
            return pos1GridX == pos2GridX && pos1GridY == pos2GridY;
        }

	    private string curLayer
		{
			get
			{
				if (curEffectVo.UIEffect)
				{
					return UI_EFFECT_LAYER;
				}
				else
				{
					return MAIN_EFFECT_LAYER;
				}
			}
		}

		private GameObject curEffectRoot
		{
			get
			{
				if (curEffectVo.UIEffect)
				{
					return uiEffectRoot;
				}
				else
				{
					return mainEffectRoot;
				}
			}
		}

		private void CreatePreloadEffectObj(string effectUrl, GameObject effectObj)
		{
			if (!effectDictionary.ContainsKey(effectUrl))
			{
				GameObject effectGO = GameObject.Instantiate(effectObj) as GameObject;
				GameObject effectBase = new GameObject(effectUrl);
				effectGO.transform.parent = effectBase.transform;
				NGUITools.SetLayer(effectBase, LayerMask.NameToLayer(MAIN_EFFECT_LAYER));
				effectBase.SetActive(false);
				GameObject.DontDestroyOnLoad(effectBase);
				effectBase.transform.parent = mainEffectRoot.transform;

				lock(effectDictionary)
				{
					effectDictionary.Add(effectUrl, effectBase);
				}
			}
		}

		public void ResetParticleSystem(GameObject effectObj)
		{
			ParticleSystem[] particles = effectObj.GetComponentsInChildren<ParticleSystem>();

			foreach (ParticleSystem item in particles)
			{
				item.Clear();
				item.Simulate(0, false, true);
				item.Play();
			}
		}

        //特效对象加载完后处理
        private void EffectLoaded(GameObject effectObj)
        {
            try
            {
                //将特效对象缓存
                if (!resDictionary.ContainsKey(curEffectUrl) && (null != effectObj) && curEffectVo.NeedCache)
                {
                    resDictionary.Add(curEffectUrl, effectObj);
                }

				//特效资源不存在处理
                if (null == effectObj)
				{
                    Log.error(this,"特效加载出错：特效url" + curEffectVo.URL);
					return;
				}

				//获得Hierachy面板特效对象
                GameObject realObj = effectObj;
				GameObject effectBase = GetHierEffectObj(curEffectUrl, realObj);

                if(null == effectBase) 
				{
					return;
				}

				effectBase.SetActive(false);

                //处理位置
                if (Directions.Left == curEffectVo.Direction)
                {
                    curEffectVo.Offset = new Vector3(curEffectVo.Offset.x * -1, curEffectVo.Offset.y, curEffectVo.Offset.z);
                }
                effectBase.transform.position = curEffectVo.BasePosition + curEffectVo.Offset;    

                //处理旋转
                Quaternion rotation = Quaternion.identity;
                rotation.eulerAngles = curEffectVo.EulerAngles;
                effectBase.transform.localRotation = rotation;

				//处理方向
				if (Directions.Left == curEffectVo.Direction)
				{
					effectBase.transform.localRotation = new Quaternion(0, 180, 0, 0);
				}

				//处理缩放
				if (curEffectVo.Zoom > 0f)
				{
					effectBase.transform.localScale = Vector3.one * curEffectVo.Zoom;
				}

                effectBase.SetActive(true);

				//创建成功后回调
				if (null != curEffectVo.CreatedCallback)
				{
					curEffectVo.CreatedCallback(effectBase);
				}

				//绑定脚本
				EffectControler controler = effectBase.GetComponent<EffectControler>();
				if (null == controler)
				{
                	controler = effectBase.AddComponent<EffectControler>();
				}
				else
				{
					controler.enabled = true;
				}
				controler.SetVo(curEffectVo);  

				//重置粒子系统
				ResetParticleSystem(effectBase);
            }
            catch (Exception e)
            {
                Log.error(this, "CreateEffect error, exception is: " + e.StackTrace + "Message is：" + e.Message);
            }
            finally 
            {
				SortEffectCachePlayed(curEffectVo);                
            }
        } 

		//当特效缓存列表完成一个特效播放后，再播放下一个
		private void SortEffectCachePlayed(Effect vo)
		{
			if (effectCache.Contains(vo))
			{
				lock(effectCache)
				{
					effectCache.Remove(vo);
				}
			}
			
			if (effectCache.Count > 0)
			{
				CreateInternalEffect(effectCache[0]);
			}
		}

		//获得特效GameObject
		private GameObject GetEffectGameObject(string url)
		{
			if (StringUtils.isEmpty(url))
			{
				return null;
			}	
			
			if (effectDictionary.ContainsKey(url))
			{
				return effectDictionary[url];
			}
			else
			{
				return null;
			}
		}

		//获得主相机特效对象
		public GameObject GetMainEffectGameObject(string effectId, string suffix = "")
		{
			string url = UrlUtils.GetMainEffectUrl(effectId) + suffix;			
			return GetEffectGameObject(url);
		}

		//获得技能特效对象
		public GameObject GetSkillEffectGameObject(string effectId)
		{
			string url = UrlUtils.GetSkillEffectUrl(effectId);			
			return GetEffectGameObject(url);
		}

		//获得UI特效对象
		public GameObject GetUIEffectGameObject(string effectId)
		{
			string url = UrlUtils.GetUIEffectUrl(effectId);			
			return GetEffectGameObject(url);
		}

		//获得剧情场景特效对象
		public GameObject GetStorySceneEffectGameObject(string effectId)
		{
			string url = UrlUtils.GetStorySceneEffectUrl(effectId);			
			return GetEffectGameObject(url);
		}

		//删除所有UI特效
		public void RemoveAllUIEffect()
		{
			for (int i=autoDestroyEffectList.Count-1; i>=0; i--)
			{
				Effect item = autoDestroyEffectList[i];

				if (item.UIEffect)
				{
					RemoveEffect(item.DictKey);
				}
			}
		}
    }
}
