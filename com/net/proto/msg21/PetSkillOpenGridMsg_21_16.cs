/**
 * 技能开格子 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillOpenGridMsg_21_16
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 16)
        return 5392;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}