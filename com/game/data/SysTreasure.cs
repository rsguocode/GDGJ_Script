/**
 * 地宫寻宝(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysTreasure
    {
    public uint unikey; //唯一key
    public int id; //地点id
    public string place; //地点名称
    public string award; //寻宝奖励
    public string order; //可获得额外奖励的顺序
    public string other_award; //额外奖励
    }
}
