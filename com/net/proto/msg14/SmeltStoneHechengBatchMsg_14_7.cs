/**
 * 批量合成宝石 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltStoneHechengBatchMsg_14_7
  	{

    public ushort code = 0;
    public uint id = 0;
    public uint count = 0;

    public static int getCode()
    {
        // (14, 7)
        return 3591;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        count = proto_util.readUInt(msdata);
    }
   }
}