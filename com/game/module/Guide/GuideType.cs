﻿﻿
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/22 03:25:59 
 * function: 指引类型枚举
 * *******************************************************/

namespace com.game.module.Guide
{
    public class GuideType
    {
        public const int GuideEquip = 111;              //穿戴装备
        public const int GuideGrow = 112;               //指引培育
        public const int GuideMedal = 113;              //指引勋章
        public const int GuideSkillOpen = 120;             //技能开启加第二个技能学习
        public const int GuideSkill3Learn = 121;           //指引第三个技能学习
        public const int GuideSkill4Learn = 122;           //指引第四个技能学习
        public const int GuideSkillRollLearn = 123;        //瞬移技能学习
        public const int GuideForgeOpen = 130;             //锻造
        public const int GuideEquipRefine = 131;           //装备精炼
        public const int GuideEquipInlay = 132;            //装备镶嵌
        public const int GuideEquipMerge = 133;            //装备充灵
        public const int GuidePetOpen = 140;               //宠物
        public const int GuidePetEquip = 141;              //宠物装备
        public const int GuidePetSkill = 142;              //宠物技能
        public const int GuideMedalOpen = 113;             //勋章
        public const int GuideGuildOpen = 150;             //公会
        public const int GuideGoldSilverIslandOpen = 210;  //金银岛
        public const int GuideGoldHitOpen = 220;           //击石成金
        public const int GuideArenaOpen = 230;             //英雄榜
        public const int GuideDaemonIslandOpen = 240;      //恶魔岛
        public const int GuideShopOpen = 310;             //商城指引
        public const int GuideLuckDraw = 320;             //萌宠献礼
        public const int GuideGoldBoxOpen = 330;          //黄金宝箱
        public const int GuideWorldBoss = 340;            //世界BOSS
        public const int GuideCopy = 0;                   //副本指引，特殊指引，不走配表     

    }
}