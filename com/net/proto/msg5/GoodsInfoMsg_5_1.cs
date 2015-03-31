/**
 * 物品信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class GoodsInfoMsg_5_1
  	{

    public byte repos = 0;
    public List<PGoods> goodsInfo = new List<PGoods>();

    public static int getCode()
    {
        // (5, 1)
        return 1281;
    }

    public void read(MemoryStream msdata)
    {
        repos = proto_util.readUByte(msdata);
        PGoods.readLoop(msdata, goodsInfo);
    }
   }
}