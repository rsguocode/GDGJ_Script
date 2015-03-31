/**
 * 怪物死亡 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMonDeadMsg_4_12
  	{

    public uint id = 0;
    public ushort code = 0;

    public static int getCode()
    {
        // (4, 12)
        return 1036;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        code = proto_util.readUShort(msdata);
    }
   }
}