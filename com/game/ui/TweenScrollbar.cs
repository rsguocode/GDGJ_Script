﻿﻿﻿using System;
using UnityEngine;

namespace com.game.ui
{
    /// <summary>
    /// 滚动滚动条
    /// </summary>
    [AddComponentMenu("NGUI/Tween/Slider")]
    public class TweenScrollbar : UITweener
    {
        public float from = 0f;
        public float to = 0f;

        public float scrollValue
        {
            get
            {
                UIScrollBar scrollbar = gameObject.GetComponent<UIScrollBar>();
                if (scrollbar == null) Destroy(this);
                return scrollbar.scrollValue;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            gameObject.GetComponent<UIScrollBar>().scrollValue = from * (1f - factor) + to * factor;
        }

        public static TweenScrollbar Begin(GameObject go, float duration, float value, Style style = Style.Once)
        {
            TweenScrollbar comp = UITweener.Begin<TweenScrollbar>(go, duration);

            float scrollValue = comp.scrollValue;
            switch (style)
            {
                case Style.Once:
                    break;
                case Style.Loop:
                    if (value < scrollValue) scrollValue = 0f;
                    break;
                case Style.PingPong:
                    if (value > scrollValue) scrollValue = 1f;
                    break;
            }

            comp.from = scrollValue;
            comp.to = value;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }
    }
}
