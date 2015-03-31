/**
 * 客户端信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginClientInfoMsg_1_6
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (1, 6)
        return 262;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}