/**
 * 请求宠物附身 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetAttachMsg_21_3
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte pos = 0;

    public static int getCode()
    {
        // (21, 3)
        return 5379;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        pos = proto_util.readUByte(msdata);
    }
   }
}