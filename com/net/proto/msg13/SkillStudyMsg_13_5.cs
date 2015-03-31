/**
 * 学习技能 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillStudyMsg_13_5
  	{

    public ushort code = 0;
    public uint skillId = 0;
    public uint nextId = 0;

    public static int getCode()
    {
        // (13, 5)
        return 3333;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        skillId = proto_util.readUInt(msdata);
        nextId = proto_util.readUInt(msdata);
    }
   }
}