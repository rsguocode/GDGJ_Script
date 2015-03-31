/**
 * push邀请好友请求 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationPushAcceptMsg_7_3
  	{

    public uint roleId = 0;
    public string name = "";
    public byte job = 0;
    public byte lvl = 0;
    public byte sex = 0;
    public byte vip = 0;
    public uint fightpoint = 0;

    public static int getCode()
    {
        // (7, 3)
        return 1795;
    }

    public void read(MemoryStream msdata)
    {
        roleId = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        job = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        vip = proto_util.readUByte(msdata);
        fightpoint = proto_util.readUInt(msdata);
    }
   }
}