/**
 * 技能列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSkill
  	{

    public uint skillId = 0;
    public uint nextiD = 0;

    public void read(MemoryStream msdata)
    {
        
        skillId = proto_util.readUInt(msdata);
        nextiD = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, skillId);
        proto_util.writeUInt(msdata, nextiD);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSkill> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSkill _pm = new PSkill();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSkill> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSkill ps in p) ps.write(msdata);
        }
    
    
   }
}