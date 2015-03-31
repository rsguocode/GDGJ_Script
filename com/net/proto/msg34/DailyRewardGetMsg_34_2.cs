/**
 * 领取奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DailyRewardGetMsg_34_2
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (34, 2)
        return 8706;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}