/**
 * 玩家离开视野 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapRoleLeaveMsg_4_5
  	{

    public List<uint> roles = new List<uint>();

    public static int getCode()
    {
        // (4, 5)
        return 1029;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, roles);
    }
   }
}