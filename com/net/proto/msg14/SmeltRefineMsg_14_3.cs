/**
 * 精炼 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltRefineMsg_14_3
  	{

    public ushort code = 0;
    public ushort result = 0;
    public uint id = 0;
    public byte repos = 0;

    public static int getCode()
    {
        // (14, 3)
        return 3587;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        result = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
    }
   }
}