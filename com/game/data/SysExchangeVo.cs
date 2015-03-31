/**
 * 兑换表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysExchangeVo
    {
    public uint unikey; //唯一key
    public int id; //抽奖ID
    public int type; //抽奖类别
    public string rewarditemid; //奖品id
    public string higheritemid; //高级奖品id
    public string needs; //抽奖所需
    public int limit; //每日抽奖上限
    }
}
