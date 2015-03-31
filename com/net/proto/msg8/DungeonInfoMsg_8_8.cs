/**
 * 副本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonInfoMsg_8_8
  	{

    public uint expire = 0;

    public static int getCode()
    {
        // (8, 8)
        return 2056;
    }

    public void read(MemoryStream msdata)
    {
        expire = proto_util.readUInt(msdata);
    }
   }
}