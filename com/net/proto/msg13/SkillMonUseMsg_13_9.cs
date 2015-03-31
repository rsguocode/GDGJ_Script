/**
 * 怪物使用技能 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillMonUseMsg_13_9
  	{

    public byte seq = 0;
    public uint skillId = 0;
    public byte dir = 0;
    public byte type = 0;
    public uint actId = 0;
    public uint objId = 0;

    public static int getCode()
    {
        // (13, 9)
        return 3337;
    }

    public void read(MemoryStream msdata)
    {
        seq = proto_util.readUByte(msdata);
        skillId = proto_util.readUInt(msdata);
        dir = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
        actId = proto_util.readUInt(msdata);
        objId = proto_util.readUInt(msdata);
    }
   }
}