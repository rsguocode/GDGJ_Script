/**
 * 提取邮件附件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailTakeAttachMsg_12_4
  	{

    public ushort code = 0;
    public uint mailId = 0;

    public static int getCode()
    {
        // (12, 4)
        return 3076;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        mailId = proto_util.readUInt(msdata);
    }
   }
}