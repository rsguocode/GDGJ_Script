﻿//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：AttrStatus;
//文件描述：属性状态表;
//创建者：潘振峰;
//创建日期：2014/6/21 14:33:54;
//////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.u3d.bases.consts
{
    public class AttrStatus
    {
        private static readonly int[,] AttrStatusChangeMatrix =
        {
                //同右边
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},//死亡;
                {1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1},//待机;
                {1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1},//移动;
                {1,0,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1},//攻击;
                {1,0,0,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1},//技能;
                {1,0,0,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1},//受击僵直;
                {1,0,0,0,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1},//击倒;
                {1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1},//抓取;
                {1,0,0,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1},//被抓取;
                {1,0,0,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,1},//击飞;
                {1,0,0,1,1,0,0,1,0,0,1,1,1,0,0,0,0,0,0},//闪避;
                {1,1,1,1,1,0,0,1,0,0,1,1,1,1,1,1,1,1,1},//霸体;
                {1,1,1,1,1,0,0,1,0,0,1,0,1,0,0,0,0,0,0},//无敌;
                {1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},//昏迷;
                {1,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1},//定身;
                {1,0,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1},//缴械;
                {1,0,0,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,1},//恐惧;
                {1,0,1,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,1},//变形;
                {1,0,0,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,1}//致盲;
        };
    }
}
