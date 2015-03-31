/**
 * 技能使用 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillUseMsg_13_0
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (13, 0)
        return 3328;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}