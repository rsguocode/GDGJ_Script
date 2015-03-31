/**
 * 培养 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DeedDoMsg_18_2
  	{

    public ushort code = 0;
    public uint id = 0;
    public uint count = 0;

    public static int getCode()
    {
        // (18, 2)
        return 4610;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        count = proto_util.readUInt(msdata);
    }
   }
}