/**
 * 销毁物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsDestroyMsg_5_10
  	{

    public ushort code = 0;
    public uint goodsId = 0;
    public byte repos = 0;

    public static int getCode()
    {
        // (5, 10)
        return 1290;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        goodsId = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
    }
   }
}