/**
 * 领取额外奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonExtRewardFetchMsg_8_9
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (8, 9)
        return 2057;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}