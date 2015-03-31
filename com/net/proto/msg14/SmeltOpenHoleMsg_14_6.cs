/**
 * 开孔 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltOpenHoleMsg_14_6
  	{

    public ushort code = 0;
    public uint id = 0;
    public uint holeNum = 0;
    public byte result = 0;

    public static int getCode()
    {
        // (14, 6)
        return 3590;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        holeNum = proto_util.readUInt(msdata);
        result = proto_util.readUByte(msdata);
    }
   }
}