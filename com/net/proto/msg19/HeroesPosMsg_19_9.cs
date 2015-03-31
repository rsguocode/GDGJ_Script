/**
 * 排名变更 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesPosMsg_19_9
  	{

    public ushort pos = 0;

    public static int getCode()
    {
        // (19, 9)
        return 4873;
    }

    public void read(MemoryStream msdata)
    {
        pos = proto_util.readUShort(msdata);
    }
   }
}