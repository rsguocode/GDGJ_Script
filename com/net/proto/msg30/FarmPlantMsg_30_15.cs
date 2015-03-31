/**
 * 种植 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmPlantMsg_30_15
  	{

    public ushort code = 0;
    public uint seedid = 0;
    public byte pos = 0;
    public uint exp = 0;

    public static int getCode()
    {
        // (30, 15)
        return 7695;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        seedid = proto_util.readUInt(msdata);
        pos = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
    }
   }
}