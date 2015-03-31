/**
 * 技能点信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillPointInfoMsg_21_7
  	{

    public byte point = 0;
    public uint timestamp = 0;
    public ushort buyTimes = 0;

    public static int getCode()
    {
        // (21, 7)
        return 5383;
    }

    public void read(MemoryStream msdata)
    {
        point = proto_util.readUByte(msdata);
        timestamp = proto_util.readUInt(msdata);
        buyTimes = proto_util.readUShort(msdata);
    }
   }
}