/**
 * 兑换信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ExchangeInfoMsg_16_1
  	{

    public List<uint> id = new List<uint>();
    public List<uint> remain = new List<uint>();

    public static int getCode()
    {
        // (16, 1)
        return 4097;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, id);
        proto_util.readLoopUInt(msdata, remain);
    }
   }
}