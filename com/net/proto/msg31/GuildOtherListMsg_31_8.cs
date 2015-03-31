/**
 * 其他公会信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildOtherListMsg_31_8
  	{

    public ushort code = 0;
    public ushort sum = 0;
    public List<PGuildBase> list = new List<PGuildBase>();

    public static int getCode()
    {
        // (31, 8)
        return 7944;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        sum = proto_util.readUShort(msdata);
        PGuildBase.readLoop(msdata, list);
    }
   }
}