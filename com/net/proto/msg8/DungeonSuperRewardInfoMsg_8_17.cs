/**
 * 精英副本奖励信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonSuperRewardInfoMsg_8_17
  	{

    public List<PSuperDungeonReward> list = new List<PSuperDungeonReward>();

    public static int getCode()
    {
        // (8, 17)
        return 2065;
    }

    public void read(MemoryStream msdata)
    {
        PSuperDungeonReward.readLoop(msdata, list);
    }
   }
}