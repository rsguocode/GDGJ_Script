/**
 * 强化 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltStrenMsg_14_1
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte repos = 0;
    public byte result = 0;
    public byte stren = 0;

    public static int getCode()
    {
        // (14, 1)
        return 3585;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
        result = proto_util.readUByte(msdata);
        stren = proto_util.readUByte(msdata);
    }
   }
}