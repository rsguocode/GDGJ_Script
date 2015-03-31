/**
 * 种子商店信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class FarmSeedShopMsg_30_11
  	{

    public byte type = 0;
    public List<PSeed> seed = new List<PSeed>();

    public static int getCode()
    {
        // (30, 11)
        return 7691;
    }

    public void read(MemoryStream msdata)
    {
        type = proto_util.readUByte(msdata);
        PSeed.readLoop(msdata, seed);
    }
   }
}