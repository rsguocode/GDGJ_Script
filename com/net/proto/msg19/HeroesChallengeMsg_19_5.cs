/**
 * 挑战 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesChallengeMsg_19_5
  	{

    public ushort code = 0;
    public ushort pos = 0;
    public uint cd = 0;
    public byte result = 0;

    public static int getCode()
    {
        // (19, 5)
        return 4869;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        pos = proto_util.readUShort(msdata);
        cd = proto_util.readUInt(msdata);
        result = proto_util.readUByte(msdata);
    }
   }
}