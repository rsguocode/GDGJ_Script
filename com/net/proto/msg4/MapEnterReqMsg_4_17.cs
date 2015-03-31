/**
 * 服务器主动请求切图 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapEnterReqMsg_4_17
  	{

    public uint mapid = 0;
    public ushort x = 0;
    public ushort y = 0;

    public static int getCode()
    {
        // (4, 17)
        return 1041;
    }

    public void read(MemoryStream msdata)
    {
        mapid = proto_util.readUInt(msdata);
        x = proto_util.readUShort(msdata);
        y = proto_util.readUShort(msdata);
    }
   }
}