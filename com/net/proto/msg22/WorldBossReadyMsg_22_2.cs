/**
 * 准备时间倒计时 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WorldBossReadyMsg_22_2
  	{

    public uint time = 0;

    public static int getCode()
    {
        // (22, 2)
        return 5634;
    }

    public void read(MemoryStream msdata)
    {
        time = proto_util.readUInt(msdata);
    }
   }
}