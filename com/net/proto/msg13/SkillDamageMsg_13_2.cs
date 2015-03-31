/**
 * 技能伤害 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillDamageMsg_13_2
  	{

    public byte seq = 0;
    public uint skillId = 0;
    public byte type = 0;
    public uint actId = 0;
    public List<PDamage> damage = new List<PDamage>();

    public static int getCode()
    {
        // (13, 2)
        return 3330;
    }

    public void read(MemoryStream msdata)
    {
        seq = proto_util.readUByte(msdata);
        skillId = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
        actId = proto_util.readUInt(msdata);
        PDamage.readLoop(msdata, damage);
    }
   }
}