/**
 * 副本刷怪表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysDungeonMon
    {
    public uint unikey; //唯一key
    public int map_id; //地图ID
    public int phase; //阶段
    public string list; //怪物列表
    public string pos; //区域触发点
    public string trigger_list; //触发怪物列表
    }
}
