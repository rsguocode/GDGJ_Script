/**
 * 宠物属性信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PPetAttr
  	{

    public uint hp = 0;
    public uint att = 0;
    public uint defP = 0;
    public uint defM = 0;
    public uint hurtRe = 0;

    public void read(MemoryStream msdata)
    {
        
        hp = proto_util.readUInt(msdata);
        att = proto_util.readUInt(msdata);
        defP = proto_util.readUInt(msdata);
        defM = proto_util.readUInt(msdata);
        hurtRe = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUInt(msdata, att);
        proto_util.writeUInt(msdata, defP);
        proto_util.writeUInt(msdata, defM);
        proto_util.writeUInt(msdata, hurtRe);
    }
    
    public static void readLoop(MemoryStream msdata, List<PPetAttr> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PPetAttr _pm = new PPetAttr();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PPetAttr> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PPetAttr ps in p) ps.write(msdata);
        }
    
    
   }
}