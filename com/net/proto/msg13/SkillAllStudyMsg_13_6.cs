/**
 * 一键学习 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillAllStudyMsg_13_6
  	{

    public ushort code = 0;
    public List<uint> skillids = new List<uint>();

    public static int getCode()
    {
        // (13, 6)
        return 3334;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        proto_util.readLoopUInt(msdata, skillids);
    }
   }
}