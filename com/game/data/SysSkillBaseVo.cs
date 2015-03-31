/**
 * 系统技能(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysSkillBaseVo
    {
    public uint unikey; //唯一key
    public int id; //技能ID
    public string name; //技能名称
    public int icon; //技能图标
    public int position; //技能位置
    public uint skill_group; //技能组
    public int lvl; //要求角色等级
    public int point; //灵魂点
    public int job; //要求职业
    public bool active; //是否为主动技能
    public int pre; //前置技能
    public int next; //下个技能
    public int evolve; //进化技能
    public string evo_relation; //关联技能进化
    public int reset_skillid; //重置技能
    public int beiley; //升级金钱
    public int skill_lvl; //技能等级
    public int type; //技能类型
    public int subtype; //技能子类型
    public int target_type; //技能目标类型
    public bool group; //是否为群攻
    public int target_dir; //攻击方向
    public string cover_start_pos; //攻击矩形区域计算起始点
    public int cover_width; //覆盖宽度(全宽)
    public int cover_height; //覆盖高度（全高）
    public int cover_thickness; //覆盖厚度（半厚）
    public int Floating_Value; //浮空系数
    public string Per_Atk_Data; //每一击的攻击信息
    public string Rush_Data; //使用技能时突进信息（带加速度的位移）
    public string Move_Data; //使用技能时位移信息（匀速的位移）
    public string Move_During_Skilling; //使用技能时可控制移动信息(匀速)
    public int back_dis; //击退距离
    public int target_num; //目标数量
    public int need_type; //消耗类型
    public int need_value; //消耗值
    public int att_type; //攻击类型
    public int key; //效果类型
    public int value; //效果值
    public int data_per; //数据百分比
    public int data_fixed; //数据固定值
    public int cd_public; //公共冷却
    public int cd; //自身冷却
    public bool need_slow_speed; //是否需要减慢速度
    public int warn_time; //技能施放预警时间(ms)
    public bool can_break; //预警是否可打断
    public int warn_prolong_time; //预警延长时间(ms)
    public int warn_eff; //技能施放预警特效
    public int release_delay; //释放后停顿时间ms
    public int spcID; //特殊效果
    public int spcArg; //特殊效果参数
    public int buff_condition; //buff出现条件
    public string buff_id; //buff
    public string desc; //描述
    public string sound_id; //音效
    public string sound_hit; //击中目标音效
    public int hit_type; //受击表现
    public bool shake; //是否震屏
    public bool need_keep; //是否是持续技能
    public int color; //技能品质
    public int scale; //是否拉伸屏幕
    }
}
