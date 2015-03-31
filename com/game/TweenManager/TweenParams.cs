using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.TweenManager
{

    public class TweenParams<T, V> where T: UITweener
    {
        /// <summary>
        /// 动画脚本类型
        /// </summary>
        public T TweenType { get; set; }
        /// <summary>
        /// 动画开始值
        /// </summary>
        public V FromValue { get; set; }
        /// <summary>
        /// 动画结束值
        /// </summary>
        public V ToValue { get; set; }
        /// <summary>
        /// 动画组
        /// </summary>
        public int GroupIndex { get; set; }
        /// <summary>
        /// 动画延时时间
        /// </summary>
        public float Delay { get; set; }
        /// <summary>
        /// 动画时间
        /// </summary>
        public float Duration { get; set; }
        /// <summary>
        /// 动画缓动类型
        /// </summary>
        public UITweener.Method Method { get; set; }
        /// <summary>
        /// 动画脚本挂载GameObject
        /// </summary>
        public GameObject Target { get; set; }
        /// <summary>
        /// 动画结束回调
        /// </summary>
        public List<EventDelegate> OnFinished = new List<EventDelegate>();

        /*public TweenParams(GameObject target, T type, float duration)
        {
            this.Target = target;
            this.TweenType = type;
            this.Duration = duration;
        }
        public TweenParams(GameObject target, T type, float duration, V from, V to, int group = 0,
            float delay = 0f, UITweener.Method method = UITweener.Method.Linear)
            : this(target, type, duration)
        {
            this.FromValue = from;
            this.ToValue = to;
            this.GroupIndex = group;
            this.Delay = delay;
            this.Method = method;

        }*/
    }
}
