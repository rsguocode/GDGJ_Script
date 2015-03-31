/**
 * 技能使用同步,客户端发起(暂时) (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillUseSyncMsg_13_11
  	{

    public uint id = 0;
    public byte type = 0;
    public uint skillId = 0;
    public byte dir = 0;

    public static int getCode()
    {
        // (13, 11)
        return 3339;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
        skillId = proto_util.readUInt(msdata);
        dir = proto_util.readUByte(msdata);
    }
   }
}