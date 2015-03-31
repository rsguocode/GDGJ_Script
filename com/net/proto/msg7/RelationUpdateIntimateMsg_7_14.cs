/**
 * 更新亲密度 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationUpdateIntimateMsg_7_14
  	{

    public uint roleId = 0;
    public uint intimate = 0;

    public static int getCode()
    {
        // (7, 14)
        return 1806;
    }

    public void read(MemoryStream msdata)
    {
        roleId = proto_util.readUInt(msdata);
        intimate = proto_util.readUInt(msdata);
    }
   }
}