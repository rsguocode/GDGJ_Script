/**
 * 捐献 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class NationDonateMsg_9_3
  	{

    public ushort code = 0;
    public uint gold = 0;
    public uint diam = 0;

    public static int getCode()
    {
        // (9, 3)
        return 2307;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
    }
   }
}