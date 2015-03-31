/**
 * 选择角色进入游戏 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginSelectRoleMsg_1_2
  	{

    public ushort code = 0;
    public string version = "";
    public uint time = 0;
    public List<PRole> role = new List<PRole>();

    public static int getCode()
    {
        // (1, 2)
        return 258;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        version = proto_util.readString(msdata);
        time = proto_util.readUInt(msdata);
        PRole.readLoop(msdata, role);
    }
   }
}