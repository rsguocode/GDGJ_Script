/**
 * 装备继承 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltInheritMsg_14_4
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (14, 4)
        return 3588;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}