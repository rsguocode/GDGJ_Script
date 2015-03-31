﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.game.manager
{   
	public enum DownLoadState
	{
        Init = 0,//初始化
       	Loading = 1,//下载中
        Loaded = 2,//下载或加载完成
        Stored = 3,//本地存储完成
        LoadFailure = 4,//加载失败
		StoreFailure = 5,//存储失败
        Cached = 6//缓存的
	}
}
