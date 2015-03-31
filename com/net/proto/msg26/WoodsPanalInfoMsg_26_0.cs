/**
 * 面板信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsPanalInfoMsg_26_0
  	{

    public List<PWoodsPlayer> players = new List<PWoodsPlayer>();

    public static int getCode()
    {
        // (26, 0)
        return 6656;
    }

    public void read(MemoryStream msdata)
    {
        PWoodsPlayer.readLoop(msdata, players);
    }
   }
}