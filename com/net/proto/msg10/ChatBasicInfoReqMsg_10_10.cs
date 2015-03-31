/**
 * 请求私聊基本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatBasicInfoReqMsg_10_10
  	{

    public ushort code = 0;
    public uint roleId = 0;
    public string roleName = "";
    public byte roleJob = 0;
    public byte roleLvl = 0;
    public byte roleSex = 0;

    public static int getCode()
    {
        // (10, 10)
        return 2570;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        roleId = proto_util.readUInt(msdata);
        roleName = proto_util.readString(msdata);
        roleJob = proto_util.readUByte(msdata);
        roleLvl = proto_util.readUByte(msdata);
        roleSex = proto_util.readUByte(msdata);
    }
   }
}