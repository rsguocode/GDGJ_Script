/**
 * 开始冒险 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsStartAdvenMsg_26_14
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (26, 14)
        return 6670;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}