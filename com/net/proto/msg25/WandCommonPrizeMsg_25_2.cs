/**
 * 魔杖开奖记录 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandCommonPrizeMsg_25_2
  	{

    public List<PWandInfo> commonList = new List<PWandInfo>();

    public static int getCode()
    {
        // (25, 2)
        return 6402;
    }

    public void read(MemoryStream msdata)
    {
        PWandInfo.readLoop(msdata, commonList);
    }
   }
}