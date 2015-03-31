/**
 * 国家信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class NationInfoMsg_9_2
  	{

    public List<uint> power = new List<uint>();
    public List<PNationGwy> gwy = new List<PNationGwy>();
    public List<PBuff> buff = new List<PBuff>();

    public static int getCode()
    {
        // (9, 2)
        return 2306;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, power);
        PNationGwy.readLoop(msdata, gwy);
        PBuff.readLoop(msdata, buff);
    }
   }
}