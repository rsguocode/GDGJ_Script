/**
 * boss血量 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossHpMsg_22_6
  	{

    public uint currHp = 0;
    public uint fullHp = 0;
    public uint damage = 0;

    public static int getCode()
    {
        // (22, 6)
        return 5638;
    }

    public void read(MemoryStream msdata)
    {
        currHp = proto_util.readUInt(msdata);
        fullHp = proto_util.readUInt(msdata);
        damage = proto_util.readUInt(msdata);
    }
   }
}