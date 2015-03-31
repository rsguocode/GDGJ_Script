/**
 * 清除抢劫cd时间 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsClearRobCdMsg_26_5
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (26, 5)
        return 6661;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}