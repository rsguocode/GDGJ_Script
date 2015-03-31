/**
 * 勋章信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MedalListMsg_28_1
  	{

    public List<ushort> list = new List<ushort>();

    public static int getCode()
    {
        // (28, 1)
        return 7169;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUShort(msdata, list);
    }
   }
}