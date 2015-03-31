/**
 * 玩家移动 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMoveMsg_4_6
  	{

    public uint id = 0;
    public ushort x = 0;
    public ushort y = 0;
    public byte moveStatus = 0;

    public static int getCode()
    {
        // (4, 6)
        return 1030;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        x = proto_util.readUShort(msdata);
        y = proto_util.readUShort(msdata);
        moveStatus = proto_util.readUByte(msdata);
    }
   }
}