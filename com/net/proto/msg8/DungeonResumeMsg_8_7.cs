/**
 * 副本继续 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonResumeMsg_8_7
  	{

    public ushort code = 0;
    public ushort remainTime = 0;

    public static int getCode()
    {
        // (8, 7)
        return 2055;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        remainTime = proto_util.readUShort(msdata);
    }
   }
}