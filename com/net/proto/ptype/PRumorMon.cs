/**
 * 传闻需要的怪物信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRumorMon
  	{

    public uint monId = 0;

    public void read(MemoryStream msdata)
    {
        
        monId = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, monId);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRumorMon> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRumorMon _pm = new PRumorMon();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRumorMon> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRumorMon ps in p) ps.write(msdata);
        }
    
    
   }
}