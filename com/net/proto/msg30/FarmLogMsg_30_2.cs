/**
 * 日志 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmLogMsg_30_2
  	{

    public ushort code = 0;
    public List<PFarmLog> log = new List<PFarmLog>();

    public static int getCode()
    {
        // (30, 2)
        return 7682;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PFarmLog.readLoop(msdata, log);
    }
   }
}