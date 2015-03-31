﻿﻿﻿using UnityEngine;

namespace com.game.module
{
    /// <summary>
    /// 自定义视图树。
    /// </summary>
    [AddComponentMenu("NGUI/UI/Viewport")]
    public class Viewport : MonoBehaviour
    {
        static public GameObject go;

        void Awake()
        {
            go = gameObject;
        }
    }
}
