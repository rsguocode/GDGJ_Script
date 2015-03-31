/**
 * 历史挑战记录 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesHistoryMsg_19_7
  	{

    public List<PHistory> history = new List<PHistory>();

    public static int getCode()
    {
        // (19, 7)
        return 4871;
    }

    public void read(MemoryStream msdata)
    {
        PHistory.readLoop(msdata, history);
    }
   }
}