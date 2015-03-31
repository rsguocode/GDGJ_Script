/**
 * 已激活的最大勋章id (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MedalMaxIdMsg_28_1
  	{

    public byte id = 0;

    public static int getCode()
    {
        // (28, 1)
        return 7169;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUByte(msdata);
    }
   }
}