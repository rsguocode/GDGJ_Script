/**
 * 好友当前最大数量 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationFriendsCountMsg_7_17
  	{

    public ushort nowCount = 0;
    public ushort maxCount = 0;

    public static int getCode()
    {
        // (7, 17)
        return 1809;
    }

    public void read(MemoryStream msdata)
    {
        nowCount = proto_util.readUShort(msdata);
        maxCount = proto_util.readUShort(msdata);
    }
   }
}