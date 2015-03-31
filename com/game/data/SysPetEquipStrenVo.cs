/**
 * 宠物装备强化(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysPetEquipStrenVo
    {
    public uint unikey; //唯一key
    public int lvl; //锤炼等级
    public int real_pro; //真实成功几率
    public int remain_pro; //失败不掉星概率
    public int fake_pro; //前端概率
    public string material; //消耗道具
    public int silver; //消耗银币
    public string bless; //保佑道具id
    }
}
