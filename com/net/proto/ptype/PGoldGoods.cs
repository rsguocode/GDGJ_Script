/**
 * 金币 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGoldGoods
  	{

    public uint id = 0;
    public uint remain = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        remain = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, remain);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGoldGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGoldGoods _pm = new PGoldGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGoldGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGoldGoods ps in p) ps.write(msdata);
        }
    
    
   }
}