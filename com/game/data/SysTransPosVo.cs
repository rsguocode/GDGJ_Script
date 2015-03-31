/**
 * 系统传送点列表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysTransPosVo
    {
    public uint unikey; //唯一key
    public int id; //传送点id
    public int mapid; //地图id
    public int x; //x坐标
    public int y; //y坐标
    public string name; //传送点名
    public string map_show; //地图显示
    }
}
