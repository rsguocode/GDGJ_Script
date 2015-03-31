/**
 * 领取礼包 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GiftGetLoginGiftMsg_29_1
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (29, 1)
        return 7425;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}