/**
 * 副本暂停 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonPauseMsg_8_6
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (8, 6)
        return 2054;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}