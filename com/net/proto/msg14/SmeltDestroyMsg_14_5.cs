/**
 * 装备分解 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltDestroyMsg_14_5
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (14, 5)
        return 3589;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}