/**
 * 删除冒险玩家信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsClearPlayersMsg_26_8
  	{

    public uint playerId = 0;

    public static int getCode()
    {
        // (26, 8)
        return 6664;
    }

    public void read(MemoryStream msdata)
    {
        playerId = proto_util.readUInt(msdata);
    }
   }
}