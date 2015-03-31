/**
 * 通过会长名字查询 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildSearchByOwenerMsg_31_10
  	{

    public ushort code = 0;
    public List<PGuildBase> list = new List<PGuildBase>();

    public static int getCode()
    {
        // (31, 10)
        return 7946;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PGuildBase.readLoop(msdata, list);
    }
   }
}