/**
 * 购买技能点 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetBuySkillPointMsg_21_9
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (21, 9)
        return 5385;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}