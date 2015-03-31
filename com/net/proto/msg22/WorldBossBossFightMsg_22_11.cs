/**
 * boss攻击 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossBossFightMsg_22_11
  	{

    public uint skillId = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (22, 11)
        return 5643;
    }

    public void read(MemoryStream msdata)
    {
        skillId = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}