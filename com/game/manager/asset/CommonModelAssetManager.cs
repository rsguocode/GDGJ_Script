﻿﻿﻿
using System.ComponentModel.Design.Serialization;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/02/18 03:47:59 
 * function: 模型公用资源管理
 * *******************************************************/

namespace com.game.manager
{
    public class CommonModelAssetManager
    {
        private const string Url = "Common/Model/ModelCommon.assetbundle";
        public static CommonModelAssetManager Instance = new CommonModelAssetManager();
        private GameObject _commonGameObject;
        private GameObject _footShadowGameObject;

        private CommonModelAssetManager() { }

        public void Init()
        {
            AssetManager.Instance.LoadAsset<GameObject>(Url, LoadCommonModelCallback);
        }


        private void LoadCommonModelCallback(GameObject commonGameObject)
        {
            _commonGameObject = commonGameObject;
        }

        public GameObject GetFootShadowGameObject()
        {
            if (_footShadowGameObject == null)
            {
                _footShadowGameObject = _commonGameObject.transform.FindChild("FootShadow").gameObject;
            }
            return _footShadowGameObject;
        }

    }
}