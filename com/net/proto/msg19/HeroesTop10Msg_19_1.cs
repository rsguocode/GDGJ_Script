/**
 * 前十玩家 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesTop10Msg_19_1
  	{

    public List<PHeroes> list = new List<PHeroes>();

    public static int getCode()
    {
        // (19, 1)
        return 4865;
    }

    public void read(MemoryStream msdata)
    {
        PHeroes.readLoop(msdata, list);
    }
   }
}