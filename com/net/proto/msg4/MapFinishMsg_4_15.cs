/**
 * 通关 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapFinishMsg_4_15
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (4, 15)
        return 1039;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}