/**
 * 技能遗忘 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillForgetMsg_21_13
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 13)
        return 5389;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}