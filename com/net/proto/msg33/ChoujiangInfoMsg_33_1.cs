/**
 * 基础信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChoujiangInfoMsg_33_1
  	{

    public ushort code = 0;
    public bool first = false;
    public uint freeCd = 0;

    public static int getCode()
    {
        // (33, 1)
        return 8449;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        first = proto_util.readBool(msdata);
        freeCd = proto_util.readUInt(msdata);
    }
   }
}