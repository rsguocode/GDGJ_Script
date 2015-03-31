/**
 * 增加冒险玩家 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsInsertPlayerMsg_26_9
  	{

    public List<PWoodsPlayer> playerList = new List<PWoodsPlayer>();

    public static int getCode()
    {
        // (26, 9)
        return 6665;
    }

    public void read(MemoryStream msdata)
    {
        PWoodsPlayer.readLoop(msdata, playerList);
    }
   }
}