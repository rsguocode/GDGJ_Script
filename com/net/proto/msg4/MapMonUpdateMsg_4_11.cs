/**
 * 更新p_map_mon部分数据 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapMonUpdateMsg_4_11
  	{

    public uint id = 0;
    public List<PItem> changeList = new List<PItem>();

    public static int getCode()
    {
        // (4, 11)
        return 1035;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        PItem.readLoop(msdata, changeList);
    }
   }
}