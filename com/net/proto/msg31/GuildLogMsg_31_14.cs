/**
 * 公会日志 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildLogMsg_31_14
  	{

    public List<PGuildLog> list = new List<PGuildLog>();

    public static int getCode()
    {
        // (31, 14)
        return 7950;
    }

    public void read(MemoryStream msdata)
    {
        PGuildLog.readLoop(msdata, list);
    }
   }
}