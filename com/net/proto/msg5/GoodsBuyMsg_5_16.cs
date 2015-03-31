/**
 * npc商店购买 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsBuyMsg_5_16
  	{

    public ushort code = 0;
    public uint goodsId = 0;

    public static int getCode()
    {
        // (5, 16)
        return 1296;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        goodsId = proto_util.readUInt(msdata);
    }
   }
}