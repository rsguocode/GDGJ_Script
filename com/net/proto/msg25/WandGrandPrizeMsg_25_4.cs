/**
 * 魔杖活动至尊大奖 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandGrandPrizeMsg_25_4
  	{

    public List<PWandCommonInfo> commonList = new List<PWandCommonInfo>();

    public static int getCode()
    {
        // (25, 4)
        return 6404;
    }

    public void read(MemoryStream msdata)
    {
        PWandCommonInfo.readLoop(msdata, commonList);
    }
   }
}