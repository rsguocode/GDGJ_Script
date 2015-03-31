/**
 * 击石成金角色攻击 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonAttackMsg_8_14
  	{

    public ushort code = 0;
    public uint silver = 0;

    public static int getCode()
    {
        // (8, 14)
        return 2062;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        silver = proto_util.readUInt(msdata);
    }
   }
}