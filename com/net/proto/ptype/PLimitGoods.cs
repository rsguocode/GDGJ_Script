/**
 * 限购商品信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PLimitGoods
  	{

    public byte pos = 0;
    public uint id = 0;
    public uint price = 0;
    public uint sum = 0;
    public uint count = 0;

    public void read(MemoryStream msdata)
    {
        
        pos = proto_util.readUByte(msdata);
        id = proto_util.readUInt(msdata);
        price = proto_util.readUInt(msdata);
        sum = proto_util.readUInt(msdata);
        count = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, price);
        proto_util.writeUInt(msdata, sum);
        proto_util.writeUInt(msdata, count);
    }
    
    public static void readLoop(MemoryStream msdata, List<PLimitGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PLimitGoods _pm = new PLimitGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PLimitGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PLimitGoods ps in p) ps.write(msdata);
        }
    
    
   }
}