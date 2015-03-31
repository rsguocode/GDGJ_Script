/**
 * 技能动作配置(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysSkillActionVo
    {
    public uint unikey; //唯一key
    public int id; //技能组
    public bool penetrate; //是否穿透
    public string name; //技能名称
    public int action_id; //使用者动作编号
    public string use_eff; //使用者特效
    public bool use_eff_need_follow; //使用者特效是否跟随主角
    public int environ_eff; //环境特效
    public string process_eff; //过程特效
    public string tar_eff; //目标特效
    public int buff_eff; //buff特效
    public int bullet_x; //出生点X
    public int bullet_y; //出生点Y
    public int impact_style; //碰撞类型
    public bool is_att; //是否为攻击技能
    public string shake; //震屏系统
    public int bullet_speed; //子弹速度
    public bool IsBullet; //是否是子弹技能
    public int BulletWidth; //子弹
    public int BulletHeight; //子弹高度
    public int BulletType; //子弹类型
    public int CheckInterval; //检测间隔
    public int BulletTravelDistance; //子弹飞行距离
    }
}
