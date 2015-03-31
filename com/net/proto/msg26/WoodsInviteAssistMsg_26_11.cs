/**
 * 邀请协助者 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsInviteAssistMsg_26_11
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (26, 11)
        return 6667;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}