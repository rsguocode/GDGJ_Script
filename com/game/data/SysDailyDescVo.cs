/**
 * 日常活动描述配置(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysDailyDescVo
    {
    public uint unikey; //唯一key
    public int id; //序列
    public int output; //产出
    public int type; //类型
    public int subtype; //子类型
    public string name; //名称
    public int lvl; //参与等级
    public int times_min; //最小值
    public int times_max; //最大值
    public int count_down; //活动倒计时
    public string npc; //npc
    public string entrust; //是否委托
    public string desc; //描述
    public int exp_index; //经验指数
    public int silver_index; //银币指数
    public string item_index; //道具指数
    public int show_box; //显示宝箱
    public int entrust_id; //对应委托id
    }
}
