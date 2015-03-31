/**
 * 状态同步（服务端只广播） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleSyncMsg_3_44
  	{

    public uint id = 0;
    public byte state = 0;
    public uint x = 0;
    public uint y = 0;
    public byte dir = 0;

    public static int getCode()
    {
        // (3, 44)
        return 812;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        state = proto_util.readUByte(msdata);
        x = proto_util.readUInt(msdata);
        y = proto_util.readUInt(msdata);
        dir = proto_util.readUByte(msdata);
    }
   }
}