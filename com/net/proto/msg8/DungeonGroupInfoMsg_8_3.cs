/**
 * 副本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonGroupInfoMsg_8_3
  	{

    public List<PDungeonMapInfo> group = new List<PDungeonMapInfo>();
    public List<uint> cities = new List<uint>();

    public static int getCode()
    {
        // (8, 3)
        return 2051;
    }

    public void read(MemoryStream msdata)
    {
        PDungeonMapInfo.readLoop(msdata, group);
        proto_util.readLoopUInt(msdata, cities);
    }
   }
}