/**
 * 购买种子 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmBuySeedMsg_30_12
  	{

    public ushort code = 0;
    public byte type = 0;
    public uint goodsTypeId = 0;
    public uint num = 0;

    public static int getCode()
    {
        // (30, 12)
        return 7692;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        goodsTypeId = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }
   }
}