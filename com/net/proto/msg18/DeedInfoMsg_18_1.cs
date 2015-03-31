/**
 * 信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DeedInfoMsg_18_1
  	{

    public ushort code = 0;
    public List<uint> id = new List<uint>();
    public List<uint> count = new List<uint>();

    public static int getCode()
    {
        // (18, 1)
        return 4609;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        proto_util.readLoopUInt(msdata, id);
        proto_util.readLoopUInt(msdata, count);
    }
   }
}