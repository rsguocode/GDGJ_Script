/**
 * 怪物死亡增加金币 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonGoldTreeAddGoldMsg_8_14
  	{

    public uint monid = 0;
    public uint gold = 0;

    public static int getCode()
    {
        // (8, 14)
        return 2062;
    }

    public void read(MemoryStream msdata)
    {
        monid = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
    }
   }
}