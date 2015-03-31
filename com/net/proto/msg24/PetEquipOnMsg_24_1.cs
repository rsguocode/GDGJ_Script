/**
 * 穿装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipOnMsg_24_1
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (24, 1)
        return 6145;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}