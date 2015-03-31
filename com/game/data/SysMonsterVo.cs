/**
 * 怪物配置(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysMonsterVo
    {
    public uint unikey; //唯一key
    public int id; //怪物编号
    public int res; //模型
    public int icon; //图标
    public int lvl; //等级
    public string name; //名称
    public int quality; //怪物品质
    public int type; //怪物类型
    public int damage_cal; //伤害计算方式
    public int camp; //怪物阵营
    public int sex; //性别
    public string ai_list; //ai序列表
    public int hp; //生命值
    public int att_p_min; //最小物攻
    public int att_p_max; //最大物攻
    public int att_m_min; //最小魔攻
    public int att_m_max; //最大魔攻
    public int def_p; //物理防御力
    public int def_m; //魔法防御力
    public int hit; //命中等级
    public int dodge; //闪避等级
    public int crit; //暴击等级
    public int crit_ratio; //暴击伤害
    public int flex; //韧性
    public int hurt_re; //伤害减免
    public int ice_attr; //冰攻
    public int fir_attr; //火攻
    public int thunder_attr; //雷攻
    public int light_attr; //光攻
    public int dark_attr; //暗攻
    public int ice_def; //冰防
    public int fir_def; //火防
    public int thunder_def; //雷防
    public int light_def; //光防
    public int dark_def; //暗防
    public int stiff_opp; //僵直概率
    public int stiff_time; //僵直时间
    public int speed; //移动速度
    public string rect_stand; //站立矩形
    public string rect_down; //倒地矩形
    public bool active; //是否主动攻击
    public int searchLength; //寻路步长
    public int searchStop; //寻路停顿
    public int attackAccuracy; //攻击精度
    public int attackMovePro; //攻击后游走概率
    public int cruise_range; //游弋范围
    public int sight_range; //视野范围
    public int attack_range; //攻击范围
    public int blood_change_state; //血量切换状态
    public int near_defend; //近身保护范围
    public int track_range; //追敌距离
    public string skill_ids; //常规攻击技能ID
    public int unconvent_skill; //非常规技能施放规则
    public int sound_dead; //死亡音效id
    public int born_effect; //出生效果
    public int born_show; //出生屏显
    public int born_show_time; //屏显时间
    public int hp_count; //血管数
    public int hurt_resist; //攻击抵抗
    public string idle_speak; //怪物待机说话
    public string fight_speak; //怪物战斗说话
    public string death_speak; //怪物死亡说话
    public int hurt_down_resist; //击倒抵抗
    public int Floating_Resist; //浮空抵抗
    public int move_ratio; //移动系数
    }
}
