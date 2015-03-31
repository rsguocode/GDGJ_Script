/**
 * 战况（在登陆时下发） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossInfoMsg_22_9
  	{

    public uint hpfull = 0;
    public uint damage = 0;
    public uint gold = 0;
    public uint repu = 0;

    public static int getCode()
    {
        // (22, 9)
        return 5641;
    }

    public void read(MemoryStream msdata)
    {
        hpfull = proto_util.readUInt(msdata);
        damage = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
    }
   }
}