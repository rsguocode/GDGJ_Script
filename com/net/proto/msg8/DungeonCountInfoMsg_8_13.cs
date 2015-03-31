/**
 * 副本计数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonCountInfoMsg_8_13
  	{

    public List<PDungeonCount> dungeonCount = new List<PDungeonCount>();

    public static int getCode()
    {
        // (8, 13)
        return 2061;
    }

    public void read(MemoryStream msdata)
    {
        PDungeonCount.readLoop(msdata, dungeonCount);
    }
   }
}