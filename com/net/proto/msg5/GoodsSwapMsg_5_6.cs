/**
 * 交换位置 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsSwapMsg_5_6
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (5, 6)
        return 1286;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}