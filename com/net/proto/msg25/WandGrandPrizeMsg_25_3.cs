/**
 * 魔杖活动至尊大奖 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandGrandPrizeMsg_25_3
  	{

    public List<PWandInfo> grandList = new List<PWandInfo>();

    public static int getCode()
    {
        // (25, 3)
        return 6403;
    }

    public void read(MemoryStream msdata)
    {
        PWandInfo.readLoop(msdata, grandList);
    }
   }
}