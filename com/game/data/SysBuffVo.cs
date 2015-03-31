/**
 * 系统buff(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysBuffVo
    {
    public uint unikey; //唯一key
    public int id; //BUFF_ID
    public int lvl; //BUFF等级
    public string name; //BUFF名称
    public string desc; //BUFF描述
    public bool is_debuff; //是否为debuff
    public int type; //类型
    public string valueArray; //数值
    public int last_type; //持续类型
    public int last_val; //持续值
    public int last_interval; //时间间隔
    public int pos_type; //施放类型
    public bool dead_remove; //死亡后是否删除
    public int res; //icon
    public bool show; //身上显示
    public int effect; //特效资源
    public int target; //buff目标
    public int state_perform; //状态表现
    }
}
