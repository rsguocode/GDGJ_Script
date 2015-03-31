﻿﻿﻿using System.Collections.Generic;
using UnityEngine;

namespace com.game.utils
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Tools
    {

        /// <summary>
        /// 获取指定路径下的子游戏物体
        /// </summary>
        /// <param name="gameObject">父游戏物体</param>
        /// <param name="url">路径</param>
        /// <returns>若找到子游戏物体，则返回该子游戏物体，否则返回null</returns>
        public static GameObject find(GameObject gameObject, string url)
        {
            Transform mTrans = gameObject.transform.Find(url);
            if (null != mTrans)
            {
                return mTrans.gameObject;
            }

            return null;
        }

        /// <summary>
        /// 获取指定游戏物体的所有直接子游戏物体
        /// </summary>
        /// <param name="gameObject">父游戏物体</param>
        /// <returns></returns>
        public static List<GameObject> getChildren(GameObject gameObject)
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform mTrans in gameObject.transform)
            {
                GameObject child = mTrans.gameObject;
                children.Add(child);
            }

            return children;
        }

        /// <summary>
        /// 移除指定游戏物体的所有子游戏物体
        /// </summary>
        /// <param name="gameObject">父游戏物体</param>
        public static void clearChildren(GameObject gameObject)
        {
            List<GameObject> children = getChildren(gameObject);
            foreach (GameObject child in children)
            {
                NGUITools.Destroy(child);
            }
        }

        /// <summary>
        /// 获取指定游戏对象的所有活跃子游戏对象的数量
        /// </summary>
        /// <param name="gameObject">游戏对象</param>
        /// <returns>返回指定游戏对象的所有活跃子游戏对象的数量</returns>
        public static int getActiveChildCount(GameObject gameObject)
        {
            int count = 0;
            foreach (Transform mTrans in gameObject.transform)
            {
                if (NGUITools.GetActive(mTrans.gameObject)) count++;
            }

            return count;
        }

        /// <summary>
        /// 获取指定游戏物体的直接父游戏物体 
        /// </summary>
        /// <param name="gameObject">子游戏物体</param>
        /// <returns></returns>
        public static GameObject getParent(GameObject gameObject)
        {
            Transform parent = gameObject.transform.parent;

            return parent.gameObject;
        }

        /// <summary>
        /// 添加子游戏物体
        /// </summary>
        /// <param name="parent">父游戏物体</param>
        /// <param name="layer">层</param>
        /// <returns></returns>
        static public GameObject addChild(GameObject parent, float layer = 0f)
        {
            GameObject go = new GameObject();

            if (parent != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = new Vector3(0f, 0f, layer);
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        /// <summary>
        /// 添加子游戏物体 
        /// </summary>
        /// <param name="parent">父游戏物体</param>
        /// <param name="prefab">预设的游戏物体</param>
        /// <param name="layer">层</param>
        /// <returns></returns>
        public static GameObject addChild(GameObject parent, GameObject prefab, float layer = 0f)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;

            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = new Vector3(0f, 0f, layer);
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary>
        /// 递归设置层
        /// </summary>
        /// <param name="parent">父对象</param>
        /// <param name="layer">层</param>
        public static void setLayerRecursively(GameObject parent, int layer)
        {
            if (null == parent) return;

            parent.layer = layer;
            foreach (Transform child in parent.transform)
            {
                if (null == child) continue;
                setLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
