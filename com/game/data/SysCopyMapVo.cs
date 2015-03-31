/**
 * 副本地图(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysCopyMapVo
    {
    public uint unikey; //唯一key
    public int id; //ID
    public uint mapId; //场景ID
    public int queue; //序号
    public int parentId; //父ID
    public string name; //名字
    public string remark; //备注
    public int x; //X坐标
    public int y; //Y坐标
    public string icon; //图标
    public string goodsId; //掉落物品ID
    public string animation; //怪物动画
    public int level; //等级限制
    public int vigour; //体力
    }
}
