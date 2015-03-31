/**
 * 宠物穿在身上的装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PPetEquip
  	{

    public byte pos = 0;
    public uint equipid = 0;

    public void read(MemoryStream msdata)
    {
        
        pos = proto_util.readUByte(msdata);
        equipid = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUInt(msdata, equipid);
    }
    
    public static void readLoop(MemoryStream msdata, List<PPetEquip> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PPetEquip _pm = new PPetEquip();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PPetEquip> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PPetEquip ps in p) ps.write(msdata);
        }
    
    
   }
}