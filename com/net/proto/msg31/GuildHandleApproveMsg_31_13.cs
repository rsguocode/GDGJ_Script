/**
 * 审核申请 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildHandleApproveMsg_31_13
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (31, 13)
        return 7949;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}