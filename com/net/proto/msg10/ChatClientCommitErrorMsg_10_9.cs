/**
 * 客户端提交错误 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatClientCommitErrorMsg_10_9
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (10, 9)
        return 2569;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}