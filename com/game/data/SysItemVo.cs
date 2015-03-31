/**
 * 道具信息表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysItemVo
    {
    public uint unikey; //唯一key
    public int id; //物品ID
    public string name; //物品名称
    public int icon; //物品ICON
    public int big_icon; //物品大ICON
    public int down_icon; //物品地面上显示ICON
    public string desc; //物品描述
    public int type; //物品类型
    public int subtype; //物品子类型
    public int color; //道具品质颜色
    public int lvl; //使用等级
    public int vip; //要求VIP等级
    public string need_goods; //要求道具
    public bool can_use; //是否可使用
    public string type_tips; //类别说明
    public string use_tips; //使用说明
    public int job; //职业限制
    public int max_stack; //最大堆叠数量
    public bool can_sell; //是否出售
    public bool can_market; //是否可交易
    public bool sell_prompt; //出售是否提示
    public int price; //出售价格
    public bool can_drop; //是否丢弃
    public bool destruct_prompt; //是否销毁提示
    public int time_type; //期限类型
    public int time; //时间期限
    public int goods_price; //道具价值
    public int cost; //合成费用
    public string display; //显示形象
    public int value; //属性
    public string other; //其它数据
    public string source; //道具来源
    }
}
