/**
 * 获得奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class VipGetRewardMsg_32_2
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (32, 2)
        return 8194;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}