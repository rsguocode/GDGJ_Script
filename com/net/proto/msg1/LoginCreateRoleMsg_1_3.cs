/**
 * 创建角色 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginCreateRoleMsg_1_3
  	{

    public ushort code = 0;
    public uint id = 0;

    public static int getCode()
    {
        // (1, 3)
        return 259;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
    }
   }
}