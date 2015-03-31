/**
 * 传闻需要的道具信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRumorGoods
  	{

    public uint goodsId = 0;
    public uint uniqueId = 0;

    public void read(MemoryStream msdata)
    {
        
        goodsId = proto_util.readUInt(msdata);
        uniqueId = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, goodsId);
        proto_util.writeUInt(msdata, uniqueId);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRumorGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRumorGoods _pm = new PRumorGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRumorGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRumorGoods ps in p) ps.write(msdata);
        }
    
    
   }
}