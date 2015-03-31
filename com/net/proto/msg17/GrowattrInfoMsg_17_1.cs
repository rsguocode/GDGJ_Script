/**
 * 培养信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GrowattrInfoMsg_17_1
  	{

    public List<uint> attr = new List<uint>();
    public uint payTimes = 0;
    public uint lv1Free = 0;
    public uint lv1Cd = 0;
    public uint lv2Free = 0;
    public uint lv2Cd = 0;
    public uint lv3Free = 0;
    public uint lv3Cd = 0;

    public static int getCode()
    {
        // (17, 1)
        return 4353;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, attr);
        payTimes = proto_util.readUInt(msdata);
        lv1Free = proto_util.readUInt(msdata);
        lv1Cd = proto_util.readUInt(msdata);
        lv2Free = proto_util.readUInt(msdata);
        lv2Cd = proto_util.readUInt(msdata);
        lv3Free = proto_util.readUInt(msdata);
        lv3Cd = proto_util.readUInt(msdata);
    }
   }
}