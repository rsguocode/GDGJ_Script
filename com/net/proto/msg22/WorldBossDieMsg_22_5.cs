/**
 * boss死亡 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossDieMsg_22_5
  	{

    public uint gold = 0;
    public uint extGold = 0;
    public uint repu = 0;
    public uint extRepu = 0;

    public static int getCode()
    {
        // (22, 5)
        return 5637;
    }

    public void read(MemoryStream msdata)
    {
        gold = proto_util.readUInt(msdata);
        extGold = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
        extRepu = proto_util.readUInt(msdata);
    }
   }
}