/**
 * 修改宠物名字 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetNameMsg_21_5
  	{

    public ushort code = 0;
    public uint id = 0;
    public string name = "";

    public static int getCode()
    {
        // (21, 5)
        return 5381;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
    }
   }
}