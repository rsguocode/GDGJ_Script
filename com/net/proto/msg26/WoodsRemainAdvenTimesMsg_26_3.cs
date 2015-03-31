/**
 * 剩余冒险次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRemainAdvenTimesMsg_26_3
  	{

    public byte times = 0;

    public static int getCode()
    {
        // (26, 3)
        return 6659;
    }

    public void read(MemoryStream msdata)
    {
        times = proto_util.readUByte(msdata);
    }
   }
}