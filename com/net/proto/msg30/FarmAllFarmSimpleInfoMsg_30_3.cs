/**
 * 所有好友农场的简略信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmAllFarmSimpleInfoMsg_30_3
  	{

    public ushort code = 0;
    public List<PFarmSimple> info = new List<PFarmSimple>();

    public static int getCode()
    {
        // (30, 3)
        return 7683;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PFarmSimple.readLoop(msdata, info);
    }
   }
}