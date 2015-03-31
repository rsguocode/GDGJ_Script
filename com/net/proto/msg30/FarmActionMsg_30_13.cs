/**
 * 农场操作 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmActionMsg_30_13
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte pos = 0;
    public byte type = 0;
    public List<PSeed> goods = new List<PSeed>();

    public static int getCode()
    {
        // (30, 13)
        return 7693;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        pos = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
        PSeed.readLoop(msdata, goods);
    }
   }
}