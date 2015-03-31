/**
 * 英雄榜历史信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PHistory
  	{

    public uint rumorId = 0;
    public List<PRumor> data = new List<PRumor>();

    public void read(MemoryStream msdata)
    {
        
        rumorId = proto_util.readUInt(msdata);
        PRumor.readLoop(msdata, data);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, rumorId);
        PRumor.writeLoop(msdata, data);
    }
    
    public static void readLoop(MemoryStream msdata, List<PHistory> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PHistory _pm = new PHistory();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PHistory> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PHistory ps in p) ps.write(msdata);
        }
    
    
   }
}