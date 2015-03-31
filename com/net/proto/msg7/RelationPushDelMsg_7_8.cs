/**
 * 通知好友被删除 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationPushDelMsg_7_8
  	{

    public uint roleId = 0;

    public static int getCode()
    {
        // (7, 8)
        return 1800;
    }

    public void read(MemoryStream msdata)
    {
        roleId = proto_util.readUInt(msdata);
    }
   }
}