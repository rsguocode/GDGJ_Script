/**
 * 怪物移动（暂不使用，组队副本使用） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMonMoveMsg_4_9
  	{

    public uint id = 0;
    public ushort x = 0;
    public ushort y = 0;
    public ushort speed = 0;

    public static int getCode()
    {
        // (4, 9)
        return 1033;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        x = proto_util.readUShort(msdata);
        y = proto_util.readUShort(msdata);
        speed = proto_util.readUShort(msdata);
    }
   }
}