/**
 * 个人历史挑战记录 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesHistoryPeronalMsg_19_8
  	{

    public List<PHeroesHistory> history = new List<PHeroesHistory>();

    public static int getCode()
    {
        // (19, 8)
        return 4872;
    }

    public void read(MemoryStream msdata)
    {
        PHeroesHistory.readLoop(msdata, history);
    }
   }
}