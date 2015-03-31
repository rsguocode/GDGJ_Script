/**
 * 一键删除所有邮件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailDelAllMsg_12_7
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (12, 7)
        return 3079;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}