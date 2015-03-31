/**
 * 传闻数据 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRumor
  	{

    public List<PRumorRole> role = new List<PRumorRole>();
    public List<PRumorGoods> goods = new List<PRumorGoods>();
    public List<PRumorMon> mon = new List<PRumorMon>();
    public List<uint> iData = new List<uint>();
    public List<String> sData = new List<String>();

    public void read(MemoryStream msdata)
    {
        
        PRumorRole.readLoop(msdata, role);
        PRumorGoods.readLoop(msdata, goods);
        PRumorMon.readLoop(msdata, mon);
        proto_util.readLoopUInt(msdata, iData);
        proto_util.readLoopString(msdata, sData);
    }

    public void write(MemoryStream msdata)
    {
        
        PRumorRole.writeLoop(msdata, role);
        PRumorGoods.writeLoop(msdata, goods);
        PRumorMon.writeLoop(msdata, mon);
        proto_util.writeLoopUInt(msdata, iData);
        proto_util.writeLoopString(msdata, sData);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRumor> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRumor _pm = new PRumor();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRumor> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRumor ps in p) ps.write(msdata);
        }
    
    
   }
}