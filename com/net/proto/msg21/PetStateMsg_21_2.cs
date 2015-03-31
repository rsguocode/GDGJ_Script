/**
 * 请求宠物出战 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetStateMsg_21_2
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (21, 2)
        return 5378;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}