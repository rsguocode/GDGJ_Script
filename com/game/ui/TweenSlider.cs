﻿﻿﻿using System;
using UnityEngine;

namespace com.game.ui
{
    /// <summary>
    /// 滚动进度条
    /// </summary>
    [AddComponentMenu("NGUI/Tween/Slider")]
    public class TweenSlider : UITweener
    {
        public float from = 0f;
        public float to = 0f;

        public float sliderValue
        {
            get
            {
                UISlider slider = gameObject.GetComponent<UISlider>();
                if (slider == null) Destroy(this);
                return slider.sliderValue;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            gameObject.GetComponent<UISlider>().sliderValue = from * (1f - factor) + to * factor;
        }

        public static TweenSlider Begin(GameObject go, float duration, float value, Style style = Style.Once)
        {
            TweenSlider comp = UITweener.Begin<TweenSlider>(go, duration);

            float sliderValue = comp.sliderValue;
            switch (style)
            {
                case Style.Once:
                    break;
                case Style.Loop:
                    if (value < sliderValue) sliderValue = 0f;
                    break;
                case Style.PingPong:
                    if (value > sliderValue) sliderValue = 1f;
                    break;
            }

            comp.from = sliderValue;
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
