/**
 * 抢劫 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRobPlayerMsg_26_15
  	{

    public ushort code = 0;
    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public byte job = 0;
    public List<PBaseAttr> attr = new List<PBaseAttr>();
    public List<uint> skills = new List<uint>();

    public static int getCode()
    {
        // (26, 15)
        return 6671;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        PBaseAttr.readLoop(msdata, attr);
        proto_util.readLoopUInt(msdata, skills);
    }
   }
}