/**
 * 加速成长 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmSpeedSeedUpMsg_30_8
  	{

    public ushort code = 0;
    public byte pos = 0;
    public uint remainTime = 0;

    public static int getCode()
    {
        // (30, 8)
        return 7688;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        pos = proto_util.readUByte(msdata);
        remainTime = proto_util.readUInt(msdata);
    }
   }
}