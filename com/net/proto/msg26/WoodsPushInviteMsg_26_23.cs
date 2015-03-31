/**
 * 推送邀请协助 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsPushInviteMsg_26_23
  	{

    public uint id = 0;
    public string name = "";
    public byte grade = 0;

    public static int getCode()
    {
        // (26, 23)
        return 6679;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        grade = proto_util.readUByte(msdata);
    }
   }
}