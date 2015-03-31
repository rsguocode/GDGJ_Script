/**
 * 玩家进入视野 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapRoleEnterMsg_4_4
  	{

    public List<PMapRole> roles = new List<PMapRole>();

    public static int getCode()
    {
        // (4, 4)
        return 1028;
    }

    public void read(MemoryStream msdata)
    {
        PMapRole.readLoop(msdata, roles);
    }
   }
}