/**
 * 培养 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GrowattrDoMsg_17_2
  	{

    public ushort code = 0;
    public byte type = 0;
    public bool isFree = false;
    public List<sbyte> addAttr = new List<sbyte>();

    public static int getCode()
    {
        // (17, 2)
        return 4354;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        isFree = proto_util.readBool(msdata);
        proto_util.readLoopByte(msdata, addAttr);
    }
   }
}