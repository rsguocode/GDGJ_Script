/**
 * 剧情对话(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysStoryTalkListVo
    {
    public uint unikey; //唯一key
    public int talkId; //对话顺序Id
    public uint chapter_id; //剧情Id
    public uint bgId; //背景图Id
    public uint npcId; //npcId
    public int npcAlign; //npc显示位置
    public uint effectId; //特效Id
    public int effectAlign; //特效显示位置
    public int effectShowTime; //特效出现时间
    public string words; //对话内容
    }
}
