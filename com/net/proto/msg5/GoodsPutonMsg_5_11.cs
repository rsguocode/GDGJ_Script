/**
 * 穿装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsPutonMsg_5_11
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (5, 11)
        return 1291;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}