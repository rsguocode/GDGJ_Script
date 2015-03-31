/**
 * 特效配置表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysEffectVo
    {
    public uint unikey; //唯一key
    public int id; //特效ID
    public string name; //名称
    public string effect_start; //特效开始
    public string effect_keep; //特效保持
    public string effect_end; //特效结束
    public int skill_id; //技能ID
    public int type; //特效类型(1使用2环境3过程4目标5buff)
    public int width; //子弹宽度
    public int height; //子弹高度
    public int environ_eff_level; //场景特效层次(1人前 2人后 0其他)
    public bool mul_language; //是否多语言
    public int effect_type; //特效类型（位图、原件特效）
    public int link; //链接名
    public int frameCount; //元件特效长度
    }
}
