/**
 * 激活 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MedalUpgradeMsg_28_2
  	{

    public ushort code = 0;
    public byte id = 0;

    public static int getCode()
    {
        // (28, 2)
        return 7170;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUByte(msdata);
    }
   }
}