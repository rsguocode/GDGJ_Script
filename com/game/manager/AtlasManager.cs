using System;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.game.manager;
using com.u3d.bases.debug;

namespace Com.Game.Module.Manager
{
    public delegate void LoadFinished();

    public delegate void LoadCallBack(UIAtlas atlas);
	public class AtlasUrl
	{
	    public const string EquipIconHold = "UI/Icon/EquipIcon/EquipIconHold.assetbundle";
	    public const string EquipIconNormal = "EquipIcon";
	    public const string EquipIconGray = "EquipIcon_Gray";

        public const string SkillIconHold = "UI/Icon/SkillIcon/SkillIconHold.assetbundle";
        public const string SkillIconNormal = "SkillIcon";
        public const string SkillIconGray = "SkillIcon_Gray";

        public const string PropIconURL = "UI/Icon/PropIcon/PropIcon.assetbundle";
        public const string PropIcon = "PropIcon";

		public const string GemIconURL = "UI/Icon/GemIcon/GemIcon.assetbundle";
		public const string GemIcon = "GemIcon";

		public const string CopyIconHold = "UI/Icon/CopyMapIcon/CopyIconHold.assetbundle";
		public const string CopyIconNormal = "CopyIconAtlas";
		public const string CopyIconGray = "CopyIconGrayAtlas";

        public const string HeaderIconURL = "UI/Icon/Header/Header.assetbundle";
        public const string HeaderIcon = "Header";
	}
    public class AtlasManager : Singleton<AtlasManager>
    {
        public const string Header = "Header";
        public const string Common = "Common";
		public const string Chat = "ChatAtlas";

        public LoadFinished LoadFinishedCallBack;

        private LoadCallBack loadCallBack;

        private bool isCache;
		private int cacheLength = 5;
        private Dictionary<string, UIAtlas> atlasDic = new Dictionary<string, UIAtlas>();


        //初始化加载
        public void Init()
        {
            //加载Icon图集
            //LoadAtlas("UI/Icon/EquipIcon/EquipIcon.assetbundle", "EquipIcon");
            
            LoadAtlasHold(AtlasUrl.EquipIconHold, AtlasUrl.EquipIconNormal,null,true);
			LoadAtlasHold(AtlasUrl.SkillIconHold, AtlasUrl.SkillIconNormal, null, true);
			LoadAtlasHold(AtlasUrl.CopyIconHold, AtlasUrl.CopyIconNormal, null, true);

            LoadAtlas("UI/Icon/PropIcon/PropIcon.assetbundle", "PropIcon");  //道具图标
            //LoadAtlas("UI/Icon/SmeltIcon/SmeltIcon.assetbundle", "SmeltIcon");
			LoadAtlas("UI/Icon/GemIcon/GemIcon.assetbundle" , "GemIcon");//宝石图标
			LoadAtlas("UI/Icon/MonsterHeadIcon/MonsterHeadAtlas.assetbundle", "MonsterHeadAtlas");  //怪物头像图标
            LoadAtlas("UI/Common/common.assetbundle", "common");
            LoadAtlas("UI/Icon/Header/Header.assetbundle", AtlasManager.Header);
			LoadAtlas("UI/Icon/ChatIcon/ChatAtlas.assetbundle", AtlasManager.Chat);
        }


        [System.Obsolete(
            "Use 'LoadAtlas(string url, string name, LoadCallBack loadCallBack, bool isCache = false)' instead")]
        public void LoadAtlas(string url, string name)
        {
            AssetManager.Instance.LoadAsset<GameObject>(url, LoadAtlasCallBack, name);
        }

        private void LoadAtlasCallBack(GameObject asset)
        {
            //Log.info(this,"add atlas name : "+ asset.name);
            if (!atlasDic.ContainsKey(asset.name))
            {
                atlasDic.Add(asset.name, asset.GetComponent<UIAtlas>());
            }
            if (null != LoadFinishedCallBack)
            {
                LoadFinishedCallBack();
            }
        }
        /// <summary>
        /// 加载图集
        /// </summary>
        /// <param name="url">图集路径</param>
        /// <param name="name">图集的名称</param>
        /// <param name="loadCallBack">图集加载完成回调</param>
        /// <param name="isCache">是否缓存，默认不缓存</param>
        public void LoadAtlas(string url, string name, LoadCallBack loadCallBack, bool isCache = false)
        {
            this.loadCallBack = loadCallBack;
            this.isCache = isCache;
			if(atlasDic.ContainsKey(name))
			{
				loadCallBack(atlasDic[name]);
			}
			else
            	AssetManager.Instance.LoadAsset<GameObject>(url, LoadAtlasCallBack1, name);
        }

        public void LoadAtlasHold(string holdUrl,string normalName, LoadCallBack loadCallBack, bool isCache = false)
        {
            this.loadCallBack = loadCallBack;
            this.isCache = isCache;
            if (atlasDic.ContainsKey(normalName))
            {
                loadCallBack(atlasDic[normalName]);
            }
            else
                AssetManager.Instance.LoadAsset<GameObject>(holdUrl, LoadAtlasHoldCallBack);
        }

        private void LoadAtlasHoldCallBack(GameObject asset)
        {
            UISprite sprite = asset.GetComponent<UISprite>();
            UIAtlas normalAtlas = sprite.normalAtlas;
            UIAtlas grayAtlas = sprite.grayAtlas;

            if (grayAtlas != null)
            {
				atlasDic.Add(grayAtlas.name, grayAtlas);
//        		atlasDic.Add(asset.name.Replace("Hold","")+"_Gray", grayAtlas);
            }

            if (normalAtlas != null)
            {
				atlasDic.Add(normalAtlas.name, normalAtlas);
//                atlasDic.Add(asset.name.Replace("Hold", ""), normalAtlas);

                if (loadCallBack != null)
                    loadCallBack(normalAtlas);
            }

        }

        private void LoadAtlasCallBack1(GameObject asset)
        {
            UIAtlas atlas = asset.GetComponent<UIAtlas>();
            if (atlas != null)
            {
                if(isCache)
				{

                    atlasDic.Add(asset.name, asset.GetComponent<UIAtlas>());
				}

            }
            
        }

        public void AddAtlas(string name, UIAtlas atlas)
        {
            atlasDic.Add(name, atlas);
        }
		public void RemoveAtlas(string name )
		{
			atlasDic.Remove(name);
		}
        public UIAtlas GetAtlas(string name)
        {
            UIAtlas atlas;
            if (string.IsNullOrEmpty(name))
                return null;
            if (!atlasDic.TryGetValue(name, out atlas))
            {
                Log.info(this, "Can't find in Atlas Cache , Please Load it first!");
                //UILabel label;
                //label.pivot
            }
            return atlas;
        }

    }
}
