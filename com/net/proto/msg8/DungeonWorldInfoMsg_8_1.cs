/**
 * 世界地图信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonWorldInfoMsg_8_1
  	{

    public List<uint> ids = new List<uint>();

    public static int getCode()
    {
        // (8, 1)
        return 2049;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, ids);
    }
   }
}