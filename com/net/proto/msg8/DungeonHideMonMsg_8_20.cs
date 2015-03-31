/**
 * 刷隐藏怪 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonHideMonMsg_8_20
  	{

    public ushort code = 0;
    public uint hidePhase = 0;

    public static int getCode()
    {
        // (8, 20)
        return 2068;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        hidePhase = proto_util.readUInt(msdata);
    }
   }
}