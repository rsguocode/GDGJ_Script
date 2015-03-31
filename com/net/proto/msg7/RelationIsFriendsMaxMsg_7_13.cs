/**
 * 好友数是否最大 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationIsFriendsMaxMsg_7_13
  	{

    public ushort code = 0;
    public uint roleId = 0;

    public static int getCode()
    {
        // (7, 13)
        return 1805;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        roleId = proto_util.readUInt(msdata);
    }
   }
}