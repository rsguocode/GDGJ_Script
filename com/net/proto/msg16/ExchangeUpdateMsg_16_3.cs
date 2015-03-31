/**
 * 兑换信息变更 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ExchangeUpdateMsg_16_3
  	{

    public uint id = 0;
    public uint remain = 0;

    public static int getCode()
    {
        // (16, 3)
        return 4099;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        remain = proto_util.readUInt(msdata);
    }
   }
}