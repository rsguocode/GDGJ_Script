/**
 * 打开魔杖 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandDrawMsg_25_5
  	{

    public ushort code = 0;
    public byte type = 0;
    public uint subType = 0;
    public uint count = 0;

    public static int getCode()
    {
        // (25, 5)
        return 6405;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        subType = proto_util.readUInt(msdata);
        count = proto_util.readUInt(msdata);
    }
   }
}