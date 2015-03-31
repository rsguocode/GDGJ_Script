/**
 * 解散公会 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildDisbandMsg_31_2
  	{

    public ushort code = 0;
    public uint guildId = 0;

    public static int getCode()
    {
        // (31, 2)
        return 7938;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        guildId = proto_util.readUInt(msdata);
    }
   }
}