//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SoundId
//文件描述：
//创建者：黄江军
//创建日期：2014-02-18
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace com.game.sound
{
	public class SoundId 
	{	
		//UI音效		
		public const string Sound_Teleport          = "3001";    //主城_传送进入副本
		public const string Sound_StoneEmbed        = "3002";    //宝石_镶嵌、强化成功
		public const string Sound_StoreSumption     = "3003";    //商城_消费成功
		public const string Sound_ConfirmOk         = "3004";    //通用_确定
		public const string Sound_ConfirmClose      = "3005";    //窗口回退关闭
		public const string Sound_ConfirmSel        = "3006";    //通用_选择
		public const string Sound_EatDiam1          = "3007";    //玩法_吃到钻石1
		public const string Sound_EatDiam2          = "3008";    //玩法_吃到钻石2
		public const string Sound_OpenGoldBox       = "3009";    //玩法_开启宝箱
		public const string Sound_ButtonFold        = "3010";    //主城_折叠按钮
		public const string Sound_WearArmor         = "3011";    //装备_穿戴护甲
		public const string Sound_WearWeapon        = "3012";    //装备_穿戴武器
		public const string Sound_StreassEquip      = "3013";    //装备_强化成功
		public const string Sound_BattleWin         = "3014";    //战斗胜利音乐
		public const string Sound_Login             = "3015";    //进入游戏按钮（账号密码）
		public const string Sound_ReceieTask        = "3016";    //接受任务
		public const string Sound_FinishTask        = "3017";    //完成任务
		public const string Sound_CopySelect        = "3018";    //选择具体的副本
		public const string Sound_CopyChallenge     = "3019";    //副本挑战按钮

		//技能表音效
		public const string Sound_SwordSkill4       = "60043";   //剑士技能4音效 
		public const string Sound_SwordSkill3Bullet = "60631";   //剑士技能3子弹音效 
		public const string Sound_MagicWin          = "77001";   //法师胜利欢呼 

		//场景音乐
		public const string Music_EnterGame         = "1000";    //进入游戏前的音乐
		public const string Music_PrepareBattle     = "1006";    //副本备战音乐
		public const string Music_StartMovie        = "1017";    //片头音乐
		
	}
}
