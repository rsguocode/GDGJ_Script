/**
 * 悬赏任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRecruit
  	{

    public uint id = 0;
    public byte color = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        color = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, color);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRecruit> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRecruit _pm = new PRecruit();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRecruit> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRecruit ps in p) ps.write(msdata);
        }
    
    
   }
}