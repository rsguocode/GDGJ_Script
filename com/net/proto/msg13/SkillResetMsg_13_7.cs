/**
 * 重置 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillResetMsg_13_7
  	{

    public ushort code = 0;
    public List<uint> skillids = new List<uint>();

    public static int getCode()
    {
        // (13, 7)
        return 3335;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        proto_util.readLoopUInt(msdata, skillids);
    }
   }
}