/**
 * 请求小喇叭队列长度 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatHornLengthMsg_10_5
  	{

    public byte length = 0;

    public static int getCode()
    {
        // (10, 5)
        return 2565;
    }

    public void read(MemoryStream msdata)
    {
        length = proto_util.readUByte(msdata);
    }
   }
}