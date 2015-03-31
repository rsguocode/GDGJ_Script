/**
 * 副本 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_8
  	{

    /**
     * 世界地图信息
     */
    public static void write_8_1(MemoryStream msdata)
    {
    }

    /**
     * 副本信息
     */
    public static void write_8_3(MemoryStream msdata, uint sublayerid)
    {
        proto_util.writeUInt(msdata, sublayerid);
    }

    /**
     * 离开副本
     */
    public static void write_8_4(MemoryStream msdata)
    {
    }

    /**
     * 副本暂停
     */
    public static void write_8_6(MemoryStream msdata)
    {
    }

    /**
     * 副本继续
     */
    public static void write_8_7(MemoryStream msdata)
    {
    }

    /**
     * 领取额外奖励
     */
    public static void write_8_9(MemoryStream msdata, uint mapid)
    {
        proto_util.writeUInt(msdata, mapid);
    }

    /**
     * 点石成金面板信息
     */
    public static void write_8_11(MemoryStream msdata)
    {
    }

    /**
     * 清理副本cd
     */
    public static void write_8_12(MemoryStream msdata)
    {
    }

    /**
     * 副本扫荡
     */
    public static void write_8_15(MemoryStream msdata, uint mapid, byte times)
    {
        proto_util.writeUInt(msdata, mapid);
        proto_util.writeUByte(msdata, times);
    }

    /**
     * 精英副本信息
     */
    public static void write_8_16(MemoryStream msdata, uint cityid)
    {
        proto_util.writeUInt(msdata, cityid);
    }

    /**
     * 精英副本奖励信息
     */
    public static void write_8_17(MemoryStream msdata, uint cityid)
    {
        proto_util.writeUInt(msdata, cityid);
    }

    /**
     * 领取超级副本奖励
     */
    public static void write_8_18(MemoryStream msdata, uint cityid, byte award)
    {
        proto_util.writeUInt(msdata, cityid);
        proto_util.writeUByte(msdata, award);
    }

    /**
     * 激活女神
     */
    public static void write_8_19(MemoryStream msdata, uint cityid)
    {
        proto_util.writeUInt(msdata, cityid);
    }

    /**
     * 刷隐藏怪
     */
    public static void write_8_20(MemoryStream msdata, uint hidePhase)
    {
        proto_util.writeUInt(msdata, hidePhase);
    }
   }
}