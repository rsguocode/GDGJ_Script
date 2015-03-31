/**
 * 装备升级 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipUpgradeMsg_24_5
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (24, 5)
        return 6149;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}