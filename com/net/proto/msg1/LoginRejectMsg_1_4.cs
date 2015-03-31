/**
 * 服务端主动断开链接 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginRejectMsg_1_4
  	{

    public byte code = 0;

    public static int getCode()
    {
        // (1, 4)
        return 260;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUByte(msdata);
    }
   }
}