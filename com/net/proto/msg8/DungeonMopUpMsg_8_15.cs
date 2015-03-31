/**
 * 副本扫荡 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonMopUpMsg_8_15
  	{

    public ushort code = 0;
    public uint mapid = 0;
    public List<PDungeonReward> reward = new List<PDungeonReward>();

    public static int getCode()
    {
        // (8, 15)
        return 2063;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        mapid = proto_util.readUInt(msdata);
        PDungeonReward.readLoop(msdata, reward);
    }
   }
}