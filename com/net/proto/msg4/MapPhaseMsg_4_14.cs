/**
 * 地图阶段 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapPhaseMsg_4_14
  	{

    public ushort phase = 0;
    public List<uint> box = new List<uint>();

    public static int getCode()
    {
        // (4, 14)
        return 1038;
    }

    public void read(MemoryStream msdata)
    {
        phase = proto_util.readUShort(msdata);
        proto_util.readLoopUInt(msdata, box);
    }
   }
}