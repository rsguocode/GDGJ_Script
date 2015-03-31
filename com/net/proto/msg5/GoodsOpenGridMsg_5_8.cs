/**
 * 开格子 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsOpenGridMsg_5_8
  	{

    public ushort code = 0;
    public byte repos = 0;
    public uint cd = 0;

    public static int getCode()
    {
        // (5, 8)
        return 1288;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        repos = proto_util.readUByte(msdata);
        cd = proto_util.readUInt(msdata);
    }
   }
}