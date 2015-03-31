/**
 * 种子背包信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmSeedBagMsg_30_5
  	{

    public List<PSeed> seed = new List<PSeed>();

    public static int getCode()
    {
        // (30, 5)
        return 7685;
    }

    public void read(MemoryStream msdata)
    {
        PSeed.readLoop(msdata, seed);
    }
   }
}