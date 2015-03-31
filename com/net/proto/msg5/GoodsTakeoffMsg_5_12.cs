/**
 * 卸装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsTakeoffMsg_5_12
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (5, 12)
        return 1292;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}