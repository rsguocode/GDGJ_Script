/**
 * 副本奖励表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysDungeonRewardVo
    {
    public uint unikey; //唯一key
    public int map_id; //地图ID
    public int fix_gold; //固定金币奖励
    public int fix_diam; //固定钻石奖励
    public int fix_soul; //首次通关灵魂点奖励
    public string goods1; //道具奖励1
    public string goods2; //道具奖励2
    public string goods3; //道具奖励3
    public string goods4; //道具奖励4
    public string ext_goods; //额外道具奖励
    public string time; //时间加成
    public string attack; //连斩加成
    }
}
