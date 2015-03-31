/**
 * 怪物离开场景 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMonLeaveMsg_4_8
  	{

    public List<uint> ids = new List<uint>();

    public static int getCode()
    {
        // (4, 8)
        return 1032;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, ids);
    }
   }
}