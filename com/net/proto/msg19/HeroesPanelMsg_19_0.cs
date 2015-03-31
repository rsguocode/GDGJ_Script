/**
 * 主面板涉及信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesPanelMsg_19_0
  	{

    public ushort code = 0;
    public ushort pos = 0;
    public ushort restTimes = 0;
    public uint cd = 0;
    public ushort win = 0;
    public ushort best = 0;
    public ushort buyTimes = 0;

    public static int getCode()
    {
        // (19, 0)
        return 4864;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        pos = proto_util.readUShort(msdata);
        restTimes = proto_util.readUShort(msdata);
        cd = proto_util.readUInt(msdata);
        win = proto_util.readUShort(msdata);
        best = proto_util.readUShort(msdata);
        buyTimes = proto_util.readUShort(msdata);
    }
   }
}