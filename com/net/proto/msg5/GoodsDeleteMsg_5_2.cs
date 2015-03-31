/**
 * 删除物品(支持多个) (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsDeleteMsg_5_2
  	{

    public byte repos = 0;
    public List<uint> id = new List<uint>();

    public static int getCode()
    {
        // (5, 2)
        return 1282;
    }

    public void read(MemoryStream msdata)
    {
        repos = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, id);
    }
   }
}