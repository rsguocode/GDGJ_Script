/**
 * 怪物调整表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysMonsterAdaptRuleVo
    {
    public uint unikey; //唯一key
    public int lvl; //角色等级
    public int hp_ratio; //生命值
    public int att_p_min_ratio; //最小物攻
    public int att_p_max_ratio; //最大物攻
    public int att_m_min_ratio; //最小魔攻
    public int att_m_max_ratio; //最大魔攻
    public int def_p_ratio; //物理防御力
    public int def_m_ratio; //魔法防御力
    public int hit_ratio; //命中等级
    public int dodge_ratio; //闪避等级
    public int crit_lvl_ratio; //暴击等级
    public int crit_hurt_ratio; //暴击伤害
    public int flex_ratio; //韧性
    public int hurt_re_ratio; //伤害减免
    public int ice_attr_ratio; //冰攻
    public int fir_attr_ratio; //火攻
    public int thunder_attr_ratio; //雷攻
    public int light_attr_ratio; //光攻
    public int dark_attr_ratio; //暗攻
    public int ice_def_ratio; //冰防
    public int fir_def_ratio; //火防
    public int thunder_def_ratio; //雷防
    public int light_def_ratio; //光防
    public int dark_def_ratio; //暗防
    public int exp_ratio; //经验系数
    public int gold_ratio; //银币系数
    }
}
