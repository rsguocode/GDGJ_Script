/**
 * 宠物成长 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetGrowMsg_21_7
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte crit = 0;

    public static int getCode()
    {
        // (21, 7)
        return 5383;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        crit = proto_util.readUByte(msdata);
    }
   }
}