/**
 * 宠物模块 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_21
  	{

    /**
     * 宠物信息列表
     */
    public static void write_21_1(MemoryStream msdata)
    {
    }

    /**
     * 请求宠物出战
     */
    public static void write_21_2(MemoryStream msdata, uint id, byte type)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 宠物升星
     */
    public static void write_21_3(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 宠物进阶
     */
    public static void write_21_4(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 升级天赋技能
     */
    public static void write_21_5(MemoryStream msdata, uint id, byte pos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, pos);
    }

    /**
     * 技能点信息
     */
    public static void write_21_7(MemoryStream msdata)
    {
    }

    /**
     * 激活宠物
     */
    public static void write_21_8(MemoryStream msdata, ushort id)
    {
        proto_util.writeUShort(msdata, id);
    }

    /**
     * 购买技能点
     */
    public static void write_21_9(MemoryStream msdata)
    {
    }

    /**
     * 宠物经验升级
     */
    public static void write_21_10(MemoryStream msdata, uint petId, uint goodsId, byte count)
    {
        proto_util.writeUInt(msdata, petId);
        proto_util.writeUInt(msdata, goodsId);
        proto_util.writeUByte(msdata, count);
    }
   }
}