/**
 * 农场 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_30
  	{

    /**
     * 农场信息
     */
    public static void write_30_1(MemoryStream msdata)
    {
    }

    /**
     * 日志
     */
    public static void write_30_2(MemoryStream msdata)
    {
    }

    /**
     * 所有好友农场的简略信息
     */
    public static void write_30_3(MemoryStream msdata)
    {
    }

    /**
     * 好友农场信息
     */
    public static void write_30_4(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 种子背包信息
     */
    public static void write_30_5(MemoryStream msdata)
    {
    }

    /**
     * 加速成长
     */
    public static void write_30_8(MemoryStream msdata, byte pos, uint goodsTypeId)
    {
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUInt(msdata, goodsTypeId);
    }

    /**
     * 扩充土地
     */
    public static void write_30_9(MemoryStream msdata)
    {
    }

    /**
     * 升级土地
     */
    public static void write_30_10(MemoryStream msdata)
    {
    }

    /**
     * 种子商店信息
     */
    public static void write_30_11(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 购买种子
     */
    public static void write_30_12(MemoryStream msdata, byte type, uint goodsTypeId, uint num)
    {
        proto_util.writeUByte(msdata, type);
        proto_util.writeUInt(msdata, goodsTypeId);
        proto_util.writeUInt(msdata, num);
    }

    /**
     * 农场操作
     */
    public static void write_30_13(MemoryStream msdata, uint id, byte pos, byte type)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 一键收获
     */
    public static void write_30_14(MemoryStream msdata)
    {
    }

    /**
     * 种植
     */
    public static void write_30_15(MemoryStream msdata, uint seedid, byte pos)
    {
        proto_util.writeUInt(msdata, seedid);
        proto_util.writeUByte(msdata, pos);
    }
   }
}