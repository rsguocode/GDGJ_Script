/**
 * 购买金币 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleGoldBuyMsg_3_43
  	{

    public ushort code = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (3, 43)
        return 811;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}