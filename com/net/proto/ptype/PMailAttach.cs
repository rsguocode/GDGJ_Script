/**
 * 角色邮件附件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMailAttach
  	{

    public uint id = 0;
    public byte count = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        count = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, count);
    }
    
    public static void readLoop(MemoryStream msdata, List<PMailAttach> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMailAttach _pm = new PMailAttach();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMailAttach> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMailAttach ps in p) ps.write(msdata);
        }
    
    
   }
}