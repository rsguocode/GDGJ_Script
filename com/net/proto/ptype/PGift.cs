/**
 * 抽奖的奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGift
  	{

    public uint id = 0;
    public byte num = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        num = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, num);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGift> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGift _pm = new PGift();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGift> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGift ps in p) ps.write(msdata);
        }
    
    
   }
}