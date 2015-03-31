/**
 * 国家 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_9
  	{

    /**
     * 国家选择
     */
    public static void write_9_1(MemoryStream msdata, byte type, byte id)
    {
        proto_util.writeUByte(msdata, type);
        proto_util.writeUByte(msdata, id);
    }

    /**
     * 国家信息
     */
    public static void write_9_2(MemoryStream msdata)
    {
    }

    /**
     * 捐献
     */
    public static void write_9_3(MemoryStream msdata, byte type, uint number, List<PKeyValue> goods)
    {
        proto_util.writeUByte(msdata, type);
        proto_util.writeUInt(msdata, number);
        PKeyValue.writeLoop(msdata, goods);
    }

    /**
     * 捐献查询
     */
    public static void write_9_4(MemoryStream msdata)
    {
    }
   }
}