/**
 * 宠物被动技能 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PPetSkill
  	{

    public byte id = 0;
    public byte lvl = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, id);
        proto_util.writeUByte(msdata, lvl);
    }
    
    public static void readLoop(MemoryStream msdata, List<PPetSkill> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PPetSkill _pm = new PPetSkill();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PPetSkill> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PPetSkill ps in p) ps.write(msdata);
        }
    
    
   }
}