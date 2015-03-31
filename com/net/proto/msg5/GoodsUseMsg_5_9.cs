/**
 * 使用物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsUseMsg_5_9
  	{

    public ushort code = 0;
    public ushort goodsId = 0;

    public static int getCode()
    {
        // (5, 9)
        return 1289;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        goodsId = proto_util.readUShort(msdata);
    }
   }
}