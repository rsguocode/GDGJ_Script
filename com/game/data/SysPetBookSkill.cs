/**
 * 宠物图鉴技能(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysPetBookSkill
    {
    public uint unikey; //唯一key
    public string name; //技能名字
    public int id; //技能id
    public int list_id; //序号
    public string att; //属性
    public string pet; //关联宠物,与pet_book的id对应
    }
}
