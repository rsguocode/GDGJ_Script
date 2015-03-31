/**
 * 更新p_map_role部分数据 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapRoleUpdateMsg_4_10
  	{

    public uint id = 0;
    public List<PItem> changeList = new List<PItem>();

    public static int getCode()
    {
        // (4, 10)
        return 1034;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        PItem.readLoop(msdata, changeList);
    }
   }
}