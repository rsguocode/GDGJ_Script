/**
 * 返回应答协议 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsReplyAnswerMsg_26_25
  	{

    public ushort code = 0;
    public string name = "";

    public static int getCode()
    {
        // (26, 25)
        return 6681;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        name = proto_util.readString(msdata);
    }
   }
}