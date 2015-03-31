/**
 * 清理副本cd (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonSilverTreeClearCdMsg_8_12
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (8, 12)
        return 2060;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}