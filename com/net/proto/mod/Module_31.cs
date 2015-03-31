/**
 * 公会 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_31
  	{

    /**
     * 创建公会
     */
    public static void write_31_1(MemoryStream msdata, string name)
    {
        proto_util.writeString(msdata, name);
    }

    /**
     * 解散公会
     */
    public static void write_31_2(MemoryStream msdata)
    {
    }

    /**
     * 管理公会
     */
    public static void write_31_3(MemoryStream msdata, byte operation, uint roleId)
    {
        proto_util.writeUByte(msdata, operation);
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 退出公会
     */
    public static void write_31_4(MemoryStream msdata)
    {
    }

    /**
     * 修改公会公告
     */
    public static void write_31_5(MemoryStream msdata, string announcement)
    {
        proto_util.writeString(msdata, announcement);
    }

    /**
     * 公会基本信息
     */
    public static void write_31_6(MemoryStream msdata, uint guildId)
    {
        proto_util.writeUInt(msdata, guildId);
    }

    /**
     * 公会的人员信息
     */
    public static void write_31_7(MemoryStream msdata, uint guildId)
    {
        proto_util.writeUInt(msdata, guildId);
    }

    /**
     * 其他公会信息
     */
    public static void write_31_8(MemoryStream msdata, ushort page, bool mask)
    {
        proto_util.writeUShort(msdata, page);
        proto_util.writeBool(msdata, mask);
    }

    /**
     * 通过公会名字查找
     */
    public static void write_31_9(MemoryStream msdata, string guildName)
    {
        proto_util.writeString(msdata, guildName);
    }

    /**
     * 通过会长名字查询
     */
    public static void write_31_10(MemoryStream msdata, string ownerName)
    {
        proto_util.writeString(msdata, ownerName);
    }

    /**
     * 申请加入某个公会
     */
    public static void write_31_11(MemoryStream msdata, uint guildId)
    {
        proto_util.writeUInt(msdata, guildId);
    }

    /**
     * 申请审核公会列表
     */
    public static void write_31_12(MemoryStream msdata)
    {
    }

    /**
     * 审核申请
     */
    public static void write_31_13(MemoryStream msdata, uint roleId, bool result)
    {
        proto_util.writeUInt(msdata, roleId);
        proto_util.writeBool(msdata, result);
    }

    /**
     * 副会长辞职
     */
    public static void write_31_14(MemoryStream msdata)
    {
    }

    /**
     * 公会日志
     */
    public static void write_31_15(MemoryStream msdata)
    {
    }
   }
}