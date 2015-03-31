/**
 * 领取福利礼包 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GiftGetMsg_29_2
  	{

    public ushort code = 0;
    public ushort type = 0;

    public static int getCode()
    {
        // (29, 2)
        return 7426;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUShort(msdata);
    }
   }
}