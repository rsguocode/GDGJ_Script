/**
 * 审核申请 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildApproveMsg_31_12
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (31, 12)
        return 7948;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}