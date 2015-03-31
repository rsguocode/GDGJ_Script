﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**资源类型**/
namespace com.u3d.bases.loader.consts
{
    public class ResType
    {
        public const int XML       = 1000;      //XML文件
        public const int TXT       = 2000;      //TXT文件
        public const int IMGAE     = 3000;      //材质贴图
        public const int SOUND     = 4000;      //声音文件
        public const int ZIP       = 5000;      //zip文件
        public const int DATAVO    = 6000;      //VO数据
        public const int BINARY    = 7000;      //二进制流
        public const int ANIMATION = 8000;      //模型动画

        //public const int RES_LOAD  = 500;       //Resources.Load()加载文件
        //public const int SCENE     = 10000;     //场景文件

    }
}
