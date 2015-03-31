/**
 * 玩家鼓舞情况（在登陆时下发） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossSelfGuwuMsg_22_8
  	{

    public uint attTime = 0;
    public uint dmageTime = 0;

    public static int getCode()
    {
        // (22, 8)
        return 5640;
    }

    public void read(MemoryStream msdata)
    {
        attTime = proto_util.readUInt(msdata);
        dmageTime = proto_util.readUInt(msdata);
    }
   }
}