/**
 * 技能信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillInfoMsg_13_4
  	{

    public List<uint> skillids = new List<uint>();
    public uint restPoint = 0;

    public static int getCode()
    {
        // (13, 4)
        return 3332;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, skillids);
        restPoint = proto_util.readUInt(msdata);
    }
   }
}