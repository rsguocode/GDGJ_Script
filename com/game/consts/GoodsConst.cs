//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：GoodsConst
//文件描述：物品枚举
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace com.game.consts
{
    public class GoodsConst
    {
       
        //大类
        public const int TYPE_COMMON = 0;//普通装备
        public const int TYPE_PET    = 3;//宠物装备

        //小类
        public const int SUBTYPE_COMMON = 0;

        //部位
        public const int POS_WEAPON   = 1;//武器
        public const int POS_HELMET   = 2;//头盔
        public const int POS_CLOTHE   = 3;//衣服
        public const int POS_TROUSERS = 4;//裤子
        public const int POS_SHOE     = 5;//鞋子
        public const int POS_NECKLACE = 6;//项链
        public const int POS_BRACELET = 7;//手环
        public const int POS_RING     = 8;//戒指
        public const int POS_GLOVE    = 9;//手套
        public const int POS_BAGGES   = 10;//徽章


        //品质
        public const int WHITE  = 1;//白
        public const int GREEN  = 2;//绿
        public const int BLUE   = 3;//蓝
        public const int PURPLE = 4;//紫
        public const int ORANGE = 5;//橙
        public const int RED    = 6;//红


        public const int ATTR_ID_STR = 1;     //力
        public const int ATTR_ID_AGI = 2;     //敏
        public const int ATTR_ID_PHY = 3;     //体
        public const int ATTR_ID_WIT = 4;     //智
        public const int ATTR_ID_HP = 5;      //生命
        public const int ATTR_ID_MP = 6;         //魔法
        public const int ATTR_ID_ATT_P_MIN = 7;   //最小物功
        public const int ATTR_ID_ATT_P_MAX = 8;   //最大物功
        public const int ATTR_ID_ATT_M_MIN = 9;   //最小魔功
        public const int ATTR_ID_ATT_M_MAX = 10;   //最大魔功
        public const int ATTR_ID_ATT_DEF_P = 11;   //物防
        public const int ATTR_ID_ATT_DEF_M = 12;   //魔防
        public const int ATTR_ID_HIT = 13;         //命中
        public const int ATTR_ID_DODGE = 14;       //闪避
        public const int ATTR_ID_CRIT = 15;        //暴击
        public const int ATTR_ID_CRIT_RATIO = 16;  //暴击伤害比例
        public const int ATTR_ID_FLEX = 17;        //韧性
        public const int ATTR_ID_HURT_RE = 18;     //格挡
        public const int ATTR_ID_SPEED = 19;       //速度
        public const int ATTR_ID_LUCK = 20;        //幸运值
        public const int ATTR_ID_ATT_MIN = 21;     //最小攻击
        public const int ATTR_ID_ATT_MAX = 22;   //最大攻击  (人物攻击)
       
        
        //装备表分类
        public const int PET_EQUIP = 3;//宠物装备
        public const int ROLE_EQUIP = 0;//普通装备

        //物品表分类
        public const int SMELT_GOODS = 3;//宝石类别
        public const int PET_GOODS = 5;//宠物


    }
}
