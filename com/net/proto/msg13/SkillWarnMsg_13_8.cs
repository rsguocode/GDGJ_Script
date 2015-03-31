/**
 * 技能预警 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillWarnMsg_13_8
  	{

    public byte type = 0;
    public uint actId = 0;
    public uint skillId = 0;

    public static int getCode()
    {
        // (13, 8)
        return 3336;
    }

    public void read(MemoryStream msdata)
    {
        type = proto_util.readUByte(msdata);
        actId = proto_util.readUInt(msdata);
        skillId = proto_util.readUInt(msdata);
    }
   }
}