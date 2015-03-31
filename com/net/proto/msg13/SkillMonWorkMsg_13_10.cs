/**
 * 怪物使用技能确认 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillMonWorkMsg_13_10
  	{

    public byte seq = 0;
    public uint skillId = 0;
    public byte dir = 0;
    public List<PTarget> target = new List<PTarget>();

    public static int getCode()
    {
        // (13, 10)
        return 3338;
    }

    public void read(MemoryStream msdata)
    {
        seq = proto_util.readUByte(msdata);
        skillId = proto_util.readUInt(msdata);
        dir = proto_util.readUByte(msdata);
        PTarget.readLoop(msdata, target);
    }
   }
}