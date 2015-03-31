/**
 * 英雄榜 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_19
  	{

    /**
     * 主面板涉及信息
     */
    public static void write_19_0(MemoryStream msdata)
    {
    }

    /**
     * 前十玩家
     */
    public static void write_19_1(MemoryStream msdata)
    {
    }

    /**
     * 清cd
     */
    public static void write_19_3(MemoryStream msdata)
    {
    }

    /**
     * 增加购买次数
     */
    public static void write_19_4(MemoryStream msdata)
    {
    }

    /**
     * 挑战
     */
    public static void write_19_5(MemoryStream msdata, byte result)
    {
        proto_util.writeUByte(msdata, result);
    }

    /**
     * 个人历史挑战记录
     */
    public static void write_19_8(MemoryStream msdata)
    {
    }

    /**
     * 开始挑战，加锁
     */
    public static void write_19_10(MemoryStream msdata, uint id, ushort pos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUShort(msdata, pos);
    }
   }
}