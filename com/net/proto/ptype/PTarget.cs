/**
 * 目标 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PTarget
  	{

    public uint id = 0;
    public byte type = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, type);
    }
    
    public static void readLoop(MemoryStream msdata, List<PTarget> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PTarget _pm = new PTarget();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PTarget> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PTarget ps in p) ps.write(msdata);
        }
    
    
   }
}