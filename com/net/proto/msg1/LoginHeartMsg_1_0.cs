/**
 * 心跳 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginHeartMsg_1_0
  	{

    public uint serverTime = 0;

    public static int getCode()
    {
        // (1, 0)
        return 256;
    }

    public void read(MemoryStream msdata)
    {
        serverTime = proto_util.readUInt(msdata);
    }
   }
}