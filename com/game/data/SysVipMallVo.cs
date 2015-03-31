/**
 * 商城表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysVipMallVo
    {
    public uint unikey; //唯一key
    public int type; //归类
    public int id; //道具ID
    public string name; //道具名称
    public int icon_big; //大图标资源
    public int money; //货币类型
    public int former_price; //原价
    public int curr_price; //现价
    public int mark; //特殊标识
    public int buy_max; //单人购买上限
    public int queue; //排序
    public int giving; //赠送标识
    public int small_type; //小类
    public string avatar; //形象
    public string buy_group; //购买组
    public string show_time; //出现时间
    }
}
