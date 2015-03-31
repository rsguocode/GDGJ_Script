/**
 * 新邮件通知 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailNewMailInformMsg_12_6
  	{

    public ushort code = 0;
    public byte mailCount = 0;

    public static int getCode()
    {
        // (12, 6)
        return 3078;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        mailCount = proto_util.readUByte(msdata);
    }
   }
}