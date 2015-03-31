/**
 * 打劫结果 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRobResultMsg_26_20
  	{

    public uint repu = 0;
    public uint gold = 0;

    public static int getCode()
    {
        // (26, 20)
        return 6676;
    }

    public void read(MemoryStream msdata)
    {
        repu = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
    }
   }
}