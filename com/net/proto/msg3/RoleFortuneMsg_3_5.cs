/**
 * 财富更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleFortuneMsg_3_5
  	{

    public uint gold = 0;
    public uint diam = 0;
    public uint diamBind = 0;
    public uint repu = 0;

    public static int getCode()
    {
        // (3, 5)
        return 773;
    }

    public void read(MemoryStream msdata)
    {
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
        diamBind = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
    }
   }
}