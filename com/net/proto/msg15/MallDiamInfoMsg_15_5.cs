/**
 * 钻石专区信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MallDiamInfoMsg_15_5
  	{

    public List<PDiamGoods> goods = new List<PDiamGoods>();

    public static int getCode()
    {
        // (15, 5)
        return 3845;
    }

    public void read(MemoryStream msdata)
    {
        PDiamGoods.readLoop(msdata, goods);
    }
   }
}