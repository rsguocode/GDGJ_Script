/**
 * 技能升级 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillUpMsg_21_12
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 12)
        return 5388;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}