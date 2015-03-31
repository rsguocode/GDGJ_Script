/**
 * 地图 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_4
  	{

    /**
     * 请求场景切换
     */
    public static void write_4_1(MemoryStream msdata, uint mapid, ushort x, ushort y)
    {
        proto_util.writeUInt(msdata, mapid);
        proto_util.writeUShort(msdata, x);
        proto_util.writeUShort(msdata, y);
    }

    /**
     * 视野信息
     */
    public static void write_4_2(MemoryStream msdata)
    {
    }

    /**
     * 玩家移动
     */
    public static void write_4_6(MemoryStream msdata, ushort x, ushort y, ushort moveStatus)
    {
        proto_util.writeUShort(msdata, x);
        proto_util.writeUShort(msdata, y);
        proto_util.writeUShort(msdata, moveStatus);
    }

    /**
     * 怪物死亡
     */
    public static void write_4_12(MemoryStream msdata, uint id, uint lastKill, uint hp, uint mp)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, lastKill);
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUInt(msdata, mp);
    }

    /**
     * 角色死亡
     */
    public static void write_4_13(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }
   }
}