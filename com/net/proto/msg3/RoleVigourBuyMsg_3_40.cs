/**
 * 购买体力 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleVigourBuyMsg_3_40
  	{

    public ushort code = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (3, 40)
        return 808;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}