/**
 * 副本陷阱(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysTrap
    {
    public uint unikey; //唯一key
    public int Id; //陷阱id
    public int Model; //资源模型id
    public int Type; //陷阱类型
    public int AttackType; //攻击类型
    public int BuffId; //对应的BuffId
    public int BuffLvl; //Buff等级
    public bool HurtFly; //是否击飞
    public int AttackInterval; //攻击间隔（ms）
    public string SkillIds; //技能Id
    }
}
