/**
 * 打劫预览 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRobRewardPreviewMsg_26_26
  	{

    public ushort code = 0;
    public uint id = 0;
    public uint repu = 0;
    public uint gold = 0;

    public static int getCode()
    {
        // (26, 26)
        return 6682;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
    }
   }
}