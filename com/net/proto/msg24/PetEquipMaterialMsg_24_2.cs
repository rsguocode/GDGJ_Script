/**
 * 宠物装备合成 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipMaterialMsg_24_2
  	{

    public ushort code = 0;
    public uint id = 0;

    public static int getCode()
    {
        // (24, 2)
        return 6146;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
    }
   }
}