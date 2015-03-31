/**
 * 副本树信息表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysDungeonTreeVo
    {
    public uint unikey; //唯一key
    public int id; //ID
    public int type; //类型
    public int parentId; //父ID
    public string list; //下辖ID列表
    public string name; //名字
    public int x; //X坐标
    public int y; //Y坐标
    public string icon; //图标
    public string anim; //怪物动画
    public string remark; //备注
    public string attrAdd; //满星属性加成
    public string monster_icon; //怪物头像
    public string boss_icon; //boss头像
    }
}
