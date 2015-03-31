/**
 * 摘除宝石 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltRemoveMsg_14_12
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte repos = 0;
    public byte pos = 0;

    public static int getCode()
    {
        // (14, 12)
        return 3596;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
        pos = proto_util.readUByte(msdata);
    }
   }
}