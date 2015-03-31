/**
 * 种植配置(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysSeedVo
    {
    public uint unikey; //唯一key
    public int id; //种子ID
    public string name; //名字
    public string desc; //描述
    public int lvl; //种子等级
    public string normal_gains; //常规产出
    public string rate_gains; //概率产出
    public int exp_reward; //种植经验
    public int mature_time; //成熟时间
    }
}
