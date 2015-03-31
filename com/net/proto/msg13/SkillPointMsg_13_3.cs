/**
 * 技能点信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillPointMsg_13_3
  	{

    public uint restPoint = 0;

    public static int getCode()
    {
        // (13, 3)
        return 3331;
    }

    public void read(MemoryStream msdata)
    {
        restPoint = proto_util.readUInt(msdata);
    }
   }
}