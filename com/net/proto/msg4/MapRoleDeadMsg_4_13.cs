/**
 * 角色死亡 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapRoleDeadMsg_4_13
  	{

    public uint id = 0;

    public static int getCode()
    {
        // (4, 13)
        return 1037;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
    }
   }
}