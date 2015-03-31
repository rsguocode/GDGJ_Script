/**
 * 副会长辞职 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildResignMsg_31_14
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (31, 14)
        return 7950;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}