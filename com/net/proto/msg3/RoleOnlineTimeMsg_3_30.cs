/**
 * 角色在线时间 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleOnlineTimeMsg_3_30
  	{

    public uint time = 0;
    public byte cm = 0;

    public static int getCode()
    {
        // (3, 30)
        return 798;
    }

    public void read(MemoryStream msdata)
    {
        time = proto_util.readUInt(msdata);
        cm = proto_util.readUByte(msdata);
    }
   }
}