/**
 * 好友升级 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationFriendsLvlMsg_7_12
  	{

    public uint roleId = 0;
    public byte lvl = 0;

    public static int getCode()
    {
        // (7, 12)
        return 1804;
    }

    public void read(MemoryStream msdata)
    {
        roleId = proto_util.readUInt(msdata);
        lvl = proto_util.readUByte(msdata);
    }
   }
}