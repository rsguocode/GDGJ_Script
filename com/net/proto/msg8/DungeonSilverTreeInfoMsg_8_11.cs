/**
 * 点石成金面板信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonSilverTreeInfoMsg_8_11
  	{

    public byte times = 0;
    public byte remainTime = 0;

    public static int getCode()
    {
        // (8, 11)
        return 2059;
    }

    public void read(MemoryStream msdata)
    {
        times = proto_util.readUByte(msdata);
        remainTime = proto_util.readUByte(msdata);
    }
   }
}