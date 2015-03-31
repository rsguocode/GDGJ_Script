/**
 * 好友农场信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmFriendInfoMsg_30_4
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte lvl = 0;
    public uint exp = 0;
    public uint fullExp = 0;
    public List<PLand> land = new List<PLand>();

    public static int getCode()
    {
        // (30, 4)
        return 7684;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        lvl = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
        fullExp = proto_util.readUInt(msdata);
        PLand.readLoop(msdata, land);
    }
   }
}