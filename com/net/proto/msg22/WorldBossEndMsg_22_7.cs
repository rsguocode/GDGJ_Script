/**
 * 结束通知 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossEndMsg_22_7
  	{

    public byte code = 0;
    public uint time = 0;

    public static int getCode()
    {
        // (22, 7)
        return 5639;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUByte(msdata);
        time = proto_util.readUInt(msdata);
    }
   }
}