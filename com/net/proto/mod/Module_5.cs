/**
 * 物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_5
  	{

    /**
     * 物品信息
     */
    public static void write_5_1(MemoryStream msdata, byte repos)
    {
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 整理包裹
     */
    public static void write_5_5(MemoryStream msdata, byte repos)
    {
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 交换位置
     */
    public static void write_5_6(MemoryStream msdata, byte repos, ushort from, ushort to)
    {
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUShort(msdata, from);
        proto_util.writeUShort(msdata, to);
    }

    /**
     * 拆分
     */
    public static void write_5_7(MemoryStream msdata, byte repos, uint id, ushort count)
    {
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUInt(msdata, id);
        proto_util.writeUShort(msdata, count);
    }

    /**
     * 使用物品
     */
    public static void write_5_9(MemoryStream msdata, uint id, string name)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
    }

    /**
     * 销毁物品
     */
    public static void write_5_10(MemoryStream msdata, uint id, byte repos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 穿装备
     */
    public static void write_5_11(MemoryStream msdata, uint id, byte pos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, pos);
    }

    /**
     * 卸装备
     */
    public static void write_5_12(MemoryStream msdata, uint id, byte pos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, pos);
    }

    /**
     * 批量使用
     */
    public static void write_5_13(MemoryStream msdata, uint id, ushort count)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUShort(msdata, count);
    }

    /**
     * 仓库背包交换位置
     */
    public static void write_5_14(MemoryStream msdata, byte repos, uint id)
    {
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUInt(msdata, id);
    }

    /**
     * npc商店出售
     */
    public static void write_5_15(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * npc商店购买
     */
    public static void write_5_16(MemoryStream msdata, uint npcId, uint shopId, uint goodsId, ushort count)
    {
        proto_util.writeUInt(msdata, npcId);
        proto_util.writeUInt(msdata, shopId);
        proto_util.writeUInt(msdata, goodsId);
        proto_util.writeUShort(msdata, count);
    }

    /**
     * 宝石能量
     */
    public static void write_5_17(MemoryStream msdata, uint gemId)
    {
        proto_util.writeUInt(msdata, gemId);
    }
   }
}