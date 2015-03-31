/**
 * 修改公会公告 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildModifyAnnouncementMsg_31_5
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (31, 5)
        return 7941;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}