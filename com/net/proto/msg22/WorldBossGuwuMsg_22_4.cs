/**
 * 鼓舞 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossGuwuMsg_22_4
  	{

    public ushort code = 0;
    public byte type = 0;
    public uint fight = 0;
    public uint count = 0;

    public static int getCode()
    {
        // (22, 4)
        return 5636;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        fight = proto_util.readUInt(msdata);
        count = proto_util.readUInt(msdata);
    }
   }
}