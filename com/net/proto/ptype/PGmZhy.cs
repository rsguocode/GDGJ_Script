/**
 * 指导员信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGmZhy
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
    
    public static void readLoop(MemoryStream msdata, List<PGmZhy> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGmZhy _pm = new PGmZhy();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGmZhy> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGmZhy ps in p) ps.write(msdata);
        }
    
    
   }
}