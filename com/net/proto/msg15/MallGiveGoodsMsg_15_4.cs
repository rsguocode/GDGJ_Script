/**
 * 赠送物品 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MallGiveGoodsMsg_15_4
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (15, 4)
        return 3844;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}