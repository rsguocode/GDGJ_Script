/**
 * 开启通知 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossStartNoticeMsg_22_1
  	{

    public uint restTime = 0;

    public static int getCode()
    {
        // (22, 1)
        return 5633;
    }

    public void read(MemoryStream msdata)
    {
        restTime = proto_util.readUInt(msdata);
    }
   }
}