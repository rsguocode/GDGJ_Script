/**
 * 排行榜信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RankInfoMsg_20_1
  	{

    public ushort code = 0;
    public ushort type = 0;
    public List<PRank> info = new List<PRank>();
    public ushort pos = 0;

    public static int getCode()
    {
        // (20, 1)
        return 5121;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUShort(msdata);
        PRank.readLoop(msdata, info);
        pos = proto_util.readUShort(msdata);
    }
   }
}