/**
 * 限购物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MallBuyLimitGoodsMsg_15_3
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (15, 3)
        return 3843;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}