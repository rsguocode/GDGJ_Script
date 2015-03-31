/**
 * 魔杖活动大奖记录 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandSuperPrizeMsg_25_3
  	{

    public List<PWandSuperInfo> superList = new List<PWandSuperInfo>();

    public static int getCode()
    {
        // (25, 3)
        return 6403;
    }

    public void read(MemoryStream msdata)
    {
        PWandSuperInfo.readLoop(msdata, superList);
    }
   }
}