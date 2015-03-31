/**
 * 拆分 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsSplitMsg_5_7
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (5, 7)
        return 1287;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}