/**
 * 升级土地 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmUpgradeLandMsg_30_10
  	{

    public ushort code = 0;
    public byte pos = 0;
    public byte color = 0;

    public static int getCode()
    {
        // (30, 10)
        return 7690;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        pos = proto_util.readUByte(msdata);
        color = proto_util.readUByte(msdata);
    }
   }
}