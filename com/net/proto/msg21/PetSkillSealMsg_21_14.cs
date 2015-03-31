/**
 * 技能封印 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillSealMsg_21_14
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 14)
        return 5390;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}