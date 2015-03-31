﻿﻿﻿using UnityEngine;

namespace com.game.module
{
    /// <summary>
    /// 自定义视图树。
    /// </summary>
    [AddComponentMenu("NGUI/UI/ViewTree")]
    public class ViewTree : MonoBehaviour
    {
        static public GameObject go;
        public static GameObject battle;
        public static GameObject city;
        void Awake()
        {
            go = gameObject;
        }

        public static void SetSubObj()
        {
            if (!ReferenceEquals(go,null))
            {
                battle = go.transform.FindChild("battle(Clone)").gameObject;
                city = go.transform.FindChild("city(Clone)").gameObject;
            }
        }
    }
}
