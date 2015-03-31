/**
 * 捐献查询 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class NationDonateInfoMsg_9_4
  	{

    public ushort code = 0;
    public uint gold = 0;
    public uint diam = 0;

    public static int getCode()
    {
        // (9, 4)
        return 2308;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
    }
   }
}