/**
 * 增加购买次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesAddTimesMsg_19_4
  	{

    public ushort code = 0;
    public ushort restTimes = 0;
    public ushort buyTimes = 0;

    public static int getCode()
    {
        // (19, 4)
        return 4868;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        restTimes = proto_util.readUShort(msdata);
        buyTimes = proto_util.readUShort(msdata);
    }
   }
}