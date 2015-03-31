/**
 * 镶嵌在装备上的宝石信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGemInHole
  	{

    public uint gemId = 0;
    public ushort energy = 0;

    public void read(MemoryStream msdata)
    {
        
        gemId = proto_util.readUInt(msdata);
        energy = proto_util.readUShort(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, gemId);
        proto_util.writeUShort(msdata, energy);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGemInHole> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGemInHole _pm = new PGemInHole();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGemInHole> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGemInHole ps in p) ps.write(msdata);
        }
    
    
   }
}