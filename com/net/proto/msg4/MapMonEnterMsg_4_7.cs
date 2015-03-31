/**
 * 怪物进入场景 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMonEnterMsg_4_7
  	{

    public List<PMapMon> mons = new List<PMapMon>();

    public static int getCode()
    {
        // (4, 7)
        return 1031;
    }

    public void read(MemoryStream msdata)
    {
        PMapMon.readLoop(msdata, mons);
    }
   }
}