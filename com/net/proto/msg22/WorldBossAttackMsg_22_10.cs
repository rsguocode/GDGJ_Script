/**
 * 角色攻击 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossAttackMsg_22_10
  	{

    public ushort code = 0;
    public uint hurt = 0;

    public static int getCode()
    {
        // (22, 10)
        return 5642;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        hurt = proto_util.readUInt(msdata);
    }
   }
}