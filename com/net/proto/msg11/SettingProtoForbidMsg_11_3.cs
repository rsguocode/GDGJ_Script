/**
 * 禁止协议 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SettingProtoForbidMsg_11_3
  	{

    public byte section = 0;
    public byte method = 0;

    public static int getCode()
    {
        // (11, 3)
        return 2819;
    }

    public void read(MemoryStream msdata)
    {
        section = proto_util.readUByte(msdata);
        method = proto_util.readUByte(msdata);
    }
   }
}