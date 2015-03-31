/**
 * 地图信息表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysMapVo
    {
    public uint unikey; //唯一key
    public int id; //地图ID
    public int resource_id; //地图资源ID
    public string name; //地图名称
    public string bgMusic; //背景音乐ID
    public string bg_sound_effect; //背景音效
    public string npcList; //NPC配置
    public string trapList; //陷阱配置
    public int type; //地图类型
    public int subtype; //地图子类型
    public int lvl; //要求等级
    public int team_num_min; //组队最小人数
    public int group; //副本组
    public int pk_mode; //PK模式模式
    public bool skill_limited; //是否能使用技能
    public bool adapt; //是否自适应
    public string ai; //ai模块名
    public int vigour; //消耗总体力数量
    public string open_condition; //开放条件
    public string enter_condition; //进入副本等级差
    public int revive; //复活点列表
    public int icon_id; //显示ICON
    public string scene_pos; //场景特效坐标
    public bool can_revive; //是否可复活（0否，1是）
    public int revive_cost; //原地复活花费金币
    public int phase; //关卡分段数
    public string trans_pos; //副本传送阵位置
    public string world_trans_pos; //世界地图传送阵位置
    public string monster_words; //怪物说话
    public int enter_count; //副本进入次数
    public bool need_synchronization; //是否需要同步
    }
}
