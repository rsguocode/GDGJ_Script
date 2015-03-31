/**
 * 推送邀请应答 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsPushReplyInviteMsg_26_22
  	{

    public byte result = 0;

    public static int getCode()
    {
        // (26, 22)
        return 6678;
    }

    public void read(MemoryStream msdata)
    {
        result = proto_util.readUByte(msdata);
    }
   }
}