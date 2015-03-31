/**
 * 国家公务员 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PNationGwy
  	{

    public uint id = 0;
    public uint userid = 0;
    public string name = "";
    public uint val = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        userid = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        val = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, userid);
        proto_util.writeString(msdata, name);
        proto_util.writeUInt(msdata, val);
    }
    
    public static void readLoop(MemoryStream msdata, List<PNationGwy> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PNationGwy _pm = new PNationGwy();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PNationGwy> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PNationGwy ps in p) ps.write(msdata);
        }
    
    
   }
}