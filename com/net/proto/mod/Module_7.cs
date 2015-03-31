/**
 * 好友 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_7
  	{

    /**
     * 朋友信息
     */
    public static void write_7_1(MemoryStream msdata)
    {
    }

    /**
     * 邀请好友
     */
    public static void write_7_2(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 回复好友
     */
    public static void write_7_4(MemoryStream msdata, uint roleId, byte answer)
    {
        proto_util.writeUInt(msdata, roleId);
        proto_util.writeUByte(msdata, answer);
    }

    /**
     * 删除好友
     */
    public static void write_7_6(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 移至黑名单
     */
    public static void write_7_7(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 根据角色名加好友
     */
    public static void write_7_10(MemoryStream msdata, string name)
    {
        proto_util.writeString(msdata, name);
    }

    /**
     * 删除黑名单
     */
    public static void write_7_11(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 好友数是否最大
     */
    public static void write_7_13(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }

    /**
     * 批量删除
     */
    public static void write_7_16(MemoryStream msdata, List<uint> ids)
    {
        proto_util.writeLoopUInt(msdata, ids);
    }

    /**
     * 附近玩家
     */
    public static void write_7_18(MemoryStream msdata)
    {
    }
   }
}