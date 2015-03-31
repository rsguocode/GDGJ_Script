/**
 * 伤害排行榜信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossRankMsg_22_3
  	{

    public uint hurtTotal = 0;
    public uint roleTotal = 0;
    public List<uint> hurt = new List<uint>();
    public List<String> name = new List<String>();

    public static int getCode()
    {
        // (22, 3)
        return 5635;
    }

    public void read(MemoryStream msdata)
    {
        hurtTotal = proto_util.readUInt(msdata);
        roleTotal = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, hurt);
        proto_util.readLoopString(msdata, name);
    }
   }
}