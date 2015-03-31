/**
 * 获取排名接近的玩家 供挑战使用 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesNearMsg_19_2
  	{

    public List<PHeroes> list = new List<PHeroes>();

    public static int getCode()
    {
        // (19, 2)
        return 4866;
    }

    public void read(MemoryStream msdata)
    {
        PHeroes.readLoop(msdata, list);
    }
   }
}