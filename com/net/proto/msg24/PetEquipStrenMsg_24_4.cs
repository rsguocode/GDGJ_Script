/**
 * 装备锤炼 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipStrenMsg_24_4
  	{

    public ushort code = 0;
    public byte succ = 0;

    public static int getCode()
    {
        // (24, 4)
        return 6148;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        succ = proto_util.readUByte(msdata);
    }
   }
}