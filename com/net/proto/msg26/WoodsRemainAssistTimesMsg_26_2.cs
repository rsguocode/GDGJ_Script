/**
 * 剩余协助次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRemainAssistTimesMsg_26_2
  	{

    public byte times = 0;

    public static int getCode()
    {
        // (26, 2)
        return 6658;
    }

    public void read(MemoryStream msdata)
    {
        times = proto_util.readUByte(msdata);
    }
   }
}