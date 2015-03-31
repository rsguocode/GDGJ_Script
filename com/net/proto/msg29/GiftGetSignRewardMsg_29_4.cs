/**
 * 领取签到奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GiftGetSignRewardMsg_29_4
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (29, 4)
        return 7428;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}