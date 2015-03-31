/**
 * 宠物表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysPet
    {
    public uint unikey; //唯一key
    public int id; //宠物ID
    public int icon; //图标
    public int stone_id; //灵魂石ID
    public string name; //宠物名
    public string res; //资源ID
    public int size; //缩放比例
    public int fly; //是否飞行
    public int type; //宠物类型
    public int star; //星级
    public string hp; //生命
    public string att_max; //攻击上限
    public string def_p; //物防
    public string def_m; //魔防
    public string hurt_re; //减伤
    public int unique_skill; //天赋技能
    public string passive_skill; //被动技能
    }
}
