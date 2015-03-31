/**
 * 签到信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GiftSignInfoMsg_29_3
  	{

    public byte times = 0;
    public byte status = 0;

    public static int getCode()
    {
        // (29, 3)
        return 7427;
    }

    public void read(MemoryStream msdata)
    {
        times = proto_util.readUByte(msdata);
        status = proto_util.readUByte(msdata);
    }
   }
}