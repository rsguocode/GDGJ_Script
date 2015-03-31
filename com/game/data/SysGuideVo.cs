/**
 * 指引(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysGuideVo
    {
    public uint unikey; //唯一key
    public int guideID; //引导ID
    public int trigger_type; //触发类型
    public string condition; //触发条件
    public int guide_type; //解锁类型
    public string guide_describe; //开启说明
    }
}
