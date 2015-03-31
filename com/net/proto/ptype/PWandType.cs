/**
 * 魔杖类型 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PWandType
  	{

    public byte index = 0;
    public byte type = 0;

    public void read(MemoryStream msdata)
    {
        
        index = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, index);
        proto_util.writeUByte(msdata, type);
    }
    
    public static void readLoop(MemoryStream msdata, List<PWandType> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PWandType _pm = new PWandType();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PWandType> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PWandType ps in p) ps.write(msdata);
        }
    
    
   }
}