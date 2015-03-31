/**
 * 七天登陆礼包 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GiftLoginGiftStatusMsg_29_0
  	{

    public byte day = 0;
    public byte status = 0;

    public static int getCode()
    {
        // (29, 0)
        return 7424;
    }

    public void read(MemoryStream msdata)
    {
        day = proto_util.readUByte(msdata);
        status = proto_util.readUByte(msdata);
    }
   }
}