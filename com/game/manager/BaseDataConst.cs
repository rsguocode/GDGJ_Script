using System;
using System.Collections.Generic;
using com.game.data;

namespace com.game.manager
{
    public class BaseDataConst
    {
        public const String SYS_BUFF_VO = "SysBuffVo"; //Buff表
        public const String SYS_DAILY_BUFF_VO = "SysDailyBuffVo"; //日常Buff表
        public const String SYS_DAILY_DESC_VO = "SysDailyDescVo"; //日常活动描述
        public const String SYS_DEED_VO = "SysDeedVo"; //契约表
        public const String SYS_DUNGEON_HELP_VO = "SysDungeonHelpVo"; //
        public const String SYS_EFFECT_VO = "SysEffectVo"; //特效表
        public const String SYS_EQUIP_VO = "SysEquipVo"; //装备表
        public const String SYS_ITEM_VO = "SysItemVo"; //物品表
        public const String SYS_MAP_VO = "SysMapVo"; //场景表
        public const String SYS_MEDIA_GIFT_VO = "SysMediaGiftVo"; //媒体礼包
        public const String SYS_MONSTER_AI_VO = "SysMonsterAiVo"; //怪物AI
        public const String SYS_MONSTER_VO = "SysMonsterVo"; //怪物表
        public const String SYS_SKILL_ACTION_VO = "SysSkillActionVo"; //技能动作表
        public const String SYS_SKILL_BASE_VO = "SysSkillBaseVo"; //技能表	   
        public const String SYS_TRANS_POS_VO = "SysTransPosVo"; //传送点表
        public const String SYS_JOB_DAMAGE_RATIO = "SysJobDamageRatioVo"; //职业伤害系数
        public const String Sys_READY_LOAD_VO = "SysReadyLoadVo"; //资源预加载表
        public const String SYS_DUNGEON_TREE_VO = "SysDungeonTreeVo"; //副本表
        public const String SYS_DUNGEON_REWARD_VO = "SysDungeonRewardVo"; //副本奖励
		public const String SYS_DUNGEON_MON = "SysDungeonMon"; //怪物预加载
        public const String SYS_ERROR_CODE_VO = "SysErrorCodeVo"; //错误代号表
        public const String SYS_NPC_VO = "SysNpcVo"; //NPC表
        public const String SYS_STORY_CHAPTER_VO = "SysStoryChapterVo"; //剧情章节表
        public const String SYS_STORY_BG_VO = "SysStoryBgVo"; //剧情背景表
        public const String SYS_STORY_TALKLIST_VO = "SysStoryTalkListVo"; //剧情对话表
        public const String SYS_VIP_MALL_VO = "SysVipMallVo"; //商城数据表
        public const String SYS_PRICE_VO = "SysPriceVo"; //价格表
        public const String SYS_EXCHANGE_VO = "SysExchangeVo"; //价格表
        public const String SYS_TASK_VO = "SysTask"; //任务表
		public const String SYS_RUMOR_VO = "SysRumor"; //传闻表
		public const String SYS_PET_VO = "SysPet"; //宠物表
		public const String SYS_PET_BOOK_VO = "SysPetBook"; //宠物图鉴表
		public const String SYS_PET_BOOK_SKILL_VO = "SysPetBookSkill"; //技能图鉴表
		public const String SYS_TREASURE_VO = "SysTreasure"; //地宫寻宝表
        public const String SYS_TRAP_VO = "SysTrap"; //陷阱表
		public const String SYS_FOREST_VO = "SysForest"; //冒险森林表
        public const String SYS_GIFT_VO = "SysGiftVo";  //登陆礼包
        public const String SYS_GUIDE_VO = "SysGuideVo";  //登陆礼包
        public const String SYS_VIP_INFO = "SysVipInfoVo";  //VIP信息描述
		public const String SYS_SEED_MALL_VO = "SysSeedMallVo";  //种子商店配表
		public const String SYS_SEED_VO = "SysSeedVo";  //种子信息配表
        public const String SYS_ROLE_BASE_INFO_VO = "SysRoleBaseInfoVo";  //角色基础信息配表
		public const String SYS_VIP_DROIT_VO = "SysVipDroitVo";  //VIP特权信息表
        public const String SYS_MEDAL_VO = "SysMedalVo";   //勋章表
        public const String SYS_MONSTER_ADAPTER_RULE_VO = "SysMonsterAdaptRuleVo";    //怪物自适应的表
		public const String SYS_DAEMONISLAND_VO = "SysDaemonislandVo";    //怪物自适应的表
		public const String SYS_SIGN_VO = "SysSignVo";  //签到奖励表

        public static Dictionary<string, Type> registerClzType()
        {
            var clzTypeData = new Dictionary<string, Type>();

            clzTypeData.Add(SYS_BUFF_VO, typeof (SysBuffVo));
            clzTypeData.Add(SYS_DAILY_BUFF_VO, typeof (SysDailyBuffVo));
            clzTypeData.Add(SYS_DAILY_DESC_VO, typeof (SysDailyDescVo));
            clzTypeData.Add(SYS_DEED_VO, typeof (SysDeedVo));
            clzTypeData.Add(SYS_DUNGEON_HELP_VO, typeof (SysDungeonHelpVo));
            clzTypeData.Add(SYS_EFFECT_VO, typeof (SysEffectVo));
            clzTypeData.Add(SYS_EQUIP_VO, typeof (SysEquipVo));
            clzTypeData.Add(SYS_ITEM_VO, typeof (SysItemVo));
            clzTypeData.Add(SYS_MAP_VO, typeof (SysMapVo));
            clzTypeData.Add(SYS_MEDIA_GIFT_VO, typeof (SysMediaGiftVo));
            clzTypeData.Add(SYS_MONSTER_AI_VO, typeof (SysMonsterAiVo));
            clzTypeData.Add(SYS_MONSTER_VO, typeof (SysMonsterVo));
            clzTypeData.Add(SYS_SKILL_ACTION_VO, typeof (SysSkillActionVo));
            clzTypeData.Add(SYS_SKILL_BASE_VO, typeof (SysSkillBaseVo));
            clzTypeData.Add(SYS_TRANS_POS_VO, typeof (SysTransPosVo));
            clzTypeData.Add(SYS_JOB_DAMAGE_RATIO, typeof (SysJobDamageRatioVo));
            clzTypeData.Add(Sys_READY_LOAD_VO, typeof (SysReadyLoadVo));
            clzTypeData.Add(SYS_DUNGEON_TREE_VO, typeof (SysDungeonTreeVo));
            clzTypeData.Add(SYS_DUNGEON_REWARD_VO, typeof (SysDungeonRewardVo));
            clzTypeData.Add(SYS_ERROR_CODE_VO, typeof (SysErrorCodeVo));
            clzTypeData.Add(SYS_NPC_VO, typeof (SysNpcVo));
            clzTypeData.Add(SYS_STORY_CHAPTER_VO, typeof (SysStoryChapterVo));
            clzTypeData.Add(SYS_STORY_BG_VO, typeof (SysStoryBgVo));
            clzTypeData.Add(SYS_STORY_TALKLIST_VO, typeof (SysStoryTalkListVo));
            clzTypeData.Add(SYS_VIP_MALL_VO, typeof (SysVipMallVo));
            clzTypeData.Add(SYS_PRICE_VO, typeof (SysPriceVo));
            clzTypeData.Add(SYS_EXCHANGE_VO, typeof (SysExchangeVo));
            clzTypeData.Add(SYS_TASK_VO, typeof (SysTask));
			clzTypeData.Add(SYS_RUMOR_VO, typeof (SysRumor));
			clzTypeData.Add(SYS_PET_VO, typeof (SysPet));
			clzTypeData.Add(SYS_PET_BOOK_VO, typeof (SysPetBook));
			clzTypeData.Add(SYS_PET_BOOK_SKILL_VO, typeof (SysPetBookSkill));
			clzTypeData.Add(SYS_TREASURE_VO, typeof (SysTreasure));
			clzTypeData.Add(SYS_FOREST_VO, typeof (SysForest));
			clzTypeData.Add(SYS_DUNGEON_MON, typeof (SysDungeonMon));
            clzTypeData.Add(SYS_TRAP_VO, typeof(SysTrap));
            clzTypeData.Add(SYS_GIFT_VO, typeof(SysGiftVo));
            clzTypeData.Add(SYS_VIP_INFO, typeof(SysVipInfoVo));
			clzTypeData.Add(SYS_SEED_VO, typeof(SysSeedVo));
			clzTypeData.Add(SYS_SEED_MALL_VO, typeof(SysSeedMallVo));
            clzTypeData.Add(SYS_ROLE_BASE_INFO_VO, typeof(SysRoleBaseInfoVo));
			clzTypeData.Add(SYS_VIP_DROIT_VO, typeof(SysVipDroitVo));
            clzTypeData.Add(SYS_MEDAL_VO, typeof(SysMedalVo));
            clzTypeData.Add(SYS_MONSTER_ADAPTER_RULE_VO, typeof(SysMonsterAdaptRuleVo));
			clzTypeData.Add(SYS_DAEMONISLAND_VO, typeof(SysDaemonislandVo));
            clzTypeData.Add(SYS_GUIDE_VO, typeof(SysGuideVo));
			clzTypeData.Add(SYS_SIGN_VO,typeof(SysSignVo));
            return clzTypeData;
        }
    }
}