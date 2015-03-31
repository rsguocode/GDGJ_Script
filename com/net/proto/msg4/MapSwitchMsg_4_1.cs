/**
 * 请求场景切换 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapSwitchMsg_4_1
  	{

    public ushort code = 0;
    public string name = "";

    public static int getCode()
    {
        // (4, 1)
        return 1025;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        name = proto_util.readString(msdata);
    }
   }
}