/**
 * 领取超级副本奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonGetSuperRewardMsg_8_18
  	{

    public ushort code = 0;
    public List<PSuperDungeonReward> list = new List<PSuperDungeonReward>();

    public static int getCode()
    {
        // (8, 18)
        return 2066;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PSuperDungeonReward.readLoop(msdata, list);
    }
   }
}