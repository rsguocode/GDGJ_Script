/**
 * 一键收获 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmHarvestOnekeyMsg_30_14
  	{

    public ushort code = 0;
    public List<PSeed> goods = new List<PSeed>();

    public static int getCode()
    {
        // (30, 14)
        return 7694;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PSeed.readLoop(msdata, goods);
    }
   }
}