/**
 * NEW标志更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonNoticeNewMsg_8_10
  	{

    public uint mapid = 0;

    public static int getCode()
    {
        // (8, 10)
        return 2058;
    }

    public void read(MemoryStream msdata)
    {
        mapid = proto_util.readUInt(msdata);
    }
   }
}