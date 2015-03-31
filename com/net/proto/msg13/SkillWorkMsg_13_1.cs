/**
 * 技能确认 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillWorkMsg_13_1
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (13, 1)
        return 3329;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}