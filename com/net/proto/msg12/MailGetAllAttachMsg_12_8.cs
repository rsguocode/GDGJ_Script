/**
 * 一键提取附件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailGetAllAttachMsg_12_8
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (12, 8)
        return 3080;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}