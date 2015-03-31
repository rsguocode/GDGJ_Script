/**
 * 设置内容 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SettingSetOneSettingMsg_11_2
  	{

    public uint code = 0;
    public uint key = 0;

    public static int getCode()
    {
        // (11, 2)
        return 2818;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUInt(msdata);
        key = proto_util.readUInt(msdata);
    }
   }
}