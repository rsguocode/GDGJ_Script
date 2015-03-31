/**
 * 农场信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmInfoMsg_30_1
  	{

    public ushort code = 0;
    public byte lvl = 0;
    public uint exp = 0;
    public uint fullExp = 0;
    public List<PLand> land = new List<PLand>();

    public static int getCode()
    {
        // (30, 1)
        return 7681;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        lvl = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
        fullExp = proto_util.readUInt(msdata);
        PLand.readLoop(msdata, land);
    }
   }
}