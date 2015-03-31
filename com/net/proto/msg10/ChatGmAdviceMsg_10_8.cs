/**
 * GM反馈 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatGmAdviceMsg_10_8
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (10, 8)
        return 2568;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}