/**
 * 批量使用 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsBatchUseMsg_5_13
  	{

    public ushort code = 0;
    public uint goodsId = 0;

    public static int getCode()
    {
        // (5, 13)
        return 1293;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        goodsId = proto_util.readUInt(msdata);
    }
   }
}