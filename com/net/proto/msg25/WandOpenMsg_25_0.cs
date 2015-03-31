/**
 * 开启魔杖活动信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandOpenMsg_25_0
  	{

    public byte free = 0;
    public ushort diam = 0;
    public List<PWandType> spriteList = new List<PWandType>();

    public static int getCode()
    {
        // (25, 0)
        return 6400;
    }

    public void read(MemoryStream msdata)
    {
        free = proto_util.readUByte(msdata);
        diam = proto_util.readUShort(msdata);
        PWandType.readLoop(msdata, spriteList);
    }
   }
}