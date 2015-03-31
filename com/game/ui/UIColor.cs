﻿﻿﻿using System;
using UnityEngine;

namespace com.game.ui
{
    /// <summary>
    /// 颜色
    /// </summary>
    public class UIColor : MonoBehaviour
    {
        protected UISprite sprite;

        void Awake()
        {
            sprite = GetComponent<UISprite>();
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="color">颜色</param>
        public void setColor(object color)
        {
            Type type = color.GetType();

            if (type == typeof(String))
            {
                sprite.color = NGUITools.ParseColor((string)color, 0);
            }
            else if (type == typeof(Color))
            {
                sprite.color = (Color)color;
            }
        }

    }
}
