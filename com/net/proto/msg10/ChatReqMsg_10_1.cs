/**
 * 聊天请求，包含世界，公会 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatReqMsg_10_1
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (10, 1)
        return 2561;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}