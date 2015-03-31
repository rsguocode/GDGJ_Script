/**
 * 申请加入公会列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GuildApproveListMsg_31_11
  	{

    public ushort code = 0;
    public List<PGuildApplyMember> list = new List<PGuildApplyMember>();

    public static int getCode()
    {
        // (31, 11)
        return 7947;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PGuildApplyMember.readLoop(msdata, list);
    }
   }
}