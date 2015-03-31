/**
 * 推送邀请应答 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsReplyInviteMsg_26_12
  	{

    public byte code = 0;

    public static int getCode()
    {
        // (26, 12)
        return 6668;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUByte(msdata);
    }
   }
}