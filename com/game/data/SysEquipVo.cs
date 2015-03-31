/**
 * 装备信息表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysEquipVo
    {
    public uint unikey; //唯一key
    public int id; //物品ID
    public string name; //物品名称
    public int icon; //物品ICOn
    public int down_icon; //物品地面上显示ICON
    public string desc; //装备描述
    public int type; //大类
    public int subtype; //子类
    public bool can_sell; //是否可出售
    public bool sell_prompt; //出售是否提示
    public int price; //出售价格
    public bool can_drop; //是否可丢弃
    public bool destruct_prompt; //是否销毁提示
    public string use_tips; //使用说明
    public int max_stack; //最大堆叠数
    public int job; //职业要求
    public string lvl; //等级要求
    public int pos; //装备部位
    public string gem_type; //可镶嵌宝石种类
    public int grade; //阶位
    public string material; //合成所需材料
    public int max_stren; //最大强化星级
    public string stren_type; //装备强化属性类别
    public int str; //力量
    public int agi; //敏捷
    public int phy; //体质
    public int wit; //智力
    public string hp; //生命
    public int mp; //魔法
    public int att_p_min; //最小物攻
    public string att_p_max; //最大物攻
    public int att_m_min; //最小魔攻
    public string att_m_max; //最大魔攻
    public string def_p; //物防
    public string def_m; //魔防
    public string hit; //命中
    public string dodge; //闪避
    public string crit; //暴击等级
    public string crit_ratio; //暴伤等级
    public string flex; //韧性等级
    public string hurt_re; //减伤
    public int speed; //移动速度
    public string luck; //幸运等级
    public int att_max; //最大攻击
    public int goods_price; //装备价值
    public int resource_id; //资源id
    public string skin_effect; //武器特效
    public int sex; //性别限制
    public int suit_id; //套装ID
    public int ava_book; //对应图鉴id
    public string display; //显示形象
    public bool can_market; //是否可交易
    public int color; //品质（决定装备可开孔数量）
    public int spend; //合成费用
    public string source; //装备来源
    public int sort; //图鉴排序
    }
}
