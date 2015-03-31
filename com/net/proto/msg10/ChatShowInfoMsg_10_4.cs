/**
 * 查看玩家展示信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatShowInfoMsg_10_4
  	{

    public ushort code = 0;
    public byte type = 0;
    public List<PGoods> goodsInfo = new List<PGoods>();

    public static int getCode()
    {
        // (10, 4)
        return 2564;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        PGoods.readLoop(msdata, goodsInfo);
    }
   }
}