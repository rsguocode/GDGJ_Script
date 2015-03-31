/**
 * 公会基本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildBasicInfoMsg_31_6
  	{

    public ushort code = 0;
    public uint guildId = 0;
    public string guildName = "";
    public uint rank = 0;
    public byte level = 0;
    public uint curExp = 0;
    public byte memberNum = 0;
    public byte memberMax = 0;
    public string announcement = "";

    public static int getCode()
    {
        // (31, 6)
        return 7942;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        guildId = proto_util.readUInt(msdata);
        guildName = proto_util.readString(msdata);
        rank = proto_util.readUInt(msdata);
        level = proto_util.readUByte(msdata);
        curExp = proto_util.readUInt(msdata);
        memberNum = proto_util.readUByte(msdata);
        memberMax = proto_util.readUByte(msdata);
        announcement = proto_util.readString(msdata);
    }
   }
}