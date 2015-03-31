/**
 * 扩充土地 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmNewLandMsg_30_9
  	{

    public ushort code = 0;
    public byte pos = 0;

    public static int getCode()
    {
        // (30, 9)
        return 7689;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        pos = proto_util.readUByte(msdata);
    }
   }
}