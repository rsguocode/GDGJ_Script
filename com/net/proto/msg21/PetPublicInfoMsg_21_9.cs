/**
 * 宠物公共信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetPublicInfoMsg_21_9
  	{

    public uint growMaxLv = 0;
    public uint exp = 0;
    public List<uint> skillBag = new List<uint>();

    public static int getCode()
    {
        // (21, 9)
        return 5385;
    }

    public void read(MemoryStream msdata)
    {
        growMaxLv = proto_util.readUInt(msdata);
        exp = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, skillBag);
    }
   }
}