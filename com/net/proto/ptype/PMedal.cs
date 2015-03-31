/**
 * 单个勋章 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMedal
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
    
    public static void readLoop(MemoryStream msdata, List<PMedal> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMedal _pm = new PMedal();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMedal> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMedal ps in p) ps.write(msdata);
        }
    
    
   }
}