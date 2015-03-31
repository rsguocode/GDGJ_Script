/**
 * 装备鉴定 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltGetSuitAttrMsg_14_6
  	{

    public ushort code = 0;
    public uint id = 0;

    public static int getCode()
    {
        // (14, 6)
        return 3590;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
    }
   }
}