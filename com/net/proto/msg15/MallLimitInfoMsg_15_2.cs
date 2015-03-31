/**
 * 限购信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MallLimitInfoMsg_15_2
  	{

    public uint remainTime = 0;
    public List<PLimitGoods> limit = new List<PLimitGoods>();

    public static int getCode()
    {
        // (15, 2)
        return 3842;
    }

    public void read(MemoryStream msdata)
    {
        remainTime = proto_util.readUInt(msdata);
        PLimitGoods.readLoop(msdata, limit);
    }
   }
}