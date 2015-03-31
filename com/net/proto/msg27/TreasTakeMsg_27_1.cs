/**
 * 寻宝 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TreasTakeMsg_27_1
  	{

    public ushort code = 0;
    public uint goodsId = 0;
    public uint goodsIdFinal = 0;

    public static int getCode()
    {
        // (27, 1)
        return 6913;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        goodsId = proto_util.readUInt(msdata);
        goodsIdFinal = proto_util.readUInt(msdata);
    }
   }
}