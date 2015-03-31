/**
 * 完成冒险 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsFinishAdvenMsg_26_21
  	{

    public uint repu = 0;
    public uint gold = 0;

    public static int getCode()
    {
        // (26, 21)
        return 6677;
    }

    public void read(MemoryStream msdata)
    {
        repu = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
    }
   }
}