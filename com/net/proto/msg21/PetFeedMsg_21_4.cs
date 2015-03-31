/**
 * 请求宠物喂养 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetFeedMsg_21_4
  	{

    public ushort code = 0;
    public uint id = 0;

    public static int getCode()
    {
        // (21, 4)
        return 5380;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
    }
   }
}