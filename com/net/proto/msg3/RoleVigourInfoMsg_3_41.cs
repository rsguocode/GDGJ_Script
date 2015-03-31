/**
 * 体力信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleVigourInfoMsg_3_41
  	{

    public ushort vigour = 0;
    public ushort vigourFull = 0;
    public ushort diam = 0;

    public static int getCode()
    {
        // (3, 41)
        return 809;
    }

    public void read(MemoryStream msdata)
    {
        vigour = proto_util.readUShort(msdata);
        vigourFull = proto_util.readUShort(msdata);
        diam = proto_util.readUShort(msdata);
    }
   }
}