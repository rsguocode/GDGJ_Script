/**
 * 宝石能量 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsGemEnergyMsg_5_17
  	{

    public uint gemId = 0;
    public ushort energy = 0;

    public static int getCode()
    {
        // (5, 17)
        return 1297;
    }

    public void read(MemoryStream msdata)
    {
        gemId = proto_util.readUInt(msdata);
        energy = proto_util.readUShort(msdata);
    }
   }
}