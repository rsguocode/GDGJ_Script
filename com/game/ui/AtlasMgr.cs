﻿﻿﻿using UnityEngine;
using System;
using com.u3d.bases.loader;

namespace com.game.ui
{
    public class AtlasMgr
    {
        protected string dir;
        protected int count;
        protected int increment;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dir">预设目录</param>
        /// <param name="count">每个图集包含多少个精灵</param>
        /// <param name="increment">增量</param>
        public AtlasMgr(string dir, int count, int increment = 0)
        {
            this.dir = dir;
            this.count = count;
            this.increment = increment;
        }

        /// <summary>
        /// 获取图集
        /// </summary>
        /// <param name="name">图片名称</param>
        /// <returns>图集</returns>
        public UIAtlas getAtlas(int id)
        {
            int n = (id - increment) / count;
            UnityEngine.Object obj = ResMgr.instance.load(String.Format("{0}/{1}", dir, n), typeof(UIAtlas));
            return (obj != null) ? obj as UIAtlas : null;
        }
    }
}
