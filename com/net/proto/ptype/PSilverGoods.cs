/**
 * 银子 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSilverGoods
  	{

    public uint id = 0;
    public uint sum = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        sum = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, sum);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSilverGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSilverGoods _pm = new PSilverGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSilverGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSilverGoods ps in p) ps.write(msdata);
        }
    
    
   }
}