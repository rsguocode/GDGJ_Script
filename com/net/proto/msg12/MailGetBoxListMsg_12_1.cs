/**
 * 收件箱邮件列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailGetBoxListMsg_12_1
  	{

    public ushort code = 0;
    public byte totalCount = 0;
    public List<PMailBasicInfo> mailList = new List<PMailBasicInfo>();

    public static int getCode()
    {
        // (12, 1)
        return 3073;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        totalCount = proto_util.readUByte(msdata);
        PMailBasicInfo.readLoop(msdata, mailList);
    }
   }
}