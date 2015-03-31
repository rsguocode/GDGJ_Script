/**
 * 当前的协助玩家 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsAssistInfoMsg_26_7
  	{

    public uint assistId = 0;
    public string assistName = "";

    public static int getCode()
    {
        // (26, 7)
        return 6663;
    }

    public void read(MemoryStream msdata)
    {
        assistId = proto_util.readUInt(msdata);
        assistName = proto_util.readString(msdata);
    }
   }
}