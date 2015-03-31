/**
 * 技能位置变化 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillPosChangeMsg_21_15
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 15)
        return 5391;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}