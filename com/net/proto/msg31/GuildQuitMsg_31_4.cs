/**
 * 退出公会 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildQuitMsg_31_4
  	{

    public ushort code = 0;
    public uint guildId = 0;

    public static int getCode()
    {
        // (31, 4)
        return 7940;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        guildId = proto_util.readUInt(msdata);
    }
   }
}