/**
 * 刷新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TreasRefreshMsg_27_2
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (27, 2)
        return 6914;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}