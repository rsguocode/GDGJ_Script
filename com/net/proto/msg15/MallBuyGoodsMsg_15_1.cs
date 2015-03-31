/**
 * 购买非限制物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MallBuyGoodsMsg_15_1
  	{

    public ushort code = 0;
    public uint id = 0;
    public uint num = 0;

    public static int getCode()
    {
        // (15, 1)
        return 3841;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }
   }
}