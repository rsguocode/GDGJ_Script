/**
 * 升级 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleUpgradeMsg_3_11
  	{

    public byte lvl = 0;
    public UInt64 exp = 0;
    public UInt64 expFull = 0;

    public static int getCode()
    {
        // (3, 11)
        return 779;
    }

    public void read(MemoryStream msdata)
    {
        lvl = proto_util.readUByte(msdata);
        exp = proto_util.readULong(msdata);
        expFull = proto_util.readULong(msdata);
    }
   }
}