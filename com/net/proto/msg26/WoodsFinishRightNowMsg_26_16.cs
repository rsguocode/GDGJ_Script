/**
 * 疾风术 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsFinishRightNowMsg_26_16
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (26, 16)
        return 6672;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}