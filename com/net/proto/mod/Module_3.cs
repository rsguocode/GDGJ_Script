/**
 * 角色 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_3
  	{

    /**
     * 角色信息
     */
    public static void write_3_1(MemoryStream msdata)
    {
    }

    /**
     * buff列表
     */
    public static void write_3_2(MemoryStream msdata)
    {
    }

    /**
     * 其他人信息（角色属性面板）
     */
    public static void write_3_3(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 财富更新
     */
    public static void write_3_5(MemoryStream msdata)
    {
    }

    /**
     * 血蓝变化
     */
    public static void write_3_6(MemoryStream msdata, uint hp, uint mp)
    {
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUInt(msdata, mp);
    }

    /**
     * 复活
     */
    public static void write_3_20(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 购买体力
     */
    public static void write_3_40(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 购买金币信息
     */
    public static void write_3_42(MemoryStream msdata)
    {
    }

    /**
     * 购买金币
     */
    public static void write_3_43(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 状态同步（服务端只广播）
     */
    public static void write_3_44(MemoryStream msdata, byte state, uint x, uint y, byte dir)
    {
        proto_util.writeUByte(msdata, state);
        proto_util.writeUInt(msdata, x);
        proto_util.writeUInt(msdata, y);
        proto_util.writeUByte(msdata, dir);
    }

    /**
     * gm和新手指导员列表
     */
    public static void write_3_50(MemoryStream msdata)
    {
    }
   }
}