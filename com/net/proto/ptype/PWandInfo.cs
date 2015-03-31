/**
 * 魔杖玩家开奖信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PWandInfo
  	{

    public string str = "";
    public uint prize = 0;
    public uint num = 0;

    public void read(MemoryStream msdata)
    {
        
        str = proto_util.readString(msdata);
        prize = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeString(msdata, str);
        proto_util.writeUInt(msdata, prize);
        proto_util.writeUInt(msdata, num);
    }
    
    public static void readLoop(MemoryStream msdata, List<PWandInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PWandInfo _pm = new PWandInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PWandInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PWandInfo ps in p) ps.write(msdata);
        }
    
    
   }
}