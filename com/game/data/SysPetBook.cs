/**
 * 宠物图鉴(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysPetBook
    {
    public uint unikey; //唯一key
    public int id; //宠物id
    public int list_id; //序号
    public string name; //名字
    public int icon; //ICON
    public string att; //属性
    public string cost; //幻化花费
    public int res; //幻化资源
    public string get; //获得途径说明
    }
}
