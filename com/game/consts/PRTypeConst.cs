﻿﻿using System;

//预加载资源类型定义
namespace com.game.consts
{
    public class PRTypeConst
    {
        public const int MT_SCENE = 1;  //场景切换
        public const int MT_TASK = 2;  //任务切换
        public const int MT_MODULE = 3;  //模块加载

        public const int ST_SKILL = 1;  //子类别，特效
        public const int ST_MODULE = 2;  //子类别，模块
        public const int ST_MUL = 3;  //场景特效
        public const int ST_ICON = 4;  //特殊：功能指引图片
        public const int ST_SOUND = 5;  //场景音效
    }
}

