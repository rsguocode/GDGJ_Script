/**
 * 删除邮件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailDelMailMsg_12_3
  	{

    public ushort code = 0;
    public List<uint> mailIds = new List<uint>();
    public byte totalCount = 0;

    public static int getCode()
    {
        // (12, 3)
        return 3075;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        proto_util.readLoopUInt(msdata, mailIds);
        totalCount = proto_util.readUByte(msdata);
    }
   }
}