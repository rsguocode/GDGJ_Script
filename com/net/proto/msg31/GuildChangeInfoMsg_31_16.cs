/**
 * 公会信息发生变化 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildChangeInfoMsg_31_16
  	{

    public uint guildId = 0;
    public string guildName = "";

    public static int getCode()
    {
        // (31, 16)
        return 7952;
    }

    public void read(MemoryStream msdata)
    {
        guildId = proto_util.readUInt(msdata);
        guildName = proto_util.readString(msdata);
    }
   }
}