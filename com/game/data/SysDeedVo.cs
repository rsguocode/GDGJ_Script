/**
 * 契约配置表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysDeedVo
    {
    public uint unikey; //唯一key
    public int id; //契约id
    public int lvl; //等级
    public int lvl_limit; //等级限制
    public int item_id; //道具id
    public string name; //道具名字
    public int type; //属性类型
    public string val; //效果值
    public int vip_lvl; //vip等级
    public int money; //钻石消耗
    }
}
