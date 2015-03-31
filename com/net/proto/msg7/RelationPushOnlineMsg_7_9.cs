/**
 * 上线通知 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationPushOnlineMsg_7_9
  	{

    public uint roleId = 0;
    public byte isOnline = 0;

    public static int getCode()
    {
        // (7, 9)
        return 1801;
    }

    public void read(MemoryStream msdata)
    {
        roleId = proto_util.readUInt(msdata);
        isOnline = proto_util.readUByte(msdata);
    }
   }
}