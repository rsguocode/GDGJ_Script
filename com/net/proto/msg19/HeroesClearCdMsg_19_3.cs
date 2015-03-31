/**
 * 清cd (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesClearCdMsg_19_3
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (19, 3)
        return 4867;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}