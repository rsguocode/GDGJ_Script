/**
 * 装备信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PEquip
  	{

    public byte stren = 0;
    public byte refine = 0;
    public List<PGemInHole> gemList = new List<PGemInHole>();
    public List<PSuitAttr> suitList = new List<PSuitAttr>();

    public void read(MemoryStream msdata)
    {
        
        stren = proto_util.readUByte(msdata);
        refine = proto_util.readUByte(msdata);
        PGemInHole.readLoop(msdata, gemList);
        PSuitAttr.readLoop(msdata, suitList);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, stren);
        proto_util.writeUByte(msdata, refine);
        PGemInHole.writeLoop(msdata, gemList);
        PSuitAttr.writeLoop(msdata, suitList);
    }
    
    public static void readLoop(MemoryStream msdata, List<PEquip> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PEquip _pm = new PEquip();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PEquip> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PEquip ps in p) ps.write(msdata);
        }
    
    
   }
}