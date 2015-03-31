/**
 * 打劫cd (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRobCdMsg_26_4
  	{

    public ushort time = 0;

    public static int getCode()
    {
        // (26, 4)
        return 6660;
    }

    public void read(MemoryStream msdata)
    {
        time = proto_util.readUShort(msdata);
    }
   }
}