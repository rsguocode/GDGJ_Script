/**
 * 角色推送聊天物品信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PChatPushGoods
  	{

    public uint id = 0;
    public uint goodsId = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        goodsId = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, goodsId);
    }
    
    public static void readLoop(MemoryStream msdata, List<PChatPushGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PChatPushGoods _pm = new PChatPushGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PChatPushGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PChatPushGoods ps in p) ps.write(msdata);
        }
    
    
   }
}