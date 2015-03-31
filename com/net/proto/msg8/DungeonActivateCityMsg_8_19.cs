/**
 * 激活女神 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonActivateCityMsg_8_19
  	{

    public ushort code = 0;
    public uint cityid = 0;

    public static int getCode()
    {
        // (8, 19)
        return 2067;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        cityid = proto_util.readUInt(msdata);
    }
   }
}