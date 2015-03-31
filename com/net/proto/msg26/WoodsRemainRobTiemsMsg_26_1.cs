/**
 * 剩余打劫次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRemainRobTiemsMsg_26_1
  	{

    public byte times = 0;

    public static int getCode()
    {
        // (26, 1)
        return 6657;
    }

    public void read(MemoryStream msdata)
    {
        times = proto_util.readUByte(msdata);
    }
   }
}