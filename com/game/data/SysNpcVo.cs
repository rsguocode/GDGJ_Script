/**
 * NPC表格(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysNpcVo
    {
    public uint unikey; //唯一key
    public int npcId; //npc_id
    public string name; //npc名称
    public int halfImgAtlas; //半身像图集
    public string halfImgSprite; //半身像Sprite
    public int model; //皮肤
    public string fixspeak; //固定对话（点击对话）
    }
}
