/**
 * 农场更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmUpdateMsg_30_7
  	{

    public byte lvl = 0;
    public uint exp = 0;
    public uint fullExp = 0;

    public static int getCode()
    {
        // (30, 7)
        return 7687;
    }

    public void read(MemoryStream msdata)
    {
        lvl = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
        fullExp = proto_util.readUInt(msdata);
    }
   }
}