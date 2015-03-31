/**
 * 根据角色名加好友 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationAcceptNameMsg_7_10
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (7, 10)
        return 1802;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}