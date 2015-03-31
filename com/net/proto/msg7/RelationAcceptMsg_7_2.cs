/**
 * 邀请好友 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationAcceptMsg_7_2
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (7, 2)
        return 1794;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}