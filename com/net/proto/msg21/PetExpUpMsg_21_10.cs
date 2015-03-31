/**
 * 宠物经验升级 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetExpUpMsg_21_10
  	{

    public ushort code = 0;
    public uint petId = 0;

    public static int getCode()
    {
        // (21, 10)
        return 5386;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        petId = proto_util.readUInt(msdata);
    }
   }
}