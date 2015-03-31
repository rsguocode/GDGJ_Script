/**
 * 兑换 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_16
  	{

    /**
     * 兑换信息
     */
    public static void write_16_1(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 兑换
     */
    public static void write_16_2(MemoryStream msdata, uint id, ushort times)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUShort(msdata, times);
    }
   }
}