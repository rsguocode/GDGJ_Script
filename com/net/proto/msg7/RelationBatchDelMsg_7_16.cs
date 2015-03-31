/**
 * 批量删除 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationBatchDelMsg_7_16
  	{

    public ushort code = 0;

    public static int getCode()
    {
        // (7, 16)
        return 1808;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
    }
   }
}