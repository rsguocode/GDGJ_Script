/**
 * 累计奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossAccRewardMsg_22_2
  	{

    public uint hurtTotal = 0;
    public uint silver = 0;
    public uint repu = 0;

    public static int getCode()
    {
        // (22, 2)
        return 5634;
    }

    public void read(MemoryStream msdata)
    {
        hurtTotal = proto_util.readUInt(msdata);
        silver = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
    }
   }
}