/**
 * 更新玩家被打劫的次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsUpdateRobTimesMsg_26_10
  	{

    public uint playerId = 0;
    public byte robTimes = 0;

    public static int getCode()
    {
        // (26, 10)
        return 6666;
    }

    public void read(MemoryStream msdata)
    {
        playerId = proto_util.readUInt(msdata);
        robTimes = proto_util.readUByte(msdata);
    }
   }
}