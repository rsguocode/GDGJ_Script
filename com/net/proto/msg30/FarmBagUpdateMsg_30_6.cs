/**
 * 种子背包更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmBagUpdateMsg_30_6
  	{

    public List<PSeed> seed = new List<PSeed>();

    public static int getCode()
    {
        // (30, 6)
        return 7686;
    }

    public void read(MemoryStream msdata)
    {
        PSeed.readLoop(msdata, seed);
    }
   }
}