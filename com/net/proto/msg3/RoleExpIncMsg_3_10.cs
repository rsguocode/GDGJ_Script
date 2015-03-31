/**
 * 获得经验 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleExpIncMsg_3_10
  	{

    public uint inc = 0;
    public byte hasWorldExp = 0;

    public static int getCode()
    {
        // (3, 10)
        return 778;
    }

    public void read(MemoryStream msdata)
    {
        inc = proto_util.readUInt(msdata);
        hasWorldExp = proto_util.readUByte(msdata);
    }
   }
}