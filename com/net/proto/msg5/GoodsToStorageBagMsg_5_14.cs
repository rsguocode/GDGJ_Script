/**
 * 仓库背包交换位置 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsToStorageBagMsg_5_14
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (5, 14)
        return 1294;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}