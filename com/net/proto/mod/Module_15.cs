/**
 * 商城相关 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_15
  	{

    /**
     * 购买非限制物品
     */
    public static void write_15_1(MemoryStream msdata, uint id, uint num, ushort type, ushort subtype)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, num);
        proto_util.writeUShort(msdata, type);
        proto_util.writeUShort(msdata, subtype);
    }

    /**
     * 限购信息
     */
    public static void write_15_2(MemoryStream msdata)
    {
    }

    /**
     * 限购物品
     */
    public static void write_15_3(MemoryStream msdata, uint id, uint num)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, num);
    }

    /**
     * 赠送物品
     */
    public static void write_15_4(MemoryStream msdata, uint id, uint num, ushort type, ushort subtype, string givename, string goodsname)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, num);
        proto_util.writeUShort(msdata, type);
        proto_util.writeUShort(msdata, subtype);
        proto_util.writeString(msdata, givename);
        proto_util.writeString(msdata, goodsname);
    }

    /**
     * 钻石专区信息
     */
    public static void write_15_5(MemoryStream msdata)
    {
    }
   }
}