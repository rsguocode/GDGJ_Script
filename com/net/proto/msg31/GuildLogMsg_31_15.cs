/**
 * 公会日志 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildLogMsg_31_15
  	{

    public ushort code = 0;
    public List<PGuildLog> list = new List<PGuildLog>();

    public static int getCode()
    {
        // (31, 15)
        return 7951;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PGuildLog.readLoop(msdata, list);
    }
   }
}