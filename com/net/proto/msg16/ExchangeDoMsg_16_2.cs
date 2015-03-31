/**
 * 兑换 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ExchangeDoMsg_16_2
  	{

    public ushort code = 0;
    public uint id = 0;
    public List<uint> item = new List<uint>();

    public static int getCode()
    {
        // (16, 2)
        return 4098;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, item);
    }
   }
}