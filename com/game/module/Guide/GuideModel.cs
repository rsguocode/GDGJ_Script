﻿﻿﻿
using com.game.module.test;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/21 09:36:26 
 * function: 指引数据
 * *******************************************************/

namespace com.game.module.Guide
{
    public class GuideModel : BaseMode<GuideModel>
    {
        public const int ShowGuideStatuUpdate = 1;
        private bool _isShowGuide;

        /// <summary>
        /// 是否在显示指引
        /// </summary>
        public bool IsShowGuide
        {
            get { return _isShowGuide; }
            set
            {
                _isShowGuide = value;
                DataUpdate(ShowGuideStatuUpdate);
            }
        }


    }
}